// File: Localization/LocaleKO.cs
// Purpose: Korean (ko-KR) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("ko-KR", ...).
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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "도구 색상 방식" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "키 바인딩" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "가이드라인" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "헌정" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "불도저 + 도로" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "불도저나 도로 도구가 활성화되어 있을 때 임시 윤곽선 색상을 제어합니다.\n\n" +
                    "**1. 추천**은 철거에는 게임의 경고 색상을, 도로에는 더 부드러운 바닐라 파란색을 사용합니다.\n" +
                    "**2. 바닐라 도구 색상**은 해당 도구가 활성화되어 있을 때 게임 기본 바닐라 파란색으로 되돌립니다.\n" +
                    "**3. 내 사용자 색상 유지**는 선택한 색상을 모든 곳에 사용합니다.\n\n" +
                    "색상 선택기에 자동 저장된 사용자 색상은 덮어쓰지 않습니다.\n"+
                    "일부 사용자는 불도저 사용 중 사용자 색상이 잘 보이지 않아, 도구 사용 중에는 강한 윤곽선 색상이 자동으로 돌아오기를 원했습니다."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 추천" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. 바닐라 도구 색상" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 내 사용자 색상 유지" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "가이드라인 불투명도 (알파)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "도로, 프롭 등을 배치할 때 표시되는 게임 내 가이드라인(색 화살표/선)의 투명도를 조절합니다.\n\n" +
                    "**100%**는 바닐라 기본 모습을 유지합니다.\n" +
                    "**낮게** 설정하면 가이드라인이 더 투명해집니다.\n" +
                    "**0%**는 완전히 숨깁니다 - <권장하지 않음>.\n" +           
                    "15% 이상을 권장합니다. 그보다 낮으면 상황을 보기 어렵습니다.\n" +
                    "같은 슬라이더가 도시 모드 패널에도 있습니다. 둘은 서로 동기화됩니다.\n" +
                    "여기서 바꾸면 도시 안 패널의 슬라이더도 같이 바뀝니다."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "메인 패널 열기/닫기" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "도시 안 Hover Colors 패널을 열거나 닫는 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors 패널 전환" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface 도구 미리보기 켜기/끄기" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "표면을 배치하는 동안 활성 Surface 도구의 경계 미리보기 선을 숨기거나 다시 표시하는 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface 도구 선 켜기/끄기" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "프리셋 1+2 전환" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "프리셋 슬롯 1과 2를 전환하는 단축키입니다." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "프리셋 1과 2 전환" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "모드" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "버전" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "작성자의 Paradox Mods 페이지를 엽니다." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mochi를 사랑하며 기억합니다."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "이 모드는 Mochi에게 바칩니다. Mochi는 7살에 입양된 사랑스러운 강아지였고,\n" +
                    "13년 동안 사랑과 기쁨을 주었습니다. Mochi가 없었다면 이 모드는 가능하지 않았습니다."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
