// File: Mod.cs
// Purpose: Mod entrypoint; registers settings, localization, systems, keybindings, and the dedicated mod log.

namespace HoverColors
{
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Colossal.Localization;
    using Colossal.Logging;
    using CS2Shared.RiverMochi;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;
    using HoverColors.Localization;
    using HoverColors.Settings;
    using HoverColors.Systems;
    using HoverColors.UI;
    using System;
    using System.Reflection;
    using Unity.Entities;

    public sealed class Mod : IMod
    {
        public const string ModName = "Hover Colors";
        public const string ModId = "HoverColors";
        public const string ModTag = "[HC]";
        public const string kTogglePanelActionName = "TogglePanel";
        public const string kToggleSurfaceToolAreasActionName = "ToggleSurfaceToolAreas";
        public const string kTogglePresetActionName = "TogglePreset";

        public static readonly string ModVersion =
            Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "0.5.0";

        public static string BuildFlavor =>
#if DEBUG
            "DEBUG";
#else
            "RELEASE";
#endif

        public static readonly ILog s_Log =
            LogManager.GetLogger(ModId).SetShowsErrorsInUI(false);

        public static HoverColorsSettings? Settings { get; private set; }

        private static bool s_BannerLogged;

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void DebugLog(string message)
        {
            LogUtils.Info(() => message);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void DebugLog(Func<string> messageFactory)
        {
            LogUtils.Info(messageFactory);
        }

        public void OnLoad(UpdateSystem updateSystem)
        {
            LogUtils.Configure(ModId, s_Log);
            LogStartupBanner();

            if (GameManager.instance == null)
            {
                LogUtils.Warn(() => $"{ModTag} GameManager.instance is null; {ModName} cannot initialize.");
                return;
            }

            HoverColorsSettings setting = new HoverColorsSettings(this);
            Settings = setting;

            // Register localization sources before settings/options UI reads the dictionary so labels resolve.
            AddLocaleSource("en-US", new LocaleEN(setting));
            AddLocaleSource("fr-FR", new LocaleFR(setting));
            AddLocaleSource("es-ES", new LocaleES(setting));
            AddLocaleSource("de-DE", new LocaleDE(setting));
            AddLocaleSource("it-IT", new LocaleIT(setting));
            AddLocaleSource("ja-JP", new LocaleJA(setting));
            AddLocaleSource("ko-KR", new LocaleKO(setting));
            AddLocaleSource("pl-PL", new LocalePL(setting));
            AddLocaleSource("pt-BR", new LocalePT_BR(setting));
            AddLocaleSource("zh-HANS", new LocaleZH_HANS(setting));
            AddLocaleSource("zh-HANT", new LocaleZH_HANT(setting));
            AddLocaleSource("th-TH", new LocaleTH(setting));    // Thai
            AddLocaleSource("vi-VN", new LocaleVI(setting));    // Vietnamese
            AddLocaleSource("tr-TR", new LocaleTR(setting));    // Turkish
            AddLocaleSource("pt-PT", new LocalePT_PT(setting)); // European Portuguese

            try
            {
                AssetDatabase.global.LoadSettings(ModId, setting, new HoverColorsSettings(this));
            }
            catch (Exception ex)
            {
                LogUtils.Error(() => $"{ModTag} Settings load failed: {ex.GetType().Name}: {ex.Message}", ex);
            }

            try
            {
                setting.RegisterInOptionsUI();
            }
            catch (Exception ex)
            {
                LogUtils.Error(() => $"{ModTag} Options UI registration failed: {ex.GetType().Name}: {ex.Message}", ex);
            }

            try
            {
                // Registers the ProxyAction defined by Setting.TogglePanelBinding's
                // [SettingsUIKeyboardBinding] attribute. UISystem then fetches + enables it
                // and polls WasReleasedThisFrame() each tick (same as CityWatchdog).
                setting.RegisterKeyBindings();
            }
            catch (Exception ex)
            {
                LogUtils.Error(() => $"{ModTag} Keybinding registration failed: {ex.GetType().Name}: {ex.Message}", ex);
            }

            try
            {
                ScheduleSystems(updateSystem);
            }
            catch (Exception ex)
            {
                LogUtils.Error(() => $"{ModTag} System scheduling failed: {ex.GetType().Name}: {ex.Message}", ex);
            }
        }

        private static bool AddLocaleSource(string localeId, IDictionarySource source)
        {
            if (string.IsNullOrEmpty(localeId))
            {
                return false;
            }

            LocalizationManager? lm = GameManager.instance?.localizationManager;
            if (lm == null)
            {
                LogUtils.Warn(() => $"{ModTag} AddLocaleSource: no LocalizationManager; '{localeId}' not registered.");
                return false;
            }

            try
            {
                lm.AddSource(localeId, source);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.Warn(() => $"{ModTag} AddLocaleSource('{localeId}') failed: {ex.GetType().Name}: {ex.Message}", ex);
                return false;
            }
        }

        public void OnDispose()
        {
            DebugLog(() => $"{ModTag} Mod Dispose");

            HoverColorsSettings? setting = Settings;
            if (setting != null)
            {
                try
                {
                    setting.UnregisterInOptionsUI();
                }
                catch (Exception ex)
                {
                    LogUtils.Warn(() => $"{ModTag} UnregisterInOptionsUI failed: {ex.GetType().Name}: {ex.Message}", ex);
                }
            }

            Settings = null;
        }

        private static void ScheduleSystems(UpdateSystem updateSystem)
        {
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<OutlineColorSystem>();
            updateSystem.UpdateAt<OutlineColorSystem>(SystemUpdatePhase.Rendering);

            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<GuidelineColorSystem>();
            updateSystem.UpdateAt<GuidelineColorSystem>(SystemUpdatePhase.Rendering);

            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SurfaceToolOverlaySystem>();
            updateSystem.UpdateAt<SurfaceToolOverlaySystem>(SystemUpdatePhase.Rendering);

            World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<DistrictColorSystem>();
            updateSystem.UpdateAt<DistrictColorSystem>(SystemUpdatePhase.Rendering);

            updateSystem.UpdateAt<HoverColorsUISystem>(SystemUpdatePhase.UIUpdate);
        }

        private static void LogStartupBanner()
        {
            if (s_BannerLogged)
            {
                return;
            }

            s_BannerLogged = true;
            LogUtils.Info(() => $"{ModName} v{ModVersion} {BuildFlavor} loaded");
        }
    }
}
