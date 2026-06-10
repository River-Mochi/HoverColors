// File: Localization/LocaleZH_HANT.cs
// Purpose: Traditional Chinese (zh-HANT) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/zh-HANT.json.

namespace HoverColors
{
    using Colossal;
    using System.Collections.Generic;

    public sealed class LocaleZH_HANT : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleZH_HANT(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "關於" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "工具顏色行為" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "面板" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "按鍵綁定" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "輔助線" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "獻詞" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "推土機 + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "控制推土機或道路工具啟用時的臨時輪廓顏色。\n\n" +
                    "**1. 推薦** 拆除用遊戲警告色（黃色），道路用較柔和的原版藍色。\n" +
                    "**2. 原版工具顏色** 使用推土機或道路工具時，恢復遊戲原版藍色。\n" +
                    "**3. 保持我的自訂顏色** 到處都用你選的顏色。\n\n" +

                    "用途：有些玩家拆除時覺得自訂顏色不好看清。\n" +
                    "這裡提供工具使用時更醒目的顏色。\n" +
                    "不會覆蓋色彩選擇器中自動儲存的自訂顏色。"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 推薦" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 原版工具顏色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 保持我的自訂顏色" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "啟用重疊物品輪廓" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<建議啟用>\n" +
                    "物件或網路放置被重疊物品擋住時，保留遊戲原版鮭紅色輪廓。\n" +
                    "區域限制，例如專業工業農場半徑輔助線，會保持原樣。\n\n" +
                    "適用所有推土機 + 道路模式，也不會覆蓋已儲存的自訂顏色。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "允許 NetLanes 使用自訂顏色" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<建議啟用>\n" +
                    "放置 NetLane 細節物件時，使用你儲存的 HC 顏色/透明度，例如圍欄、樹籬、標線和類似的車道工具。\n\n" +
                    "- 一般道路仍依照你在下拉選單選的推土機 + 道路設定。\n" +
                    "- 若想讓這些工具使用遊戲原版藍色輪廓，請關閉此選項。\n" +
                    "- 啟用時，重疊錯誤顏色仍會優先（原版錯誤色 = 鮭紅色）。"
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "較深色面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "啟用使用 <深色面板>：給舊版 UI 玩家用；如果你喜歡深色面板，Modern UI 也可用。\n" +
                    "關閉使用 <標準面板>：Hover Colors 自訂半透明樣式。\n" +
                    "- 較明亮、較現代。\n" +
                    "- 適合大多數使用新版 Modern UI 的玩家。\n\n" +
                    "兩種都試試看！這只改本模組面板背景，不會改遊戲 UI。"
                },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "虛線輔助線顏色" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)),
                    "設定道路角度、90 度輔助和連接提示用的虛線對齊輔助線顏色。\n\n" +
                    "兩個透明度滑桿會同步：Options 裡的滑桿和城市面板滑桿控制同一個虛線輔助線透明度。"
                },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "原版白色" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "高可見度綠色" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "青藍色" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "高可見度黃色" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "輔助線透明度（Alpha）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "控制虛線對齊輔助線透明度，放置道路、圍欄、道具等時很有用。\n\n" +
                    "**100%** 保持原版預設外觀。\n" +
                    "**降低** 讓輔助線更透明。\n" +
                    "**0%** 完全隱藏 - <不建議>。\n" +
                    "建議保持 15% 以上，不然很難看清楚。\n" +
                    "城市模組面板也有同一個滑桿。兩邊會同步；\n" +
                    "改這個，城市裡的也會一起改。"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "開啟/關閉主面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "開啟 / 關閉城市內 Hover 物件顏色面板的快捷鍵。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "切換 Hover Colors 面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "切換 Surface 工具預覽" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "放置 Surface 時，用快捷鍵隱藏或恢復目前的 Surface 工具邊界預覽線。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface 工具預覽層 On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "切換預設 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "在預設槽 1 和槽 2 之間切換的快捷鍵。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "在預設 1 和 2 之間切換" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "開啟作者的 Paradox Mods 頁面。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "謹以此紀念 Mochi。"
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "此模組獻給 Mochi。她是一隻深受喜愛的狗狗，7 歲時被領養，\n" +
                    "帶來了 13 年的愛與快樂。沒有 Mochi，就不會有這個模組。"
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
