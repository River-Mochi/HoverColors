// File: Localization/LocaleZH_HANT.cs
// Purpose: Traditional Chinese (zh-HANT) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/zh-HANT.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "動作" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "關於" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "工具顏色行為" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "面板" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "快捷鍵" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "引導線" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "獻給" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "推土機 + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "當推土機或道路工具啟用時，控制暫時的輪廓顏色。\n" +
                    "\n" +
                    "**1. 建議**：拆除時使用遊戲的警告色（黃色），道路工具使用較柔和的原版藍。\n" +
                    "**2. 原版工具顏色**：當推土機或道路工具啟用時，恢復遊戲正常的原版藍。\n" +
                    "**3. 保留我的自訂顏色**：在所有地方使用你選擇的顏色。\n" +
                    "\n" +
                    "目的：部分使用者/測試者在拆除時覺得自訂顏色不夠明顯。\n" +
                    "這些選項能在使用工具時提供高可見度顏色。\n" +
                    "這不會覆蓋顏色選擇器中自動儲存的自訂顏色。"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 建議" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 原版工具顏色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 保留我的自訂顏色" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "啟用重疊項目輪廓" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<建議啟用>\n" +
                    "當物件或網路放置因重疊項目而被阻擋時，保持遊戲原版鮭紅色輪廓可見。\n" +
                    "區域限制，例如專精工業農場半徑引導，不會被改變。\n" +
                    "\n" +
                    "適用於所有推土機 + 道路模式，且不會覆蓋你儲存的自訂顏色。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "允許 NetLanes 使用自訂顏色" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<建議啟用>\n" +
                    "放置 NetLane 細節項目（例如圍欄、樹籬、標線及類似的車道型工具）時，使用你儲存的 HC 顏色/透明度。\n" +
                    "\n" +
                    "- 一般道路仍會遵循你在下拉選單中選擇的推土機 + 道路設定。\n" +
                    "- 如果你希望這些工具改用遊戲的原版藍，請停用此選項。\n" +
                    "- 啟用時，重疊錯誤顏色仍然優先（原版錯誤顏色 = 鮭紅色）。"
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "更暗面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "啟用 = <暗色面板>：為 Legacy UI 玩家製作；如果你偏好更暗的面板，也可在 Modern UI 中使用。\n" +
                    "停用 = <標準面板>：自訂半透明 Hover Colors 風格。\n" +
                    "- 更明亮、更現代的外觀。\n" +
                    "- 最適合使用新版 Modern UI 的大多數玩家。\n" +
                    "\n" +
                    "兩個都試試，選擇你喜歡的！這只會改變此模組面板的背景，不會改變遊戲 UI。"
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "引導線不透明度（Alpha）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "控制虛線對齊引導的不透明度，放置道路、圍欄、道具等時很有用。\n" +
                    "\n" +
                    "**100%** 保持原版預設外觀。\n" +
                    "**更低** 會讓引導線更透明。\n" +
                    "**0%** 會完全隱藏 - <不建議>。\n" +
                    "建議保持在 15% 以上，否則很難看清發生了什麼。\n" +
                    "同一個滑桿也在城市中的模組面板裡。兩者會同步；\n" +
                    "如果你改變這個，城市裡的那個也會改變。"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "開啟/關閉主面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "用於開啟 / 關閉城市中游標懸停物件顏色面板的快捷鍵。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "切換 Hover Colors 面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "切換 Surface 工具預覽" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "放置表面時，用快捷鍵隱藏或恢復 Surface 工具的作用中邊界預覽線。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface 預覽圖層開/關" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "切換預設 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "用於在預設槽 1 和槽 2 之間切換的快捷鍵。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "在預設 1 和 2 之間切換" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "模組" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "版本" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "開啟作者的 Paradox Mods 頁面。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "紀念 Mochi。"
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "此模組獻給 Mochi。她是一隻深受喜愛的狗狗，7 歲時被收養，\n" +
                    "帶來了 13 年的愛與快樂。沒有 Mochi，就不會有這個模組。"
                },
            };
        }

        public void Unload()
        {
        }
    }
}
