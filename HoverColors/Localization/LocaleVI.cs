// File: Localization/LocaleVI.cs
// Purpose: Vietnamese (vi-VN) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/vi-VN.json.

namespace HoverColors
{
    using Colossal;
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Hành động" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Giới thiệu" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Cách hoạt động màu công cụ" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Bảng điều khiển" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Phím tắt" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Đường dẫn hướng" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Tưởng nhớ" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + Đường" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Điều khiển màu viền tạm thời khi công cụ bulldozer hoặc công cụ đường đang hoạt động.\n\n**1. Khuyên dùng** dùng màu Cảnh báo của game (vàng) cho phá dỡ và màu xanh vanilla dịu hơn cho đường.\n**2. Màu công cụ vanilla** khôi phục màu xanh vanilla bình thường của game khi dùng bulldozer hoặc công cụ đường.\n**3. Giữ màu tùy chỉnh của tôi** dùng màu bạn đã chọn ở mọi nơi.\n\nMục đích: một số người dùng/người thử nghiệm thấy màu tùy chỉnh khó nhìn khi phá dỡ.\nTùy chọn này cung cấp màu dễ nhìn hơn khi dùng công cụ.\nKhông ghi đè màu tùy chỉnh đã tự động lưu trong bộ chọn màu." },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Khuyên dùng" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Màu công cụ vanilla" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Giữ màu tùy chỉnh của tôi" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Bật viền cho mục chồng lấn" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<Nên bật>\nGiữ viền đỏ cá hồi vanilla của game khi việc đặt vật thể hoặc mạng bị chặn do các mục chồng lấn.\nCác giới hạn khu vực, như vòng bán kính trang trại Công nghiệp chuyên biệt, được giữ nguyên.\n\nHoạt động với mọi chế độ Bulldozer + Đường và không ghi đè màu tùy chỉnh đã lưu." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Cho phép màu tùy chỉnh cho NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<Nên bật>\nDùng màu/độ trong suốt HC đã lưu khi đặt các chi tiết NetLane như hàng rào, hàng cây, vạch kẻ và các công cụ theo làn tương tự.\n\n- Đường bình thường vẫn theo cài đặt Bulldozer + Đường bạn chọn từ danh sách thả xuống.\n- Tắt nếu bạn muốn các công cụ đó dùng màu viền xanh vanilla của game.\n- Màu lỗi chồng lấn vẫn được ưu tiên khi bật (màu lỗi vanilla = đỏ cá hồi)." },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Bảng tối hơn" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Bật dùng <Bảng tối>: dành cho người chơi dùng UI cũ; cũng có thể dùng trong Modern UI nếu bạn thích bảng tối hơn.\nTắt dùng <Bảng chuẩn>: phong cách Hover Colors trong mờ tùy chỉnh.\n- Giao diện sáng hơn, hiện đại hơn.\n- Tốt nhất cho đa số người chơi dùng UI game Modern mới.\n\nHãy thử cả hai và chọn cái bạn thích! Cài đặt này chỉ đổi nền của bảng mod này, không đổi UI của game." },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Màu đường dẫn hướng nét đứt" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Đặt màu cho đường dẫn hướng căn chỉnh nét đứt dùng cho góc đường, trợ giúp 90 độ và gợi ý kết nối.\n\nHai thanh trượt độ mờ được đồng bộ: thanh trong Options và thanh trong bảng trong thành phố điều khiển cùng độ mờ của đường dẫn hướng nét đứt." },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "Trắng vanilla" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "Xanh lá dễ nhìn" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "Xanh cyan" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "Vàng dễ nhìn" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Độ mờ đường dẫn hướng (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Điều khiển độ mờ của đường dẫn hướng căn chỉnh nét đứt, hữu ích khi đặt đường, hàng rào, props, v.v.\n\n**100%** giữ giao diện vanilla mặc định.\n**Thấp hơn** làm đường dẫn hướng trong suốt hơn.\n**0%** ẩn hoàn toàn - <Không khuyên dùng>.\nNên để trên 15%, nếu không sẽ khó nhìn chuyện gì đang xảy ra.\nThanh trượt tương tự cũng nằm trong bảng mod trong thành phố. Hai thanh được đồng bộ;\nnếu đổi thanh này, thanh trong thành phố cũng đổi theo." },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Mở/đóng bảng chính" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Phím tắt để mở / đóng Bảng màu đối tượng Hover trong thành phố." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Bật/tắt bảng Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Bật/tắt xem trước công cụ Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Phím tắt để ẩn hoặc khôi phục các đường biên xem trước đang hoạt động của công cụ Surface khi đặt bề mặt." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Lớp xem trước công cụ Surface On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Chuyển preset 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Phím tắt để chuyển giữa ô preset 1 và ô 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Chuyển giữa preset 1 và 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Mở trang Paradox Mods của tác giả." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Tưởng nhớ Mochi bằng tất cả yêu thương." },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Mod này được dành tặng cho Mochi. Cô bé là một chú chó được yêu thương, được nhận nuôi khi 7 tuổi,\nvà đã mang đến 13 năm tình yêu cùng niềm vui. Mod này sẽ không thể có nếu thiếu Mochi." },
            };
        }

        public void Unload()
        {
        }
    }
}
