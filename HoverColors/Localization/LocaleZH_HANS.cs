// File: Localization/LocaleZH_HANS.cs
// Purpose: Simplified Chinese (zh-HANS) strings for the Options Menu.
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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "面板" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "按键绑定" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "辅助线" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "献词" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "推土机 + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "控制推土机或道路工具启用时的临时轮廓颜色。\n\n**1. 推荐**：拆除时使用游戏的警告色（黄色），道路使用更柔和的原版蓝色。\n**2. 原版工具颜色**：推土机或道路工具启用时恢复游戏正常的原版蓝色。\n**3. 保持我的自定义颜色**：所有地方都使用你选择的颜色。\n\n用途：有些用户/测试者觉得拆除时自定义颜色不容易看清。\n这些选项可在使用工具时提供高可见度颜色。\n不会覆盖颜色选择器中自动保存的自定义颜色。" },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 推荐" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 原版工具颜色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 保持我的自定义颜色" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "启用重叠物品轮廓" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<建议启用>\n当对象或网络放置被重叠物品阻挡时，保留游戏原版的鲑红色轮廓。\n区域限制（例如专业工业农场半径引导线）保持不变。\n\n适用于所有推土机 + 道路模式，并且不会覆盖已保存的自定义颜色。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "允许 NetLanes 使用自定义颜色" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<建议启用>\n放置 NetLane 细节物品时使用已保存的 HC 颜色/透明度，例如围栏、树篱、标线以及类似的基于车道的工具。\n\n- 普通道路仍会遵循你在下拉列表中选择的推土机 + 道路设置。\n- 如果希望这些工具改用游戏原版蓝色轮廓，请关闭此项。\n- 启用时，重叠错误颜色仍然优先（原版错误颜色 = 鲑红色）。" },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "更深色面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "启用时使用 <深色面板>：为旧版 UI 玩家制作；如果你喜欢更深的面板，也可在 Modern UI 中使用。\n关闭时使用 <标准面板>：Hover Colors 自定义半透明样式。\n- 更明亮、更现代的外观。\n- 适合大多数使用新版 Modern 游戏 UI 的玩家。\n\n两种都试试，选择你喜欢的！这只会更改此模组面板的背景，不会更改游戏 UI。" },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "虚线辅助线颜色" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "设置道路角度、90 度辅助和连接提示所使用的虚线对齐辅助线颜色。\n\n两个不透明度滑块会同步：此选项滑块和城市内面板滑块控制同一个虚线辅助线不透明度。" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "原版白色" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "高可见度绿色" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "青蓝色" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "高可见度黄色" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "辅助线不透明度（Alpha）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "控制虚线对齐辅助线的不透明度，放置道路、围栏、道具等时很有用。\n\n**100%** 保持原版默认外观。\n**降低** 会让辅助线更透明。\n**0%** 会完全隐藏 - <不推荐>。\n建议保持在 15% 以上，否则很难看清发生了什么。\n城市内模组面板也有相同滑块。两者会同步；\n更改这个滑块时，城市内的滑块也会随之更改。" },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "打开/关闭主面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "用于打开 / 关闭城市内 Hover 对象颜色面板的快捷键。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "切换 Hover Colors 面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "开关 Surface 工具预览" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "放置 Surface 时，用快捷键隐藏或恢复当前 Surface 工具边界预览线。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface 工具预览层 On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "切换预设 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "用于在预设槽 1 和槽 2 之间切换的快捷键。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "在预设 1 和 2 之间切换" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "打开作者的 Paradox Mods 页面。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "谨以此纪念 Mochi。" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "此模组献给 Mochi。她是一只深受喜爱的狗狗，7 岁时被领养，\n带来了 13 年的爱与快乐。没有 Mochi，就不会有这个模组。" },
            };
        }

        public void Unload()
        {
        }
    }
}
