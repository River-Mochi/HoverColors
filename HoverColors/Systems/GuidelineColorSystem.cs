// File: Systems/GuidelineColorSystem.cs
// Purpose: Apply the player's guideline color/opacity to vanilla GuideLineSettingsData.
// The guideline overlay is what the game draws while placing roads/zones/props
// (high/medium/low/very-low priority arrows + the positive-feedback green).
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

        // TEMP probe for mapping vanilla guideline priority colors in-game.
        // Set false after testing; normal picker/preset colors then apply to all priorities again.
        private static readonly bool GuidelinePriorityProbeMode = true;
        private static readonly Color ProbeVeryLowColor = new(1f, 0f, 1f, 1f);      // magenta
        private static readonly Color ProbeLowColor = new(1f, 0.9f, 0f, 1f);        // yellow
        private static readonly Color ProbeMediumColor = new(1f, 0.45f, 0f, 1f);    // orange
        private static readonly Color ProbeHighColor = new(0f, 1f, 0.15f, 1f);      // bright green

        // Snapshot of the game's default colors. Opacity multiplies the default alphas and
        // custom RGB replaces only the four priority colors, not positive-feedback green.
        private Color m_DefVeryLow;
        private Color m_DefLow;
        private Color m_DefMedium;
        private Color m_DefHigh;
        private Color m_DefPositive;
        private bool m_DefaultsCaptured;
        private Entity m_LastEntity = Entity.Null;

        public static Color CapturedVanillaGuidelineColor { get; private set; } = FallbackVanillaGuidelineColor;

        // Last applied values. NaN/int sentinels ensure the first apply always runs.
        private float m_LastOpacity = float.NaN;
        private int m_LastPreset = int.MinValue;
        private float m_LastR = float.NaN;
        private float m_LastG = float.NaN;
        private float m_LastB = float.NaN;

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
            int preset = settings.GuidelineColorPreset;
            float customR = Mathf.Clamp01(settings.GuidelineR);
            float customG = Mathf.Clamp01(settings.GuidelineG);
            float customB = Mathf.Clamp01(settings.GuidelineB);

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
                && preset == m_LastPreset
                && customR == m_LastR
                && customG == m_LastG
                && customB == m_LastB)
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
                CapturedVanillaGuidelineColor = WithoutAlpha(m_DefHigh);
                m_DefaultsCaptured = true;
                m_LastEntity = entity;
                LogUtils.Info(() => $"{Mod.ModTag} GuidelineColorSystem captured default colors " +
                    $"VL={FormatColor(m_DefVeryLow)} L={FormatColor(m_DefLow)} " +
                    $"M={FormatColor(m_DefMedium)} H={FormatColor(m_DefHigh)} " +
                    $"P={FormatColor(m_DefPositive)}");
            }

            if (GuidelinePriorityProbeMode)
            {
                data.m_VeryLowPriorityColor = ApplyGuidelineColor(m_DefVeryLow, ProbeVeryLowColor, opacity);
                data.m_LowPriorityColor = ApplyGuidelineColor(m_DefLow, ProbeLowColor, opacity);
                data.m_MediumPriorityColor = ApplyGuidelineColor(m_DefMedium, ProbeMediumColor, opacity);
                data.m_HighPriorityColor = ApplyGuidelineColor(m_DefHigh, ProbeHighColor, opacity);
            }
            else
            {
                Color guidelineRgb = GetGuidelineColor(settings);
                data.m_VeryLowPriorityColor = ApplyGuidelineColor(m_DefVeryLow, guidelineRgb, opacity);
                data.m_LowPriorityColor = ApplyGuidelineColor(m_DefLow, guidelineRgb, opacity);
                data.m_MediumPriorityColor = ApplyGuidelineColor(m_DefMedium, guidelineRgb, opacity);
                data.m_HighPriorityColor = ApplyGuidelineColor(m_DefHigh, guidelineRgb, opacity);
            }

            data.m_PositiveFeedbackColor = WithAlpha(m_DefPositive, m_DefPositive.a * opacity);

            EntityManager.SetComponentData(entity, data);
            m_LastOpacity = opacity;
            m_LastPreset = preset;
            m_LastR = customR;
            m_LastG = customG;
            m_LastB = customB;
        }

        public static Color GetGuidelineColor(HoverColorsSettings? settings)
        {
            if (settings == null)
            {
                return CapturedVanillaGuidelineColor;
            }

            return settings.GuidelineColorPreset switch
            {
                HoverColorsSettings.GuidelineColorPresetWhite => Color.white,
                HoverColorsSettings.GuidelineColorPresetSoftBlue => SoftBlueGuidelineColor,
                HoverColorsSettings.GuidelineColorPresetHighVisibility => HighVisibilityGuidelineColor,
                HoverColorsSettings.GuidelineColorPresetCustom => new Color(
                    Mathf.Clamp01(settings.GuidelineR),
                    Mathf.Clamp01(settings.GuidelineG),
                    Mathf.Clamp01(settings.GuidelineB),
                    1f),
                _ => CapturedVanillaGuidelineColor,
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
