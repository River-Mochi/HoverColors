// File: Systems/GuidelineColorSystem.cs
// Purpose: Apply player guideline color/opacity to vanilla GuideLineSettingsData.
// The game splits guideline overlays into priority colors. HC exposes only the
// two useful player-facing groups and leaves the rest close to vanilla.
//
// Background: HighlightsAndGuidelinesTweaks repo pointed at GuideLineSettingsData on
// the rendering-settings prefab. We instead write to the runtime singleton each time the slider
// moves, which:
//   - applies live without needing the player to reload the city, and
//   - is read every frame by Game.Rendering.GuideLinesSystem
//     (m_RenderingSettingsQuery.GetSingleton<GuideLineSettingsData>()).
//
// Performance — same as OutlineColorSystem.cs:
//   - Capture the game's default colors once per rendering-settings singleton. The
//     opacity multiplier scales those defaults; defaults themselves are never modified.
//   - Compare current settings against last-applied values and early-return while idle.

namespace HoverColors.Systems
{
    using CS2Shared.RiverMochi;
    using Game;
    using Game.Prefabs;
    using HoverColors.Settings;
    using Unity.Entities;
    using UnityEngine;

    public partial class GuidelineColorSystem : GameSystemBase
    {
        private EntityQuery m_Query;

        private static readonly Color FallbackVanillaGuidelineColor = new(0.7f, 0.7f, 1f, 1f);
        private static readonly Color SoftBlueGuidelineColor = new(0.502f, 0.869f, 1f, 1f);
        private static readonly Color HighVisibilityGuidelineColor = new(0.85f, 1f, 1f, 1f);

        // Snapshot of the game's default colors. Opacity scales default alphas.
        private Color m_DefVeryLow;
        private Color m_DefLow;
        private Color m_DefMedium;
        private Color m_DefHigh;
        private Color m_DefPositive;
        private bool m_DefaultsCaptured;
        private Entity m_LastEntity = Entity.Null;

        public static Color CapturedVanillaGuidelineLinesColor { get; private set; } = FallbackVanillaGuidelineColor;
        public static Color CapturedVanillaGuidelinePreviewColor { get; private set; } = FallbackVanillaGuidelineColor;

        // Last applied values. NaN/int sentinels ensure the first apply always runs.
        private float m_LastOpacity = float.NaN;
        private int m_LastLinesPreset = int.MinValue;
        private float m_LastLinesR = float.NaN;
        private float m_LastLinesG = float.NaN;
        private float m_LastLinesB = float.NaN;
        private int m_LastPreviewPreset = int.MinValue;
        private float m_LastPreviewR = float.NaN;
        private float m_LastPreviewG = float.NaN;
        private float m_LastPreviewB = float.NaN;

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Query = GetEntityQuery(ComponentType.ReadWrite<GuideLineSettingsData>());
            LogUtils.Info(() => $"{Mod.ModTag} GuidelineColorSystem created");
        }

        protected override void OnUpdate()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null)
            {
                return;
            }

            float opacity = Mathf.Clamp01(settings.GuidelineOpacityPercent / 100f);
            int linesPreset = settings.GuidelineLinesColorPreset;
            float linesR = Mathf.Clamp01(settings.GuidelineLinesR);
            float linesG = Mathf.Clamp01(settings.GuidelineLinesG);
            float linesB = Mathf.Clamp01(settings.GuidelineLinesB);
            int previewPreset = settings.GuidelinePreviewColorPreset;
            float previewR = Mathf.Clamp01(settings.GuidelinePreviewR);
            float previewG = Mathf.Clamp01(settings.GuidelinePreviewG);
            float previewB = Mathf.Clamp01(settings.GuidelinePreviewB);

            if (m_Query.IsEmptyIgnoreFilter)
            {
                return;
            }

            Entity entity = m_Query.GetSingletonEntity();
            bool entityChanged = entity != m_LastEntity;

            // Hot-path early-return: defaults captured, same singleton, and no relevant setting changed.
            if (m_DefaultsCaptured
                && !entityChanged
                && opacity == m_LastOpacity
                && linesPreset == m_LastLinesPreset
                && linesR == m_LastLinesR
                && linesG == m_LastLinesG
                && linesB == m_LastLinesB
                && previewPreset == m_LastPreviewPreset
                && previewR == m_LastPreviewR
                && previewG == m_LastPreviewG
                && previewB == m_LastPreviewB)
            {
                return;
            }

            GuideLineSettingsData data = EntityManager.GetComponentData<GuideLineSettingsData>(entity);

            if (!m_DefaultsCaptured || entityChanged)
            {
                m_DefVeryLow = data.m_VeryLowPriorityColor;
                m_DefLow = data.m_LowPriorityColor;
                m_DefMedium = data.m_MediumPriorityColor;
                m_DefHigh = data.m_HighPriorityColor;
                m_DefPositive = data.m_PositiveFeedbackColor;
                CapturedVanillaGuidelineLinesColor = WithoutAlpha(m_DefHigh);
                CapturedVanillaGuidelinePreviewColor = WithoutAlpha(m_DefMedium);
                m_DefaultsCaptured = true;
                m_LastEntity = entity;
                LogUtils.Info(() => $"{Mod.ModTag} GuidelineColorSystem captured default colors " +
                    $"VL={FormatColor(m_DefVeryLow)} L={FormatColor(m_DefLow)} " +
                    $"M={FormatColor(m_DefMedium)} H={FormatColor(m_DefHigh)} " +
                    $"P={FormatColor(m_DefPositive)}");
            }

            Color linesRgb = GetGuidelineLinesColor(settings);
            Color previewRgb = GetGuidelinePreviewColor(settings);

            // Low/VeryLow are the big vanilla guide circles/arcs. Keep RGB vanilla for phase 1.
            data.m_VeryLowPriorityColor = WithAlpha(m_DefVeryLow, m_DefVeryLow.a * opacity);
            data.m_LowPriorityColor = WithAlpha(m_DefLow, m_DefLow.a * opacity);
            data.m_MediumPriorityColor = ApplyGuidelineColor(m_DefMedium, previewRgb, opacity);
            data.m_HighPriorityColor = ApplyGuidelineColor(m_DefHigh, linesRgb, opacity);

            // Leave positive placement feedback fully vanilla so valid-placement green stays familiar.
            data.m_PositiveFeedbackColor = m_DefPositive;

            EntityManager.SetComponentData(entity, data);
            m_LastOpacity = opacity;
            m_LastLinesPreset = linesPreset;
            m_LastLinesR = linesR;
            m_LastLinesG = linesG;
            m_LastLinesB = linesB;
            m_LastPreviewPreset = previewPreset;
            m_LastPreviewR = previewR;
            m_LastPreviewG = previewG;
            m_LastPreviewB = previewB;
        }

        public static Color GetGuidelineLinesColor(HoverColorsSettings? settings)
        {
            if (settings == null)
            {
                return CapturedVanillaGuidelineLinesColor;
            }

            return GetPresetColor(
                settings.GuidelineLinesColorPreset,
                settings.GuidelineLinesR,
                settings.GuidelineLinesG,
                settings.GuidelineLinesB,
                CapturedVanillaGuidelineLinesColor);
        }

        public static Color GetGuidelinePreviewColor(HoverColorsSettings? settings)
        {
            if (settings == null)
            {
                return CapturedVanillaGuidelinePreviewColor;
            }

            return GetPresetColor(
                settings.GuidelinePreviewColorPreset,
                settings.GuidelinePreviewR,
                settings.GuidelinePreviewG,
                settings.GuidelinePreviewB,
                CapturedVanillaGuidelinePreviewColor);
        }

        private static Color GetPresetColor(int preset, float r, float g, float b, Color vanilla)
        {
            return preset switch
            {
                HoverColorsSettings.GuidelineColorPresetWhite => Color.white,
                HoverColorsSettings.GuidelineColorPresetSoftBlue => SoftBlueGuidelineColor,
                HoverColorsSettings.GuidelineColorPresetHighVisibility => HighVisibilityGuidelineColor,
                HoverColorsSettings.GuidelineColorPresetCustom => new Color(
                    Mathf.Clamp01(r),
                    Mathf.Clamp01(g),
                    Mathf.Clamp01(b),
                    1f),
                _ => vanilla,
            };
        }

        private static Color ApplyGuidelineColor(Color defaultColor, Color rgb, float opacity)
        {
            return new Color(rgb.r, rgb.g, rgb.b, defaultColor.a * opacity);
        }

        private static Color WithAlpha(Color c, float a)
        {
            c.a = a;
            return c;
        }

        private static Color WithoutAlpha(Color c)
        {
            c.a = 1f;
            return c;
        }

        private static string FormatColor(Color c)
        {
            return $"RGBA=({c.r:F3}, {c.g:F3}, {c.b:F3}, {c.a:F3})";
        }
    }
}
