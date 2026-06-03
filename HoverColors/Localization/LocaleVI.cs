// File: Localization/LocaleVI.cs
// Purpose: Vietnamese (vi-VN) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("vi-VN", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/vi-VN.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleVI : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleVI(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Thao tác" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Giới thiệu" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Cách hoạt động màu công cụ" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Bảng điều khiển" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Phím tắt" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Đường dẫn hướng" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Lời dành tặng" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Ủi phá + Đường" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Điều khiển màu viền tạm thời khi công cụ ủi phá hoặc công cụ đường đang hoạt động\n\n" +
                    "**1. Khuyên dùng** dùng màu cảnh báo của game khi phá dỡ và màu xanh vanilla dịu hơn cho đường\n" +
                    "**2. Màu công cụ vanilla** khôi phục màu xanh vanilla bình thường của game khi các công cụ này đang hoạt động\n" +
                    "**3. Giữ màu tùy chỉnh của tôi** dùng màu đã chọn ở mọi nơi\n\n" +

                    "Mục đích: một số người dùng thấy màu tùy chỉnh khó nhìn khi ủi phá. \\n" +
                    "Tùy chọn này chuyển sang màu dễ thấy khi đang dùng công cụ.\n" +
                    "Tùy chọn này không ghi đè màu tùy chỉnh đã tự động lưu trong bộ chọn màu."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Khuyên dùng" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Màu công cụ vanilla" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Giữ màu tùy chỉnh của tôi" },
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Bật viền khi vật bị chồng lên nhau" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "Bật lại hành vi vanilla bình thường khi đặt vật chồng lên vật khác.\n" +
                    "Dùng viền lỗi màu cá hồi của game khi thử đặt chồng lên nhau.\n\n" +
                    "Hoạt động với mọi chế độ Bulldozer + Roads và không ghi đè màu tùy chỉnh đã lưu."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Bảng tối hơn" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<Bảng tối>: dành cho người dùng Legacy UI; cũng dùng được trong Modern UI nếu thích bảng tối hơn.\n" +
                    "<Bảng chuẩn>: kiểu trong suốt riêng của Hover Colors.\n" +
                    "Nhìn sáng hơn và hiện đại hơn.\n" +
                    "Phù hợp nhất với đa số người chơi dùng Modern UI mới của game.\n" +
                    "Thử cả hai để chọn kiểu thích hơn. Chỉ đổi nền bảng của mod này, không đổi UI của game."
                },
                
                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Độ mờ đường dẫn hướng (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Điều chỉnh đường dẫn hướng trong game (các mũi tên/đường màu hiện khi đặt đường, prop, v.v.)\n\n" +
                    "**100%** giữ giao diện vanilla mặc định\n" +
                    "**Thấp hơn** làm đường dẫn hướng trong suốt hơn\n" +
                    "**0%** ẩn hoàn toàn - <Không khuyên dùng>\n" +           
                    "Nên để trên 15%, nếu không sẽ khó thấy điều đang xảy ra\n" +
                    "Thanh trượt tương tự cũng có trong bảng mod trong thành phố. Cả hai được đồng bộ;\n" +
                    "nếu đổi ở đây, thanh trượt trong thành phố cũng sẽ đổi theo."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Mở/đóng bảng chính" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Phím tắt để mở / đóng bảng màu Hover objects trong thành phố." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Bật/tắt bảng Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Bật/tắt xem trước công cụ Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Phím tắt để ẩn hoặc khôi phục các đường ranh giới xem trước đang hoạt động của công cụ Surface khi đặt bề mặt." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Bật/tắt đường Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Chuyển preset 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Phím tắt để chuyển giữa ô preset 1 và ô 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Chuyển giữa preset 1 và 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Phiên bản" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Mở trang Paradox Mods của tác giả." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Tưởng nhớ Mochi với tất cả yêu thương."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mod này dành tặng Mochi. Em là một chú chó được yêu thương, được nhận nuôi khi 7 tuổi,\n" +
                    "và đã mang đến 13 năm yêu thương cùng niềm vui. Không có Mochi thì mod này đã không thể thành hiện thực."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
