// File: Localization/LocaleTR.cs
// Purpose: Turkish (tr-TR) strings for the Options Menu.
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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Araç renk davranışı" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Kısayol tuşları" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Kılavuzlar" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "İthaf" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Buldozer + Yollar" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Buldozer veya yol araçları aktifken geçici dış çizgi renklerini kontrol eder.\n\n**1. Önerilen** yıkım için oyunun Uyarı rengini (sarı), yollar için daha yumuşak bir vanilla mavisini kullanır.\n**2. Vanilla araç renkleri** buldozer veya yol araçları aktifken oyunun normal vanilla mavisini geri getirir.\n**3. Özel rengimi koru** seçtiğiniz rengi her yerde kullanır.\n\nAmaç: bazı kullanıcılar/test edenler yıkım sırasında özel renklerini görmekte zorlanıyor.\nBu seçenekler araç kullanımı sırasında yüksek görünürlüklü renkler sunar.\nRenk seçicide otomatik kaydedilmiş özel renginizin üzerine yazmaz." },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Önerilen" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Vanilla araç renkleri" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Özel rengimi koru" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Çakışan öğe dış çizgisini etkinleştir" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<Etkin olması önerilir>\nNesne veya ağ yerleşimi çakışan öğeler tarafından engellendiğinde oyunun vanilla somon kırmızısı dış çizgisini görünür tutar.\nUzmanlaşmış Endüstri çiftlik yarıçapı kılavuzları gibi alan sınırlarına dokunulmaz.\n\nTüm Buldozer + Yollar modlarıyla çalışır ve kaydedilmiş özel renginizin üzerine yazmaz." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "NetLanes için özel renklere izin ver" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<Etkin olması önerilir>\nÇitler, çalılar, işaretlemeler ve benzer şerit tabanlı araçlar gibi NetLane detay öğelerini yerleştirirken kayıtlı HC renginizi/saydamlığınızı kullanır.\n\n- Normal yollar hâlâ açılır listeden seçtiğiniz Buldozer + Yollar ayarını takip eder.\n- Bu araçların oyunun vanilla mavi dış çizgisini kullanmasını istiyorsanız devre dışı bırakın.\n- Etkinken çakışma hata rengi hâlâ önceliklidir (vanilla hata rengi = somon kırmızısı)." },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Daha koyu panel" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Etkinse <Koyu panel> kullanır: eski UI oyuncuları için yapılmıştır; daha koyu panel seviyorsanız Modern UI’da da kullanılabilir.\nDevre dışıysa <Standart panel> kullanır: özel yarı saydam Hover Colors stili.\n- Daha açık, daha modern görünüm.\n- Yeni Modern oyun UI’sini kullanan çoğu oyuncu için en iyisi.\n\nİkisini de deneyin ve hangisini sevdiğinizi görün! Bu yalnızca bu mod panelinin arka planını değiştirir, oyunun UI’sini değil." },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Kesikli kılavuz çizgi rengi" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Yol açıları, 90 derece yardımcıları ve bağlantı ipuçları için kullanılan kesikli hizalama kılavuzu rengini ayarlar.\n\nİki opaklık kaydırıcısı eşitlenir: Bu Options kaydırıcısı ve şehir içi panel kaydırıcısı aynı kesikli kılavuz opaklığını kontrol eder." },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "Vanilla beyaz" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "Yüksek görünürlüklü sarı" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Pink"), "Mochi Pink" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "Yüksek görünürlüklü yeşil" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Kılavuz opaklığı (alfa)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Yol, çit, prop vb. yerleştirirken yararlı olan kesikli hizalama kılavuzlarının opaklığını kontrol eder.\n\n**100%** vanilla varsayılan görünümü korur.\n**Daha düşük** kılavuzları daha saydam yapar.\n**0%** tamamen gizler - <Önerilmez>.\nNe olduğunu görmek zorlaşacağı için 15% üzerinde kalmanız önerilir.\nAynı kaydırıcı şehir mod panelinde de bulunur. İkisi eşitlenir;\nbunu değiştirirseniz şehirdeki de değişir." },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Ana paneli aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Şehir içi Hover nesneleri Renk Panelini açmak / kapatmak için kısayol." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Hover Colors panelini aç/kapat" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Surface aracı önizlemelerini aç/kapat" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Yüzey yerleştirirken aktif Surface aracı sınır önizleme çizgilerini gizlemek veya geri getirmek için kısayol." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Surface aracı önizleme katmanı On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Preset 1+2 değiştir" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Preset yuvası 1 ile yuva 2 arasında geçiş yapmak için kısayol." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Preset 1 ve 2 arasında geçiş yap" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Yazarın Paradox Mods sayfasını aç." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Mochi’nin sevgi dolu anısına." },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Bu mod Mochi’ye adanmıştır. O, 7 yaşında sahiplenilen çok sevilen bir köpekti\nve 13 yıl boyunca sevgi ve neşe verdi. Bu mod Mochi olmadan mümkün olmazdı." },
            };
        }

        public void Unload()
        {
        }
    }
}
