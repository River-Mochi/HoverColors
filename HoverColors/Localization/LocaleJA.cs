// File: Localization/LocaleJA.cs
// Purpose: Japanese (ja-JP) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/ja-JP.json.

namespace HoverColors
{
    using Colossal;
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "操作" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "情報" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "ツール色の動作" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "パネル" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "キー割り当て" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "ガイドライン" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "捧げる言葉" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "ブルドーザー + 道路" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "ブルドーザーまたは道路ツールが有効な間の一時的なアウトライン色を制御します。\n\n**1. おすすめ** は、解体にはゲームの警告色（黄色）、道路には少し柔らかいバニラブルーを使います。\n**2. バニラのツール色** は、ブルドーザーまたは道路ツールが有効な間、ゲーム標準のバニラブルーに戻します。\n**3. カスタム色を維持** は、選択した色をすべての場面で使います。\n\n目的: 一部のユーザー/テスターから、解体中にカスタム色が見づらいという声がありました。\nツール使用中に見やすい高視認性の色を選べるようにします。\nカラーピッカーに自動保存されたカスタム色は上書きされません。" },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. おすすめ" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. バニラのツール色" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. カスタム色を維持" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "重なり項目のアウトラインを有効化" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<有効化をおすすめします>\nオブジェクトまたはネットワークの配置が重なりによってブロックされたとき、ゲーム標準のサーモンレッドのアウトラインを表示したままにします。\n専門産業の農場半径ガイドなどのエリア制限は変更しません。\n\nすべてのブルドーザー + 道路モードで機能し、保存済みのカスタム色は上書きしません。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "NetLanes にカスタム色を使う" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<有効化をおすすめします>\nフェンス、垣根、マーキングなどのレーン系 NetLane 詳細アイテムを配置するとき、保存済みの HC 色/透明度を使います。\n\n- 通常の道路は、ドロップダウンで選んだブルドーザー + 道路設定に従います。\n- これらのツールにゲーム標準のバニラブルーを使わせたい場合は無効にしてください。\n- 有効な場合、重なりエラー色が引き続き優先されます（バニラのエラー色 = サーモンレッド）。" },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "暗めのパネル" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "有効にすると <暗いパネル> を使います: レガシー UI プレイヤー向けですが、暗いパネルが好みなら Modern UI でも使えます。\n無効にすると <標準パネル> を使います: Hover Colors 独自の半透明スタイルです。\n- より明るく、よりモダンな見た目。\n- 新しい Modern UI を使う多くのプレイヤーにおすすめです。\n\n両方試して好みの方を選んでください！変更されるのはこの Mod パネルの背景だけで、ゲームの UI は変わりません。" },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "破線ガイドの色" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "道路角度、90度補助、接続ヒントに使われる破線の位置合わせガイド色を設定します。\n\n2つの不透明度スライダーは同期されています: この Options スライダーと街中パネルのスライダーは、同じ破線ガイド不透明度を制御します。" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "バニラ白" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "高視認性の緑" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "シアンブルー" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "高視認性の黄色" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "ガイドライン不透明度（アルファ）" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "破線の位置合わせガイドの不透明度を制御します。道路、フェンス、プロップなどの配置時に便利です。\n\n**100%** はバニラの既定表示を維持します。\n**低くする** とガイドがより透明になります。\n**0%** は完全に非表示にします - <非推奨>。\n15%以上を推奨します。それ以下では状況が見えにくくなります。\n同じスライダーが街中の Mod パネルにもあります。両方は同期されています。\nこちらを変更すると、街中のスライダーも同時に変更されます。" },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "メインパネルを開く/閉じる" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "街中の Hover オブジェクト色パネルを開く / 閉じるホットキーです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors パネルを切り替え" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface ツールのプレビューをオン/オフ" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface を配置中、アクティブな Surface ツール境界プレビュー線を隠す、または戻すホットキーです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface ツール プレビューレイヤー On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "プリセット 1+2 を切り替え" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "プリセットスロット 1 とスロット 2 を切り替えるホットキーです。" },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "プリセット 1 と 2 を切り替え" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "作者の Paradox Mods ページを開きます。" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "愛する Mochi の思い出に。" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "この Mod は Mochi に捧げられています。Mochi は7歳で迎えられた大切なワンちゃんで、\n13年間たくさんの愛と喜びをくれました。この Mod は Mochi なしには生まれませんでした。" },
            };
        }

        public void Unload()
        {
        }
    }
}
