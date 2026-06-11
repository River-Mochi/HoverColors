// File: Localization/LocalePT_BR.cs
// Purpose: Portuguese Brazil (pt-BR) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/pt-BR.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                    "Controla cores temporárias do contorno enquanto o bulldozer ou ferramentas de estrada estão ativos.\n" +
                    "\n" +
                    "**1. Recomendado** usa a cor de aviso do jogo (amarelo) para demolição e um azul vanilla mais suave para estradas.\n" +
                    "**2. Cores vanilla das ferramentas** restaura o azul vanilla normal do jogo enquanto bulldozer ou ferramentas de estrada estão ativos.\n" +
                    "**3. Manter minha cor personalizada** usa sua cor escolhida em todos os lugares.\n" +
                    "\n" +
                    "Objetivo: alguns usuários/testadores acham difícil ver a cor personalizada ao demolir.\n" +
                    "Isso oferece opções de cores de alta visibilidade durante o uso das ferramentas.\n" +
                    "Isso não substitui sua cor personalizada salva automaticamente no seletor de cores."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Cores vanilla das ferramentas" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Manter minha cor personalizada" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Ativar contorno de itens sobrepostos" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Ativado é recomendado>\n" +
                    "Mantém visível o contorno vanilla vermelho-salmão do jogo quando o posicionamento de objeto ou rede é bloqueado por itens sobrepostos.\n" +
                    "Limites de área, como guias de raio de fazendas de Indústria Especializada, ficam inalterados.\n" +
                    "\n" +
                    "Funciona com todos os modos Bulldozer + estradas e não substitui sua cor personalizada salva."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Permitir cores personalizadas para NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Ativado é recomendado>\n" +
                    "Usa sua cor/transparência HC salva ao posicionar detalhes NetLane, como cercas, sebes, marcações e ferramentas semelhantes baseadas em faixas.\n" +
                    "\n" +
                    "- Estradas normais ainda seguem a configuração Bulldozer + estradas escolhida na lista suspensa.\n" +
                    "- Desative isto se quiser que essas ferramentas usem o azul vanilla do jogo.\n" +
                    "- A cor de erro de sobreposição ainda tem prioridade quando ativada (cor de erro vanilla = vermelho-salmão)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Painel mais escuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Ativado = <Painel escuro>: feito para jogadores com Legacy UI; também pode ser usado na Modern UI se você preferir um painel mais escuro.\n" +
                    "Desativado = <Painel padrão>: estilo translúcido personalizado do Hover Colors.\n" +
                    "- Visual mais claro e moderno.\n" +
                    "- Melhor para a maioria dos jogadores usando a nova Modern UI do jogo.\n" +
                    "\n" +
                    "Teste os dois e veja qual prefere! Isso muda apenas o fundo deste painel do mod, não a UI do jogo."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidade das guias (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Controla a opacidade das guias tracejadas de alinhamento, útil ao posicionar estradas, cercas, props etc.\n" +
                    "\n" +
                    "**100%** mantém o visual vanilla padrão.\n" +
                    "**Menor** deixa as guias mais transparentes.\n" +
                    "**0%** oculta tudo - <Não recomendado>.\n" +
                    "Recomenda-se ficar acima de 15% ou será difícil ver o que está acontecendo.\n" +
                    "O mesmo controle deslizante fica no painel do mod na cidade. Ambos são sincronizados;\n" +
                    "se você alterar este, o da cidade também muda."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/fechar painel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Atalho de teclado para abrir / fechar o painel de cores dos objetos sob o cursor na cidade." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar painel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Alternar prévias da ferramenta Superfície" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Atalho de teclado para ocultar ou restaurar as linhas ativas de prévia de limite da ferramenta Superfície ao posicionar superfícies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Camada de prévia de Superfície On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar presets 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Atalho de teclado para alternar entre o slot de preset 1 e o slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Alternar entre presets 1 e 2" },

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
                    "Este mod é dedicado à Mochi. Ela foi uma cachorrinha muito amada, adotada aos 7 anos,\n" +
                    "e deu 13 anos de amor e alegria. Este mod não seria possível sem Mochi."
                },
            };
        }

        public void Unload()
        {
        }
    }
}
