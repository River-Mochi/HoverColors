// File: Localization/LocaleZH_HANS.cs
// Purpose: Simplified Chinese (zh-HANS) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/zh-HANS.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "工具颜色行为" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "面板" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "快捷键" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "引导线" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "献给" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "推土机 + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "当推土机或道路工具处于激活状态时，控制临时轮廓颜色。\n" +
                    "\n" +
                    "**1. 推荐**：拆除时使用游戏的警告色（黄色），道路工具使用更柔和的原版蓝。\n" +
                    "**2. 原版工具颜色**：当推土机或道路工具处于激活状态时，恢复游戏正常的原版蓝。\n" +
                    "**3. 保留我的自定义颜色**：在所有地方使用你选择的颜色。\n" +
                    "\n" +
                    "目的：一些用户/测试者在拆除时觉得自己的自定义颜色不够明显。\n" +
                    "这些选项可在使用工具时提供高可见度颜色。\n" +
                    "这不会覆盖颜色选择器中自动保存的自定义颜色。"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 推荐" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 原版工具颜色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 保留我的自定义颜色" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "启用重叠项目轮廓" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<推荐启用>\n" +
                    "当对象或网络放置因重叠项目而被阻止时，保持游戏原版鲑红色轮廓可见。\n" +
                    "区域限制，例如专业工业农场半径引导，不会被改变。\n" +
                    "\n" +
                    "适用于所有推土机 + 道路模式，并且不会覆盖你保存的自定义颜色。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "允许 NetLanes 使用自定义颜色" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<推荐启用>\n" +
                    "在放置 NetLane 细节项目（如围栏、树篱、标线以及类似的基于车道的工具）时，使用你保存的 HC 颜色/透明度。\n" +
                    "\n" +
                    "- 普通道路仍会遵循你在下拉列表中选择的推土机 + 道路设置。\n" +
                    "- 如果你希望这些工具改用游戏的原版蓝，请关闭此选项。\n" +
                    "- 启用时，重叠错误颜色仍然优先（原版错误颜色 = 鲑红色）。"
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "更暗面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "启用 = <暗色面板>：为 Legacy UI 玩家制作；如果你喜欢更暗的面板，也可以在 Modern UI 中使用。\n" +
                    "禁用 = <标准面板>：自定义半透明 Hover Colors 风格。\n" +
                    "- 更浅、更现代的外观。\n" +
                    "- 最适合使用新版 Modern UI 的大多数玩家。\n" +
                    "\n" +
                    "两个都试试，选择你喜欢的！这只会改变此模组面板的背景，不会改变游戏 UI。"
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "引导线不透明度（Alpha）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "控制虚线对齐引导的不透明度，在放置道路、围栏、道具等时很有用。\n" +
                    "\n" +
                    "**100%** 保持原版默认外观。\n" +
                    "**更低** 会让引导线更透明。\n" +
                    "**0%** 会完全隐藏它们 - <不推荐>。\n" +
                    "建议保持在 15% 以上，否则很难看清发生了什么。\n" +
                    "同一个滑块也在城市中的模组面板里。两者会同步；\n" +
                    "如果你改变这个，城市里的那个也会改变。"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "打开/关闭主面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "用于打开 / 关闭城市中悬停对象颜色面板的快捷键。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "切换 Hover Colors 面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "切换 Surface 工具预览" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "放置表面时，用快捷键隐藏或恢复 Surface 工具的活动边界预览线。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface 预览图层开/关" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "切换预设 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "用于在预设槽 1 和槽 2 之间切换的快捷键。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "在预设 1 和 2 之间切换" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "模组" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "版本" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "打开作者的 Paradox Mods 页面。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "纪念 Mochi。"
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "此模组献给 Mochi。她是一只深受喜爱的狗狗，7 岁时被收养，\n" +
                    "带来了 13 年的爱与快乐。没有 Mochi，就不会有这个模组。"
                },
            };
        }

        public void Unload()
        {
        }
    }
}
