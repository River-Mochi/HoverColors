// File: Localization/LocalePT_BR.cs
// Purpose: Brazilian Portuguese (pt-BR) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("pt-BR", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/pt-BR.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocalePT_BR : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocalePT_BR(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Ações" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Sobre" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Comportamento das cores das ferramentas" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Atalhos de teclado" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guias" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedicatória" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Demolição + vias" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controla as cores temporárias do contorno enquanto a ferramenta de demolição ou as ferramentas de vias estão ativas.\n\n" +
                    "**1. Recomendado** usa a Cor de Aviso do jogo para demolição e um azul vanilla mais suave para vias.\n" +
                    "**2. Cores vanilla das ferramentas** restaura o azul vanilla normal do jogo enquanto essas ferramentas estão ativas.\n" +
                    "**3. Manter minha cor personalizada** usa a cor escolhida em todos os lugares.\n\n" +
                    "Isso não sobrescreve a cor personalizada salva automaticamente no seletor de cores.\n"+
                    "Alguns jogadores acham difícil ver a cor personalizada durante demolições e queriam que contornos fortes voltassem automaticamente durante o uso das ferramentas."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Cores vanilla das ferramentas" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Manter minha cor personalizada" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidade das guias (alfa)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Ajusta as guias do jogo (as setas/linhas coloridas mostradas ao colocar vias, props etc.)\n\n" +
                    "**100%** mantém o visual vanilla padrão.\n" +
                    "**Mais baixo** deixa as guias mais transparentes.\n" +
                    "**0%** oculta tudo completamente - <Não recomendado>.\n" +           
                    "Recomenda-se ficar acima de 15%, senão fica difícil ver o que está acontecendo.\n" +
                    "O mesmo controle fica no painel do mod dentro da cidade. Os dois são sincronizados;\n" +
                    "se você mudar este, o da cidade também muda automaticamente."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/fechar painel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Atalho de teclado para abrir / fechar o painel Hover Colors dentro da cidade." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar painel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Ativar/desativar prévias da ferramenta Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Atalho de teclado para ocultar ou restaurar as linhas de limite ativas da ferramenta Surface ao colocar superfícies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Ativar/desativar linhas Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar presets 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Atalho de teclado para alternar entre o slot de preset 1 e o slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Alternar entre presets 1 e 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Versão" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Abre a página do autor no Paradox Mods." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Em memória carinhosa de Mochi."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Este mod é dedicado à Mochi. Ela foi uma cachorrinha amada, adotada aos 7 anos,\n" +
                    "e deu 13 anos de amor e alegria. Este mod não seria possível sem Mochi."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
