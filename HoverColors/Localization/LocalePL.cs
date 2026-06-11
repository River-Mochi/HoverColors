// File: Localization/LocalePL.cs
// Purpose: Polish (pl-PL) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/pl-PL.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

    public sealed class LocalePL : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocalePL(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Akcje" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Informacje" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Zachowanie kolorów narzędzi" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Skróty klawiszowe" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Linie pomocnicze" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Dedykacja" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Buldożer + drogi" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Steruje tymczasowymi kolorami obrysu, gdy aktywny jest buldożer lub narzędzia drogowe.\n" +
                    "\n" +
                    "**1. Zalecane** używa koloru ostrzegawczego gry (żółtego) do wyburzania i łagodniejszego vanilla blue dla dróg.\n" +
                    "**2. Kolory narzędzi vanilla** przywraca normalny vanilla blue gry, gdy aktywny jest buldożer lub narzędzia drogowe.\n" +
                    "**3. Zachowaj mój kolor niestandardowy** używa wszędzie wybranego koloru.\n" +
                    "\n" +
                    "Cel: niektórzy użytkownicy/testerzy mają trudność z dostrzeżeniem swojego koloru podczas wyburzania.\n" +
                    "Te opcje oferują dobrze widoczne kolory podczas używania narzędzi.\n" +
                    "Nie nadpisuje to automatycznie zapisanego koloru niestandardowego w selektorze koloru."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Zalecane" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Kolory narzędzi vanilla" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Zachowaj mój kolor niestandardowy" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Włącz obrys nakładających się elementów" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Włączenie jest zalecane>\n" +
                    "Zachowuje widoczny vanilla łososiowoczerwony obrys gry, gdy umieszczanie obiektu lub sieci jest blokowane przez nakładające się elementy.\n" +
                    "Limity obszarów, takie jak promienie farm przemysłu wyspecjalizowanego, pozostają bez zmian.\n" +
                    "\n" +
                    "Działa ze wszystkimi trybami Buldożer + drogi i nie nadpisuje zapisanego koloru niestandardowego."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Zezwalaj na kolory niestandardowe dla NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Włączenie jest zalecane>\n" +
                    "Używa zapisanego koloru/przezroczystości HC podczas umieszczania detali NetLane, takich jak płoty, żywopłoty, oznaczenia i podobne narzędzia oparte na pasach.\n" +
                    "\n" +
                    "- Normalne drogi nadal używają ustawienia Buldożer + drogi wybranego z listy.\n" +
                    "- Wyłącz to, jeśli te narzędzia mają używać vanilla blue gry.\n" +
                    "- Kolor błędu nakładania nadal ma pierwszeństwo, gdy jest włączony (vanilla kolor błędu = łososiowoczerwony)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Ciemniejszy panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Włączone = <Ciemny panel>: przygotowane dla graczy z Legacy UI; można też używać w Modern UI, jeśli wolisz ciemniejszy panel.\n" +
                    "Wyłączone = <Panel standardowy>: niestandardowy półprzezroczysty styl Hover Colors.\n" +
                    "- Jaśniejszy, nowocześniejszy wygląd.\n" +
                    "- Najlepszy dla większości graczy używających nowej Modern UI gry.\n" +
                    "\n" +
                    "Wypróbuj oba i wybierz, który wolisz! Zmienia to tylko tło tego panelu moda, a nie UI gry."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Przezroczystość linii pomocniczych (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Steruje przezroczystością przerywanych linii wyrównania, przydatnych podczas umieszczania dróg, płotów, propów itd.\n" +
                    "\n" +
                    "**100%** zachowuje domyślny wygląd vanilla.\n" +
                    "**Niżej** sprawia, że linie są bardziej przezroczyste.\n" +
                    "**0%** całkowicie je ukrywa - <Niezalecane>.\n" +
                    "Zalecane jest pozostanie powyżej 15%, inaczej trudno zobaczyć, co się dzieje.\n" +
                    "Ten sam suwak znajduje się w panelu moda w mieście. Oba są zsynchronizowane;\n" +
                    "jeśli zmienisz ten, miejski też się zmieni."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Otwórz/zamknij panel główny" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Skrót klawiszowy do otwierania / zamykania miejskiego panelu kolorów obiektów pod kursorem." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Przełącz panel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Przełącz podglądy narzędzia Powierzchnia" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Skrót klawiszowy do ukrywania lub przywracania aktywnych linii podglądu granic narzędzia Powierzchnia podczas umieszczania powierzchni." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Warstwa podglądu Powierzchni On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Przełącz presety 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Skrót klawiszowy do przełączania między slotem presetu 1 i 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Przełącz między presetami 1 i 2" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Wersja" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Otwórz stronę autora w Paradox Mods." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Pamięci Mochi."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Ten mod jest dedykowany Mochi. Była ukochaną suczką, adoptowaną w wieku 7 lat,\n" +
                    "i dała 13 lat miłości oraz radości. Ten mod nie byłby możliwy bez Mochi."
                },
            };
        }

        public void Unload()
        {
        }
    }
}
