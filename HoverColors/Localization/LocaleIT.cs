// File: Localization/LocaleIT.cs
// Purpose: Italian (it-IT) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/it-IT.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Comportamento colore strumenti" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Pannello" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Scorciatoie da tastiera" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Guide" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Dedica" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + strade" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controlla i colori temporanei del contorno mentre sono attivi il bulldozer o gli strumenti stradali.\n" +
                    "\n" +
                    "**1. Consigliato** usa il colore di avviso del gioco (giallo) per la demolizione e un blu vanilla più morbido per le strade.\n" +
                    "**2. Colori vanilla degli strumenti** ripristina il normale blu vanilla del gioco mentre sono attivi bulldozer o strumenti stradali.\n" +
                    "**3. Mantieni il mio colore personalizzato** usa ovunque il colore scelto.\n" +
                    "\n" +
                    "Scopo: alcuni utenti/tester trovano difficile vedere il proprio colore personalizzato durante la demolizione.\n" +
                    "Queste opzioni offrono colori ad alta visibilità durante l’uso degli strumenti.\n" +
                    "Questo non sovrascrive il colore personalizzato salvato automaticamente nel selettore colore."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Consigliato" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Colori vanilla degli strumenti" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Mantieni il mio colore personalizzato" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Abilita contorno degli elementi sovrapposti" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Abilitato consigliato>\n" +
                    "Mantiene visibile il contorno vanilla rosso salmone del gioco quando il posizionamento di oggetti o reti è bloccato da elementi sovrapposti.\n" +
                    "I limiti delle aree, come le guide del raggio delle fattorie di industria specializzata, restano invariati.\n" +
                    "\n" +
                    "Funziona con tutti i modi Bulldozer + strade e non sovrascrive il tuo colore personalizzato salvato."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Consenti colori personalizzati per NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Abilitato consigliato>\n" +
                    "Usa il colore/la trasparenza HC salvati quando posizioni dettagli NetLane come recinzioni, siepi, segnaletica e strumenti simili basati sulle corsie.\n" +
                    "\n" +
                    "- Le strade normali seguono ancora l’impostazione Bulldozer + strade scelta dall’elenco a discesa.\n" +
                    "- Disattiva questa opzione se vuoi che quegli strumenti usino invece il blu vanilla del gioco.\n" +
                    "- Il colore di errore per sovrapposizione ha comunque la priorità quando abilitato (colore errore vanilla = rosso salmone)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Pannello più scuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Abilitato = <Pannello scuro>: pensato per i giocatori con Legacy UI; può essere usato anche con Modern UI se preferisci un pannello più scuro.\n" +
                    "Disabilitato = <Pannello standard>: stile Hover Colors traslucido personalizzato.\n" +
                    "- Aspetto più chiaro e moderno.\n" +
                    "- Ideale per la maggior parte dei giocatori che usano la nuova Modern UI del gioco.\n" +
                    "\n" +
                    "Provali entrambi e scegli quello che preferisci! Cambia solo lo sfondo di questo pannello del mod, non la UI del gioco."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacità guide (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Controlla l’opacità delle guide di allineamento tratteggiate, utile mentre posizioni strade, recinzioni, prop, ecc.\n" +
                    "\n" +
                    "**100%** mantiene l’aspetto vanilla predefinito.\n" +
                    "**Più basso** rende le guide più trasparenti.\n" +
                    "**0%** le nasconde completamente - <Non consigliato>.\n" +
                    "Si consiglia di restare sopra il 15%, altrimenti è difficile vedere cosa sta succedendo.\n" +
                    "Lo stesso cursore si trova nel pannello del mod in città. Sono sincronizzati;\n" +
                    "se cambi questo, cambia anche quello in città."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Apri/chiudi pannello principale" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Scorciatoia da tastiera per aprire / chiudere il pannello colore degli oggetti in hover nella città." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Attiva/disattiva pannello Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Attiva/disattiva anteprime strumento Superficie" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Scorciatoia da tastiera per nascondere o ripristinare le linee di anteprima del bordo attivo dello strumento Superficie durante il posizionamento di superfici." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Livello anteprima Superficie On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alterna preset 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Scorciatoia da tastiera per passare dallo slot preset 1 allo slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Alterna tra preset 1 e 2" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Versione" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Apri la pagina Paradox Mods dell’autore." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "In memoria affettuosa di Mochi."
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
