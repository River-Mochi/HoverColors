// File: Localization/LocalePT_PT.cs
// Purpose: European Portuguese (pt-PT) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("pt-PT", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/pt-PT.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocalePT_PT : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocalePT_PT(HoverColorsSettings settings)
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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Painel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Atalhos de teclado" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guias" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedicatória" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + estradas" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controla as cores temporárias do contorno enquanto o bulldozer ou as ferramentas de estrada estão ativas.\n\n" +
                    "**1. Recomendado** usa a cor de aviso do jogo para demolição e um azul vanilla mais suave para estradas.\n" +
                    "**2. Cores vanilla das ferramentas** repõe o azul vanilla normal do jogo enquanto essas ferramentas estão ativas.\n" +
                    "**3. Manter a minha cor personalizada** usa a cor escolhida em todo o lado.\n\n" +
                    "Isto não substitui a cor personalizada guardada automaticamente no seletor de cores.\n" +
                    "Alguns jogadores acham difícil ver a cor personalizada ao demolir e pediram contornos fortes durante o uso das ferramentas."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Cores vanilla das ferramentas" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Manter a minha cor personalizada" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Ativar contorno de itens sobrepostos" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "Ativa o comportamento vanilla normal ao colocar itens que se sobrepõem a outros itens.\n" +
                    "Usa o contorno de erro do jogo (cor salmão) ao tentar sobrepor.\n\n" +
                    "Funciona com todos os modos Bulldozer + estradas e não substitui a tua cor personalizada guardada."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Painel mais escuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<Painel escuro>: feito para quem usa Legacy UI; também pode ser usado na Modern UI se preferires um painel mais escuro.\n" +
                    "<Painel padrão>: estilo translúcido personalizado do Hover Colors.\n" +
                    "Visual mais claro e moderno.\n" +
                    "Melhor para a maioria dos jogadores que usam a nova Modern UI do jogo.\n" +
                    "Experimenta os dois e vê qual preferes. Só muda o fundo deste painel do mod, não a UI do jogo."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidade das guias (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Ajusta as guias do jogo (as setas/linhas coloridas mostradas ao colocar estradas, props, etc.)\n\n" +
                    "**100%** mantém o aspeto vanilla padrão.\n" +
                    "**Mais baixo** torna as guias mais transparentes.\n" +
                    "**0%** esconde-as completamente - <Não recomendado>.\n" +
                    "Recomenda-se ficar acima de 15%, senão é difícil ver o que está a acontecer.\n" +
                    "O mesmo controlo está no painel do mod dentro da cidade. Os dois ficam sincronizados;\n" +
                    "se mudares este, o da cidade também muda."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/fechar painel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Atalho de teclado para abrir / fechar o painel Hover Colors dentro da cidade." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar painel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Ativar/desativar pré-visualizações da ferramenta Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Atalho de teclado para esconder ou repor as linhas de limite ativas da ferramenta Surface ao colocar superfícies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Ativar/desativar linhas Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar predefinições 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Atalho de teclado para alternar entre a predefinição 1 e a predefinição 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Alternar entre predefinições 1 e 2" },

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
                    "Este mod é dedicado à Mochi. Ela foi uma cadelinha muito amada, adotada aos 7 anos,\n" +
                    "e deu 13 anos de amor e alegria. Este mod não seria possível sem Mochi."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
