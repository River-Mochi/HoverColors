// File: Localization/LocalePT_PT.cs
// Purpose: Portuguese Portugal (pt-PT) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/pt-PT.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Comportamento das cores das ferramentas" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Painel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Atalhos de teclado" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Guias" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Dedicatória" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + estradas" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controla cores temporárias do contorno enquanto o bulldozer ou as ferramentas de estrada estão ativas.\n" +
                    "\n" +
                    "**1. Recomendado** usa a cor de aviso do jogo (amarelo) para demolição e um azul vanilla mais suave para estradas.\n" +
                    "**2. Cores vanilla das ferramentas** restaura o azul vanilla normal do jogo enquanto o bulldozer ou as ferramentas de estrada estão ativas.\n" +
                    "**3. Manter a minha cor personalizada** usa a cor escolhida em todo o lado.\n" +
                    "\n" +
                    "Objetivo: alguns utilizadores/testers acham difícil ver a sua cor personalizada ao demolir.\n" +
                    "Isto oferece opções de cores de alta visibilidade durante a utilização das ferramentas.\n" +
                    "Isto não substitui a cor personalizada guardada automaticamente no seletor de cores."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Cores vanilla das ferramentas" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Manter a minha cor personalizada" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Ativar contorno de itens sobrepostos" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Ativado é recomendado>\n" +
                    "Mantém visível o contorno vanilla vermelho-salmão do jogo quando a colocação de objetos ou redes é bloqueada por itens sobrepostos.\n" +
                    "Limites de área, como guias de raio de quintas de Indústria Especializada, ficam inalterados.\n" +
                    "\n" +
                    "Funciona com todos os modos Bulldozer + estradas e não substitui a cor personalizada guardada."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Permitir cores personalizadas para NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Ativado é recomendado>\n" +
                    "Usa a cor/transparência HC guardada ao colocar detalhes NetLane, como vedações, sebes, marcações e ferramentas semelhantes baseadas em faixas.\n" +
                    "\n" +
                    "- Estradas normais continuam a seguir a definição Bulldozer + estradas escolhida na lista pendente.\n" +
                    "- Desative isto se quiser que essas ferramentas usem o azul vanilla do jogo.\n" +
                    "- A cor de erro de sobreposição continua a ter prioridade quando ativada (cor de erro vanilla = vermelho-salmão)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Painel mais escuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Ativado = <Painel escuro>: feito para jogadores com Legacy UI; também pode ser usado na Modern UI se preferir um painel mais escuro.\n" +
                    "Desativado = <Painel padrão>: estilo translúcido personalizado do Hover Colors.\n" +
                    "- Visual mais claro e moderno.\n" +
                    "- Melhor para a maioria dos jogadores que usam a nova Modern UI do jogo.\n" +
                    "\n" +
                    "Experimente ambos e veja qual prefere! Isto muda apenas o fundo deste painel do mod, não a UI do jogo."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidade das guias (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Controla a opacidade das guias tracejadas de alinhamento, útil ao colocar estradas, vedações, props, etc.\n" +
                    "\n" +
                    "**100%** mantém o aspeto vanilla predefinido.\n" +
                    "**Mais baixo** torna as guias mais transparentes.\n" +
                    "**0%** oculta-as totalmente - <Não recomendado>.\n" +
                    "Recomenda-se ficar acima de 15% ou será difícil ver o que está a acontecer.\n" +
                    "O mesmo controlo deslizante está no painel do mod na cidade. Ambos estão sincronizados;\n" +
                    "se alterar este, o da cidade também muda."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/fechar painel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Atalho de teclado para abrir / fechar o painel de cor dos objetos sob o cursor na cidade." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar painel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Alternar pré-visualizações da ferramenta Superfície" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Atalho de teclado para ocultar ou restaurar as linhas ativas de pré-visualização de limites da ferramenta Superfície ao colocar superfícies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Camada de pré-visualização de Superfície On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar predefinições 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Atalho de teclado para alternar entre o slot de predefinição 1 e o slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Alternar entre predefinições 1 e 2" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Versão" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Abrir a página Paradox Mods do autor." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Em memória de Mochi."
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
