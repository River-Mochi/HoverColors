// File: Localization/LocalePL.cs
// Purpose: Polish (pl-PL) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("pl-PL", ...).
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
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Kontroluje tymczasowe kolory obrysu, gdy aktywny jest buldożer albo narzędzia dróg.\n\n" +
                    "**1. Zalecane** używa koloru ostrzeżenia z gry do wyburzania i łagodniejszego waniliowego niebieskiego dla dróg.\n" +
                    "**2. Waniliowe kolory narzędzi** przywraca normalny waniliowy niebieski gry, gdy te narzędzia są aktywne.\n" +
                    "**3. Zachowaj mój własny kolor** używa wybranego koloru wszędzie.\n\n" +
                    "Nie nadpisuje to automatycznie zapisanego własnego koloru w próbniku kolorów.\n"+
                    "Niektórym graczom trudno zobaczyć własny kolor podczas wyburzania, więc chcieli automatycznego powrotu mocnych obrysów podczas używania narzędzi."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Zalecane" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Waniliowe kolory narzędzi" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Zachowaj mój własny kolor" },
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Włącz obrys nachodzących obiektów" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "Włącza normalne zachowanie gry przy stawianiu obiektów nachodzących na inne.\n" +
                    "Używa obrysu błędu z gry (łososiowy kolor), gdy próbujesz coś nałożyć.\n\n" +
                    "Działa ze wszystkimi trybami Bulldozer + drogi i nie nadpisuje zapisanego własnego koloru."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Ciemniejszy panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<Ciemny panel>: dla graczy z Legacy UI; działa też w Modern UI, jeśli wolisz ciemniejszy panel.\n" +
                    "<Panel standardowy>: własny, półprzezroczysty styl Hover Colors.\n" +
                    "Jaśniejszy i bardziej nowoczesny wygląd.\n" +
                    "Najlepszy dla większości graczy używających nowego Modern UI.\n" +
                    "Wypróbuj oba. Zmienia tylko tło panelu tego moda, nie interfejs gry."
                },
                
                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Przezroczystość linii pomocniczych (alfa)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Skaluje linie pomocnicze w grze (kolorowe strzałki/linie widoczne podczas stawiania dróg, propów itd.)\n\n" +
                    "**100%** zachowuje domyślny waniliowy wygląd.\n" +
                    "**Niżej** sprawia, że linie pomocnicze są bardziej przezroczyste.\n" +
                    "**0%** ukrywa je całkowicie - <Niezalecane>.\n" +           
                    "Zalecane jest pozostanie powyżej 15%, bo inaczej trudno zobaczyć, co się dzieje.\n" +
                    "Ten sam suwak jest też w miejskim panelu moda. Oba są zsynchronizowane;\n" +
                    "jeśli zmienisz ten, ten w mieście też wygodnie się zmieni."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Otwórz/zamknij panel główny" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Skrót klawiszowy do otwierania / zamykania miejskiego panelu Hover Colors." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Przełącz panel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Włącz/wyłącz podgląd narzędzia Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Skrót klawiszowy do ukrywania lub przywracania aktywnych linii granic narzędzia Surface podczas stawiania powierzchni." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Włącz/wyłącz linie Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Przełącz presety 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Skrót klawiszowy do przełączania między slotem presetu 1 i slotem 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Przełącz między presetem 1 i 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Wersja" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Otwiera stronę autora w Paradox Mods." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Ku pamięci ukochanej Mochi."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Ten mod jest dedykowany Mochi. Była ukochanym psem, adoptowanym w wieku 7 lat,\n" +
                    "i dała 13 lat miłości oraz radości. Bez Mochi ten mod nie byłby możliwy."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
