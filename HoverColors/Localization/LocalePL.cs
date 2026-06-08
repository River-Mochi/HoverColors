// File: Localization/LocalePL.cs
// Purpose: Polish (pl-PL) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/pl-PL.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "O modzie" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Zachowanie kolorów narzędzi" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Skróty klawiszowe" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Linie pomocnicze" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedykacja" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Buldożer + drogi" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Steruje tymczasowymi kolorami obrysu, gdy aktywny jest buldożer lub narzędzia drogowe.\n\n**1. Zalecane** używa koloru ostrzeżenia gry (żółtego) dla wyburzania oraz łagodniejszego vanilla niebieskiego dla dróg.\n**2. Vanilla kolory narzędzi** przywraca normalny vanilla niebieski gry, gdy aktywny jest buldożer lub narzędzia drogowe.\n**3. Zachowaj mój kolor** używa wszędzie wybranego koloru.\n\nCel: niektórzy użytkownicy/testerzy uznali, że ich własny kolor jest słabo widoczny podczas wyburzania.\nTe opcje dają kolory o wysokiej widoczności podczas używania narzędzi.\nNie nadpisuje to automatycznie zapisanego koloru w próbniku kolorów." },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Zalecane" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Vanilla kolory narzędzi" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Zachowaj mój kolor" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Włącz obrys nakładających się elementów" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<Zalecane włączenie>\nUtrzymuje widoczny vanilla łososiowoczerwony obrys gry, gdy umieszczanie obiektu lub sieci jest blokowane przez nakładające się elementy.\nLimity obszarów, takie jak przewodniki promienia farm wyspecjalizowanego przemysłu, pozostają bez zmian.\n\nDziała ze wszystkimi trybami Buldożer + drogi i nie nadpisuje zapisanego koloru niestandardowego." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Zezwalaj na własne kolory dla NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<Zalecane włączenie>\nUżywa zapisanego koloru/przezroczystości HC podczas umieszczania elementów szczegółowych NetLane, takich jak płoty, żywopłoty, oznaczenia i podobne narzędzia oparte na pasach.\n\n- Zwykłe drogi nadal używają ustawienia Buldożer + drogi wybranego z listy.\n- Wyłącz, jeśli te narzędzia mają używać vanilla niebieskiego obrysu gry.\n- Kolor błędu nakładania nadal ma pierwszeństwo, gdy jest włączony (vanilla kolor błędu = łososiowoczerwony)." },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Ciemniejszy panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Włączone używa <Ciemny panel>: stworzone dla graczy ze starszą UI; można też używać w Modern UI, jeśli wolisz ciemniejszy panel.\nWyłączone używa <Panel standardowy>: własny półprzezroczysty styl Hover Colors.\n- Jaśniejszy, bardziej nowoczesny wygląd.\n- Najlepszy dla większości graczy używających nowej Modern UI gry.\n\nSprawdź oba i wybierz, który wolisz! Zmienia to tylko tło panelu tego moda, a nie UI gry." },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Kolor przerywanych linii pomocniczych" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Ustawia kolor przerywanych linii wyrównania używanych dla kątów dróg, pomocy 90 stopni i wskazówek połączeń.\n\nOba suwaki krycia są zsynchronizowane: ten suwak Opcji i suwak w panelu w mieście kontrolują to samo krycie przerywanych linii." },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "Vanilla biały" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "Zielony wysokiej widoczności" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "Cyjanowy niebieski" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "Żółty wysokiej widoczności" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Krycie linii pomocniczych (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Steruje kryciem przerywanych linii wyrównania, przydatne przy stawianiu dróg, płotów, propów itd.\n\n**100%** zachowuje domyślny vanilla wygląd.\n**Niżej** sprawia, że linie są bardziej przezroczyste.\n**0%** ukrywa je całkowicie - <Niezalecane>.\nZalecane jest pozostanie powyżej 15%, inaczej trudno zobaczyć, co się dzieje.\nTen sam suwak jest w panelu moda w mieście. Oba są zsynchronizowane;\njeśli zmienisz ten, miejski też się zmieni." },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Otwórz/zamknij panel główny" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Skrót klawiszowy do otwierania / zamykania miejskiego Panelu kolorów obiektów Hover." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Przełącz panel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Przełącz podglądy narzędzia Powierzchnia" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Skrót klawiszowy do ukrycia lub przywrócenia aktywnych linii granic narzędzia Powierzchnia podczas umieszczania powierzchni." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Warstwa podglądu Powierzchni On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Przełącz presety 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Skrót klawiszowy do przełączania między slotem presetu 1 i 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Przełącz między presetami 1 i 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Otwórz stronę autora w Paradox Mods." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Pamięci kochanej Mochi." },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Ten mod jest dedykowany Mochi. Była ukochaną suczką, adoptowaną w wieku 7 lat,\ni dała 13 lat miłości oraz radości. Ten mod nie byłby możliwy bez Mochi." },
            };
        }

        public void Unload()
        {
        }
    }
}
