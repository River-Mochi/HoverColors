// File: Localization/LocaleDE.cs
// Purpose: German (de-DE) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/de-DE.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Über" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Werkzeug-Farbverhalten" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Tastenbelegungen" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Hilfslinien" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Widmung" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + Straßen" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Steuert temporäre Umrissfarben, solange der Bulldozer oder Straßenwerkzeuge aktiv sind.\n" +
                    "\n" +
                    "**1. Empfohlen** nutzt die Warnfarbe des Spiels (Gelb) für Abriss und ein weicheres Vanilla-Blau für Straßen.\n" +
                    "**2. Vanilla-Werkzeugfarben** stellt das normale Vanilla-Blau des Spiels wieder her, solange Bulldozer oder Straßenwerkzeuge aktiv sind.\n" +
                    "**3. Meine eigene Farbe behalten** nutzt überall deine gewählte Farbe.\n" +
                    "\n" +
                    "Zweck: Einige Nutzer/Tester finden ihre eigene Farbe beim Abreißen schwer zu sehen.\n" +
                    "Diese Optionen bieten gut sichtbare Farben während der Werkzeugnutzung.\n" +
                    "Die automatisch gespeicherte eigene Farbe im Farbwähler wird nicht überschrieben."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Empfohlen" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Vanilla-Werkzeugfarben" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Meine eigene Farbe behalten" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Umriss für überlappende Elemente aktivieren" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Aktiviert wird empfohlen>\n" +
                    "Hält den Vanilla-lachsroten Umriss des Spiels sichtbar, wenn Objekt- oder Netzwerkplatzierung durch überlappende Elemente blockiert ist.\n" +
                    "Flächenbegrenzungen, wie Radius-Hilfen für spezialisierte Industrie-Farmen, bleiben unverändert.\n" +
                    "\n" +
                    "Funktioniert mit allen Bulldozer + Straßen-Modi und überschreibt deine gespeicherte eigene Farbe nicht."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Eigene Farben für NetLanes erlauben" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Aktiviert wird empfohlen>\n" +
                    "Verwendet deine gespeicherte HC-Farbe/Transparenz beim Platzieren von NetLane-Details wie Zäunen, Hecken, Markierungen und ähnlichen spur-basierten Werkzeugen.\n" +
                    "\n" +
                    "- Normale Straßen folgen weiterhin der Bulldozer + Straßen-Einstellung aus der Dropdown-Liste.\n" +
                    "- Deaktiviere dies, wenn diese Werkzeuge stattdessen das Vanilla-Blau des Spiels verwenden sollen.\n" +
                    "- Die Überlappungs-Fehlerfarbe hat bei Aktivierung weiterhin Vorrang (Vanilla-Fehlerfarbe = Lachsrot)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Dunkleres Panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Aktiviert = <Dunkles Panel>: für Spieler mit Legacy UI gemacht; kann auch in Modern UI genutzt werden, wenn du ein dunkleres Panel magst.\n" +
                    "Deaktiviert = <Standard-Panel>: eigener transparenter Hover Colors-Stil.\n" +
                    "- Hellerer, modernerer Look.\n" +
                    "- Am besten für die meisten Spieler mit der neuen Modern UI des Spiels.\n" +
                    "\n" +
                    "Probier beides aus und nimm, was dir besser gefällt! Das ändert nur den Hintergrund dieses Mod-Panels, nicht die Spiel-UI."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Deckkraft der Hilfslinien (Alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Steuert die Deckkraft gestrichelter Ausrichtungshilfen, nützlich beim Platzieren von Straßen, Zäunen, Props usw.\n" +
                    "\n" +
                    "**100%** behält den Vanilla-Standard-Look.\n" +
                    "**Niedriger** macht Hilfslinien transparenter.\n" +
                    "**0%** blendet sie vollständig aus - <Nicht empfohlen>.\n" +
                    "Bleib möglichst über 15%, sonst ist kaum zu erkennen, was passiert.\n" +
                    "Derselbe Regler befindet sich im Stadt-Mod-Panel. Beide sind synchronisiert;\n" +
                    "wenn du diesen änderst, ändert sich der in der Stadt ebenfalls."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Hauptpanel öffnen/schließen" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Tastenkürzel zum Öffnen / Schließen des Hover-Objekt-Farbpanels in der Stadt." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors-Panel umschalten" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Vorschauen des Oberflächenwerkzeugs ein/aus" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Tastenkürzel zum Ausblenden oder Wiederherstellen aktiver Begrenzungsvorschau-Linien des Oberflächenwerkzeugs beim Platzieren von Flächen." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Oberflächen-Vorschau-Ebene Ein/Aus" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Voreinstellungen 1+2 umschalten" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Tastenkürzel zum Wechseln zwischen Voreinstellungsplatz 1 und 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Zwischen Voreinstellungen 1 und 2 umschalten" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Öffnet die Paradox Mods-Seite des Autors." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "In liebevoller Erinnerung an Mochi."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Dieser Mod ist Mochi gewidmet. Sie war eine geliebte Hündin, adoptiert im Alter von 7 Jahren,\n" +
                    "und schenkte 13 Jahre Liebe und Freude. Ohne Mochi wäre dieser Mod nicht möglich gewesen."
                },
            };
        }

        public void Unload()
        {
        }
    }
}
