// File: Localization/LocaleEN.cs
// Purpose: English (en-US) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("en-US", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/en-US.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleEN : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleEN(HoverColorsSettings settings)
        {
            m_Settings = settings;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            string title = Mod.ModName;
            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title += " (" + Mod.ModVersion + ")";
            }

            return new Dictionary<string, string>
            {
                // Mod title in the left rail of the Options menu.
                { m_Settings.GetSettingsLocaleID(), title },

                // Tabs
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Actions" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "About" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Tool Color Behavior" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Key bindings" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guidelines" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedication" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + Roads" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controls temporary outline colors while bulldozer, or road tools are active.\n\n" +
                    "**1. Recommended** uses game's WarningColor for demolition and a softer vanilla blue for roads.\n" +
                    "**2. Vanilla tool colors** restores the game's normal vanilla blue while those tools are active.\n" +
                    "**3. Keep my custom color** uses your chosen color everywhere.\n\n" +
                    "This does not overwrite your automatically saved custom color in the color picker.\n"+
                    "Some users find their custom color hard to see while bulldozing, and wanted strong color outlines back on automatically during tool usage."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recommended" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Vanilla tool colors" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Keep my custom color" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Guidelines opacity" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Scales the in-game guideline overlay (the colored arrows/lines shown while placing roads, " +
                    "zones, props, etc.) relative to the game's defaults.\n\n" +
                    "**100%** keeps vanilla default look.\n" +
                    "**Lower** makes guidelines more transparent.\n" +
                    "**0%** hides them entirely - <Not recommended>.\n" +           
                    "Recommend stay above 15% or it's hard to see what is happening\n" +
                    "The same slider lives on the city mod panel. They are both synced;\n" +
                    "if you change this one, the one in-city conveniently changes."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Main panel open/close" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Hotkey shortcut to open / close the in-city Hover objects Color Panel." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Toggle Hover Colors panel" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Toggle Surface tool previews on/off" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Hotkey shortcut to hide or restore active Surface tool boundary preview lines while placing surfaces." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Toggle Surface tool lines on/off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Toggle presets 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Hotkey shortcut to flip between preset slot 1 and slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Toggle between presets 1 and 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Open the author's Paradox Mods page." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "In loving memory of Mochi."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "This mod is dedicated to Mochi. She was a beloved dog, adopted at age 7,\n" +
                    "she gave 13 years of love and joy. This mod would not be possible without Mochi."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
