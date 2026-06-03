// File: Localization/LocaleIT.cs
// Purpose: Italian (it-IT) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("it-IT", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/it-IT.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleIT : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleIT(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Azioni" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Informazioni" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Comportamento colori strumenti" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Pannello" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Scorciatoie da tastiera" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guide" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedica" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + strade" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controlla i colori temporanei del contorno quando bulldozer o strumenti strada sono attivi.\n\n" +
                    "**1. Consigliato** usa il colore di avviso del gioco per le demolizioni e un blu vanilla più morbido per le strade.\n" +
                    "**2. Colori vanilla degli strumenti** ripristina il normale blu vanilla del gioco mentre questi strumenti sono attivi.\n" +
                    "**3. Tieni il colore personalizzato** usa ovunque il colore scelto.\n\n" +
                    "Questo non sovrascrive il colore personalizzato salvato automaticamente nel selettore colore.\n"+
                    "Alcuni utenti trovano il colore personalizzato difficile da vedere durante le demolizioni e volevano il ritorno automatico di contorni forti durante l’uso degli strumenti."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Consigliato" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Colori vanilla degli strumenti" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Tieni il colore personalizzato" },
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Attiva contorno oggetti sovrapposti" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "Attiva il comportamento vanilla normale quando piazzi oggetti che si sovrappongono ad altri.\n" +
                    "Usa il contorno errore del gioco (color salmone) quando provi a sovrapporre.\n\n" +
                    "Funziona con tutti i modi Bulldozer + strade e non sovrascrive il colore personalizzato salvato."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Pannello più scuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<Pannello scuro>: pensato per chi usa Legacy UI; va bene anche in UI moderna se preferisci un pannello più scuro.\n" +
                    "<Pannello standard>: stile traslucido personalizzato di Hover Colors.\n" +
                    "Aspetto più chiaro e moderno.\n" +
                    "Ideale per la maggior parte dei giocatori con la nuova UI moderna del gioco.\n" +
                    "Provali entrambi. Cambia solo lo sfondo di questo pannello del mod, non l’UI del gioco."
                },
                
                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacità guide (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Regola le guide di gioco (frecce/linee colorate mostrate mentre si piazzano strade, prop, ecc.)\n\n" +
                    "**100%** mantiene l’aspetto vanilla predefinito.\n" +
                    "**Più basso** rende le guide più trasparenti.\n" +
                    "**0%** le nasconde completamente - <Non consigliato>.\n" +           
                    "Consigliato restare sopra il 15%, altrimenti è difficile capire cosa succede.\n" +
                    "Lo stesso slider si trova nel pannello del mod in città. I due sono sincronizzati;\n" +
                    "se questo cambia, cambia anche quello in città."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Apri/chiudi pannello principale" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Scorciatoia da tastiera per aprire / chiudere il pannello Hover Colors in città." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Attiva/disattiva pannello Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Attiva/disattiva anteprime Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Scorciatoia da tastiera per nascondere o ripristinare le linee di confine attive dello strumento Surface mentre si piazzano superfici." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Attiva/disattiva linee Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alterna preset 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Scorciatoia da tastiera per passare dallo slot preset 1 allo slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Alterna tra preset 1 e 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Versione" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Apre la pagina Paradox Mods dell’autore." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "In amorevole memoria di Mochi."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Questo mod è dedicato a Mochi. Era una cagnolina amata, adottata all’età di 7 anni,\n" +
                    "e ha donato 13 anni di amore e gioia. Questo mod non sarebbe stato possibile senza Mochi."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
