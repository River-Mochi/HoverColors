// File: Localization/LocaleTR.cs
// Purpose: Turkish (tr-TR) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("tr-TR", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/tr-TR.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleTR : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleTR(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Eylemler" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Hakkında" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Araç rengi davranışı" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Kısayol tuşları" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Kılavuz çizgiler" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "İthaf" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Buldozer + Yollar" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Buldozer veya yol araçları etkinken geçici dış çizgi renklerini kontrol eder\n\n" +
                    "**1. Önerilen** yıkım için oyunun Uyarı Rengini, yollar için daha yumuşak vanilla mavisini kullanır\n" +
                    "**2. Vanilla araç renkleri** bu araçlar etkinken oyunun normal vanilla mavisini geri getirir\n" +
                    "**3. Özel rengimi koru** seçilen rengi her yerde kullanır\n\n" +

                    "Amaç: bazı kullanıcılar buldozer kullanırken özel renklerini görmekte zorlanıyor. \\n" +
                    "Bu seçenekler araç kullanımı sırasında yüksek görünürlüklü renklere geçer.\n" +
                    "Bu, renk seçicide otomatik kaydedilen özel rengin üzerine yazmaz."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Önerilen" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Vanilla araç renkleri" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Özel rengimi koru" },
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Çakışan öğe dış çizgisini aç" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "Başka öğelerle çakışan öğeler yerleştirirken oyunun normal vanilla davranışını açar.\n" +
                    "Çakışma denemesinde oyunun hata dış çizgisini (somon rengi) kullanır.\n\n" +
                    "Tüm Bulldozer + Roads modlarıyla çalışır ve kayıtlı özel renginin üzerine yazmaz."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Daha koyu panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<Koyu panel>: Legacy UI kullananlar için yapıldı; Modern UI içinde daha koyu panel seviyorsan da kullanılabilir.\n" +
                    "<Standart panel>: Hover Colors'ın özel yarı saydam stili.\n" +
                    "Daha açık, daha modern görünüm.\n" +
                    "Yeni Modern oyun UI'sini kullanan çoğu oyuncu için en iyi seçenek.\n" +
                    "İkisini de dene. Bu yalnızca bu mod panelinin arka planını değiştirir, oyunun UI'sini değil."
                },
                
                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Kılavuz çizgi opaklığı (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Oyun içi kılavuz çizgilerini ölçekler (yol, prop vb. yerleştirirken görünen renkli oklar/çizgiler)\n\n" +
                    "**100%** vanilla varsayılan görünümü korur\n" +
                    "**Daha düşük** kılavuz çizgileri daha saydam yapar\n" +
                    "**0%** tamamen gizler - <Önerilmez>\n" +           
                    "15% üzerinde kalması önerilir; yoksa ne olduğunu görmek zorlaşır\n" +
                    "Aynı kaydırıcı şehir mod panelinde de bulunur. İkisi senkronizedir;\n" +
                    "bunu değiştirirsen şehir içindeki de rahatça değişir."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Ana paneli aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Şehir içindeki Hover nesneleri renk panelini açmak / kapatmak için kısayol tuşu." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors panelini aç/kapat" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface aracı önizlemelerini aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Yüzey yerleştirirken etkin Surface aracı sınır önizleme çizgilerini gizlemek veya geri getirmek için kısayol tuşu." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface aracı çizgilerini aç/kapat" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Preset 1+2 arasında geçiş" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Preset yuvası 1 ile yuva 2 arasında geçiş yapmak için kısayol tuşu." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Preset 1 ve 2 arasında geçiş" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Sürüm" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Yazarın Paradox Mods sayfasını açar." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mochi'nin sevgi dolu anısına."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Bu mod Mochi'ye adanmıştır. 7 yaşında sahiplenilmiş, çok sevilen bir köpekti,\n" +
                    "ve 13 yıl boyunca sevgi ve mutluluk verdi. Mochi olmasaydı bu mod mümkün olmazdı."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
