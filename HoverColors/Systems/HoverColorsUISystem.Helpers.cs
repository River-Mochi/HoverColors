// File: Systems/HoverColorsUISystem.Helpers.cs
// Purpose: Shared comparison and preset/guideline helper methods for HoverColorsUISystem.

namespace HoverColors.UI
{
    using HoverColors.Systems;
    using System;

    public partial class HoverColorsUISystem
    {
        private void ApplySaveAndSync(HoverColorsSettings settings)
        {
            settings.ApplyAndSave();
            SyncValueBindings();
        }

        private static bool IsVanillaOutlineActive()
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null)
            {
                return false;
            }

            UnityEngine.Color hovered = OutlineColorSystem.CapturedHoveredColor;
            UnityEngine.Color owner = OutlineColorSystem.CapturedOwnerColor;
            return SameColor(settings.OutlineR, settings.OutlineG, settings.OutlineB, settings.OutlineA,
                    hovered.r, hovered.g, hovered.b, OutlineColorSystem.CapturedOutlineA)
                && SameColor(settings.OwnerR, settings.OwnerG, settings.OwnerB, settings.OwnerA,
                    owner.r, owner.g, owner.b, owner.a);
        }

        // True when the live swatch exactly matches what's stored in that slot.
        private static bool IsPresetActive(int slot)
        {
            HoverColorsSettings? s = Mod.Settings;
            if (s == null) return false;

            if (slot == 1)
            {
                return SameColor(s.OutlineR, s.OutlineG, s.OutlineB, s.OutlineA,
                        s.Preset1R, s.Preset1G, s.Preset1B, s.Preset1A)
                    && ApproxEqual(s.FillA, s.Preset1FillA);
            }

            if (slot == 2)
            {
                return SameColor(s.OutlineR, s.OutlineG, s.OutlineB, s.OutlineA,
                        s.Preset2R, s.Preset2G, s.Preset2B, s.Preset2A)
                    && ApproxEqual(s.FillA, s.Preset2FillA);
            }

            return false;
        }

        private static bool TryGetPresetTargets(
            HoverColorsSettings settings,
            int slot,
            out float targetR,
            out float targetG,
            out float targetB,
            out float targetA,
            out float targetFillA,
            out int targetGuidelinePercent)
        {
            if (slot == 1)
            {
                targetR = settings.Preset1R;
                targetG = settings.Preset1G;
                targetB = settings.Preset1B;
                targetA = settings.Preset1A;
                targetFillA = settings.Preset1FillA;
                targetGuidelinePercent = settings.Preset1GuidelinePercent;
                return true;
            }

            if (slot == 2)
            {
                targetR = settings.Preset2R;
                targetG = settings.Preset2G;
                targetB = settings.Preset2B;
                targetA = settings.Preset2A;
                targetFillA = settings.Preset2FillA;
                targetGuidelinePercent = settings.Preset2GuidelinePercent;
                return true;
            }

            targetR = 0f;
            targetG = 0f;
            targetB = 0f;
            targetA = 0f;
            targetFillA = 0f;
            targetGuidelinePercent = HoverColorsSettings.DefaultGuidelineOpacityPercent;
            return false;
        }

        private static bool ApproxEqual(float a, float b) => Math.Abs(a - b) < 0.0005f;

        private static bool SameColor(
            float r1, float g1, float b1, float a1,
            float r2, float g2, float b2, float a2)
        {
            return ApproxEqual(r1, r2)
                && ApproxEqual(g1, g2)
                && ApproxEqual(b1, b2)
                && ApproxEqual(a1, a2);
        }

        private static float Clamp01(float value) => Math.Max(0f, Math.Min(1f, value));

        private static bool PresetsAtDefaults(HoverColorsSettings s)
        {
            return ApproxEqual(s.Preset1R, DefaultPreset1R) && ApproxEqual(s.Preset1G, DefaultPreset1G)
                && ApproxEqual(s.Preset1B, DefaultPreset1B) && ApproxEqual(s.Preset1A, DefaultPreset1A)
                && ApproxEqual(s.Preset1FillA, DefaultPreset1FillA)
                && s.Preset1GuidelinePercent == HoverColorsSettings.DefaultGuidelineOpacityPercent
                && ApproxEqual(s.Preset2R, DefaultPreset2R) && ApproxEqual(s.Preset2G, DefaultPreset2G)
                && ApproxEqual(s.Preset2B, DefaultPreset2B) && ApproxEqual(s.Preset2A, DefaultPreset2A)
                && ApproxEqual(s.Preset2FillA, DefaultPreset2FillA)
                && s.Preset2GuidelinePercent == HoverColorsSettings.DefaultGuidelineOpacityPercent;
        }

        private static void SaveGuidelineToggleBackup(HoverColorsSettings settings)
        {
            settings.GuidelineBackupLinesColorPreset = settings.GuidelineLinesColorPreset;
            settings.GuidelineBackupLinesR = settings.GuidelineLinesR;
            settings.GuidelineBackupLinesG = settings.GuidelineLinesG;
            settings.GuidelineBackupLinesB = settings.GuidelineLinesB;
            settings.GuidelineBackupLinesA = settings.GuidelineLinesA;
            settings.GuidelineBackupPreviewColorPreset = settings.GuidelinePreviewColorPreset;
            settings.GuidelineBackupPreviewR = settings.GuidelinePreviewR;
            settings.GuidelineBackupPreviewG = settings.GuidelinePreviewG;
            settings.GuidelineBackupPreviewB = settings.GuidelinePreviewB;
            settings.GuidelineBackupPreviewA = settings.GuidelinePreviewA;
            settings.GuidelineVanillaToggleHasBackup = true;
        }

        private static void RestoreGuidelineToggleBackup(HoverColorsSettings settings)
        {
            settings.GuidelineLinesColorPreset = settings.GuidelineBackupLinesColorPreset;
            settings.GuidelineLinesR = settings.GuidelineBackupLinesR;
            settings.GuidelineLinesG = settings.GuidelineBackupLinesG;
            settings.GuidelineLinesB = settings.GuidelineBackupLinesB;
            settings.GuidelineLinesA = settings.GuidelineBackupLinesA;
            settings.GuidelinePreviewColorPreset = settings.GuidelineBackupPreviewColorPreset;
            settings.GuidelinePreviewR = settings.GuidelineBackupPreviewR;
            settings.GuidelinePreviewG = settings.GuidelinePreviewG;
            settings.GuidelinePreviewB = settings.GuidelinePreviewB;
            settings.GuidelinePreviewA = settings.GuidelineBackupPreviewA;
        }

        private static void ApplyVanillaGuidelineSwatches(HoverColorsSettings settings)
        {
            UnityEngine.Color lines = GuidelineColorSystem.CapturedVanillaGuidelineLinesColor;
            UnityEngine.Color preview = GuidelineColorSystem.CapturedVanillaGuidelinePreviewColor;
            settings.GuidelineLinesColorPreset = HoverColorsSettings.GuidelineColorPresetVanilla;
            settings.GuidelineLinesR = lines.r;
            settings.GuidelineLinesG = lines.g;
            settings.GuidelineLinesB = lines.b;
            settings.GuidelineLinesA = 1f;
            settings.GuidelinePreviewColorPreset = HoverColorsSettings.GuidelineColorPresetVanilla;
            settings.GuidelinePreviewR = preview.r;
            settings.GuidelinePreviewG = preview.g;
            settings.GuidelinePreviewB = preview.b;
            settings.GuidelinePreviewA = 1f;
        }
    }
}
