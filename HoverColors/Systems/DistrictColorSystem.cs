// File: Systems/DistrictColorSystem.cs
// Purpose: Applies player's District overlay color/opacity to the vanilla District area prefab.
//
// Prefab data here:
//   Districts are rendered through Game.Prefabs.AreaColorData on the default District prefab entity.
//   AreaBufferSystem reads those prefab fill + edge colors while rebuilding the overlay
//   buffer, so we write prefab-side colors and then mark live District area entities Updated.
//   Important: do not query every prefab with DistrictData. Custom assets can define their
//   own district-like area prefabs, so we now target the exact vanilla District Area prefab ID.

namespace HoverColors.Systems
{
    using Colossal.Serialization.Entities;
    using CS2Shared.RiverMochi;
    using Game;
    using Game.Common;
    using Game.Prefabs;
    using Game.Serialization;
    using HoverColors.Settings;
    using Unity.Entities;
    using UnityEngine;
    using AreaComponent = Game.Areas.Area;
    using AreaNode = Game.Areas.Node;
    using AreaTriangle = Game.Areas.Triangle;
    using DistrictComponent = Game.Areas.District;
    using PrefabAreaColorData = Game.Prefabs.AreaColorData;

    public partial class DistrictColorSystem : GameSystemBase
    {
        private static readonly PrefabID s_VanillaDistrictPrefabId =
            new(nameof(DistrictPrefab), "District Area");

        private static readonly Color s_FallbackDistrictFill = new(128f / 255f, 128f / 255f, 128f / 255f, 64f / 255f);
        private static readonly Color s_FallbackDistrictEdge = new(128f / 255f, 128f / 255f, 128f / 255f, 128f / 255f);

        public static Color CapturedDistrictFillColor { get; private set; } = s_FallbackDistrictFill;
        public static Color CapturedDistrictEdgeColor { get; private set; } = s_FallbackDistrictEdge;
        public static bool HasCapturedDistrictDefaults { get; private set; }

        private PrefabSystem? m_PrefabSystem;
        private EntityQuery m_DistrictAreaQuery;

        private bool m_CaptureLogged;
        private bool m_SeededSettingsFromCapture;
        private bool m_Applied;
        private bool m_LastEnabled;
        private float m_LastR;
        private float m_LastG;
        private float m_LastB;
        private float m_LastA;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_PrefabSystem = World.GetOrCreateSystemManaged<PrefabSystem>();
            m_DistrictAreaQuery = SystemAPI.QueryBuilder()
                .WithAll<AreaComponent, DistrictComponent, AreaNode, AreaTriangle>()
                .WithNone<Deleted>()
                .Build();

            Enabled = true;
            LogUtils.Info(() => $"{Mod.ModTag} DistrictColorSystem created");
        }

        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            base.OnGameLoadingComplete(purpose, mode);
            m_Applied = false;
        }

        protected override void OnUpdate()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null)
            {
                return;
            }

            TryCaptureDefaults();
            SeedSettingsFromCapture(settings);

            bool enabled = settings.DistrictColorEnabled;
            float r = enabled ? Mathf.Clamp01(settings.DistrictR) : CapturedDistrictFillColor.r;
            float g = enabled ? Mathf.Clamp01(settings.DistrictG) : CapturedDistrictFillColor.g;
            float b = enabled ? Mathf.Clamp01(settings.DistrictB) : CapturedDistrictFillColor.b;
            float a = enabled ? Mathf.Clamp01(settings.DistrictA) : CapturedDistrictFillColor.a;

            if (m_Applied
                && enabled == m_LastEnabled
                && ApproximatelyEqual(r, m_LastR)
                && ApproximatelyEqual(g, m_LastG)
                && ApproximatelyEqual(b, m_LastB)
                && ApproximatelyEqual(a, m_LastA))
            {
                return;
            }

            if (!enabled)
            {
                m_LastEnabled = false;
                m_LastR = r;
                m_LastG = g;
                m_LastB = b;
                m_LastA = a;
                m_Applied = true;
                return;
            }

            ApplyDistrictColors(new Color(r, g, b, a));
            MarkDistrictAreasUpdated();

            m_LastEnabled = true;
            m_LastR = r;
            m_LastG = g;
            m_LastB = b;
            m_LastA = a;
            m_Applied = true;
        }

        private void TryCaptureDefaults()
        {
            if (HasCapturedDistrictDefaults || !TryGetDefaultDistrictPrefab(out Entity prefabEntity))
            {
                return;
            }

            if (m_PrefabSystem != null
                && m_PrefabSystem.TryGetPrefab(prefabEntity, out PrefabBase prefabBase)
                && prefabBase is AreaPrefab areaPrefab)
            {
                CapturedDistrictFillColor = areaPrefab.m_Color;
                CapturedDistrictEdgeColor = areaPrefab.m_EdgeColor;
                HasCapturedDistrictDefaults = true;
            }
            else
            {
                PrefabAreaColorData colorData = EntityManager.GetComponentData<PrefabAreaColorData>(prefabEntity);
                CapturedDistrictFillColor = colorData.m_FillColor;
                CapturedDistrictEdgeColor = colorData.m_EdgeColor;
                HasCapturedDistrictDefaults = true;
            }

            if (!m_CaptureLogged && HasCapturedDistrictDefaults)
            {
                m_CaptureLogged = true;
                LogUtils.Info(() => $"{Mod.ModTag} Captured vanilla District fill color: " +
                    $"RGBA=({CapturedDistrictFillColor.r:F3}, {CapturedDistrictFillColor.g:F3}, " +
                    $"{CapturedDistrictFillColor.b:F3}, {CapturedDistrictFillColor.a:F3}); edge " +
                    $"RGBA=({CapturedDistrictEdgeColor.r:F3}, {CapturedDistrictEdgeColor.g:F3}, " +
                    $"{CapturedDistrictEdgeColor.b:F3}, {CapturedDistrictEdgeColor.a:F3})");
            }
        }

        private void SeedSettingsFromCapture(HoverColorsSettings settings)
        {
            if (m_SeededSettingsFromCapture || settings.DistrictColorEnabled || !HasCapturedDistrictDefaults)
            {
                return;
            }

            // In-memory only: this lets the panel show the real captured vanilla color without
            // writing to .coc until the player chooses a custom District color.
            settings.DistrictR = CapturedDistrictFillColor.r;
            settings.DistrictG = CapturedDistrictFillColor.g;
            settings.DistrictB = CapturedDistrictFillColor.b;
            settings.DistrictA = CapturedDistrictFillColor.a;
            m_SeededSettingsFromCapture = true;
        }

        private void ApplyDistrictColors(Color color)
        {
            if (!TryGetDefaultDistrictPrefab(out Entity prefabEntity))
            {
                return;
            }

            Color32 color32 = color;

            PrefabAreaColorData data = EntityManager.GetComponentData<PrefabAreaColorData>(prefabEntity);

            // AreaBufferSystem consumes all four colors. Driving edge + selection edge here
            // is what controls the persistent District boundary line shown while editing.
            data.m_FillColor = color32;
            data.m_EdgeColor = color32;
            data.m_SelectionFillColor = color32;
            data.m_SelectionEdgeColor = color32;
            EntityManager.SetComponentData(prefabEntity, data);
        }

        private void MarkDistrictAreasUpdated()
        {
            if (m_DistrictAreaQuery.IsEmptyIgnoreFilter)
            {
                return;
            }

            // AreaBufferSystem uses this exact signal when district display needs rebuilding
            // (for example on localization/name changes), so this uses same safe vanilla path.
            EntityManager.AddComponent<Updated>(m_DistrictAreaQuery);
        }

        private bool TryGetDefaultDistrictPrefab(out Entity prefabEntity)
        {
            prefabEntity = Entity.Null;

            if (m_PrefabSystem == null
                || !m_PrefabSystem.TryGetPrefab(s_VanillaDistrictPrefabId, out PrefabBase prefabBase)
                || !m_PrefabSystem.TryGetEntity(prefabBase, out Entity candidate)
                || candidate == Entity.Null
                || !EntityManager.Exists(candidate)
                || !EntityManager.HasComponent<PrefabAreaColorData>(candidate)
                || !EntityManager.HasComponent<DistrictData>(candidate))
            {
                return false;
            }

            prefabEntity = candidate;
            return true;
        }

        private static bool ApproximatelyEqual(float a, float b)
        {
            return Mathf.Abs(a - b) < 0.0005f;
        }
    }
}
