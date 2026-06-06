// File: Localization/LocalePT_PT.cs
// Purpose: European Portuguese (pt-PT) strings for the Options Menu.
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Actions" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Sobre" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Comportamento das cores das ferramentas" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Painel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Atalhos de teclado" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guias" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedicação" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + estradas" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Controla temporariamente as cores do contorno enquanto o bulldozer ou as ferramentas de estrada estão ativos.\n\n**1. Recomendado** usa a cor de Aviso do jogo (amarelo) para demolição e um azul vanilla mais suave para estradas.\n**2. Cores vanilla das ferramentas** restaura o azul vanilla normal do jogo enquanto o bulldozer ou as ferramentas de estrada estão ativos.\n**3. Manter minha cor personalizada** usa a cor escolhida em todos os lugares.\n\nObjetivo: alguns usuários/testadores acham difícil ver a cor personalizada ao demolir.\nIsto oferece opções de cores de alta visibilidade durante o uso das ferramentas.\nIsto não sobrescreve a cor personalizada salva automaticamente no seletor de cores." },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Cores vanilla das ferramentas" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Manter minha cor personalizada" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Ativar contorno de itens sobrepostos" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<Activado é recomendado>\nMantém visível o contorno vanilla vermelho-salmão do jogo quando a colocação de objectos ou redes é bloqueada por itens sobrepostos.\nLimites de área, como guias de raio de quintas da Indústria Especializada, ficam intactos.\n\nFunciona com todos os modos Bulldozer + estradas e não substitui a tua cor personalizada guardada." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Permitir cores personalizadas para NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<Activado é recomendado>\nUsa a tua cor/transparência HC guardada ao colocar itens de detalhe NetLane, como vedações, sebes, marcações e ferramentas semelhantes baseadas em faixas.\n\n- Estradas normais continuam a seguir a opção Bulldozer + estradas escolhida na lista.\n- Desactiva se quiseres que essas ferramentas usem o azul vanilla do jogo.\n- A cor de erro de sobreposição continua a ter prioridade quando activada (cor de erro vanilla = vermelho-salmão)." },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Painel mais escuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Ativado usa <Painel escuro>: feito para jogadores da UI antiga; também pode ser usado na UI Moderna se você gostar de um painel mais escuro.\nDesativado usa <Painel padrão>: estilo translúcido personalizado do Hover Colors.\n- Visual mais claro e moderno.\n- Melhor para a maioria dos jogadores usando a nova UI Moderna do jogo.\n\nTeste os dois e veja qual prefere! Isto muda apenas o fundo deste painel do mod, não a UI do jogo." },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Cor das linhas-guia tracejadas" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Define a cor das guias de alinhamento tracejadas usadas para ângulos de estrada, auxílios de 90 graus e dicas de conexão.\n\nOs dois controles de opacidade são sincronizados: este controle nas Opções e o controle do painel na cidade ajustam a mesma opacidade das guias tracejadas." },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "Branco vanilla" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "Amarelo de alta visibilidade" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Pink"), "Mochi Pink" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "Verde de alta visibilidade" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidade das guias (alfa)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Controla a opacidade das guias de alinhamento tracejadas, útil ao colocar estradas, cercas, props etc.\n\n**100%** mantém o visual vanilla padrão.\n**Mais baixo** deixa as guias mais transparentes.\n**0%** esconde tudo - <Não recomendado>.\nRecomenda-se ficar acima de 15%, ou fica difícil ver o que está acontecendo.\nO mesmo controle existe no painel do mod na cidade. Eles são sincronizados;\nse você mudar este, o da cidade muda também." },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/fechar painel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Atalho de teclado para abrir / fechar o Painel de Cores dos objetos em hover na cidade." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar painel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Ativar/desativar prévias da ferramenta Superfície" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Atalho de teclado para ocultar ou restaurar as linhas de limite ativas da ferramenta Superfície enquanto coloca superfícies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Camada de prévia da Superfície On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar presets 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Atalho de teclado para alternar entre o slot de preset 1 e o slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Alternar entre presets 1 e 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Abrir a página do autor no Paradox Mods." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Em memória amorosa de Mochi." },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Este mod é dedicado à Mochi. Ela foi uma cadelinha muito amada, adoptada aos 7 anos,\ne deu 13 anos de amor e alegria. Este mod não seria possível sem a Mochi." },
            };
        }

        public void Unload()
        {
        }
    }
}
