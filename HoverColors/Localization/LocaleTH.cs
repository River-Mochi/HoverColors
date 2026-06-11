// File: Localization/LocaleTH.cs
// Purpose: Thai (th-TH) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/th-TH.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "การกระทำ" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "เกี่ยวกับ" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "พฤติกรรมสีของเครื่องมือ" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "พาเนล" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "ปุ่มลัด" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "เส้นไกด์" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "คำอุทิศ" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + Roads" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "ควบคุมสีขอบชั่วคราวเมื่อเครื่องมือ bulldozer หรือเครื่องมือถนนกำลังใช้งานอยู่\n" +
                    "\n" +
                    "**1. แนะนำ** ใช้สีเตือนของเกม (สีเหลือง) สำหรับการรื้อถอน และใช้สีน้ำเงิน vanilla ที่นุ่มลงสำหรับถนน\n" +
                    "**2. สีเครื่องมือแบบ vanilla** คืนค่าสีน้ำเงิน vanilla ปกติของเกมขณะใช้ bulldozer หรือเครื่องมือถนน\n" +
                    "**3. ใช้สีที่ฉันตั้งเอง** ใช้สีที่คุณเลือกในทุกกรณี\n" +
                    "\n" +
                    "จุดประสงค์: ผู้ใช้/ผู้ทดสอบบางคนมองเห็นสีที่ตั้งเองได้ยากตอนรื้อถอน\n" +
                    "ตัวเลือกนี้ให้สีที่มองเห็นง่ายระหว่างใช้เครื่องมือ\n" +
                    "จะไม่เขียนทับสีที่คุณบันทึกไว้โดยอัตโนมัติในตัวเลือกสี"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. แนะนำ" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. สีเครื่องมือแบบ vanilla" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. ใช้สีที่ฉันตั้งเอง" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "เปิดขอบของรายการที่ซ้อนทับกัน" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<แนะนำให้เปิด>\n" +
                    "คงขอบสีแดงแซลมอนแบบ vanilla ของเกมให้มองเห็น เมื่อการวางวัตถุหรือเครือข่ายถูกขัดขวางโดยรายการที่ซ้อนทับกัน\n" +
                    "ขอบเขตพื้นที่ เช่น ไกด์รัศมีฟาร์มของ Specialized Industry จะไม่ถูกเปลี่ยน\n" +
                    "\n" +
                    "ใช้ได้กับทุกโหมด Bulldozer + Roads และไม่เขียนทับสีที่คุณบันทึกไว้"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "อนุญาตสีที่กำหนดเองสำหรับ NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<แนะนำให้เปิด>\n" +
                    "ใช้สี/ความโปร่งใส HC ที่บันทึกไว้เมื่อวางรายละเอียด NetLane เช่น รั้ว พุ่มไม้ เครื่องหมาย และเครื่องมือแบบเลนที่คล้ายกัน\n" +
                    "\n" +
                    "- ถนนปกติยังคงตามการตั้งค่า Bulldozer + Roads ที่คุณเลือกจากรายการดรอปดาวน์\n" +
                    "- ปิดตัวเลือกนี้ถ้าคุณต้องการให้เครื่องมือเหล่านั้นใช้สีน้ำเงิน vanilla ของเกมแทน\n" +
                    "- สีข้อผิดพลาดจากการซ้อนทับยังคงมีความสำคัญก่อนเมื่อเปิดใช้งาน (สีข้อผิดพลาด vanilla = แดงแซลมอน)"
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "พาเนลเข้มขึ้น" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "เปิด = <พาเนลเข้ม>: ทำไว้สำหรับผู้เล่น Legacy UI; ใช้กับ Modern UI ได้เช่นกันถ้าคุณชอบพาเนลที่เข้มกว่า\n" +
                    "ปิด = <พาเนลมาตรฐาน>: สไตล์ Hover Colors แบบโปร่งแสงที่กำหนดเอง\n" +
                    "- ดูสว่างกว่าและทันสมัยกว่า\n" +
                    "- เหมาะกับผู้เล่นส่วนใหญ่ที่ใช้ Modern UI ใหม่ของเกม\n" +
                    "\n" +
                    "ลองทั้งสองแบบแล้วเลือกแบบที่คุณชอบ! ตัวเลือกนี้เปลี่ยนเฉพาะพื้นหลังของพาเนลม็อดนี้ ไม่ใช่ UI ของเกม"
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "ความทึบของเส้นไกด์ (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "ควบคุมความทึบของเส้นไกด์ประแนวจัดตำแหน่ง มีประโยชน์ตอนวางถนน รั้ว props และอื่น ๆ\n" +
                    "\n" +
                    "**100%** คงรูปลักษณ์ vanilla เริ่มต้น\n" +
                    "**ต่ำกว่า** ทำให้เส้นไกด์โปร่งใสมากขึ้น\n" +
                    "**0%** ซ่อนทั้งหมด - <ไม่แนะนำ>\n" +
                    "แนะนำให้อยู่เหนือ 15% ไม่อย่างนั้นจะมองเห็นสิ่งที่เกิดขึ้นได้ยาก\n" +
                    "สไลเดอร์เดียวกันอยู่ในพาเนลม็อดในเมืองด้วย ทั้งสองซิงก์กัน;\n" +
                    "ถ้าคุณเปลี่ยนอันนี้ อันในเมืองก็จะเปลี่ยนตาม"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "เปิด/ปิดพาเนลหลัก" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "ปุ่มลัดสำหรับเปิด / ปิดพาเนลสีของวัตถุที่ hover ในเมือง" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "สลับพาเนล Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "สลับตัวอย่างเครื่องมือ Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "ปุ่มลัดสำหรับซ่อนหรือคืนค่าเส้นตัวอย่างขอบเขตที่กำลังใช้งานของเครื่องมือ Surface ขณะวางพื้นผิว" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "เลเยอร์ตัวอย่าง Surface On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "สลับ preset 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "ปุ่มลัดสำหรับสลับระหว่างช่อง preset 1 และช่อง 2" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "สลับระหว่าง preset 1 และ 2" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "ม็อด" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "เวอร์ชัน" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "เปิดหน้า Paradox Mods ของผู้สร้าง" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "แด่ความทรงจำของ Mochi."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "ม็อดนี้อุทิศให้ Mochi เธอเป็นสุนัขที่รักมาก รับมาเลี้ยงตอนอายุ 7 ปี\n" +
                    "และมอบความรักกับความสุขให้ 13 ปี ม็อดนี้คงเป็นไปไม่ได้ถ้าไม่มี Mochi"
                },
            };
        }

        public void Unload()
        {
        }
    }
}
