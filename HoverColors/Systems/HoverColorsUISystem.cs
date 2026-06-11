// File: Systems/HoverColorsUISystem.cs
// Bridges Mod.Settings to cs2/api bindings, owns the shared panel-open flag,
// and checks cached keybind actions for panel/tool toggles.

namespace HoverColors.UI
{
    using System;
    using System.Collections.Generic;

    using Colossal.UI.Binding;

    using CS2Shared.RiverMochi;

    using Game;
    using Game.Input;
    using Game.SceneFlow;
    using Game.UI;

    using HoverColors.Settings;
    using HoverColors.Systems;

    public partial class HoverColorsUISystem : UISystemBase
    {
        private static bool s_PanelOpen;

        // Toggle target for both the GTL button (via SetPanelOpen trigger) and the J hotkey poll below.
        public static void TogglePanel() => s_PanelOpen = !s_PanelOpen;

        // ProxyActions registered by Setting.RegisterKeyBindings() in Mod.OnLoad.
        // Cached actions are checked with WasReleasedThisFrame(); lookup is retried only if an action is missing.
        private ProxyAction? m_TogglePanelAction;
        private ProxyAction? m_ToggleSurfaceToolAreasAction;

        // Live color bindings
        private ValueBinding<float> m_OutlineRBinding = null!;
        private ValueBinding<float> m_OutlineGBinding = null!;
        private ValueBinding<float> m_OutlineBBinding = null!;
        private ValueBinding<float> m_OutlineABinding = null!;
        private ValueBinding<float> m_OwnerRBinding = null!;
        private ValueBinding<float> m_OwnerGBinding = null!;
        private ValueBinding<float> m_OwnerBBinding = null!;
        private ValueBinding<float> m_OwnerABinding = null!;
        private ValueBinding<float> m_FillABinding = null!;
        private ValueBinding<float> m_DistrictRBinding = null!;
        private ValueBinding<float> m_DistrictGBinding = null!;
        private ValueBinding<float> m_DistrictBBinding = null!;
        private ValueBinding<float> m_DistrictABinding = null!;
        private ValueBinding<float> m_GuidelineLinesColorRBinding = null!;
        private ValueBinding<float> m_GuidelineLinesColorGBinding = null!;
        private ValueBinding<float> m_GuidelineLinesColorBBinding = null!;
        private ValueBinding<float> m_GuidelineLinesColorABinding = null!;
        private ValueBinding<float> m_GuidelinePreviewColorRBinding = null!;
        private ValueBinding<float> m_GuidelinePreviewColorGBinding = null!;
        private ValueBinding<float> m_GuidelinePreviewColorBBinding = null!;
        private ValueBinding<float> m_GuidelinePreviewColorABinding = null!;
        private ValueBinding<float> m_GuidelineDashedColorRBinding = null!;
        private ValueBinding<float> m_GuidelineDashedColorGBinding = null!;
        private ValueBinding<float> m_GuidelineDashedColorBBinding = null!;
        private ValueBinding<int> m_GuidelineOpacityBinding = null!;
        private ValueBinding<int> m_GuidelineDefaultBinding = null!;
        private ValueBinding<bool> m_PanelOpenBinding = null!;
        private ValueBinding<bool> m_PanelTooltipsEnabledBinding = null!;
        private ValueBinding<bool> m_UseDarkerPanelBinding = null!;
        private ValueBinding<bool> m_SurfaceToolAreasSuppressedBinding = null!;
        private ValueBinding<bool> m_SpecializedIndustryAreasSuppressedBinding = null!;
        private ValueBinding<bool> m_VanillaOutlineActiveBinding = null!;

        // Preset slot bindings — panel reads stored colors for swatch previews + active-state dots.
        private ValueBinding<float> m_Preset1RBinding = null!;
        private ValueBinding<float> m_Preset1GBinding = null!;
        private ValueBinding<float> m_Preset1BBinding = null!;
        private ValueBinding<float> m_Preset1ABinding = null!;
        private ValueBinding<float> m_Preset1FillABinding = null!;
        private ValueBinding<float> m_Preset2RBinding = null!;
        private ValueBinding<float> m_Preset2GBinding = null!;
        private ValueBinding<float> m_Preset2BBinding = null!;
        private ValueBinding<float> m_Preset2ABinding = null!;
        private ValueBinding<float> m_Preset2FillABinding = null!;
        private ValueBinding<bool> m_Preset1ActiveBinding = null!;
        private ValueBinding<bool> m_Preset2ActiveBinding = null!;

        // Factory defaults — keep in sync with HoverColorsSettings.SetDefaults().
        private const float DefaultPreset1R = 140f / 255f, DefaultPreset1G = 140f / 255f, DefaultPreset1B = 171f / 255f;
        private const float DefaultPreset1A = 0.5f, DefaultPreset1FillA = 0f;
        private const float DefaultPreset2R = 0.25f, DefaultPreset2G = 0.15f, DefaultPreset2B = 0.25f;
        private const float DefaultPreset2A = 0.5f, DefaultPreset2FillA = 0f;

        // In-memory backup for TogglePresetDefaults (session-only, not persisted to .coc).
        private float m_BkP1R, m_BkP1G, m_BkP1B, m_BkP1A, m_BkP1FillA;
        private int m_BkP1Guideline;
        private float m_BkP2R, m_BkP2G, m_BkP2B, m_BkP2A, m_BkP2FillA;
        private int m_BkP2Guideline;
        private bool m_PresetBackupExists;

        // K hotkey toggle: alternates between applying preset 1 and preset 2.
        private ProxyAction? m_TogglePresetAction;
        private int m_LastAppliedPreset = 1;

        protected override void OnCreate()
        {
            base.OnCreate();
            LogUtils.Info(() => $"{Mod.ModTag} HoverColorsUISystem created");

            InitializeKeybindActions();
            RegisterValueBindings();
            RegisterTriggerBindings();
        }

        protected override void OnUpdate()
        {
            // CWD-style push bindings: do not call base.OnUpdate() because there are no
            // GetterValueBindings to poll. This keeps the panel idle unless a value actually changes.
            SyncValueBindings();

            // Re-fetch if the action wasn't ready at OnCreate (RegisterKeyBindings race) or got dropped.
            RefreshKeybindActions();

            // Don't fire hotkeys in main menu / editor.
            if (!IsInGame())
            {
                return;
            }

            // Read current shared state and flip it — works whether button or previous hotkey set it.
            if (m_TogglePanelAction?.WasReleasedThisFrame() == true)
            {
                TogglePanel();
                SyncValueBindings();
            }

            if (m_ToggleSurfaceToolAreasAction?.WasReleasedThisFrame() == true)
            {
                ToggleSurfaceToolAreas();
            }

            // K toggles between preset slot 1 and slot 2.
            if (m_TogglePresetAction?.WasReleasedThisFrame() == true)
            {
                m_LastAppliedPreset = (m_LastAppliedPreset == 1) ? 2 : 1;
                ApplyPresetSlot(m_LastAppliedPreset);
            }
        }

        private void RegisterValueBindings()
        {
            HoverColorsSettings? settings = Mod.Settings;
            bool suppressSurfaceToolAreas = settings?.SurfaceToolAreasSuppressed ?? true;
            bool suppressSpecializedIndustryAreas = settings?.SpecializedIndustryAreasSuppressed ?? true;
            AreaToolOverlaySystem.SetSurfaceSuppression(suppressSurfaceToolAreas);
            AreaToolOverlaySystem.SetSpecializedIndustrySuppression(suppressSpecializedIndustryAreas);

            m_OutlineRBinding = AddValueBinding("OutlineR", settings?.OutlineR ?? 0.502f);
            m_OutlineGBinding = AddValueBinding("OutlineG", settings?.OutlineG ?? 0.869f);
            m_OutlineBBinding = AddValueBinding("OutlineB", settings?.OutlineB ?? 1f);
            m_OutlineABinding = AddValueBinding("OutlineA", settings?.OutlineA ?? 0.855f);
            m_OwnerRBinding = AddValueBinding("OwnerR", settings?.OwnerR ?? 0.247f);
            m_OwnerGBinding = AddValueBinding("OwnerG", settings?.OwnerG ?? 0.981f);
            m_OwnerBBinding = AddValueBinding("OwnerB", settings?.OwnerB ?? 0.247f);
            m_OwnerABinding = AddValueBinding("OwnerA", settings?.OwnerA ?? 0.702f);
            m_FillABinding = AddValueBinding("FillA", settings?.FillA ?? 0f);
            m_DistrictRBinding = AddValueBinding("DistrictR", settings?.DistrictR ?? 128f / 255f);
            m_DistrictGBinding = AddValueBinding("DistrictG", settings?.DistrictG ?? 128f / 255f);
            m_DistrictBBinding = AddValueBinding("DistrictB", settings?.DistrictB ?? 128f / 255f);
            m_DistrictABinding = AddValueBinding("DistrictA", settings?.DistrictA ?? 64f / 255f);

            UnityEngine.Color guidelineLinesColor = GuidelineColorSystem.GetGuidelineLinesColor(settings);
            UnityEngine.Color guidelinePreviewColor = GuidelineColorSystem.GetGuidelinePreviewColor(settings);
            UnityEngine.Color guidelineDashedColor = GuidelineColorSystem.GetGuidelineDashedColor(settings);
            m_GuidelineLinesColorRBinding = AddValueBinding("GuidelineLinesColorR", guidelineLinesColor.r);
            m_GuidelineLinesColorGBinding = AddValueBinding("GuidelineLinesColorG", guidelineLinesColor.g);
            m_GuidelineLinesColorBBinding = AddValueBinding("GuidelineLinesColorB", guidelineLinesColor.b);
            m_GuidelineLinesColorABinding = AddValueBinding("GuidelineLinesColorA", settings?.GuidelineLinesA ?? 1f);
            m_GuidelinePreviewColorRBinding = AddValueBinding("GuidelinePreviewColorR", guidelinePreviewColor.r);
            m_GuidelinePreviewColorGBinding = AddValueBinding("GuidelinePreviewColorG", guidelinePreviewColor.g);
            m_GuidelinePreviewColorBBinding = AddValueBinding("GuidelinePreviewColorB", guidelinePreviewColor.b);
            m_GuidelinePreviewColorABinding = AddValueBinding("GuidelinePreviewColorA", settings?.GuidelinePreviewA ?? 1f);
            m_GuidelineDashedColorRBinding = AddValueBinding("GuidelineDashedColorR", guidelineDashedColor.r);
            m_GuidelineDashedColorGBinding = AddValueBinding("GuidelineDashedColorG", guidelineDashedColor.g);
            m_GuidelineDashedColorBBinding = AddValueBinding("GuidelineDashedColorB", guidelineDashedColor.b);
            m_GuidelineOpacityBinding = AddValueBinding("GuidelineOpacityPercent", settings?.GuidelineOpacityPercent ?? HoverColorsSettings.kDefaultGuidelineOpacityPercent);
            m_GuidelineDefaultBinding = AddValueBinding("GuidelineDefaultPercent", settings?.GuidelineDefaultPercent ?? HoverColorsSettings.kDefaultGuidelineOpacityPercent);
            m_PanelOpenBinding = AddValueBinding("PanelOpen", s_PanelOpen);
            m_PanelTooltipsEnabledBinding = AddValueBinding("PanelTooltipsEnabled", settings?.PanelTooltipsEnabled ?? true);
            m_UseDarkerPanelBinding = AddValueBinding("UseDarkerPanel", settings?.UseDarkerPanel ?? false);
            m_SurfaceToolAreasSuppressedBinding = AddValueBinding("SurfaceToolAreasSuppressed", suppressSurfaceToolAreas);
            m_SpecializedIndustryAreasSuppressedBinding = AddValueBinding("SpecializedIndustryAreasSuppressed", suppressSpecializedIndustryAreas);
            m_VanillaOutlineActiveBinding = AddValueBinding("VanillaOutlineActive", IsVanillaOutlineActive());

            // Preset slot stored-color bindings (swatch previews + active-state).
            m_Preset1RBinding = AddValueBinding("Preset1R", settings?.Preset1R ?? 140f / 255f);
            m_Preset1GBinding = AddValueBinding("Preset1G", settings?.Preset1G ?? 140f / 255f);
            m_Preset1BBinding = AddValueBinding("Preset1B", settings?.Preset1B ?? 171f / 255f);
            m_Preset1ABinding = AddValueBinding("Preset1A", settings?.Preset1A ?? 0.5f);
            m_Preset1FillABinding = AddValueBinding("Preset1FillA", settings?.Preset1FillA ?? 0f);
            m_Preset2RBinding = AddValueBinding("Preset2R", settings?.Preset2R ?? 0.25f);
            m_Preset2GBinding = AddValueBinding("Preset2G", settings?.Preset2G ?? 0.15f);
            m_Preset2BBinding = AddValueBinding("Preset2B", settings?.Preset2B ?? 0.25f);
            m_Preset2ABinding = AddValueBinding("Preset2A", settings?.Preset2A ?? 0.5f);
            m_Preset2FillABinding = AddValueBinding("Preset2FillA", settings?.Preset2FillA ?? 0f);
            m_Preset1ActiveBinding = AddValueBinding("Preset1Active", IsPresetActive(1));
            m_Preset2ActiveBinding = AddValueBinding("Preset2Active", IsPresetActive(2));
        }

        private ValueBinding<T> AddValueBinding<T>(string name, T initialValue)
        {
            ValueBinding<T> binding = new ValueBinding<T>(Mod.ModId, name, initialValue);
            AddBinding(binding);
            return binding;
        }

        private void SyncValueBindings()
        {
            HoverColorsSettings? settings = Mod.Settings;
            UpdateIfChanged(m_OutlineRBinding, settings?.OutlineR ?? 0.502f);
            UpdateIfChanged(m_OutlineGBinding, settings?.OutlineG ?? 0.869f);
            UpdateIfChanged(m_OutlineBBinding, settings?.OutlineB ?? 1f);
            UpdateIfChanged(m_OutlineABinding, settings?.OutlineA ?? 0.855f);
            UpdateIfChanged(m_OwnerRBinding, settings?.OwnerR ?? 0.247f);
            UpdateIfChanged(m_OwnerGBinding, settings?.OwnerG ?? 0.981f);
            UpdateIfChanged(m_OwnerBBinding, settings?.OwnerB ?? 0.247f);
            UpdateIfChanged(m_OwnerABinding, settings?.OwnerA ?? 0.702f);
            UpdateIfChanged(m_FillABinding, settings?.FillA ?? 0f);
            UpdateIfChanged(m_DistrictRBinding, settings?.DistrictR ?? 128f / 255f);
            UpdateIfChanged(m_DistrictGBinding, settings?.DistrictG ?? 128f / 255f);
            UpdateIfChanged(m_DistrictBBinding, settings?.DistrictB ?? 128f / 255f);
            UpdateIfChanged(m_DistrictABinding, settings?.DistrictA ?? 64f / 255f);

            UnityEngine.Color guidelineLinesColor = GuidelineColorSystem.GetGuidelineLinesColor(settings);
            UnityEngine.Color guidelinePreviewColor = GuidelineColorSystem.GetGuidelinePreviewColor(settings);
            UnityEngine.Color guidelineDashedColor = GuidelineColorSystem.GetGuidelineDashedColor(settings);
            UpdateIfChanged(m_GuidelineLinesColorRBinding, guidelineLinesColor.r);
            UpdateIfChanged(m_GuidelineLinesColorGBinding, guidelineLinesColor.g);
            UpdateIfChanged(m_GuidelineLinesColorBBinding, guidelineLinesColor.b);
            UpdateIfChanged(m_GuidelineLinesColorABinding, settings?.GuidelineLinesA ?? 1f);
            UpdateIfChanged(m_GuidelinePreviewColorRBinding, guidelinePreviewColor.r);
            UpdateIfChanged(m_GuidelinePreviewColorGBinding, guidelinePreviewColor.g);
            UpdateIfChanged(m_GuidelinePreviewColorBBinding, guidelinePreviewColor.b);
            UpdateIfChanged(m_GuidelinePreviewColorABinding, settings?.GuidelinePreviewA ?? 1f);
            UpdateIfChanged(m_GuidelineDashedColorRBinding, guidelineDashedColor.r);
            UpdateIfChanged(m_GuidelineDashedColorGBinding, guidelineDashedColor.g);
            UpdateIfChanged(m_GuidelineDashedColorBBinding, guidelineDashedColor.b);
            UpdateIfChanged(m_GuidelineOpacityBinding, settings?.GuidelineOpacityPercent ?? HoverColorsSettings.kDefaultGuidelineOpacityPercent);
            UpdateIfChanged(m_GuidelineDefaultBinding, settings?.GuidelineDefaultPercent ?? HoverColorsSettings.kDefaultGuidelineOpacityPercent);
            UpdateIfChanged(m_PanelOpenBinding, s_PanelOpen);
            UpdateIfChanged(m_PanelTooltipsEnabledBinding, settings?.PanelTooltipsEnabled ?? true);
            UpdateIfChanged(m_UseDarkerPanelBinding, settings?.UseDarkerPanel ?? false);
            UpdateIfChanged(m_SurfaceToolAreasSuppressedBinding, AreaToolOverlaySystem.SuppressSurfaceToolAreas);
            UpdateIfChanged(m_SpecializedIndustryAreasSuppressedBinding, AreaToolOverlaySystem.SuppressSpecializedIndustryToolAreas);
            UpdateIfChanged(m_VanillaOutlineActiveBinding, IsVanillaOutlineActive());

            // Preset stored colors + active flags
            UpdateIfChanged(m_Preset1RBinding, settings?.Preset1R ?? 140f / 255f);
            UpdateIfChanged(m_Preset1GBinding, settings?.Preset1G ?? 140f / 255f);
            UpdateIfChanged(m_Preset1BBinding, settings?.Preset1B ?? 171f / 255f);
            UpdateIfChanged(m_Preset1ABinding, settings?.Preset1A ?? 0.5f);
            UpdateIfChanged(m_Preset1FillABinding, settings?.Preset1FillA ?? 0f);
            UpdateIfChanged(m_Preset2RBinding, settings?.Preset2R ?? 0.25f);
            UpdateIfChanged(m_Preset2GBinding, settings?.Preset2G ?? 0.15f);
            UpdateIfChanged(m_Preset2BBinding, settings?.Preset2B ?? 0.25f);
            UpdateIfChanged(m_Preset2ABinding, settings?.Preset2A ?? 0.5f);
            UpdateIfChanged(m_Preset2FillABinding, settings?.Preset2FillA ?? 0f);
            UpdateIfChanged(m_Preset1ActiveBinding, IsPresetActive(1));
            UpdateIfChanged(m_Preset2ActiveBinding, IsPresetActive(2));
        }

        private static void UpdateIfChanged<T>(ValueBinding<T> binding, T value)
        {
            if (EqualityComparer<T>.Default.Equals(binding.value, value))
            {
                return;
            }

            binding.Update(value);
        }

        private void SetPanelOpen(bool open)
        {
            s_PanelOpen = open;
            UpdateIfChanged(m_PanelOpenBinding, s_PanelOpen);
        }

        private void InitializeKeybindActions()
        {
            m_TogglePanelAction = EnableAction(Mod.kTogglePanelActionName);
            m_ToggleSurfaceToolAreasAction = EnableAction(Mod.kToggleSurfaceToolAreasActionName);
            m_TogglePresetAction = EnableAction(Mod.kTogglePresetActionName);
        }

        private void RefreshKeybindActions()
        {
            if (m_TogglePanelAction == null)
            {
                m_TogglePanelAction = EnableAction(Mod.kTogglePanelActionName);
            }

            if (m_ToggleSurfaceToolAreasAction == null)
            {
                m_ToggleSurfaceToolAreasAction = EnableAction(Mod.kToggleSurfaceToolAreasActionName);
            }

            if (m_TogglePresetAction == null)
            {
                m_TogglePresetAction = EnableAction(Mod.kTogglePresetActionName);
            }
        }

        // CWD-style: fetch the ProxyAction registered by the [SettingsUIKeyboardBinding] attribute
        // and flip shouldBeEnabled so it actually receives input. Returns null on miss.
        private static ProxyAction? EnableAction(string actionName)
        {
            try
            {
                ProxyAction? action = Mod.Settings?.GetAction(actionName);
                if (action != null)
                {
                    action.shouldBeEnabled = true;
                }
                return action;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "missing-keybind-" + actionName,
                    () => $"{Mod.ModTag} Keybinding '{actionName}' unavailable: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return null;
            }
        }

        private static bool IsInGame()
        {
            return GameManager.instance != null && GameManager.instance.gameMode == GameMode.Game;
        }
    }
}
