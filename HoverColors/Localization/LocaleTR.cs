// File: Localization/LocaleTR.cs
// Purpose: Turkish (tr-TR) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/tr-TR.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Araç rengi davranışı" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Klavye kısayolları" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Kılavuz çizgiler" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Adama" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + yollar" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Bulldozer veya yol araçları aktifken geçici dış çizgi renklerini kontrol eder.\n" +
                    "\n" +
                    "**1. Önerilen** yıkım için oyunun Uyarı rengini (sarı), yollar için daha yumuşak bir vanilla mavisini kullanır.\n" +
                    "**2. Vanilla araç renkleri** bulldozer veya yol araçları aktifken oyunun normal vanilla mavisini geri getirir.\n" +
                    "**3. Özel rengimi koru** seçtiğiniz rengi her yerde kullanır.\n" +
                    "\n" +
                    "Amaç: bazı kullanıcılar/test edenler yıkım sırasında özel renklerini görmekte zorlanıyor.\n" +
                    "Bu, araç kullanımı sırasında yüksek görünürlüklü renk seçenekleri sunar.\n" +
                    "Renk seçicide otomatik kaydedilen özel renginizin üzerine yazmaz."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Önerilen" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Vanilla araç renkleri" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Özel rengimi koru" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Çakışan öğe dış çizgisini etkinleştir" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Etkin önerilir>\n" +
                    "Nesne veya ağ yerleştirme çakışan öğeler tarafından engellendiğinde oyunun vanilla somon kırmızısı dış çizgisini görünür tutar.\n" +
                    "Uzmanlaşmış Endüstri çiftlik yarıçapı kılavuzları gibi alan sınırları değiştirilmez.\n" +
                    "\n" +
                    "Tüm Bulldozer + yollar modlarıyla çalışır ve kayıtlı özel renginizin üzerine yazmaz."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "NetLanes için özel renklere izin ver" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Etkin önerilir>\n" +
                    "Çitler, çalılar, işaretlemeler ve benzer şerit tabanlı araçlar gibi NetLane ayrıntılarını yerleştirirken kayıtlı HC renginizi/şeffaflığınızı kullanır.\n" +
                    "\n" +
                    "- Normal yollar hâlâ açılır listeden seçtiğiniz Bulldozer + yollar ayarını izler.\n" +
                    "- Bu araçların oyunun vanilla mavisini kullanmasını istiyorsanız bunu kapatın.\n" +
                    "- Etkin olduğunda çakışma hata rengi yine önceliklidir (vanilla hata rengi = somon kırmızısı)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Daha koyu panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Etkin = <Koyu panel>: Legacy UI oyuncuları için yapılmıştır; daha koyu bir panel isterseniz Modern UI’da da kullanılabilir.\n" +
                    "Devre dışı = <Standart panel>: özel yarı saydam Hover Colors stili.\n" +
                    "- Daha açık, daha modern görünüm.\n" +
                    "- Yeni Modern UI kullanan çoğu oyuncu için en iyisi.\n" +
                    "\n" +
                    "İkisini de deneyin ve hangisini sevdiğinizi seçin! Bu yalnızca bu mod panelinin arka planını değiştirir, oyunun UI’sini değil."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Kılavuz çizgi opaklığı (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Yollar, çitler, prop’lar vb. yerleştirirken yararlı olan kesikli hizalama kılavuzlarının opaklığını kontrol eder.\n" +
                    "\n" +
                    "**100%** varsayılan vanilla görünümü korur.\n" +
                    "**Daha düşük** kılavuzları daha şeffaf yapar.\n" +
                    "**0%** tamamen gizler - <Önerilmez>.\n" +
                    "15% üzerinde kalmanız önerilir, yoksa ne olduğunu görmek zorlaşır.\n" +
                    "Aynı kaydırıcı şehirdeki mod panelinde de bulunur. İkisi senkronizedir;\n" +
                    "bunu değiştirirseniz şehirdeki de değişir."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Ana paneli aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Şehirde imleç altındaki nesneler için renk panelini açmak / kapatmak için klavye kısayolu." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors panelini aç/kapat" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface aracı önizlemelerini aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Yüzey yerleştirirken Surface aracının aktif sınır önizleme çizgilerini gizlemek veya geri getirmek için klavye kısayolu." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface önizleme katmanı On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Preset 1+2 arasında geçiş" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Preset yuvası 1 ile yuva 2 arasında geçiş yapmak için klavye kısayolu." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Preset 1 ve 2 arasında geçiş" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Sürüm" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Yazarın Paradox Mods sayfasını aç." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Mochi’nin sevgi dolu anısına."
                },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Bu mod Mochi’ye adanmıştır. 7 yaşında sahiplenilmiş çok sevilen bir köpekti,\n" +
                    "ve 13 yıl boyunca sevgi ve neşe verdi. Bu mod Mochi olmadan mümkün olmazdı."
                },
            };
        }

        public void Unload()
        {
        }
    }
}
