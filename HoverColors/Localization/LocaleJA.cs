// File: Localization/LocaleJA.cs
// Purpose: Japanese (ja-JP) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/ja-JP.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "ツール色の動作" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "パネル" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "キー割り当て" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "ガイドライン" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "献辞" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "ブルドーザー + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "ブルドーザーまたは道路ツールが有効な間、一時的なアウトライン色を制御します。\n" +
                    "\n" +
                    "**1. 推奨** は、解体にはゲームの警告色（黄色）を使い、道路にはやわらかいバニラブルーを使います。\n" +
                    "**2. バニラのツール色** は、ブルドーザーまたは道路ツールが有効な間、ゲーム通常のバニラブルーに戻します。\n" +
                    "**3. 自分のカスタム色を使う** は、選んだ色をすべての場面で使います。\n" +
                    "\n" +
                    "目的: 一部のユーザー/テスターは、解体中にカスタム色が見づらいと感じています。\n" +
                    "ツール使用中に視認性の高い色を選べるようにします。\n" +
                    "カラーピッカーに自動保存されたカスタム色は上書きされません。"
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. 推奨" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. バニラのツール色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. 自分のカスタム色を使う" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "重なったアイテムのアウトラインを有効化" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<有効を推奨>\n" +
                    "オブジェクトまたはネットワークの配置が重なりによってブロックされたとき、ゲームのバニラのサーモンレッドのアウトラインを表示したままにします。\n" +
                    "特殊産業の農場半径ガイドなどのエリア制限は変更しません。\n" +
                    "\n" +
                    "すべてのブルドーザー + 道路モードで動作し、保存済みのカスタム色を上書きしません。"
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "NetLanes にカスタム色を許可" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<有効を推奨>\n" +
                    "フェンス、生け垣、マーキングなど、レーンベースの NetLane 詳細ツールを配置するときに、保存済みの HC 色/透明度を使います。\n" +
                    "\n" +
                    "- 通常の道路は、ドロップダウンで選んだブルドーザー + 道路設定に従います。\n" +
                    "- これらのツールにゲームのバニラブルーを使わせたい場合は無効にしてください。\n" +
                    "- 有効時は、重なりエラー色が引き続き優先されます（バニラのエラー色 = サーモンレッド）。"
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "暗いパネル" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "有効 = <暗いパネル>: Legacy UI プレイヤー向けです。暗いパネルが好みなら Modern UI でも使用できます。\n" +
                    "無効 = <標準パネル>: Hover Colors 独自の半透明スタイルです。\n" +
                    "- より明るく、よりモダンな見た目です。\n" +
                    "- 新しい Modern UI を使う多くのプレイヤーにおすすめです。\n" +
                    "\n" +
                    "両方試して好みの方を選んでください！変更されるのはこの Mod パネルの背景だけで、ゲームの UI ではありません。"
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "ガイドライン不透明度（アルファ）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "道路、フェンス、プロップなどを配置するときに便利な、破線の整列ガイドの不透明度を制御します。\n" +
                    "\n" +
                    "**100%** はバニラ既定の見た目を維持します。\n" +
                    "**低くする** とガイドラインがより透明になります。\n" +
                    "**0%** は完全に非表示にします - <非推奨>。\n" +
                    "何が起きているか見づらくなるため、15%以上を推奨します。\n" +
                    "同じスライダーは都市内の Mod パネルにもあります。両方は同期されています。\n" +
                    "こちらを変更すると、都市内の方も変更されます。"
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "メインパネルを開く/閉じる" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "都市内の Hover オブジェクト色パネルを開く / 閉じるキーボードショートカットです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors パネルを切り替え" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface ツールのプレビューを切り替え" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "サーフェス配置中に、Surface ツールのアクティブな境界プレビュー線を非表示または復元するキーボードショートカットです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface プレビューレイヤー On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "プリセット 1+2 を切り替え" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "プリセットスロット 1 と 2 を切り替えるキーボードショートカットです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "プリセット 1 と 2 を切り替え" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "バージョン" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "作者の Paradox Mods ページを開きます。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mochi の思い出に捧げて。"
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "この Mod は Mochi に捧げます。7歳で迎えられた、愛された犬でした。\n" +
                    "そして13年間、愛と喜びを与えてくれました。Mochi なしではこの Mod は生まれませんでした。"
                },
            };
        }

        public void Unload()
        {
        }
    }
}
