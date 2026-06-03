// File: Localization/LocaleTH.cs
// Purpose: Thai (th-TH) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("th-TH", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/th-TH.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleTH : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleTH(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "การทำงาน" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "เกี่ยวกับ" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "พฤติกรรมสีของเครื่องมือ" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "ปุ่มลัด" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "เส้นไกด์" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "คำอุทิศ" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "บูลโดเซอร์ + ถนน" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "ควบคุมสีเส้นขอบชั่วคราวเมื่อใช้บูลโดเซอร์หรือเครื่องมือถนน\n\n" +
                    "**1. แนะนำ** ใช้สีเตือนของเกมสำหรับการรื้อถอน และใช้สีน้ำเงินวานิลลาที่นุ่มลงสำหรับถนน\n" +
                    "**2. สีเครื่องมือวานิลลา** คืนค่าสีน้ำเงินวานิลลาปกติของเกมขณะใช้เครื่องมือเหล่านี้\n" +
                    "**3. ใช้สีที่กำหนดเองต่อไป** ใช้สีที่เลือกไว้ทุกที่\n\n" +

                    "จุดประสงค์: ผู้ใช้บางคนมองเห็นสีที่กำหนดเองได้ยากขณะใช้บูลโดเซอร์ \\n" +
                    "ตัวเลือกนี้จะสลับเป็นสีที่มองเห็นชัดระหว่างใช้เครื่องมือ\n" +
                    "ค่านี้จะไม่เขียนทับสีที่กำหนดเองซึ่งบันทึกอัตโนมัติในตัวเลือกสี"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. แนะนำ" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. สีเครื่องมือวานิลลา" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. ใช้สีที่กำหนดเองต่อไป" },
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "เปิดเส้นขอบเมื่อวางทับกัน" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "เปิดพฤติกรรมปกติของเกมเมื่อวางไอเท็มทับกับไอเท็มอื่น\n" +
                    "ใช้เส้นขอบแจ้งข้อผิดพลาดสีแซลมอนของเกมเมื่อพยายามวางทับกัน\n\n" +
                    "ใช้ได้กับทุกโหมด Bulldozer + Roads และไม่เขียนทับสีที่คุณบันทึกไว้"
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "แผงสีเข้มขึ้น" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<แผงเข้ม>: ทำมาสำหรับผู้เล่นที่ใช้ Legacy UI และใช้ใน Modern UI ได้ถ้าชอบแผงเข้มกว่า\n" +
                    "<แผงมาตรฐาน>: สไตล์โปร่งแสงเฉพาะของ Hover Colors\n" +
                    "ดูสว่างและทันสมัยกว่า\n" +
                    "เหมาะกับผู้เล่นส่วนใหญ่ที่ใช้ Modern UI ใหม่ของเกม\n" +
                    "ลองทั้งสองแบบแล้วเลือกที่ชอบ สิ่งนี้เปลี่ยนแค่พื้นหลังแผงของม็อดนี้ ไม่ได้เปลี่ยน UI เกม"
                },
                
                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "ความทึบของเส้นไกด์ (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "ปรับขนาดความทึบของเส้นไกด์ในเกม (ลูกศร/เส้นสีที่แสดงขณะวางถนน พร็อพ และอื่น ๆ)\n\n" +
                    "**100%** คงรูปลักษณ์วานิลลาเริ่มต้นไว้\n" +
                    "**ลดลง** ทำให้เส้นไกด์โปร่งใสมากขึ้น\n" +
                    "**0%** ซ่อนทั้งหมด - <ไม่แนะนำ>\n" +           
                    "แนะนำให้ใช้มากกว่า 15% ไม่อย่างนั้นจะดูยากว่าเกิดอะไรขึ้น\n" +
                    "สไลเดอร์เดียวกันนี้มีในแผงม็อดภายในเมืองด้วย ทั้งสองตำแหน่งซิงก์กัน;\n" +
                    "ถ้าเปลี่ยนค่านี้ ค่าในเมืองก็จะเปลี่ยนตามอย่างสะดวก"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "เปิด/ปิดแผงหลัก" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "ปุ่มลัดสำหรับเปิด / ปิดแผงสีวัตถุ Hover Colors ภายในเมือง" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "สลับแผง Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "เปิด/ปิดตัวอย่างเครื่องมือ Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "ปุ่มลัดสำหรับซ่อนหรือคืนค่าเส้นขอบตัวอย่างของเครื่องมือ Surface ขณะวางพื้นผิว" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "เปิด/ปิดเส้นของเครื่องมือ Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "สลับพรีเซ็ต 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "ปุ่มลัดสำหรับสลับระหว่างช่องพรีเซ็ต 1 และช่อง 2" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "สลับระหว่างพรีเซ็ต 1 และ 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "ม็อด" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "เวอร์ชัน" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "เปิดหน้า Paradox Mods ของผู้สร้าง" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "เพื่อระลึกถึง Mochi ด้วยความรัก"
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "ม็อดนี้อุทิศให้ Mochi เธอเป็นสุนัขแสนรักที่ถูกรับเลี้ยงตอนอายุ 7 ปี,\n" +
                    "และมอบความรักกับความสุขให้ 13 ปี หากไม่มี Mochi ม็อดนี้คงเกิดขึ้นไม่ได้"
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
