// File: Systems/OutlineColorSystem.cs
// Purpose: Apply user-chosen outline color + fill/outline alpha to the game's selection highlight.
// Can temporarily override colors while the player is using Bulldoze / Better Bulldozer or
// Net (road) tools so invisible-alpha settings don't make targets impossible to see.
//
// Surfaces written (one color choice covers all of them, two alpha sliders control opacity):
//   - RenderingSettingsData.m_HoveredColor.RGB   ← Outline RGB (lot-pattern tint on hovered building)
//   - RenderingSettingsData.m_OwnerColor.RGB     ← Outline RGB (parent/owner objects of placed building)
//   - Material _OuterColor.RGB                   ← Outline RGB (the visible halo edge color)
//   - Material _OuterColor.a                     ← OutlineA   (halo edge opacity)
//   - Material _InnerColor.RGB                   ← Outline RGB (color of fill overlay inside silhouette)
//   - Material _InnerColor.a                     ← FillA      (fill overlay opacity)
//
// Tool override: controlled by HoverColorsSettings.ToolColorMode.
//   - Overlapping / invalid placement errors: optional vanilla ErrorColor wins first.
//   - Recommended: WarningColor for bulldozer, softer vanilla blue for roads.
//   - Vanilla: captured vanilla hover profile while those tools are active.
//   - Custom: player color everywhere.
// The dirty-flag tracks the *effective* values so an idle tool session is still ~free per frame.
//
// Performance contract (matters because this system runs every Rendering tick):
//   - The HDRP CustomPassVolume / OutlinesWorldUIPass / Material refs are found ONCE and cached.
//     The fallback scene scan is throttled while loading so we never call
//     Object.FindObjectsOfType<CustomPassVolume>() every frame.
//   - Last-applied 5-float snapshot is kept, so OnUpdate early-returns (5 compares + return) when
//     neither the sliders nor the active-tool override flag changed.
//   - Cache invalidates only when the Material reference goes destroyed-null (e.g. scene reload).

namespace HoverColors.Systems
{
    using CS2Shared.RiverMochi;
    using Game;
    using Game.Prefabs;
    using Game.Rendering;
    using Game.Tools;
    using HoverColors.Settings;
    using System;
    using System.Reflection;
    using Unity.Entities;
    using UnityEngine;
    using UnityEngine.Rendering.HighDefinition;

    public partial class OutlineColorSystem : GameSystemBase
    {
        // Vanilla cyan defaults applied during Bulldoze / Net tool override.
        // Keep in sync with HoverColorsSettings.SetDefaults().
        private const float VanillaR = 0.502f;
        private const float VanillaG = 0.869f;
        private const float VanillaB = 1f;
        private const float VanillaOutlineA = 0.855f;
        private const float VanillaFillA = 0f;
        private const float VanillaOwnerR = 0.247f;
        private const float VanillaOwnerG = 0.981f;
        private const float VanillaOwnerB = 0.247f;
        private const float VanillaOwnerA = 0.702f;
        private const float RoadRecommendedOutlineA = 0.75f;
        private const float MaterialResolveRetrySeconds = 0.5f;

        public static Color CapturedHoveredColor { get; private set; } = new Color(VanillaR, VanillaG, VanillaB, VanillaOutlineA);
        public static Color CapturedOwnerColor { get; private set; } = new Color(VanillaOwnerR, VanillaOwnerG, VanillaOwnerB, VanillaOwnerA);
        public static Color CapturedOuterColor { get; private set; } = new Color(1f, 1f, 1f, VanillaOutlineA);
        public static Color CapturedInnerColor { get; private set; } = new Color(1f, 1f, 1f, VanillaFillA);
        public static Color CapturedWarningColor { get; private set; } = new Color(1f, 1f, 0.5f, 0.447058827f);
        public static Color CapturedErrorColor { get; private set; } = new Color(1f, 0.5f, 0.5f, 0.447058827f);
        public static float CapturedOutlineA { get; private set; } = VanillaOutlineA;
        public static float CapturedFillA { get; private set; } = VanillaFillA;
        public static bool HasCapturedVanillaDefaults { get; private set; }

        private EntityQuery m_RenderSettingsQuery;
        private ToolSystem? m_ToolSystem;
        private PrefabSystem? m_PrefabSystem;
        private readonly PrefabID m_RenderingSettingsPrefab = new(nameof(m_RenderingSettingsPrefab), "RenderingSettings");

        // Cached HDRP outline material. UnityEngine.Object operator!= detects destroyed-but-not-null.
        private Material? m_OutlineMaterial;
        private float m_NextMaterialResolveTime;
        private bool m_PrefabDefaultsCaptured;
        private bool m_RenderingDefaultsCaptured;
        private bool m_MaterialDefaultsCaptured;
        private bool m_CaptureLogged;

        // Last-applied EFFECTIVE values (after tool-override decision).
        private float m_LastR, m_LastG, m_LastB, m_LastOutlineA, m_LastFillA;
        private EffectivePalette m_LastPalette;
        private bool m_Applied;
        private ToolBaseSystem? m_CachedErrorTool;
        private EntityQuery m_CachedErrorQuery;
        private bool m_HasCachedErrorQuery;

        private static readonly FieldInfo? s_ToolErrorQueryField =
            typeof(ToolBaseSystem).GetField("m_ErrorQuery", BindingFlags.Instance | BindingFlags.NonPublic);

        private enum ToolKind
        {
            None,
            Bulldoze,
            Net,
        }

        private enum EffectivePalette
        {
            Custom,
            CapturedVanilla,
            VanillaToolError,
            RecommendedBulldoze,
            RecommendedNet,
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_RenderSettingsQuery = GetEntityQuery(ComponentType.ReadWrite<RenderingSettingsData>());
            m_ToolSystem = World.GetOrCreateSystemManaged<ToolSystem>();
            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            LogUtils.Info(() => $"{Mod.ModTag} OutlineColorSystem created");
        }

        protected override void OnUpdate()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null)
            {
                return;
            }

            TryCaptureVanillaDefaults();

            float r, g, b, outlineA, fillA;
            EffectivePalette palette;
            ToolBaseSystem? activeToolSystem = m_ToolSystem?.activeTool;
            ToolKind activeTool = GetActiveToolKind(activeToolSystem);
            if (settings.UseOverlapWarningColor && HasBlockingToolError(activeToolSystem))
            {
                Color error = CapturedErrorColor;
                r = error.r;
                g = error.g;
                b = error.b;
                outlineA = error.a;
                fillA = CapturedFillA;
                palette = EffectivePalette.VanillaToolError;
            }
            else if (settings.ToolColorMode == HoverColorsSettings.ToolColorModeRecommended
                && activeTool == ToolKind.Bulldoze)
            {
                Color warning = CapturedWarningColor;
                r = warning.r;
                g = warning.g;
                b = warning.b;
                outlineA = warning.a;
                fillA = CapturedFillA;
                palette = EffectivePalette.RecommendedBulldoze;
            }
            else if (settings.ToolColorMode == HoverColorsSettings.ToolColorModeRecommended
                && activeTool == ToolKind.Net)
            {
                Color hovered = CapturedHoveredColor;
                r = hovered.r;
                g = hovered.g;
                b = hovered.b;
                outlineA = Mathf.Min(CapturedOutlineA, RoadRecommendedOutlineA);
                fillA = CapturedFillA;
                palette = EffectivePalette.RecommendedNet;
            }
            else if (settings.ToolColorMode == HoverColorsSettings.ToolColorModeVanilla
                && activeTool != ToolKind.None)
            {
                Color hovered = CapturedHoveredColor;
                r = hovered.r;
                g = hovered.g;
                b = hovered.b;
                outlineA = CapturedOutlineA;
                fillA = CapturedFillA;
                palette = EffectivePalette.CapturedVanilla;
            }
            else
            {
                r = settings.OutlineR;
                g = settings.OutlineG;
                b = settings.OutlineB;
                outlineA = settings.OutlineA;
                fillA = settings.FillA;
                palette = MatchesCapturedVanillaProfile(r, g, b, outlineA, fillA)
                    ? EffectivePalette.CapturedVanilla
                    : EffectivePalette.Custom;
            }

            // Hot-path: neither effective slider value nor the override flag has shifted.
            if (m_Applied
                && r == m_LastR
                && g == m_LastG
                && b == m_LastB
                && outlineA == m_LastOutlineA
                && fillA == m_LastFillA
                && palette == m_LastPalette)
            {
                return;
            }

            bool ecsOk = ApplyRenderingSettingsColors(r, g, b, outlineA, palette);
            bool matOk = ApplyOutlineMaterialColors(r, g, b, outlineA, fillA, palette);

            // Only cache the snapshot when BOTH writes land — otherwise retry next frame.
            if (ecsOk && matOk)
            {
                m_LastR = r;
                m_LastG = g;
                m_LastB = b;
                m_LastOutlineA = outlineA;
                m_LastFillA = fillA;
                m_LastPalette = palette;
                m_Applied = true;
            }
        }

        private void TryCaptureVanillaDefaults()
        {
            if (!m_PrefabDefaultsCaptured && m_PrefabSystem != null
                && m_PrefabSystem.TryGetPrefab(m_RenderingSettingsPrefab, out PrefabBase prefab)
                && m_PrefabSystem.TryGetEntity(prefab, out Entity prefabEntity)
                && EntityManager.HasComponent<RenderingSettingsData>(prefabEntity))
            {
                RenderingSettingsData prefabData = EntityManager.GetComponentData<RenderingSettingsData>(prefabEntity);
                if (!m_RenderingDefaultsCaptured)
                {
                    CapturedHoveredColor = prefabData.m_HoveredColor;
                    CapturedOwnerColor = prefabData.m_OwnerColor;
                    CapturedWarningColor = prefabData.m_WarningColor;
                    CapturedErrorColor = prefabData.m_ErrorColor;
                    m_RenderingDefaultsCaptured = true;
                }

                m_PrefabDefaultsCaptured = true;
            }

            if (!m_RenderingDefaultsCaptured && !m_RenderSettingsQuery.IsEmptyIgnoreFilter)
            {
                Entity entity = m_RenderSettingsQuery.GetSingletonEntity();
                RenderingSettingsData data = EntityManager.GetComponentData<RenderingSettingsData>(entity);
                CapturedHoveredColor = data.m_HoveredColor;
                CapturedOwnerColor = data.m_OwnerColor;
                CapturedWarningColor = data.m_WarningColor;
                CapturedErrorColor = data.m_ErrorColor;
                m_RenderingDefaultsCaptured = true;
            }

            if (!m_MaterialDefaultsCaptured && TryResolveOutlineMaterial())
            {
                Color outer = m_OutlineMaterial!.GetColor("_OuterColor");
                Color inner = m_OutlineMaterial.GetColor("_InnerColor");
                CapturedOuterColor = outer;
                CapturedInnerColor = inner;
                CapturedOutlineA = outer.a;
                CapturedFillA = inner.a;
                m_MaterialDefaultsCaptured = true;
            }

            if (!HasCapturedVanillaDefaults && m_RenderingDefaultsCaptured && m_MaterialDefaultsCaptured)
            {
                HasCapturedVanillaDefaults = true;
            }

            if (!m_CaptureLogged && HasCapturedVanillaDefaults)
            {
                m_CaptureLogged = true;
                LogUtils.Info(() =>
                    $"{Mod.ModTag} Captured vanilla render colors:\n" +
                    $"  Hovered RGBA = {FormatColor(CapturedHoveredColor)}\n" +
                    $"  Owner   RGBA = {FormatColor(CapturedOwnerColor)}\n" +
                    $"  Warning RGBA = {FormatColor(CapturedWarningColor)}\n" +
                    $"  Error   RGBA = {FormatColor(CapturedErrorColor)}\n" +
                    $"  Outer   RGBA = {FormatColor(CapturedOuterColor)}\n" +
                    $"  Inner   RGBA = {FormatColor(CapturedInnerColor)}");
            }
        }

        private static string FormatColor(Color color)
        {
            return $"({color.r:F3}, {color.g:F3}, {color.b:F3}, {color.a:F3})";
        }

        // ECS singleton: hovered + owner overlay color used by several vanilla render paths.
        // Building lots clamp this alpha internally, but area/surface borders read it directly,
        // so we forward OutlineA here to make extractor and painted-area borders respect the
        // same outline-opacity control as the main hover highlight.
        private bool ApplyRenderingSettingsColors(float r, float g, float b, float outlineA, EffectivePalette palette)
        {
            if (m_RenderSettingsQuery.IsEmptyIgnoreFilter)
            {
                return false;
            }

            Entity entity = m_RenderSettingsQuery.GetSingletonEntity();
            RenderingSettingsData data = EntityManager.GetComponentData<RenderingSettingsData>(entity);

            switch (palette)
            {
                case EffectivePalette.CapturedVanilla:
                    data.m_HoveredColor = CapturedHoveredColor;
                    data.m_OwnerColor = CapturedOwnerColor;
                    data.m_WarningColor = CapturedWarningColor;
                    data.m_ErrorColor = CapturedErrorColor;
                    break;
                case EffectivePalette.VanillaToolError:
                    // Blocking placement errors already carry Game.Tools.Error; vanilla render
                    // paths color those objects from m_ErrorColor. Keep the rest of the hover
                    // profile vanilla so the final salmon matches the game's own composition.
                    data.m_HoveredColor = CapturedHoveredColor;
                    data.m_OwnerColor = CapturedOwnerColor;
                    data.m_WarningColor = CapturedWarningColor;
                    data.m_ErrorColor = CapturedErrorColor;
                    break;
                case EffectivePalette.RecommendedBulldoze:
                    data.m_HoveredColor = CapturedWarningColor;
                    data.m_OwnerColor = CapturedWarningColor;
                    data.m_WarningColor = CapturedWarningColor;
                    data.m_ErrorColor = CapturedErrorColor;
                    break;
                case EffectivePalette.RecommendedNet:
                    Color hovered = CapturedHoveredColor;
                    hovered.a = outlineA;
                    Color owner = CapturedOwnerColor;
                    owner.a = Mathf.Min(owner.a, outlineA);
                    data.m_HoveredColor = hovered;
                    data.m_OwnerColor = owner;
                    break;
                default:
                    Color rgb = new Color(r, g, b, outlineA);
                    data.m_HoveredColor = rgb;
                    data.m_OwnerColor = rgb;
                    break;
            }

            EntityManager.SetComponentData(entity, data);
            return true;
        }

        // HDRP material: same RGB to both inner and outer (so the color covers the halo edge AND
        // the fill overlay inside the silhouette). Two distinct alphas:
        //   _OuterColor.a = outlineA (halo edge opacity)
        //   _InnerColor.a = fillA    (fill overlay opacity inside the silhouette)
        private bool ApplyOutlineMaterialColors(float r, float g, float b, float outlineA, float fillA, EffectivePalette palette)
        {
            if (!TryResolveOutlineMaterial())
            {
                return false;
            }

            Color outer;
            Color inner;
            switch (palette)
            {
                case EffectivePalette.CapturedVanilla:
                    outer = CapturedOuterColor;
                    inner = CapturedInnerColor;
                    break;
                case EffectivePalette.VanillaToolError:
                    // Do not paint the material red here. Vanilla salmon comes from
                    // RenderingSettingsData.m_ErrorColor plus the normal outline material.
                    outer = CapturedOuterColor;
                    inner = CapturedInnerColor;
                    break;
                case EffectivePalette.RecommendedBulldoze:
                    outer = CapturedWarningColor;
                    inner = new Color(CapturedWarningColor.r, CapturedWarningColor.g, CapturedWarningColor.b, CapturedFillA);
                    break;
                case EffectivePalette.RecommendedNet:
                    outer = CapturedOuterColor;
                    outer.a = outlineA;
                    inner = CapturedInnerColor;
                    break;
                default:
                    outer = new Color(r, g, b, outlineA);
                    inner = new Color(r, g, b, fillA);
                    break;
            }

            m_OutlineMaterial!.SetColor("_OuterColor", outer);
            m_OutlineMaterial.SetColor("_InnerColor", inner);
            return true;
        }

        private static ToolKind GetActiveToolKind(ToolBaseSystem? tool)
        {
            if (tool == null)
            {
                return ToolKind.None;
            }

            if (tool is BulldozeToolSystem)
            {
                return ToolKind.Bulldoze;
            }

            if (tool is NetToolSystem)
            {
                return ToolKind.Net;
            }

            // Better Bulldozer may still drive vanilla BulldozeToolSystem, but this keeps the
            // feature resilient if a tool wrapper becomes active instead.
            string typeName = tool.GetType().Name;
            if (typeName.IndexOf("Bulldoze", StringComparison.OrdinalIgnoreCase) >= 0
                || typeName.IndexOf("Bulldozer", StringComparison.OrdinalIgnoreCase) >= 0
                || SafeToolId(tool).IndexOf("Bulldoze", StringComparison.OrdinalIgnoreCase) >= 0
                || SafeToolId(tool).IndexOf("Bulldozer", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return ToolKind.Bulldoze;
            }

            return ToolKind.None;
        }

        private bool HasBlockingToolError(ToolBaseSystem? tool)
        {
            if (tool == null)
            {
                return false;
            }

            if (s_ToolErrorQueryField == null)
            {
                LogUtils.WarnOnce(
                    "tool-error-query-field-missing",
                    () => $"{Mod.ModTag} Cannot preserve placement error color: ToolBaseSystem.m_ErrorQuery not found.");
                return false;
            }

            try
            {
                // Vanilla ToolBaseSystem.GetAllowApply() checks this same query. It only becomes
                // non-empty for real blocking tool errors, including Overlapping items.
                if (!ReferenceEquals(m_CachedErrorTool, tool))
                {
                    object? value = s_ToolErrorQueryField.GetValue(tool);
                    m_CachedErrorTool = tool;
                    m_HasCachedErrorQuery = value is EntityQuery;
                    if (m_HasCachedErrorQuery)
                    {
                        m_CachedErrorQuery = (EntityQuery)value!;
                    }
                }

                return m_HasCachedErrorQuery && !m_CachedErrorQuery.IsEmptyIgnoreFilter;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "tool-error-query-read-failed",
                    () => $"{Mod.ModTag} Cannot preserve placement error color: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return false;
            }
        }

        private static string SafeToolId(ToolBaseSystem tool)
        {
            try
            {
                return tool.toolID ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static bool MatchesCapturedVanillaProfile(float r, float g, float b, float outlineA, float fillA)
        {
            return ApproximatelyEqual(r, CapturedHoveredColor.r)
                && ApproximatelyEqual(g, CapturedHoveredColor.g)
                && ApproximatelyEqual(b, CapturedHoveredColor.b)
                && ApproximatelyEqual(outlineA, CapturedOutlineA)
                && ApproximatelyEqual(fillA, CapturedFillA);
        }

        private static bool ApproximatelyEqual(float a, float b)
        {
            return Mathf.Abs(a - b) < 0.0005f;
        }

        // Locates the OutlinesWorldUIPass material once per scene and caches it.
        // Re-scans only when the cached Material is destroyed (Unity operator!= detects that).
        private bool TryResolveOutlineMaterial()
        {
            if (m_OutlineMaterial != null)
            {
                return true;
            }

            // Scene load can briefly run before the outline pass exists. Throttle the expensive
            // Unity object scan; once the material is found, the cached reference handles all
            // future frames until Unity destroys it on scene reload.
            float now = UnityEngine.Time.realtimeSinceStartup;
            if (now < m_NextMaterialResolveTime)
            {
                return false;
            }

            m_NextMaterialResolveTime = now + MaterialResolveRetrySeconds;

            CustomPassVolume[] volumes = UnityEngine.Object.FindObjectsOfType<CustomPassVolume>();
            for (int i = 0; i < volumes.Length; i++)
            {
                CustomPassVolume volume = volumes[i];
                if (volume == null || volume.customPasses == null)
                {
                    continue;
                }

                for (int j = 0; j < volume.customPasses.Count; j++)
                {
                    if (volume.customPasses[j] is OutlinesWorldUIPass pass && pass.m_FullscreenOutline != null)
                    {
                        m_OutlineMaterial = pass.m_FullscreenOutline;
                        LogUtils.Info(() => $"{Mod.ModTag} OutlinesWorldUIPass material cached");
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
