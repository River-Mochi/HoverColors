// Copyright (c) River Mochi.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// File: Settings/Setting.cs
// Purpose: Defines Hover Colors settings, persistent storage, and the Options UI surface.
// Layout: 2 tabs (Actions, About) following CityWatchdog/EasyZoning convention.
// Note: the in-city panel settings are intentionally NOT decorated for Options UI — they are
// persisted here, read by cs2/api bindings, and applied by Systems/HoverColorsUISystem.cs.

namespace HoverColors.Settings
{
    using Colossal.IO.AssetDatabase; // FileLocation
    using Game.Input;       // BindingKeyboard
    using Game.Modding;     // IMod
    using Game.Settings;    // ModSetting, attributes
    using Game.UI;          // ProxyBinding
    using Game.UI.Widgets;  // Unit.kPercentage

    [FileLocation("ModsSettings/HoverColors/HoverColors")]
    [SettingsUITabOrder(Actions, About)]
    [SettingsUIGroupOrder(kToolColors, kPanel, kGuidelines, kKeyBindings, kAboutInfo, kAboutLinks, kAboutDedication)]
    [SettingsUIShowGroupName(kToolColors, kPanel, kKeyBindings, kGuidelines, kAboutDedication)]
    public partial class HoverColorsSettings : ModSetting
    {
        // Tab IDs
        internal const string Actions = nameof(Actions);
        internal const string About = nameof(About);

        // Group IDs
        internal const string kToolColors = nameof(kToolColors);
        internal const string kPanel = nameof(kPanel);
        internal const string kGuidelines = nameof(kGuidelines);
        internal const string kKeyBindings = nameof(kKeyBindings);
        internal const string kAboutInfo = nameof(kAboutInfo);
        internal const string kAboutLinks = nameof(kAboutLinks);
        internal const string kAboutDedication = nameof(kAboutDedication);

        public const int kToolColorModeRecommended = 0;
        public const int kToolColorModeVanilla = 1;
        public const int kToolColorModeCustom = 2;

        public const int kGuidelineColorPresetVanilla = 0;
        public const int kGuidelineColorPresetCustom = 4;

        public const int kGuidelineDashedColorPresetVanilla = 0;
        public const int kGuidelineDashedColorPresetYellow = 1;
        public const int kGuidelineDashedColorPresetGreen = 2;
        // Value 3 used to be an old test color; now it is Mochi Blue.
        public const int kGuidelineDashedColorPresetMochiBlue = 3;
        public const int kGuidelineDashedColorPresetCyanBlue = 4;
        public const int kGuidelineDashedColorPresetCustom = 5;

        // Centralized default for the guideline opacity slider.
        // Vanilla CS2 is 100; lower = more transparent. Keep TSX fallback bindings in sync.
        public const int kDefaultGuidelineOpacityPercent = 30;

        // Kept for old local test .coc files. The city icon now resets guidelines to mod defaults.
        public int GuidelineDefaultPercent { get; set; }

        private const string kAboutLinksRow = nameof(kAboutLinksRow);

        // Same Paradox URL pattern as CityWatchdog — lands on River-Mochi's author page filtered to CS2.
        private const string kUrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        // -----------------------------------------------------------------------
        // In-city color-picker bindings (driven by Systems/HoverColorsUISystem)
        // These are data fields the cs2/api bindings read/write
        // and the OutlineColorSystem applies. Field layout after the post-alpha redesign:
        //   - OutlineR/G/B → regular hover outline/fill color
        //   - OwnerR/G/B   → parent/owner highlight color, e.g. main building while placing sub-buildings
        //   - OutlineA     → outline halo edge opacity  (material _OuterColor.a)
        //   - FillA        → fill overlay opacity inside the silhouette (material _InnerColor.a)
        // The dropped OutlineInner*/OutlineOuter* fields from the early alpha are gone — their
        // saved values from the .coc file are ignored and replaced by SetDefaults() on next load.
        // -----------------------------------------------------------------------

        [SettingsUIHidden]
        public float OutlineR { get; set; }

        [SettingsUIHidden]
        public float OutlineG { get; set; }

        [SettingsUIHidden]
        public float OutlineB { get; set; }

        [SettingsUIHidden]
        public float OutlineA { get; set; }

        [SettingsUIHidden]
        public float OwnerR { get; set; }

        [SettingsUIHidden]
        public float OwnerG { get; set; }

        [SettingsUIHidden]
        public float OwnerB { get; set; }

        [SettingsUIHidden]
        public float OwnerA { get; set; }

        [SettingsUIHidden]
        public float FillA { get; set; }

        // District overlay color. Disabled by default so we do not touch vanilla/other-mod
        // district prefabs until the player picks a color from the in-game District picker.
        [SettingsUIHidden]
        public bool DistrictColorEnabled { get; set; }

        [SettingsUIHidden]
        public float DistrictR { get; set; }

        [SettingsUIHidden]
        public float DistrictG { get; set; }

        [SettingsUIHidden]
        public float DistrictB { get; set; }

        [SettingsUIHidden]
        public float DistrictA { get; set; }

        // -----------------------------------------------------------------------
        // Player-editable preset slots (slots 1 + 2 on the in-city panel)
        // Not decorated for Options UI. Each slot stores the same five values as the
        // live swatch (Outline RGBA + FillA). The panel's Save button overwrites a slot
        // with the current live color; the slot button applies it back. Persisted in the
        // .coc like the live values, so the in-city panel restores them after a reboot.
        // -----------------------------------------------------------------------

        [SettingsUIHidden]
        public float Preset1R { get; set; }

        [SettingsUIHidden]
        public float Preset1G { get; set; }

        [SettingsUIHidden]
        public float Preset1B { get; set; }

        [SettingsUIHidden]
        public float Preset1A { get; set; }

        [SettingsUIHidden]
        public float Preset1FillA { get; set; }

        [SettingsUIHidden]
        public float Preset2R { get; set; }

        [SettingsUIHidden]
        public float Preset2G { get; set; }

        [SettingsUIHidden]
        public float Preset2B { get; set; }

        [SettingsUIHidden]
        public float Preset2A { get; set; }

        [SettingsUIHidden]
        public float Preset2FillA { get; set; }

        // Guideline opacity saved per outline preset; guideline colors stay independent.
        [SettingsUIHidden]
        public int Preset1GuidelinePercent { get; set; }

        [SettingsUIHidden]
        public int Preset2GuidelinePercent { get; set; }

        // Large guide circles/spacing lines (GuideLineSettingsData Low + VeryLow).
        // Alpha is independent from the dashed alignment guideline opacity slider.
        [SettingsUIHidden]
        public float GuidelineLinesR { get; set; }

        [SettingsUIHidden]
        public float GuidelineLinesG { get; set; }

        [SettingsUIHidden]
        public float GuidelineLinesB { get; set; }

        [SettingsUIHidden]
        public float GuidelineLinesA { get; set; }

        // Road/tool preview overlay body (GuideLineSettingsData Medium).
        [SettingsUIHidden]
        public float GuidelinePreviewR { get; set; }

        [SettingsUIHidden]
        public float GuidelinePreviewG { get; set; }

        [SettingsUIHidden]
        public float GuidelinePreviewB { get; set; }

        [SettingsUIHidden]
        public float GuidelinePreviewA { get; set; }

        // Dashed alignment guide line color (GuideLineSettingsData High).
        // Opacity stays controlled by GuidelineOpacityPercent.
        [SettingsUIHidden]
        public int GuidelineDashedColorPreset { get; set; }

        [SettingsUIHidden]
        public float GuidelineDashedR { get; set; }

        [SettingsUIHidden]
        public float GuidelineDashedG { get; set; }

        [SettingsUIHidden]
        public float GuidelineDashedB { get; set; }

        // Guidelines icon toggle backup. First click saves S1/S2/S3 + opacity and applies vanilla;
        // next click restores those values.
        [SettingsUIHidden]
        public bool GuidelineVanillaToggleActive { get; set; }

        [SettingsUIHidden]
        public bool GuidelineVanillaToggleHasBackup { get; set; }

        [SettingsUIHidden]
        public int GuidelineBackupLinesColorPreset { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupLinesR { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupLinesG { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupLinesB { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupLinesA { get; set; }

        [SettingsUIHidden]
        public int GuidelineBackupPreviewColorPreset { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupPreviewR { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupPreviewG { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupPreviewB { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupPreviewA { get; set; }
        [SettingsUIHidden]
        public int GuidelineBackupDashedColorPreset { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupDashedR { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupDashedG { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupDashedB { get; set; }

        [SettingsUIHidden]
        public int GuidelineBackupOpacityPercent { get; set; }
        // In-city info button preference. Hidden from Options UI; persisted here so
        // "tooltips off" survives closing/reopening the panel and game restarts.
        [SettingsUIHidden]
        public bool PanelTooltipsEnabled { get; set; }

        // Hidden in-city preference for the Surface tool button/hotkey.
        // Default ON because creators mainly use this mod to see layered surfaces clearly.
        [SettingsUIHidden]
        public bool SurfaceToolAreasSuppressed { get; set; }

        // Hidden in-city preference for Specialized Industry area fill previews.
        // This is AreaTypeMask.Lots, so it must be handled with Surface in one system.
        [SettingsUIHidden]
        public bool SpecializedIndustryAreasSuppressed { get; set; }

        [SettingsUIHidden]
        public bool SpecializedIndustryAreasSuppressionInitialized { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Tool color behavior
        // -----------------------------------------------------------------------
        // This controls temporary effective colors only. It never overwrites the
        // player's saved swatch color in ModsSettings/HoverColors/HoverColors.coc.

        [SettingsUIDropdown(typeof(HoverColorsSettings), nameof(GetToolColorModeItems))]
        [SettingsUISection(Actions, kToolColors)]
        public int ToolColorMode { get; set; }

        [SettingsUISection(Actions, kToolColors)]
        public bool UseOverlapWarningColor { get; set; }

        // NetLanes cover EDT-style fences/hedges/markings through NetTool.
        // ON lets those detail tools use custom HC colors while normal roads keep road overrides.
        [SettingsUISection(Actions, kToolColors)]
        public bool UseCustomColorsForNetLanes { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Panel readability
        // -----------------------------------------------------------------------
        // User-facing label is "Darker panel". LegacyUI's extra transparency exposed
        // the need for this, but Modern UI players can use it too if they prefer
        // stronger panel contrast.

        [SettingsUISection(Actions, kPanel)]
        public bool UseDarkerPanel { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Guidelines
        // -----------------------------------------------------------------------
        // Only opacity stays in Options. Dashed guide color moved to the in-city panel
        // so players can use the same color picker pattern as the other guideline colors.

        [SettingsUIHidden]
        public int GuidelineLinesColorPreset { get; set; }

        [SettingsUIHidden]
        public int GuidelinePreviewColorPreset { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(Actions, kGuidelines)]
        public int GuidelineOpacityPercent { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Key bindings
        // -----------------------------------------------------------------------

        [SettingsUISection(Actions, kKeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.J, Mod.kTogglePanelActionName)]
        public ProxyBinding TogglePanelBinding { get; set; }

        [SettingsUISection(Actions, kKeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.L, Mod.kToggleSurfaceToolAreasActionName)]
        public ProxyBinding ToggleSurfaceToolAreasBinding { get; set; }

        [SettingsUISection(Actions, kKeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.K, Mod.kTogglePresetActionName)]
        public ProxyBinding TogglePresetBinding { get; set; }

        // -----------------------------------------------------------------------
        // About tab
        // -----------------------------------------------------------------------

        [SettingsUISection(About, kAboutInfo)]
        public string NameText => Mod.ModName;

        [SettingsUISection(About, kAboutInfo)]
        public string VersionText =>
#if DEBUG
            Mod.ModVersion + " (DEBUG)";
#else
            Mod.ModVersion;
#endif

        [SettingsUIButtonGroup(kAboutLinksRow)]
        [SettingsUIButton]
        [SettingsUISection(About, kAboutLinks)]
        public bool OpenParadox
        {
            set
            {
                if (value)
                {
                    TryOpenUrl(kUrlParadox);
                }
            }
        }

        [SettingsUISection(About, kAboutDedication)]
        public string MochiDedicationText => string.Empty;

        // -----------------------------------------------------------------------
        // Construction
        // -----------------------------------------------------------------------

        public HoverColorsSettings(IMod mod) : base(mod)
        {
            SetDefaults();
        }
    }
}
