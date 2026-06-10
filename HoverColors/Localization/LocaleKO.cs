// File: Localization/LocaleKO.cs
// Purpose: Korean (ko-KR) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/ko-KR.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "도구 색상 동작" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "패널" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "키 바인딩" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "가이드라인" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "헌정" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "불도저 + 도로" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "불도저 또는 도로 도구가 활성화된 동안 임시 외곽선 색상을 제어합니다.\n" +
                    "\n" +
                    "**1. 권장**은 철거에는 게임의 경고 색상(노란색)을, 도로에는 더 부드러운 바닐라 블루를 사용합니다.\n" +
                    "**2. 바닐라 도구 색상**은 불도저 또는 도로 도구가 활성화된 동안 게임의 일반 바닐라 블루를 복원합니다.\n" +
                    "**3. 내 사용자 지정 색상 유지**는 선택한 색상을 모든 곳에 사용합니다.\n" +
                    "\n" +
                    "목적: 일부 사용자/테스터는 철거 중 사용자 지정 색상이 잘 보이지 않는다고 느낍니다.\n" +
                    "도구 사용 중 높은 가시성 색상을 선택할 수 있게 합니다.\n" +
                    "색상 선택기에 자동 저장된 사용자 지정 색상은 덮어쓰지 않습니다."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 권장" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 바닐라 도구 색상" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 내 사용자 지정 색상 유지" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "겹친 항목 외곽선 활성화" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<활성화 권장>\n" +
                    "오브젝트 또는 네트워크 배치가 겹친 항목 때문에 막혔을 때 게임의 바닐라 연어색 빨간 외곽선을 계속 표시합니다.\n" +
                    "전문화 산업 농장 반경 가이드 같은 영역 제한은 그대로 둡니다.\n" +
                    "\n" +
                    "모든 불도저 + 도로 모드에서 작동하며 저장된 사용자 지정 색상을 덮어쓰지 않습니다."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "NetLanes에 사용자 지정 색상 허용" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<활성화 권장>\n" +
                    "울타리, 생울타리, 표시선 및 비슷한 차선 기반 도구 같은 NetLane 세부 요소를 배치할 때 저장된 HC 색상/투명도를 사용합니다.\n" +
                    "\n" +
                    "- 일반 도로는 드롭다운에서 선택한 불도저 + 도로 설정을 계속 따릅니다.\n" +
                    "- 해당 도구가 게임의 바닐라 블루를 사용하길 원하면 이 옵션을 끄세요.\n" +
                    "- 겹침 오류 색상은 활성화되어 있으면 계속 우선합니다(바닐라 오류 색상 = 연어색 빨강)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "더 어두운 패널" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "활성화 = <어두운 패널>: Legacy UI 플레이어를 위해 만들었습니다. 더 어두운 패널을 원하면 Modern UI에서도 사용할 수 있습니다.\n" +
                    "비활성화 = <표준 패널>: 사용자 지정 반투명 Hover Colors 스타일입니다.\n" +
                    "- 더 밝고 현대적인 모습입니다.\n" +
                    "- 새로운 Modern UI를 사용하는 대부분의 플레이어에게 가장 좋습니다.\n" +
                    "\n" +
                    "둘 다 사용해 보고 원하는 것을 선택하세요! 이 설정은 게임 UI가 아니라 이 모드 패널의 배경만 변경합니다."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "가이드라인 불투명도(alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "도로, 울타리, 프롭 등을 배치할 때 유용한 점선 정렬 가이드의 불투명도를 제어합니다.\n" +
                    "\n" +
                    "**100%**는 바닐라 기본 모양을 유지합니다.\n" +
                    "**낮게** 하면 가이드라인이 더 투명해집니다.\n" +
                    "**0%**는 완전히 숨깁니다 - <권장하지 않음>.\n" +
                    "무슨 일이 일어나는지 보기 어려워지므로 15% 이상을 권장합니다.\n" +
                    "같은 슬라이더가 도시 내 모드 패널에도 있습니다. 둘은 동기화됩니다;\n" +
                    "이 값을 변경하면 도시 내 슬라이더도 함께 변경됩니다."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "메인 패널 열기/닫기" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "도시 내 Hover 오브젝트 색상 패널을 열거나 닫는 키보드 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors 패널 전환" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface 도구 미리보기 켜기/끄기" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "표면을 배치하는 동안 Surface 도구의 활성 경계 미리보기 선을 숨기거나 복원하는 키보드 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface 미리보기 레이어 On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "프리셋 1+2 전환" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "프리셋 슬롯 1과 슬롯 2를 전환하는 키보드 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "프리셋 1과 2 전환" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "모드" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "버전" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "작성자의 Paradox Mods 페이지를 엽니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mochi를 사랑으로 추억하며."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "이 모드는 Mochi에게 바칩니다. Mochi는 7살에 입양된 사랑받는 강아지였고,\n" +
                    "13년 동안 사랑과 기쁨을 주었습니다. Mochi 없이는 이 모드도 없었을 것입니다."
                },
            };
        }

        public void Unload()
        {
        }
    }
}
