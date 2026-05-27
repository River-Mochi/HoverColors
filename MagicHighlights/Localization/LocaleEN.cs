// File: Localization/LocaleEN.cs
// Purpose: English (en-US) strings for the Options UI (ESC -> Options -> Magic Highlights).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("en-US", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/en-US.json.

namespace MagicHighlights.Localization
{
    using Colossal;
    using MagicHighlights.Settings;
    using System.Collections.Generic;

    public sealed class LocaleEN : IDictionarySource
    {
        private readonly MagicHighlightsSettings m_Settings;

        public LocaleEN(MagicHighlightsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(MagicHighlightsSettings.Actions), "Actions" },
                { m_Settings.GetOptionTabLocaleID(MagicHighlightsSettings.About), "About" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(MagicHighlightsSettings.Guidelines), "Guidelines" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(MagicHighlightsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(MagicHighlightsSettings.AboutLinks), string.Empty },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(MagicHighlightsSettings.GuidelineOpacityPercent)), "Guidelines opacity" },
                { m_Settings.GetOptionDescLocaleID(nameof(MagicHighlightsSettings.GuidelineOpacityPercent)),
                    "Scales the in-game guideline overlay (the colored arrows/lines shown while placing roads, " +
                    "zones, props, etc.) relative to the game's defaults.\n\n" +
                    "**100%** keeps vanilla default look.\n" +
                    "**Lower** makes guidelines more transparent.\n" +
                    "**0%** hides them entirely - <Not recommended>.\n" +           
                    "Recommend not going lower than 10%" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(MagicHighlightsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(MagicHighlightsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(MagicHighlightsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(MagicHighlightsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(MagicHighlightsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(MagicHighlightsSettings.OpenParadox)), "Open the author's Paradox Mods page." },
            };
        }

        public void Unload()
        {
        }
    }
}
