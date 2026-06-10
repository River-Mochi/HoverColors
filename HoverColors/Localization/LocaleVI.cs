// File: Localization/LocaleVI.cs
// Purpose: Vietnamese (vi-VN) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/vi-VN.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Hành động" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Giới thiệu" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Cách hoạt động màu công cụ" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Bảng điều khiển" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Phím tắt" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Đường hướng dẫn" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Tưởng nhớ" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + đường" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Điều khiển màu viền tạm thời khi bulldozer hoặc công cụ đường đang hoạt động.\n" +
                    "\n" +
                    "**1. Khuyến nghị** dùng màu Cảnh báo của game (vàng) cho phá dỡ và màu xanh vanilla nhẹ hơn cho đường.\n" +
                    "**2. Màu công cụ vanilla** khôi phục màu xanh vanilla bình thường của game khi bulldozer hoặc công cụ đường đang hoạt động.\n" +
                    "**3. Giữ màu tùy chỉnh của tôi** dùng màu bạn chọn ở mọi nơi.\n" +
                    "\n" +
                    "Mục đích: một số người dùng/tester thấy màu tùy chỉnh khó nhìn khi phá dỡ.\n" +
                    "Tùy chọn này cung cấp màu dễ thấy hơn khi dùng công cụ.\n" +
                    "Nó không ghi đè màu tùy chỉnh đã được lưu tự động trong bộ chọn màu."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Khuyến nghị" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Màu công cụ vanilla" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Giữ màu tùy chỉnh của tôi" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Bật viền cho mục chồng lấn" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Khuyến nghị bật>\n" +
                    "Giữ viền đỏ cá hồi vanilla của game hiển thị khi việc đặt vật thể hoặc mạng lưới bị chặn do chồng lấn.\n" +
                    "Giới hạn khu vực, như hướng dẫn bán kính trang trại Công nghiệp Chuyên biệt, không bị thay đổi.\n" +
                    "\n" +
                    "Hoạt động với mọi chế độ Bulldozer + đường và không ghi đè màu tùy chỉnh đã lưu của bạn."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Cho phép màu tùy chỉnh cho NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Khuyến nghị bật>\n" +
                    "Dùng màu/độ trong suốt HC đã lưu khi đặt chi tiết NetLane như hàng rào, hàng cây, vạch kẻ và các công cụ tương tự dựa trên làn.\n" +
                    "\n" +
                    "- Đường bình thường vẫn theo thiết lập Bulldozer + đường bạn chọn trong danh sách thả xuống.\n" +
                    "- Tắt tùy chọn này nếu muốn các công cụ đó dùng màu xanh vanilla của game.\n" +
                    "- Màu lỗi chồng lấn vẫn được ưu tiên khi bật (màu lỗi vanilla = đỏ cá hồi)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Bảng tối hơn" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Bật = <Bảng tối>: dành cho người chơi Legacy UI; cũng có thể dùng trong Modern UI nếu bạn thích bảng tối hơn.\n" +
                    "Tắt = <Bảng tiêu chuẩn>: phong cách Hover Colors trong mờ tùy chỉnh.\n" +
                    "- Giao diện sáng hơn, hiện đại hơn.\n" +
                    "- Tốt nhất cho đa số người chơi dùng Modern UI mới của game.\n" +
                    "\n" +
                    "Hãy thử cả hai và chọn kiểu bạn thích! Tùy chọn này chỉ đổi nền của bảng mod này, không đổi UI của game."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Độ mờ đường hướng dẫn (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Điều khiển độ mờ của đường hướng dẫn căn chỉnh nét đứt, hữu ích khi đặt đường, hàng rào, prop, v.v.\n" +
                    "\n" +
                    "**100%** giữ giao diện vanilla mặc định.\n" +
                    "**Thấp hơn** làm đường hướng dẫn trong suốt hơn.\n" +
                    "**0%** ẩn hoàn toàn - <Không khuyến nghị>.\n" +
                    "Nên giữ trên 15%, nếu không sẽ khó nhìn chuyện gì đang xảy ra.\n" +
                    "Thanh trượt tương tự nằm trong bảng mod ở thành phố. Cả hai được đồng bộ;\n" +
                    "nếu bạn đổi thanh này, thanh trong thành phố cũng đổi theo."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Mở/đóng bảng chính" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Phím tắt để mở / đóng bảng màu đối tượng đang hover trong thành phố." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Bật/tắt bảng Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Bật/tắt xem trước công cụ Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Phím tắt để ẩn hoặc khôi phục các đường xem trước ranh giới đang hoạt động của công cụ Surface khi đặt bề mặt." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Lớp xem trước Surface On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Chuyển preset 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Phím tắt để chuyển giữa ô preset 1 và ô 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Chuyển giữa preset 1 và 2" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Phiên bản" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Mở trang Paradox Mods của tác giả." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Tưởng nhớ Mochi."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mod này dành tặng Mochi. Cô bé là một chú chó được yêu thương, được nhận nuôi khi 7 tuổi,\n" +
                    "và đã mang đến 13 năm tình yêu cùng niềm vui. Mod này sẽ không thể có nếu thiếu Mochi."
                },
            };
        }

        public void Unload()
        {
        }
    }
}
