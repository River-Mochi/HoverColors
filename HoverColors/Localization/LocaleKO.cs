// File: Localization/LocaleKO.cs
// Purpose: Korean (ko-KR) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/ko-KR.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleKO : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleKO(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "동작" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "정보" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "도구 색상 동작" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "패널" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "키 바인딩" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "가이드라인" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "헌정" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "불도저 + 도로" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "불도저 또는 도로 도구가 활성화되어 있을 때 임시 윤곽선 색상을 제어합니다.\n\n**1. 권장**은 철거에는 게임의 경고 색상(노란색)을, 도로에는 더 부드러운 바닐라 파란색을 사용합니다.\n**2. 바닐라 도구 색상**은 불도저 또는 도로 도구가 활성화되어 있을 때 게임의 기본 바닐라 파란색으로 되돌립니다.\n**3. 내 사용자 색상 유지**는 선택한 색상을 모든 곳에 사용합니다.\n\n목적: 일부 사용자/테스터는 철거 중 사용자 색상이 잘 보이지 않는다고 느꼈습니다.\n도구 사용 중 잘 보이는 고시인성 색상 옵션을 제공합니다.\n컬러 피커에 자동 저장된 사용자 색상은 덮어쓰지 않습니다." },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 권장" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 바닐라 도구 색상" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 내 사용자 색상 유지" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "겹치는 항목 윤곽선 활성화" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<활성화 권장>\n오브젝트 또는 네트워크 배치가 겹치는 항목 때문에 막힐 때 게임의 바닐라 연어색 빨간 윤곽선을 계속 표시합니다.\n전문화 산업 농장 반경 가이드 같은 영역 제한은 그대로 둡니다.\n\n모든 불도저 + 도로 모드에서 작동하며 저장된 사용자 색상을 덮어쓰지 않습니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "NetLanes에 사용자 색상 허용" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<활성화 권장>\n울타리, 생울타리, 표시 등 레인 기반 NetLane 세부 항목을 배치할 때 저장된 HC 색상/투명도를 사용합니다.\n\n- 일반 도로는 드롭다운에서 선택한 불도저 + 도로 설정을 계속 따릅니다.\n- 해당 도구에 게임의 바닐라 파란 윤곽선을 사용하고 싶으면 비활성화하세요.\n- 활성화된 경우 겹침 오류 색상이 계속 우선합니다(바닐라 오류 색상 = 연어색 빨강)." },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "더 어두운 패널" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "활성화하면 <어두운 패널>을 사용합니다: 레거시 UI 플레이어용으로 만들었지만, 더 어두운 패널을 선호한다면 Modern UI에서도 사용할 수 있습니다.\n비활성화하면 <표준 패널>을 사용합니다: Hover Colors의 사용자 지정 반투명 스타일입니다.\n- 더 밝고 현대적인 느낌.\n- 새 Modern 게임 UI를 사용하는 대부분의 플레이어에게 적합합니다.\n\n둘 다 사용해 보고 더 마음에 드는 것을 선택하세요! 이 설정은 이 모드 패널의 배경만 변경하며 게임 UI는 변경하지 않습니다." },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "점선 가이드 라인 색상" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "도로 각도, 90도 보조선, 연결 힌트에 사용되는 점선 정렬 가이드 색상을 설정합니다.\n\n두 불투명도 슬라이더는 동기화됩니다. 이 옵션 슬라이더와 도시 패널 슬라이더는 같은 점선 가이드 불투명도를 제어합니다." },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "바닐라 흰색" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "고시인성 초록색" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "시안 블루" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "고시인성 노란색" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "가이드라인 불투명도(알파)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "도로, 울타리, 소품 등을 배치할 때 유용한 점선 정렬 가이드의 불투명도를 제어합니다.\n\n**100%**는 바닐라 기본 모습을 유지합니다.\n**낮게** 설정하면 가이드라인이 더 투명해집니다.\n**0%**는 완전히 숨깁니다 - <권장하지 않음>.\n무슨 일이 일어나는지 보기 어렵기 때문에 15% 이상을 권장합니다.\n같은 슬라이더가 도시 모드 패널에도 있습니다. 둘은 동기화됩니다.\n이 값을 바꾸면 도시 안의 슬라이더도 함께 바뀝니다." },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "메인 패널 열기/닫기" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "도시 안 Hover 오브젝트 색상 패널을 열거나 닫는 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors 패널 전환" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface 도구 미리보기 켜기/끄기" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface 배치 중 활성 Surface 도구 경계 미리보기 선을 숨기거나 복원하는 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface 도구 미리보기 레이어 On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "프리셋 1+2 전환" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "프리셋 슬롯 1과 슬롯 2 사이를 전환하는 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "프리셋 1과 2 전환" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "작성자의 Paradox Mods 페이지를 엽니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "사랑하는 Mochi를 기리며." },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "이 모드는 Mochi에게 바칩니다. Mochi는 7살에 입양된 사랑스러운 강아지였고,\n13년 동안 사랑과 기쁨을 주었습니다. 이 모드는 Mochi 없이는 가능하지 않았습니다." },
            };
        }

        public void Unload()
        {
        }
    }
}
