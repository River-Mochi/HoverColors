// File: Localization/LocaleZH_HANS.cs
// Purpose: Simplified Chinese (zh-HANS) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("zh-HANS", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/zh-HANS.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleZH_HANS : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleZH_HANS(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "操作" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "关于" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "工具颜色行为" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "快捷键" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "辅助线" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "献给" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "推土机 + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "控制推土机或道路工具启用时的临时轮廓颜色。\n\n" +
                    "**1. 推荐**：拆除时使用游戏的警告色，道路使用更柔和的原版蓝色。\n" +
                    "**2. 原版工具颜色**：这些工具启用时恢复游戏正常的原版蓝色。\n" +
                    "**3. 保持我的自定义颜色**：到处都使用选择的颜色。\n\n" +
                    "这不会覆盖颜色选择器里自动保存的自定义颜色。\n"+
                    "有些玩家觉得拆除时自定义颜色不好看清，所以希望使用工具时能自动恢复更明显的轮廓颜色。"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 推荐" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 原版工具颜色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 保持我的自定义颜色" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "辅助线不透明度（Alpha）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "调整游戏内辅助线（放置道路、道具等时显示的彩色箭头/线条）\n\n" +
                    "**100%** 保持原版默认外观。\n" +
                    "**降低** 会让辅助线更透明。\n" +
                    "**0%** 会完全隐藏它们 - <不推荐>。\n" +           
                    "建议保持在 15% 以上，否则很难看清正在发生什么。\n" +
                    "城市内的模组面板也有同一个滑块。两者会同步；\n" +
                    "如果修改这里，城市内的那个也会自动改变。"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "打开/关闭主面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "用于打开 / 关闭城市内 Hover Colors 面板的快捷键。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "切换 Hover Colors 面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "开启/关闭地表工具预览" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "放置地表时，用于隐藏或恢复当前地表工具边界预览线的快捷键。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "开启/关闭地表工具线条" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "切换预设 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "用于在预设槽 1 和槽 2 之间切换的快捷键。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "在预设 1 和 2 之间切换" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "模组" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "版本" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "打开作者的 Paradox Mods 页面。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "谨以此纪念亲爱的 Mochi。"
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "这个模组献给 Mochi。她是一只深爱着的狗狗，7 岁时被收养，\n" +
                    "带来了 13 年的爱与快乐。没有 Mochi，就不会有这个模组。"
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
