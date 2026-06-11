// File: Systems/HoverColorsUISystem.Triggers.cs
// Purpose: TriggerBinding registration and C#-side guards for in-city panel actions.

namespace HoverColors.UI
{
    using Colossal.UI.Binding;
    using HoverColors.Settings;
    using HoverColors.Systems;
    using System;

    public partial class HoverColorsUISystem
    {
        private void RegisterTriggerBindings()
        {
            AddBinding(new TriggerBinding<float, float, float, float>(Mod.ModId, "SetOutlineColor", SetOutlineColor));
            AddBinding(new TriggerBinding<float, float, float, float>(Mod.ModId, "SetOwnerColor", SetOwnerColor));
            AddBinding(new TriggerBinding<float>(Mod.ModId, "SetFillAlpha", SetFillAlpha));
            AddBinding(new TriggerBinding<int>(Mod.ModId, "SetGuidelineOpacity", SetGuidelineOpacity));
            AddBinding(new TriggerBinding<float, float, float, float>(Mod.ModId, "SetGuidelineLinesColor", SetGuidelineLinesColor));
            AddBinding(new TriggerBinding<float, float, float, float>(Mod.ModId, "SetGuidelinePreviewColor", SetGuidelinePreviewColor));
            AddBinding(new TriggerBinding<float, float, float, float>(Mod.ModId, "SetGuidelineDashedColor", SetGuidelineDashedColor));
            AddBinding(new TriggerBinding<float, float, float, float>(Mod.ModId, "SetDistrictColor", SetDistrictColor));
            AddBinding(new TriggerBinding(Mod.ModId, "ResetDistrictToVanilla", ResetDistrictToVanilla));
            AddBinding(new TriggerBinding(Mod.ModId, "ResetToVanilla", ResetToVanilla));
            AddBinding(new TriggerBinding(Mod.ModId, "ResetOutlineToVanilla", ResetOutlineToVanilla));
            AddBinding(new TriggerBinding<bool>(Mod.ModId, "SetPanelOpen", SetPanelOpen));
            AddBinding(new TriggerBinding<bool>(Mod.ModId, "SetPanelTooltipsEnabled", SetPanelTooltipsEnabled));
            AddBinding(new TriggerBinding(Mod.ModId, "ToggleSurfaceToolAreas", ToggleSurfaceToolAreas));
            AddBinding(new TriggerBinding(Mod.ModId, "ToggleSpecializedIndustryAreas", ToggleSpecializedIndustryAreas));
            AddBinding(new TriggerBinding<int>(Mod.ModId, "ApplyPreset", ApplyPreset));
            AddBinding(new TriggerBinding<int>(Mod.ModId, "SavePreset", SavePreset));
            AddBinding(new TriggerBinding(Mod.ModId, "TogglePresetDefaults", TogglePresetDefaults));
            AddBinding(new TriggerBinding(Mod.ModId, "ResetGuidelines", ResetGuidelines));
        }

        private void SetOutlineColor(float r, float g, float b, float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            if (SameColor(settings.OutlineR, settings.OutlineG, settings.OutlineB, settings.OutlineA, r, g, b, a))
            {
                return;
            }

            settings.OutlineR = r;
            settings.OutlineG = g;
            settings.OutlineB = b;
            settings.OutlineA = a;
            ApplySaveAndSync(settings);
        }

        private void SetOwnerColor(float r, float g, float b, float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            if (SameColor(settings.OwnerR, settings.OwnerG, settings.OwnerB, settings.OwnerA, r, g, b, a))
            {
                return;
            }

            settings.OwnerR = r;
            settings.OwnerG = g;
            settings.OwnerB = b;
            settings.OwnerA = a;
            ApplySaveAndSync(settings);
        }

        private void SetFillAlpha(float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            a = Clamp01(a);
            if (ApproxEqual(settings.FillA, a))
            {
                return;
            }

            settings.FillA = a;
            ApplySaveAndSync(settings);
        }

        private void SetGuidelineOpacity(int percent)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            percent = Math.Max(0, Math.Min(100, percent));
            if (settings.GuidelineOpacityPercent == percent)
            {
                return;
            }

            settings.GuidelineOpacityPercent = percent;
            ApplySaveAndSync(settings);
        }

        private void SetGuidelineLinesColor(float r, float g, float b, float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            r = Clamp01(r);
            g = Clamp01(g);
            b = Clamp01(b);
            a = Clamp01(a);

            bool changed = settings.GuidelineLinesColorPreset != HoverColorsSettings.kGuidelineColorPresetCustom
                || settings.GuidelineVanillaToggleActive
                || !SameColor(settings.GuidelineLinesR, settings.GuidelineLinesG, settings.GuidelineLinesB, settings.GuidelineLinesA, r, g, b, a);

            if (!changed)
            {
                return;
            }

            settings.GuidelineLinesColorPreset = HoverColorsSettings.kGuidelineColorPresetCustom;
            settings.GuidelineLinesR = r;
            settings.GuidelineLinesG = g;
            settings.GuidelineLinesB = b;
            settings.GuidelineLinesA = a;
            settings.GuidelineVanillaToggleActive = false;
            ApplySaveAndSync(settings);
        }

        private void SetGuidelinePreviewColor(float r, float g, float b, float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            r = Clamp01(r);
            g = Clamp01(g);
            b = Clamp01(b);
            a = Clamp01(a);

            bool changed = settings.GuidelinePreviewColorPreset != HoverColorsSettings.kGuidelineColorPresetCustom
                || settings.GuidelineVanillaToggleActive
                || !SameColor(settings.GuidelinePreviewR, settings.GuidelinePreviewG, settings.GuidelinePreviewB, settings.GuidelinePreviewA, r, g, b, a);

            if (!changed)
            {
                return;
            }

            settings.GuidelinePreviewColorPreset = HoverColorsSettings.kGuidelineColorPresetCustom;
            settings.GuidelinePreviewR = r;
            settings.GuidelinePreviewG = g;
            settings.GuidelinePreviewB = b;
            settings.GuidelinePreviewA = a;
            settings.GuidelineVanillaToggleActive = false;
            ApplySaveAndSync(settings);
        }


        private void SetGuidelineDashedColor(float r, float g, float b, float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            r = Clamp01(r);
            g = Clamp01(g);
            b = Clamp01(b);
            a = Clamp01(a);
            int percent = Math.Max(0, Math.Min(100, (int)(Math.Round(a * 20f) * 5f)));

            bool changed = settings.GuidelineDashedColorPreset != HoverColorsSettings.kGuidelineDashedColorPresetCustom
                || settings.GuidelineVanillaToggleActive
                || !ApproxEqual(settings.GuidelineDashedR, r)
                || !ApproxEqual(settings.GuidelineDashedG, g)
                || !ApproxEqual(settings.GuidelineDashedB, b)
                || settings.GuidelineOpacityPercent != percent;

            if (!changed)
            {
                return;
            }

            // Dashed swatch alpha and the guideline opacity slider represent the same value.
            settings.GuidelineDashedColorPreset = HoverColorsSettings.kGuidelineDashedColorPresetCustom;
            settings.GuidelineDashedR = r;
            settings.GuidelineDashedG = g;
            settings.GuidelineDashedB = b;
            settings.GuidelineOpacityPercent = percent;
            settings.GuidelineVanillaToggleActive = false;
            ApplySaveAndSync(settings);
        }
        private void SetDistrictColor(float r, float g, float b, float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            r = Clamp01(r);
            g = Clamp01(g);
            b = Clamp01(b);
            a = Clamp01(a);

            bool changed = !settings.DistrictColorEnabled
                || !SameColor(settings.DistrictR, settings.DistrictG, settings.DistrictB, settings.DistrictA, r, g, b, a);

            if (!changed)
            {
                return;
            }

            settings.DistrictColorEnabled = true;
            settings.DistrictR = r;
            settings.DistrictG = g;
            settings.DistrictB = b;
            settings.DistrictA = a;
            ApplySaveAndSync(settings);
        }

        private void ResetDistrictToVanilla()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            UnityEngine.Color district = DistrictColorSystem.CapturedDistrictFillColor;
            bool changed = settings.DistrictColorEnabled
                || !SameColor(settings.DistrictR, settings.DistrictG, settings.DistrictB, settings.DistrictA,
                    district.r, district.g, district.b, district.a);

            if (!changed)
            {
                return;
            }

            settings.DistrictColorEnabled = false;
            settings.DistrictR = district.r;
            settings.DistrictG = district.g;
            settings.DistrictB = district.b;
            settings.DistrictA = district.a;
            ApplySaveAndSync(settings);
        }

        private void ResetToVanilla()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            UnityEngine.Color hovered = OutlineColorSystem.CapturedHoveredColor;
            UnityEngine.Color owner = OutlineColorSystem.CapturedOwnerColor;
            float fillA = OutlineColorSystem.CapturedFillA;

            bool changed = !SameColor(settings.OutlineR, settings.OutlineG, settings.OutlineB, settings.OutlineA,
                    hovered.r, hovered.g, hovered.b, OutlineColorSystem.CapturedOutlineA)
                || !SameColor(settings.OwnerR, settings.OwnerG, settings.OwnerB, settings.OwnerA,
                    owner.r, owner.g, owner.b, owner.a)
                || !ApproxEqual(settings.FillA, fillA);

            if (!changed)
            {
                return;
            }

            settings.OutlineR = hovered.r;
            settings.OutlineG = hovered.g;
            settings.OutlineB = hovered.b;
            settings.OutlineA = OutlineColorSystem.CapturedOutlineA;
            settings.OwnerR = owner.r;
            settings.OwnerG = owner.g;
            settings.OwnerB = owner.b;
            settings.OwnerA = owner.a;
            settings.FillA = fillA;
            ApplySaveAndSync(settings);
        }

        private void ResetOutlineToVanilla()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            UnityEngine.Color hovered = OutlineColorSystem.CapturedHoveredColor;
            UnityEngine.Color owner = OutlineColorSystem.CapturedOwnerColor;

            bool changed = !SameColor(settings.OutlineR, settings.OutlineG, settings.OutlineB, settings.OutlineA,
                    hovered.r, hovered.g, hovered.b, OutlineColorSystem.CapturedOutlineA)
                || !SameColor(settings.OwnerR, settings.OwnerG, settings.OwnerB, settings.OwnerA,
                    owner.r, owner.g, owner.b, owner.a);

            if (!changed)
            {
                return;
            }

            settings.OutlineR = hovered.r;
            settings.OutlineG = hovered.g;
            settings.OutlineB = hovered.b;
            settings.OutlineA = OutlineColorSystem.CapturedOutlineA;
            settings.OwnerR = owner.r;
            settings.OwnerG = owner.g;
            settings.OwnerB = owner.b;
            settings.OwnerA = owner.a;
            ApplySaveAndSync(settings);
        }

        private void SetPanelTooltipsEnabled(bool enabled)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null || settings.PanelTooltipsEnabled == enabled)
            {
                return;
            }

            settings.PanelTooltipsEnabled = enabled;
            ApplySaveAndSync(settings);
        }

        private void ToggleSurfaceToolAreas()
        {
            bool enabled = !AreaToolOverlaySystem.SuppressSurfaceToolAreas;
            AreaToolOverlaySystem.SetSurfaceSuppression(enabled);

            HoverColorsSettings? settings = Mod.Settings;
            if (settings != null && settings.SurfaceToolAreasSuppressed != enabled)
            {
                settings.SurfaceToolAreasSuppressed = enabled;
                ApplySaveAndSync(settings);
            }
            else
            {
                UpdateIfChanged(m_SurfaceToolAreasSuppressedBinding, AreaToolOverlaySystem.SuppressSurfaceToolAreas);
            }
        }

        private void ToggleSpecializedIndustryAreas()
        {
            bool enabled = !AreaToolOverlaySystem.SuppressSpecializedIndustryToolAreas;
            AreaToolOverlaySystem.SetSpecializedIndustrySuppression(enabled);

            HoverColorsSettings? settings = Mod.Settings;
            if (settings != null
                && (settings.SpecializedIndustryAreasSuppressed != enabled
                    || !settings.SpecializedIndustryAreasSuppressionInitialized))
            {
                settings.SpecializedIndustryAreasSuppressed = enabled;
                settings.SpecializedIndustryAreasSuppressionInitialized = true;
                ApplySaveAndSync(settings);
            }
            else
            {
                UpdateIfChanged(m_SpecializedIndustryAreasSuppressedBinding, AreaToolOverlaySystem.SuppressSpecializedIndustryToolAreas);
            }
        }

        private void ApplyPreset(int slot)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            if (!TryGetPresetTargets(settings, slot,
                    out float targetR,
                    out float targetG,
                    out float targetB,
                    out float targetA,
                    out float targetFillA,
                    out int targetGuidelinePercent))
            {
                return;
            }

            bool changed = !SameColor(settings.OutlineR, settings.OutlineG, settings.OutlineB, settings.OutlineA,
                    targetR, targetG, targetB, targetA)
                || !ApproxEqual(settings.FillA, targetFillA)
                || settings.GuidelineOpacityPercent != targetGuidelinePercent;

            if (!changed)
            {
                return;
            }

            settings.OutlineR = targetR;
            settings.OutlineG = targetG;
            settings.OutlineB = targetB;
            settings.OutlineA = targetA;
            settings.FillA = targetFillA;
            settings.GuidelineOpacityPercent = targetGuidelinePercent;
            ApplySaveAndSync(settings);
        }

        private void SavePreset(int slot)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            if (slot == 1)
            {
                bool changed = !SameColor(settings.Preset1R, settings.Preset1G, settings.Preset1B, settings.Preset1A,
                        settings.OutlineR, settings.OutlineG, settings.OutlineB, settings.OutlineA)
                    || !ApproxEqual(settings.Preset1FillA, settings.FillA)
                    || settings.Preset1GuidelinePercent != settings.GuidelineOpacityPercent;

                if (!changed)
                {
                    return;
                }

                settings.Preset1R = settings.OutlineR;
                settings.Preset1G = settings.OutlineG;
                settings.Preset1B = settings.OutlineB;
                settings.Preset1A = settings.OutlineA;
                settings.Preset1FillA = settings.FillA;
                settings.Preset1GuidelinePercent = settings.GuidelineOpacityPercent;
            }
            else if (slot == 2)
            {
                bool changed = !SameColor(settings.Preset2R, settings.Preset2G, settings.Preset2B, settings.Preset2A,
                        settings.OutlineR, settings.OutlineG, settings.OutlineB, settings.OutlineA)
                    || !ApproxEqual(settings.Preset2FillA, settings.FillA)
                    || settings.Preset2GuidelinePercent != settings.GuidelineOpacityPercent;

                if (!changed)
                {
                    return;
                }

                settings.Preset2R = settings.OutlineR;
                settings.Preset2G = settings.OutlineG;
                settings.Preset2B = settings.OutlineB;
                settings.Preset2A = settings.OutlineA;
                settings.Preset2FillA = settings.FillA;
                settings.Preset2GuidelinePercent = settings.GuidelineOpacityPercent;
            }
            else
            {
                return;
            }

            ApplySaveAndSync(settings);
        }

        private void TogglePresetDefaults()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            bool changed;
            if (!PresetsAtDefaults(settings))
            {
                // Save current player colors then apply defaults.
                m_BkP1R = settings.Preset1R; m_BkP1G = settings.Preset1G; m_BkP1B = settings.Preset1B;
                m_BkP1A = settings.Preset1A; m_BkP1FillA = settings.Preset1FillA; m_BkP1Guideline = settings.Preset1GuidelinePercent;
                m_BkP2R = settings.Preset2R; m_BkP2G = settings.Preset2G; m_BkP2B = settings.Preset2B;
                m_BkP2A = settings.Preset2A; m_BkP2FillA = settings.Preset2FillA; m_BkP2Guideline = settings.Preset2GuidelinePercent;
                m_PresetBackupExists = true;

                settings.Preset1R = DefaultPreset1R; settings.Preset1G = DefaultPreset1G; settings.Preset1B = DefaultPreset1B;
                settings.Preset1A = DefaultPreset1A; settings.Preset1FillA = DefaultPreset1FillA;
                settings.Preset1GuidelinePercent = HoverColorsSettings.kDefaultGuidelineOpacityPercent;
                settings.Preset2R = DefaultPreset2R; settings.Preset2G = DefaultPreset2G; settings.Preset2B = DefaultPreset2B;
                settings.Preset2A = DefaultPreset2A; settings.Preset2FillA = DefaultPreset2FillA;
                settings.Preset2GuidelinePercent = HoverColorsSettings.kDefaultGuidelineOpacityPercent;
                changed = true;
            }
            else if (m_PresetBackupExists)
            {
                // Restore backup.
                settings.Preset1R = m_BkP1R; settings.Preset1G = m_BkP1G; settings.Preset1B = m_BkP1B;
                settings.Preset1A = m_BkP1A; settings.Preset1FillA = m_BkP1FillA; settings.Preset1GuidelinePercent = m_BkP1Guideline;
                settings.Preset2R = m_BkP2R; settings.Preset2G = m_BkP2G; settings.Preset2B = m_BkP2B;
                settings.Preset2A = m_BkP2A; settings.Preset2FillA = m_BkP2FillA; settings.Preset2GuidelinePercent = m_BkP2Guideline;
                m_PresetBackupExists = false;
                changed = true;
            }
            else
            {
                // Already at defaults with no backup -> no-op.
                changed = false;
            }

            if (!changed)
            {
                return;
            }

            ApplySaveAndSync(settings);
        }

        private void ResetGuidelines()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            if (settings.GuidelineVanillaToggleActive && settings.GuidelineVanillaToggleHasBackup)
            {
                RestoreGuidelineToggleBackup(settings);
                settings.GuidelineVanillaToggleActive = false;
            }
            else
            {
                SaveGuidelineToggleBackup(settings);
                ApplyVanillaGuidelineSwatches(settings);
                settings.GuidelineVanillaToggleActive = true;
            }

            ApplySaveAndSync(settings);
        }

        private void ApplyPresetSlot(int slot)
        {
            ApplyPreset(slot);
        }
    }
}
