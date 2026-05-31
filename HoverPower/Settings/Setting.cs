// File: Settings/Setting.cs
// Purpose: Defines Hover Power settings, persistent storage, and the Options UI surface.
// Layout: 2 tabs (Actions, About) following CityWatchdog/EasyZoning convention.
// Note: the eight outline RGBA floats are NOT decorated for Options UI — they are read by the
// in-city color-picker panel via cs2/api bindings (see Systems/HoverPowerUISystem.cs).

namespace HoverPower.Settings
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

    [FileLocation("ModsSettings/HoverPower/HoverPower")]
    [SettingsUITabOrder(Actions, About)]
    [SettingsUIGroupOrder(ToolColors, KeyBindings, Guidelines, AboutInfo, AboutLinks, AboutDedication)]
    [SettingsUIShowGroupName(ToolColors, KeyBindings, Guidelines, AboutDedication)]
    public class HoverPowerSettings : ModSetting
    {
        // Tab IDs
        internal const string Actions = nameof(Actions);
        internal const string About = nameof(About);

        // Group IDs
        internal const string ToolColors = nameof(ToolColors);
        internal const string Guidelines = nameof(Guidelines);
        internal const string KeyBindings = nameof(KeyBindings);
        internal const string AboutInfo = nameof(AboutInfo);
        internal const string AboutLinks = nameof(AboutLinks);
        internal const string AboutDedication = nameof(AboutDedication);

        public const int ToolColorModeRecommended = 0;
        public const int ToolColorModeVanilla = 1;
        public const int ToolColorModeCustom = 2;

        // Centralised default for the guideline opacity slider.
        // Vanilla CS2 is 100; lower = more transparent. Change only here — C# UISystem and TSX both read this.
        public const int DefaultGuidelineOpacityPercent = 30;

        private const string AboutLinksRow = nameof(AboutLinksRow);
        // Same Paradox URL pattern as CityWatchdog — lands on River-Mochi's author page filtered to CS2.
        private const string UrlParadox =
            "https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime";

        // -----------------------------------------------------------------------
        // In-city color-picker bindings (driven by Systems/HoverPowerUISystem)
        // Not decorated for Options UI — these are data fields the cs2/api bindings read/write
        // and the OutlineColorSystem applies. Field layout after the post-alpha redesign:
        //   - OutlineR/G/B  → outline halo edge color + fill overlay color + lot-pattern tint
        //     (one color choice drives every visible surface so the panel only needs one swatch)
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
        public float FillA { get; set; }

        // District overlay fill color. Disabled by default so we do not touch vanilla/other-mod
        // district prefabs until the player actually picks a color from the in-game panel.
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
        // .coc like the live values AND mirrored to ModsData JSON (see PresetStore) so a
        // corrupted .coc can be repaired from the backup on next load.
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

        // -----------------------------------------------------------------------
        // Actions tab — Tool color behavior
        // -----------------------------------------------------------------------
        // This controls temporary effective colors only. It never overwrites the
        // player's saved swatch color in ModsSettings/HoverPower/HoverPower.coc.

        [SettingsUIDropdown(typeof(HoverPowerSettings), nameof(GetToolColorModeItems))]
        [SettingsUISection(Actions, ToolColors)]
        public int ToolColorMode { get; set; }

        // -----------------------------------------------------------------------
        // Actions tab — Guidelines
        // -----------------------------------------------------------------------
        // Slider is 0..100 (percent) so the SettingsUISlider can use kPercentage units.
        // GuidelineColorSystem divides by 100 and multiplies the game's default per-priority
        // alphas, so 100 = no change, 50 = half as visible, 0 = fully invisible guidelines.

        [SettingsUISection(Actions, KeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.H, Mod.kTogglePanelActionName)]
        public ProxyBinding TogglePanelBinding { get; set; }

        [SettingsUISection(Actions, KeyBindings)]
        [SettingsUIKeyboardBinding(BindingKeyboard.L, Mod.kToggleSurfaceToolAreasActionName)]
        public ProxyBinding ToggleSurfaceToolAreasBinding { get; set; }

        [SettingsUISlider(min = 0, max = 100, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(Actions, Guidelines)]
        public int GuidelineOpacityPercent { get; set; }

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

        [SettingsUIMultilineText]
        [SettingsUISection(About, AboutDedication)]
        public string MochiDedicationText => string.Empty;

        // -----------------------------------------------------------------------
        // Construction + defaults
        // -----------------------------------------------------------------------

        public HoverPowerSettings(IMod mod) : base(mod)
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

            // FillA=0 matches vanilla CS2: no extra silhouette overlay until the player turns it up.
            FillA = 0f;

            // Safe fallback for the District picker until DistrictColorSystem captures the authored
            // default district prefab color. Not applied unless DistrictColorEnabled is true.
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

            // Release default: help players see demolition/road targets even if their custom
            // alpha is very low, without changing their saved custom color.
            ToolColorMode = ToolColorModeRecommended;

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
