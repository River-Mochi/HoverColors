// File: Systems/GuidelineColorSystem.cs
// Purpose: Apply player guideline color/opacity to vanilla GuideLineSettingsData.
// The game splits guideline overlays into priority colors. HC exposes the useful
// player-facing color groups in the in-city panel.

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

        // Vanilla prefab fallbacks from Game.Prefabs.GuideLineSettings. Runtime capture wins.
        private static readonly Color s_FallbackVeryLowGuidelineColor = new(0.7f, 0.7f, 1f, 0.025f);
        private static readonly Color s_FallbackLowGuidelineColor = new(0.7f, 0.7f, 1f, 0.05f);
        private static readonly Color s_FallbackMediumGuidelineColor = new(0.7f, 0.7f, 1f, 0.1f);
        private static readonly Color s_FallbackHighGuidelineColor = new(0.7f, 0.7f, 1f, 0.2f);
        private static readonly Color s_FallbackPositiveGuidelineColor = new(0.5f, 1f, 0.5f, 0.1f);
        private static readonly Color s_HighVisibilityYellowGuidelineColor = new(1f, 0.92f, 0.2f, 1f);
        private static readonly Color s_HighVisibilityGreenGuidelineColor = new(0.28f, 1f, 0.35f, 1f);
        private static readonly Color s_MochiBlueGuidelineColor = new(0.137f, 1f, 0.973f, 1f); // #23FFF8
        private static readonly Color s_CyanBlueGuidelineColor = new(0.329f, 0.843f, 1f, 1f); // #54D7FF

        // Snapshot of the game's default colors. Opacity scales default alphas.
        private Color m_DefVeryLow = s_FallbackVeryLowGuidelineColor;
        private Color m_DefLow = s_FallbackLowGuidelineColor;
        private Color m_DefMedium = s_FallbackMediumGuidelineColor;
        private Color m_DefHigh = s_FallbackHighGuidelineColor;
        private Color m_DefPositive = s_FallbackPositiveGuidelineColor;
        private bool m_DefaultsCaptured;
        private Entity m_LastEntity = Entity.Null;

        public static Color CapturedVanillaGuidelineLinesColor { get; private set; } = WithoutAlpha(s_FallbackLowGuidelineColor);
        public static Color CapturedVanillaGuidelinePreviewColor { get; private set; } = WithoutAlpha(s_FallbackMediumGuidelineColor);
        public static Color CapturedVanillaGuidelineDashedColor { get; private set; } = WithoutAlpha(s_FallbackHighGuidelineColor);

        // Last applied values. NaN/int sentinels ensure the first apply always runs.
        private float m_LastOpacity = float.NaN;
        private int m_LastLinesPreset = int.MinValue;
        private float m_LastLinesR = float.NaN;
        private float m_LastLinesG = float.NaN;
        private float m_LastLinesB = float.NaN;
        private float m_LastLinesA = float.NaN;
        private int m_LastPreviewPreset = int.MinValue;
        private float m_LastPreviewR = float.NaN;
        private float m_LastPreviewG = float.NaN;
        private float m_LastPreviewB = float.NaN;
        private float m_LastPreviewA = float.NaN;
        private int m_LastDashedPreset = int.MinValue;
        private float m_LastDashedR = float.NaN;
        private float m_LastDashedG = float.NaN;
        private float m_LastDashedB = float.NaN;

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

            float dashedOpacity = Mathf.Clamp01(settings.GuidelineOpacityPercent / 100f);
            int linesPreset = settings.GuidelineLinesColorPreset;
            float linesR = Mathf.Clamp01(settings.GuidelineLinesR);
            float linesG = Mathf.Clamp01(settings.GuidelineLinesG);
            float linesB = Mathf.Clamp01(settings.GuidelineLinesB);
            float linesA = Mathf.Clamp01(settings.GuidelineLinesA);
            int previewPreset = settings.GuidelinePreviewColorPreset;
            float previewR = Mathf.Clamp01(settings.GuidelinePreviewR);
            float previewG = Mathf.Clamp01(settings.GuidelinePreviewG);
            float previewB = Mathf.Clamp01(settings.GuidelinePreviewB);
            float previewA = Mathf.Clamp01(settings.GuidelinePreviewA);
            int dashedPreset = settings.GuidelineDashedColorPreset;
            float dashedR = Mathf.Clamp01(settings.GuidelineDashedR);
            float dashedG = Mathf.Clamp01(settings.GuidelineDashedG);
            float dashedB = Mathf.Clamp01(settings.GuidelineDashedB);

            if (m_Query.IsEmptyIgnoreFilter)
            {
                return;
            }

            Entity entity = m_Query.GetSingletonEntity();
            bool entityChanged = entity != m_LastEntity;

            // Hot-path early-return: defaults captured, same singleton, and no relevant setting changed.
            if (m_DefaultsCaptured
                && !entityChanged
                && dashedOpacity == m_LastOpacity
                && linesPreset == m_LastLinesPreset
                && linesR == m_LastLinesR
                && linesG == m_LastLinesG
                && linesB == m_LastLinesB
                && linesA == m_LastLinesA
                && previewPreset == m_LastPreviewPreset
                && previewR == m_LastPreviewR
                && previewG == m_LastPreviewG
                && previewB == m_LastPreviewB
                && previewA == m_LastPreviewA
                && dashedPreset == m_LastDashedPreset
                && dashedR == m_LastDashedR
                && dashedG == m_LastDashedG
                && dashedB == m_LastDashedB)
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
                CapturedVanillaGuidelineLinesColor = WithoutAlpha(m_DefLow);
                CapturedVanillaGuidelinePreviewColor = WithoutAlpha(m_DefMedium);
                CapturedVanillaGuidelineDashedColor = WithoutAlpha(m_DefHigh);
                m_DefaultsCaptured = true;
                m_LastEntity = entity;
                LogUtils.Info(() => $"{Mod.ModTag} Captured vanilla guideline defaults: " +
                    $"VeryLow={FormatColor(m_DefVeryLow)} Low={FormatColor(m_DefLow)} " +
                    $"Medium={FormatColor(m_DefMedium)} High={FormatColor(m_DefHigh)} " +
                    $"Positive={FormatColor(m_DefPositive)}");
            }

            Color linesRgb = GetGuidelineLinesColor(settings);
            Color previewRgb = GetGuidelinePreviewColor(settings);
            Color dashedRgb = GetGuidelineDashedColor(settings);

            // Low/VeryLow are the big road-spacing circles/arcs/guide lines.
            // Swatch alpha scales vanilla alpha so their original ratio stays intact.
            data.m_VeryLowPriorityColor = ApplyGuidelineColor(m_DefVeryLow, linesRgb, linesA);
            data.m_LowPriorityColor = ApplyGuidelineColor(m_DefLow, linesRgb, linesA);
            data.m_MediumPriorityColor = ApplyGuidelineColor(m_DefMedium, previewRgb, previewA);
            // High draws dashed alignment/telegraph helpers. RGB comes from the panel; slider scales alpha.
            data.m_HighPriorityColor = ApplyGuidelineColor(m_DefHigh, dashedRgb, dashedOpacity);

            // Leave positive placement feedback fully vanilla so valid-placement green stays familiar.
            data.m_PositiveFeedbackColor = m_DefPositive;

            EntityManager.SetComponentData(entity, data);
            m_LastOpacity = dashedOpacity;
            m_LastLinesPreset = linesPreset;
            m_LastLinesR = linesR;
            m_LastLinesG = linesG;
            m_LastLinesB = linesB;
            m_LastLinesA = linesA;
            m_LastPreviewPreset = previewPreset;
            m_LastPreviewR = previewR;
            m_LastPreviewG = previewG;
            m_LastPreviewB = previewB;
            m_LastPreviewA = previewA;
            m_LastDashedPreset = dashedPreset;
            m_LastDashedR = dashedR;
            m_LastDashedG = dashedG;
            m_LastDashedB = dashedB;
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

        public static Color GetGuidelineDashedColor(HoverColorsSettings? settings)
        {
            if (settings == null)
            {
                return CapturedVanillaGuidelineDashedColor;
            }

            return settings.GuidelineDashedColorPreset switch
            {
                HoverColorsSettings.kGuidelineDashedColorPresetCustom => new Color(
                    Mathf.Clamp01(settings.GuidelineDashedR),
                    Mathf.Clamp01(settings.GuidelineDashedG),
                    Mathf.Clamp01(settings.GuidelineDashedB),
                    1f),
                HoverColorsSettings.kGuidelineDashedColorPresetYellow => s_HighVisibilityYellowGuidelineColor,
                HoverColorsSettings.kGuidelineDashedColorPresetMochiBlue => s_MochiBlueGuidelineColor,
                HoverColorsSettings.kGuidelineDashedColorPresetCyanBlue => s_CyanBlueGuidelineColor,
                HoverColorsSettings.kGuidelineDashedColorPresetGreen => s_HighVisibilityGreenGuidelineColor,
                _ => CapturedVanillaGuidelineDashedColor,
            };
        }

        private static Color GetPresetColor(int preset, float r, float g, float b, Color vanilla)
        {
            return preset switch
            {
                HoverColorsSettings.kGuidelineColorPresetCustom => new Color(
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
