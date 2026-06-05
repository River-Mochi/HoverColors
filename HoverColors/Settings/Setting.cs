// File: Settings/Setting.cs
// Purpose: Defines Hover Colors settings, persistent storage, and the Options UI surface.
// Layout: 2 tabs (Actions, About) following CityWatchdog/EasyZoning convention.
// Note: the in-city panel settings are intentionally NOT decorated for Options UI — they are
// persisted here, read by cs2/api bindings, and applied by Systems/HoverColorsUISystem.cs.

namespace HoverColors.Settings
{
    using Colossal.IO.AssetDatabase;
    using CS2Shared.RiverMochi;
    using Game.Input;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Game.UI.Widgets;
    using System;
    using UnityEngine;

    [FileLocation("ModsSettings/HoverColors/HoverColors")]
    [SettingsUITabOrder(Actions, About)]
    [SettingsUIGroupOrder(ToolColors, Panel, Guidelines, KeyBindings, AboutInfo, AboutLinks, AboutDedication)]
    [SettingsUIShowGroupName(ToolColors, Panel, KeyBindings, Guidelines, AboutDedication)]
    public class HoverColorsSettings : ModSetting
    {
        // Tab IDs
        internal const string Actions = nameof(Actions);
        internal const string About = nameof(About);

        // Group IDs
        internal const string ToolColors = nameof(ToolColors);
        internal const string Panel = nameof(Panel);
        internal const string Guidelines = nameof(Guidelines);
        internal const string KeyBindings = nameof(KeyBindings);
        internal const string AboutInfo = nameof(AboutInfo);
        internal const string AboutLinks = nameof(AboutLinks);
        internal const string AboutDedication = nameof(AboutDedication);

        public const int ToolColorModeRecommended = 0;
        public const int ToolColorModeVanilla = 1;
        public const int ToolColorModeCustom = 2;

        public const int GuidelineColorPresetVanilla = 0;
        public const int GuidelineColorPresetCustom = 4;

        public const int GuidelineDashedColorPresetVanilla = 0;
        public const int GuidelineDashedColorPresetYellow = 1;
        public const int GuidelineDashedColorPresetGreen = 2;
        public const int GuidelineDashedColorPresetPink = 3;

        // Centralised default for the guideline opacity slider.
        // Vanilla CS2 is 100; lower = more transparent. Keep TSX fallback bindings in sync.
        public const int DefaultGuidelineOpacityPercent = 30;

        // Kept for old local test .coc files. The city icon now resets guidelines to mod defaults.
        public int GuidelineDefaultPercent { get; set; }

        private const string AboutLinksRow = nameof(AboutLinksRow);
        // Same Paradox URL pattern as CityWatchdog — lands on River-Mochi's author page filtered to CS2.
        private const string UrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        // -----------------------------------------------------------------------
        // In-city color-picker bindings (driven by Systems/HoverColorsUISystem)
        // Not decorated for Options UI — these are data fields the cs2/api bindings read/write
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

        // In-city info button preference. Hidden from Options UI; persisted here so
        // "tooltips off" survives closing/reopening the panel and game restarts.
        [SettingsUIHidden]
        public bool PanelTooltipsEnabled { get; set; }

        // Hidden in-city preference for the Surface tool button/hotkey.
        // Default ON because creators mainly use this mod to see layered surfaces clearly.
        [SettingsUIHidden]
        public bool SurfaceToolAreasSuppressed { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Tool color behavior
        // -----------------------------------------------------------------------
        // This controls temporary effective colors only. It never overwrites the
        // player's saved swatch color in ModsSettings/HoverColors/HoverColors.coc.

        [SettingsUIDropdown(typeof(HoverColorsSettings), nameof(GetToolColorModeItems))]
        [SettingsUISection(Actions, ToolColors)]
        public int ToolColorMode { get; set; }

        [SettingsUISection(Actions, ToolColors)]
        public bool UseOverlapWarningColor { get; set; }

        // NetLanes cover EDT-style fences/hedges/markings through NetTool.
        // ON lets those detail tools use custom HC colors while normal roads keep road overrides.
        [SettingsUISection(Actions, ToolColors)]
        public bool UseCustomColorsForNetLanes { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Panel readability
        // -----------------------------------------------------------------------
        // User-facing label is "Darker panel". LegacyUI's extra transparency exposed
        // the need for this, but Modern UI players can use it too if they prefer
        // stronger panel contrast.

        [SettingsUISection(Actions, Panel)]
        public bool UseDarkerPanel { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Guidelines
        // -----------------------------------------------------------------------
        // Slider is 0..100 (percent) so the SettingsUISlider can use kPercentage units.
        // GuidelineColorSystem divides by 100 and multiplies the game's default per-priority
        // alphas, so 100 = no change, 50 = half as visible, 0 = fully invisible guidelines.

        [SettingsUIHidden]
        public int GuidelineLinesColorPreset { get; set; }

        [SettingsUIHidden]
        public int GuidelinePreviewColorPreset { get; set; }

        [SettingsUIDropdown(typeof(HoverColorsSettings), nameof(GetGuidelineDashedColorPresetItems))]
        [SettingsUISection(Actions, Guidelines)]
        public int GuidelineDashedColorPreset { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(Actions, Guidelines)]
        public int GuidelineOpacityPercent { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Key bindings
        // -----------------------------------------------------------------------

        [SettingsUISection(Actions, KeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.J, Mod.kTogglePanelActionName)]
        public ProxyBinding TogglePanelBinding { get; set; }

        [SettingsUISection(Actions, KeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.L, Mod.kToggleSurfaceToolAreasActionName)]
        public ProxyBinding ToggleSurfaceToolAreasBinding { get; set; }

        [SettingsUISection(Actions, KeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.K, Mod.kTogglePresetActionName)]
        public ProxyBinding TogglePresetBinding { get; set; }

        // -----------------------------------------------------------------------
        // About tab
        // -----------------------------------------------------------------------

        [SettingsUISection(About, AboutInfo)]
        public string NameText => Mod.ModName;

        [SettingsUISection(About, AboutInfo)]
        public string VersionText =>
#if DEBUG
            Mod.ModVersion + " (DEBUG)";
#else
            Mod.ModVersion;
#endif

        [SettingsUIButtonGroup(AboutLinksRow)]
        [SettingsUIButton]
        [SettingsUISection(About, AboutLinks)]
        public bool OpenParadox
        {
            set
            {
                if (value)
                {
                    TryOpenUrl(UrlParadox);
                }
            }
        }

        [SettingsUISection(About, AboutDedication)]
        public string MochiDedicationText => string.Empty;

        // -----------------------------------------------------------------------
        // Construction + defaults
        // -----------------------------------------------------------------------

        public HoverColorsSettings(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        public override void SetDefaults()
        {
            // Vanilla cyan-blue from the OutlinesWorldUIPass material defaults
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
            Preset1GuidelinePercent = DefaultGuidelineOpacityPercent;
            Preset2GuidelinePercent = DefaultGuidelineOpacityPercent;
            GuidelineDefaultPercent = DefaultGuidelineOpacityPercent;
            GuidelineLinesColorPreset = GuidelineColorPresetVanilla;
            GuidelineLinesR = 0.7f;
            GuidelineLinesG = 0.7f;
            GuidelineLinesB = 1f;
            GuidelineLinesA = 1f;
            GuidelinePreviewColorPreset = GuidelineColorPresetVanilla;
            GuidelinePreviewR = 0.7f;
            GuidelinePreviewG = 0.7f;
            GuidelinePreviewB = 1f;
            GuidelinePreviewA = 1f;
            GuidelineDashedColorPreset = GuidelineDashedColorPresetVanilla;
            PanelTooltipsEnabled = true;
            SurfaceToolAreasSuppressed = true;

            // Release default: help players see demolition/road targets even if their custom
            // alpha is very low, without changing their saved custom color.
            ToolColorMode = ToolColorModeRecommended;
            UseOverlapWarningColor = true;
            UseCustomColorsForNetLanes = true;
            UseDarkerPanel = false;

            // 100 = vanilla default. Lower = more transparent guidelines.
            GuidelineOpacityPercent = DefaultGuidelineOpacityPercent;
        }

        // -----------------------------------------------------------------------
        // Helpers
        // -----------------------------------------------------------------------

        public DropdownItem<int>[] GetToolColorModeItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = ToolColorModeRecommended,
                    displayName = GetToolColorModeLocaleID("Recommended"),
                },
                new DropdownItem<int>
                {
                    value = ToolColorModeVanilla,
                    displayName = GetToolColorModeLocaleID("Vanilla"),
                },
                new DropdownItem<int>
                {
                    value = ToolColorModeCustom,
                    displayName = GetToolColorModeLocaleID("Custom"),
                },
            };
        }

        public string GetToolColorModeLocaleID(string valueName)
        {
            return "Options[" + id + ".ToolColorMode." + valueName + "]";
        }

        public DropdownItem<int>[] GetGuidelineDashedColorPresetItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = GuidelineDashedColorPresetVanilla,
                    displayName = GetGuidelineDashedColorPresetLocaleID("Vanilla"),
                },
                new DropdownItem<int>
                {
                    value = GuidelineDashedColorPresetYellow,
                    displayName = GetGuidelineDashedColorPresetLocaleID("Yellow"),
                },
                new DropdownItem<int>
                {
                    value = GuidelineDashedColorPresetPink,
                    displayName = GetGuidelineDashedColorPresetLocaleID("Pink"),
                },
                new DropdownItem<int>
                {
                    value = GuidelineDashedColorPresetGreen,
                    displayName = GetGuidelineDashedColorPresetLocaleID("Green"),
                },
            };
        }

        public string GetGuidelineDashedColorPresetLocaleID(string valueName)
        {
            return "Options[" + id + ".GuidelineDashedColorPreset." + valueName + "]";
        }

        private static void TryOpenUrl(string url)
        {
            try
            {
                Application.OpenURL(url);
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "open-url-" + url,
                    () => $"Failed to open URL '{url}': {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }
    }
}
