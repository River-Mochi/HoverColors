// File: Localization/LocaleJA.cs
// Purpose: Japanese (ja-JP) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("ja-JP", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/ja-JP.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleJA : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleJA(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "アクション" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "情報" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "ツールカラーの動作" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "キー割り当て" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "ガイドライン" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "献辞" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "ブルドーザー + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "ブルドーザーまたは道路ツールが有効な間、一時的なアウトライン色を制御します。\n\n" +
                    "**1. おすすめ** は、解体にはゲームの警告色、道路には少し柔らかいバニラブルーを使います。\n" +
                    "**2. バニラのツール色** は、それらのツールが有効な間、ゲーム通常のバニラブルーに戻します。\n" +
                    "**3. カスタム色を維持** は、選んだ色をすべての場所で使います。\n\n" +
                    "カラーピッカーに自動保存されたカスタム色は上書きしません。\n"+
                    "ブルドーザー使用中にカスタム色が見づらい場合があるため、ツール使用中だけ強いアウトライン色に自動で戻せるようにしています。"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. おすすめ" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. バニラのツール色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. カスタム色を維持" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "ガイドラインの不透明度 (アルファ)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "道路やプロップなどを配置するときに表示されるゲーム内ガイドライン（色付きの矢印/線）の透明度を調整します。\n\n" +
                    "**100%** はバニラの標準表示を維持します。\n" +
                    "**低く** するとガイドラインがより透明になります。\n" +
                    "**0%** は完全に非表示にします - <おすすめしません>。\n" +           
                    "15%以上を推奨します。それより低いと何が起きているか見づらくなります。\n" +
                    "同じスライダーは都市内のModパネルにもあります。両方は同期されています。\n" +
                    "こちらを変更すると、都市内のスライダーも一緒に変わります。"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "メインパネルを開く/閉じる" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "都市内のHover Colorsパネルを開く / 閉じるためのホットキーです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colorsパネルを切り替え" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "地表ツールのプレビューをオン/オフ" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "地表を配置している間、アクティブな地表ツールの境界プレビュー線を非表示または復元するホットキーです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "地表ツールの線をオン/オフ" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "プリセット1+2を切り替え" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "プリセットスロット1とスロット2を切り替えるホットキーです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "プリセット1と2を切り替え" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "バージョン" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "作者のParadox Modsページを開きます。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mochiを偲んで。"
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "このModはMochiに捧げます。Mochiは7歳で迎えられた大切な犬で、\n" +
                    "13年間、愛と喜びを与えてくれました。MochiなしではこのModは生まれませんでした。"
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
