// File: Localization/LocaleTH.cs
// Purpose: Thai (th-TH) strings for the Options Menu.
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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "แผง" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "ปุ่มลัด" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "เส้นไกด์" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "คำอุทิศ" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "รถปรับดิน + ถนน" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "ควบคุมสีเส้นขอบชั่วคราวขณะใช้รถปรับดินหรือเครื่องมือถนน\n\n**1. แนะนำ** ใช้สีเตือนของเกม (สีเหลือง) สำหรับการรื้อถอน และใช้สีน้ำเงิน vanilla ที่นุ่มลงสำหรับถนน\n**2. สีเครื่องมือ vanilla** คืนค่าสีน้ำเงิน vanilla ปกติของเกมเมื่อใช้รถปรับดินหรือเครื่องมือถนน\n**3. ใช้สีที่ฉันกำหนดเองต่อไป** ใช้สีที่คุณเลือกในทุกกรณี\n\nจุดประสงค์: ผู้ใช้/ผู้ทดสอบบางคนมองเห็นสีที่ตั้งเองได้ยากขณะรื้อถอน\nตัวเลือกนี้ให้สีที่มองเห็นชัดขึ้นขณะใช้เครื่องมือ\nไม่เขียนทับสีที่กำหนดเองซึ่งบันทึกอัตโนมัติในตัวเลือกสี" },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. แนะนำ" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. สีเครื่องมือ vanilla" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. ใช้สีที่ฉันกำหนดเองต่อไป" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "เปิดเส้นขอบรายการที่ทับซ้อนกัน" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<แนะนำให้เปิด>\nคงเส้นขอบสีแดงแซลมอนแบบ vanilla ของเกมไว้ เมื่อการวางวัตถุหรือเครือข่ายถูกบล็อกโดยรายการที่ทับซ้อนกัน\nขอบเขตพื้นที่ เช่น ไกด์รัศมีฟาร์มอุตสาหกรรมเฉพาะทาง จะไม่ถูกเปลี่ยน\n\nใช้ได้กับทุกโหมดรถปรับดิน + ถนน และไม่เขียนทับสีที่กำหนดเองที่บันทึกไว้" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "อนุญาตสีที่กำหนดเองสำหรับ NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<แนะนำให้เปิด>\nใช้สี/ความโปร่งใส HC ที่บันทึกไว้เมื่อวางรายละเอียด NetLane เช่น รั้ว แนวพุ่มไม้ เส้นมาร์ก และเครื่องมือแบบเลนที่คล้ายกัน\n\n- ถนนปกติยังคงทำตามการตั้งค่า รถปรับดิน + ถนน ที่คุณเลือกจากรายการดรอปดาวน์\n- ปิดตัวเลือกนี้หากต้องการให้เครื่องมือเหล่านั้นใช้เส้นขอบสีน้ำเงิน vanilla ของเกมแทน\n- สีข้อผิดพลาดจากการทับซ้อนยังคงมีลำดับความสำคัญเมื่อเปิดอยู่ (สีข้อผิดพลาด vanilla = สีแดงแซลมอน)" },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "แผงสีเข้มขึ้น" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "เมื่อเปิด จะใช้ <แผงเข้ม>: ทำมาสำหรับผู้เล่น UI แบบเก่า และใช้ใน Modern UI ได้หากคุณชอบแผงที่เข้มกว่า\nเมื่อปิด จะใช้ <แผงมาตรฐาน>: สไตล์ Hover Colors แบบโปร่งแสงที่กำหนดเอง\n- ดูสว่างและทันสมัยกว่า\n- เหมาะกับผู้เล่นส่วนใหญ่ที่ใช้ UI เกมแบบ Modern ใหม่\n\nลองทั้งสองแบบแล้วเลือกแบบที่ชอบ! ตัวเลือกนี้เปลี่ยนเฉพาะพื้นหลังของแผงมอดนี้ ไม่ใช่ UI ของเกม" },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "สีเส้นไกด์แบบประ" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "ตั้งค่าสีไกด์จัดแนวแบบประที่ใช้สำหรับมุมถนน ตัวช่วย 90 องศา และคำใบ้การเชื่อมต่อ\n\nแถบเลื่อนความทึบทั้งสองซิงค์กัน: แถบเลื่อนใน Options นี้และแถบเลื่อนในแผงเมืองควบคุมความทึบของไกด์แบบประเดียวกัน" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "ขาว vanilla" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "เขียวมองเห็นชัด" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "ฟ้าไซแอน" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "เหลืองมองเห็นชัด" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "ความทึบของเส้นไกด์ (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "ควบคุมความทึบของไกด์จัดแนวแบบประ มีประโยชน์ขณะวางถนน รั้ว พร็อป ฯลฯ\n\n**100%** คงรูปลักษณ์ vanilla เริ่มต้น\n**ต่ำลง** ทำให้เส้นไกด์โปร่งใสมากขึ้น\n**0%** ซ่อนทั้งหมด - <ไม่แนะนำ>\nแนะนำให้อยู่เหนือ 15% มิฉะนั้นจะดูยากว่าเกิดอะไรขึ้น\nแถบเลื่อนเดียวกันอยู่ในแผงมอดในเมือง ทั้งสองซิงค์กัน;\nถ้าเปลี่ยนอันนี้ อันในเมืองก็จะเปลี่ยนตาม" },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "เปิด/ปิดแผงหลัก" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "ปุ่มลัดสำหรับเปิด / ปิดแผงสีวัตถุ Hover ในเมือง" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "สลับแผง Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "สลับพรีวิวเครื่องมือ Surface เปิด/ปิด" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "ปุ่มลัดสำหรับซ่อนหรือคืนเส้นพรีวิวขอบเขตของเครื่องมือ Surface ขณะวางพื้นผิว" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "เลเยอร์พรีวิวเครื่องมือ Surface On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "สลับพรีเซ็ต 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "ปุ่มลัดสำหรับสลับระหว่างช่องพรีเซ็ต 1 และช่อง 2" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "สลับระหว่างพรีเซ็ต 1 และ 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "เปิดหน้า Paradox Mods ของผู้สร้าง" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "แด่ความทรงจำอันเปี่ยมรักของ Mochi" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "มอดนี้อุทิศให้ Mochi เธอเป็นน้องหมาที่รักมาก รับมาเลี้ยงตอนอายุ 7 ปี\nและมอบความรักกับความสุขให้ถึง 13 ปี มอดนี้คงเกิดขึ้นไม่ได้หากไม่มี Mochi" },
            };
        }

        public void Unload()
        {
        }
    }
}
