// File: Localization/LocaleZH_HANT.cs
// Purpose: Traditional Chinese (zh-HANT) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("zh-HANT", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/zh-HANT.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "快捷鍵" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "輔助線" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "獻給" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "推土機 + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "控制推土機或道路工具啟用時的臨時輪廓顏色。\n\n" +
                    "**1. 推薦**：拆除時使用遊戲的警告色，道路使用更柔和的原版藍色。\n" +
                    "**2. 原版工具顏色**：這些工具啟用時恢復遊戲正常的原版藍色。\n" +
                    "**3. 保持我的自訂顏色**：到處都使用選擇的顏色。\n\n" +
                    "這不會覆蓋顏色選擇器裡自動儲存的自訂顏色。\n"+
                    "有些玩家覺得拆除時自訂顏色不容易看清，所以希望使用工具時能自動恢復更明顯的輪廓顏色。"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 推薦" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 原版工具顏色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 保持我的自訂顏色" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "輔助線不透明度（Alpha）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "調整遊戲內輔助線（放置道路、道具等時顯示的彩色箭頭/線條）\n\n" +
                    "**100%** 保持原版預設外觀。\n" +
                    "**降低** 會讓輔助線更透明。\n" +
                    "**0%** 會完全隱藏它們 - <不推薦>。\n" +           
                    "建議保持在 15% 以上，否則很難看清正在發生什麼。\n" +
                    "城市內的模組面板也有同一個滑桿。兩者會同步；\n" +
                    "如果修改這裡，城市內的那個也會自動改變。"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "開啟/關閉主面板" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "用於開啟 / 關閉城市內 Hover Colors 面板的快捷鍵。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "切換 Hover Colors 面板" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "開啟/關閉地表工具預覽" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "放置地表時，用於隱藏或恢復目前地表工具邊界預覽線的快捷鍵。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "開啟/關閉地表工具線條" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "切換預設 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "用於在預設槽 1 和槽 2 之間切換的快捷鍵。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "在預設 1 和 2 之間切換" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "模組" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "版本" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "開啟作者的 Paradox Mods 頁面。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "謹以此紀念親愛的 Mochi。"
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "這個模組獻給 Mochi。她是一隻深愛著的狗狗，7 歲時被收養，\n" +
                    "帶來了 13 年的愛與快樂。沒有 Mochi，就不會有這個模組。"
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
