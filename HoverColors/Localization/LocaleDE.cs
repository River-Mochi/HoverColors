// File: Localization/LocaleDE.cs
// Purpose: German (de-DE) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("de-DE", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/de-DE.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleDE : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleDE(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Aktionen" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Info" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Tool-Farbverhalten" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Tastenbelegung" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Hilfslinien" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Widmung" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Abriss + Straßen" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Steuert temporäre Umrissfarben, während Bulldozer- oder Straßentools aktiv sind.\n\n" +
                    "**1. Empfohlen** nutzt die Warnfarbe des Spiels für Abriss und ein weicheres Vanilla-Blau für Straßen.\n" +
                    "**2. Vanilla-Toolfarben** stellt das normale Vanilla-Blau des Spiels wieder her, während diese Tools aktiv sind.\n" +
                    "**3. Eigene Farbe behalten** nutzt die gewählte Farbe überall.\n\n" +
                    "Die automatisch gespeicherte eigene Farbe im Farbwähler wird nicht überschrieben.\n"+
                    "Manche Nutzer finden ihre eigene Farbe beim Abriss schwer erkennbar und wollten starke Umrissfarben während der Tool-Nutzung automatisch zurückhaben."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Empfohlen" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Vanilla-Toolfarben" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Eigene Farbe behalten" },
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Umriss bei überlappenden Objekten aktivieren" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "Aktiviert das normale Vanilla-Verhalten, wenn Objekte mit anderen Objekten überlappen.\n" +
                    "Nutzt den Fehler-Umriss des Spiels in Lachsrot beim Platzieren mit Überlappung.\n\n" +
                    "Funktioniert mit allen Bulldozer- und Straßenmodi und überschreibt deine gespeicherte eigene Farbe nicht."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Dunkleres Panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<Dunkles Panel>: für Legacy-UI-Spieler gedacht; geht auch in der modernen UI, wenn du es dunkler magst.\n" +
                    "<Standard-Panel>: eigener transparenter Hover-Colors-Stil.\n" +
                    "Heller, moderner Look.\n" +
                    "Am besten für die meisten Spieler mit der neuen modernen Spiel-UI.\n" +
                    "Probier beide aus. Es ändert nur den Hintergrund dieses Mod-Panels, nicht die Spiel-UI."
                },
                
                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Hilfslinien-Deckkraft (Alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Skaliert die Hilfslinien im Spiel (die farbigen Pfeile/Linien beim Platzieren von Straßen, Props usw.)\n\n" +
                    "**100%** behält den Vanilla-Standardlook.\n" +
                    "**Niedriger** macht Hilfslinien transparenter.\n" +
                    "**0%** blendet sie komplett aus - <Nicht empfohlen>.\n" +           
                    "Empfohlen ist über 15%, sonst ist schwer zu sehen, was passiert.\n" +
                    "Derselbe Slider ist auch im Stadt-Mod-Panel. Beide sind synchronisiert;\n" +
                    "wenn dieser geändert wird, ändert sich der im Spiel ebenfalls."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Hauptpanel öffnen/schließen" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Tastenkürzel zum Öffnen / Schließen des Hover Colors-Panels in der Stadt." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors-Panel umschalten" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface-Tool-Vorschau ein/aus" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Tastenkürzel zum Ausblenden oder Wiederherstellen aktiver Surface-Tool-Grenzlinien beim Platzieren von Flächen." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface-Tool-Linien ein/aus" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Presets 1+2 umschalten" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Tastenkürzel zum Wechseln zwischen Preset-Slot 1 und Slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Zwischen Preset 1 und 2 umschalten" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Öffnet die Paradox Mods-Seite des Autors." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "In liebevoller Erinnerung an Mochi."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Dieser Mod ist Mochi gewidmet. Sie war ein geliebter Hund, adoptiert mit 7 Jahren,\n" +
                    "und schenkte 13 Jahre Liebe und Freude. Ohne Mochi wäre dieser Mod nicht möglich gewesen."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
