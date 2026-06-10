// Copyright (c) River Mochi.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// File: Settings/Setting.Defaults.cs
// Purpose: Defaults and one-time migration helpers for HoverColorsSettings.

namespace HoverColors.Settings
{
    public partial class HoverColorsSettings
    {
        public override void SetDefaults()
        {
            // Vanilla cyan-blue from the OutlinesWorldUIPass material defaults.
            OutlineR = 0.502f;
            OutlineG = 0.869f;
            OutlineB = 1f;
            OutlineA = 0.855f;

            // Vanilla parent/owner green used for sub-building placement and owned objects.
            OwnerR = 0.247f;
            OwnerG = 0.981f;
            OwnerB = 0.247f;
            OwnerA = 0.702f;

            // FillA=0 matches vanilla CS2: no extra silhouette overlay until the player turns it up.
            FillA = 0f;

            // Safe fallback for the District picker until DistrictColorSystem captures the authored
            // default district prefab colors. Not applied unless DistrictColorEnabled is true.
            DistrictColorEnabled = false;
            DistrictR = 128f / 255f;
            DistrictG = 128f / 255f;
            DistrictB = 128f / 255f;
            DistrictA = 64f / 255f;

            // Starter presets (players can overwrite these with the panel's Save button).
            // Slot 1 = Mochi's gentle gray-purple. Slot 2 = yenyang-inspired purple-gray.
            Preset1R = 140f / 255f;
            Preset1G = 140f / 255f;
            Preset1B = 171f / 255f;
            Preset1A = 0.5f;
            Preset1FillA = 0f;

            Preset2R = 0.25f;
            Preset2G = 0.15f;
            Preset2B = 0.25f;
            Preset2A = 0.5f;
            Preset2FillA = 0f;

            Preset1GuidelinePercent = kDefaultGuidelineOpacityPercent;
            Preset2GuidelinePercent = kDefaultGuidelineOpacityPercent;
            GuidelineDefaultPercent = kDefaultGuidelineOpacityPercent;

            GuidelineLinesColorPreset = kGuidelineColorPresetVanilla;
            GuidelineLinesR = 0.7f;
            GuidelineLinesG = 0.7f;
            GuidelineLinesB = 1f;
            GuidelineLinesA = 1f;

            GuidelinePreviewColorPreset = kGuidelineColorPresetVanilla;
            GuidelinePreviewR = 0.7f;
            GuidelinePreviewG = 0.7f;
            GuidelinePreviewB = 1f;
            GuidelinePreviewA = 1f;

            GuidelineDashedColorPreset = kGuidelineDashedColorPresetVanilla;
            GuidelineDashedR = 0.7f;
            GuidelineDashedG = 0.7f;
            GuidelineDashedB = 1f;

            GuidelineVanillaToggleActive = false;
            GuidelineVanillaToggleHasBackup = false;
            GuidelineBackupLinesColorPreset = kGuidelineColorPresetVanilla;
            GuidelineBackupLinesR = GuidelineLinesR;
            GuidelineBackupLinesG = GuidelineLinesG;
            GuidelineBackupLinesB = GuidelineLinesB;
            GuidelineBackupLinesA = GuidelineLinesA;
            GuidelineBackupPreviewColorPreset = kGuidelineColorPresetVanilla;
            GuidelineBackupPreviewR = GuidelinePreviewR;
            GuidelineBackupPreviewG = GuidelinePreviewG;
            GuidelineBackupPreviewB = GuidelinePreviewB;
            GuidelineBackupPreviewA = GuidelinePreviewA;

            PanelTooltipsEnabled = true;
            SurfaceToolAreasSuppressed = true;
            SpecializedIndustryAreasSuppressed = true;
            SpecializedIndustryAreasSuppressionInitialized = true;

            // Release default: help players see demolition/road targets even if their custom
            // alpha is very low, without changing their saved custom color.
            ToolColorMode = kToolColorModeRecommended;
            UseOverlapWarningColor = true;
            UseCustomColorsForNetLanes = true;
            UseDarkerPanel = false;

            // 100 = vanilla default. Lower = more transparent guidelines.
            GuidelineOpacityPercent = kDefaultGuidelineOpacityPercent;
        }

        public void MigrateAfterLoad()
        {
            if (!SpecializedIndustryAreasSuppressionInitialized)
            {
                SpecializedIndustryAreasSuppressed = true;
                SpecializedIndustryAreasSuppressionInitialized = true;
                ApplyAndSave();
            }
        }
    }
}
