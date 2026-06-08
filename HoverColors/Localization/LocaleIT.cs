// File: Localization/LocaleIT.cs
// Purpose: Italian (it-IT) strings for the Options Menu.
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Actions" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Info" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Comportamento colore strumenti" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Pannello" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Scorciatoie da tastiera" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guide" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedica" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + strade" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Controlla i colori temporanei del contorno mentre sono attivi il bulldozer o gli strumenti strada.\n\n**1. Consigliato** usa il colore Avviso del gioco (giallo) per la demolizione e un blu vanilla più morbido per le strade.\n**2. Colori vanilla degli strumenti** ripristina il normale blu vanilla del gioco mentre sono attivi bulldozer o strumenti strada.\n**3. Mantieni il mio colore personalizzato** usa ovunque il colore scelto.\n\nScopo: alcuni utenti/tester trovano difficile vedere il proprio colore personalizzato durante la demolizione.\nQueste opzioni offrono colori ad alta visibilità durante l’uso degli strumenti.\nNon sovrascrive il colore personalizzato salvato automaticamente nel selettore colori." },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Consigliato" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Colori vanilla degli strumenti" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Mantieni il mio colore personalizzato" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Abilita contorno per elementi sovrapposti" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<Abilitato è consigliato>\nMantiene visibile il contorno vanilla rosso salmone del gioco quando il posizionamento di oggetti o reti è bloccato da elementi sovrapposti.\nI limiti di area, come le guide del raggio delle fattorie dell’Industria specializzata, non vengono modificati.\n\nFunziona con tutti i modi Bulldozer + strade e non sovrascrive il colore personalizzato salvato." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Consenti colori personalizzati per NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<Abilitato è consigliato>\nUsa il colore/trasparenza HC salvato mentre posizioni elementi di dettaglio NetLane come recinzioni, siepi, segnaletica e strumenti simili basati sulle corsie.\n\n- Le strade normali seguono comunque l’impostazione Bulldozer + strade scelta dal menu a discesa.\n- Disabilita questa opzione se vuoi che quegli strumenti usino invece il blu vanilla del gioco.\n- Il colore di errore da sovrapposizione ha ancora la priorità quando è abilitato (colore errore vanilla = rosso salmone)." },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Pannello più scuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Abilitato usa <Pannello scuro>: creato per i giocatori con UI legacy; può essere usato anche nella UI moderna se preferisci un pannello più scuro.\nDisabilitato usa <Pannello standard>: stile Hover Colors traslucido personalizzato.\n- Aspetto più chiaro e moderno.\n- Migliore per la maggior parte dei giocatori che usano la nuova UI moderna del gioco.\n\nProvali entrambi e scegli quello che preferisci! Cambia solo lo sfondo di questo pannello del mod, non la UI del gioco." },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Colore linee guida tratteggiate" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Imposta il colore delle guide di allineamento tratteggiate usate per angoli stradali, aiuti a 90 gradi e suggerimenti di connessione.\n\nEntrambi i cursori di opacità sono sincronizzati: questo cursore nelle Opzioni e quello nel pannello in città controllano la stessa opacità delle guide tratteggiate." },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "Bianco vanilla" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "Verde alta visibilità" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "Blu ciano" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "Giallo alta visibilità" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacità guide (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Controlla l’opacità delle guide di allineamento tratteggiate, utile mentre posizioni strade, recinzioni, props, ecc.\n\n**100%** mantiene l’aspetto vanilla predefinito.\n**Più basso** rende le guide più trasparenti.\n**0%** le nasconde completamente - <Non consigliato>.\nSi consiglia di restare sopra il 15%, altrimenti diventa difficile vedere cosa succede.\nLo stesso cursore è nel pannello del mod in città. Sono sincronizzati;\nse cambi questo, cambia anche quello in città." },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Apri/chiudi pannello principale" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Scorciatoia da tastiera per aprire / chiudere il pannello colore degli oggetti hover in città." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Attiva/disattiva pannello Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Attiva/disattiva anteprime strumento Superficie" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Scorciatoia da tastiera per nascondere o ripristinare le linee di anteprima dei confini attive dello strumento Superficie mentre posizioni superfici." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Livello anteprima Superficie On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alterna preset 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Scorciatoia da tastiera per passare dallo slot preset 1 allo slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Alterna tra preset 1 e 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Apri la pagina Paradox Mods dell’autore." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "In amorevole memoria di Mochi." },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Questo mod è dedicato a Mochi. Era una cagnolina amatissima, adottata all’età di 7 anni,\ne ha donato 13 anni di amore e gioia. Questo mod non sarebbe stato possibile senza Mochi." },
            };
        }

        public void Unload()
        {
        }
    }
}
