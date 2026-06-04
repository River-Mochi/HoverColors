// File: Systems/HoverColorsUISystem.cs
// Bridges Mod.Settings to cs2/api bindings, owns the shared panel-open flag,
// and checks cached keybind actions for panel/tool toggles.

namespace HoverColors.UI
{
    using Colossal.UI.Binding;
    using CS2Shared.RiverMochi;
    using Game;
    using Game.Input;
    using Game.SceneFlow;
    using Game.UI;
    using HoverColors.Settings;
    using HoverColors.Systems;
    using System;
    using System.Collections.Generic;

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
        private ValueBinding<int> m_GuidelineOpacityBinding = null!;
        private ValueBinding<int> m_GuidelineDefaultBinding = null!;
        private ValueBinding<bool> m_PanelOpenBinding = null!;
        private ValueBinding<bool> m_PanelTooltipsEnabledBinding = null!;
        private ValueBinding<bool> m_UseDarkerPanelBinding = null!;
        private ValueBinding<bool> m_SurfaceToolAreasSuppressedBinding = null!;
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

            AddBinding(new TriggerBinding<float, float, float, float>(
                Mod.ModId,
                "SetOutlineColor",
                (r, g, b, a) =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    settings.OutlineR = r;
                    settings.OutlineG = g;
                    settings.OutlineB = b;
                    settings.OutlineA = a;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding<float>(
                Mod.ModId,
                "SetFillAlpha",
                a =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    settings.FillA = a;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding<int>(
                Mod.ModId,
                "SetGuidelineOpacity",
                percent =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    if (percent < 0) percent = 0;
                    if (percent > 100) percent = 100;

                    settings.GuidelineOpacityPercent = percent;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding<float, float, float, float>(
                Mod.ModId,
                "SetGuidelineLinesColor",
                (r, g, b, a) =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    settings.GuidelineLinesColorPreset = HoverColorsSettings.GuidelineColorPresetCustom;
                    settings.GuidelineLinesR = Math.Max(0f, Math.Min(1f, r));
                    settings.GuidelineLinesG = Math.Max(0f, Math.Min(1f, g));
                    settings.GuidelineLinesB = Math.Max(0f, Math.Min(1f, b));
                    settings.GuidelineLinesA = Math.Max(0f, Math.Min(1f, a));
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding<float, float, float, float>(
                Mod.ModId,
                "SetGuidelinePreviewColor",
                (r, g, b, a) =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    settings.GuidelinePreviewColorPreset = HoverColorsSettings.GuidelineColorPresetCustom;
                    settings.GuidelinePreviewR = Math.Max(0f, Math.Min(1f, r));
                    settings.GuidelinePreviewG = Math.Max(0f, Math.Min(1f, g));
                    settings.GuidelinePreviewB = Math.Max(0f, Math.Min(1f, b));
                    settings.GuidelinePreviewA = Math.Max(0f, Math.Min(1f, a));
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding<float, float, float, float>(
                Mod.ModId,
                "SetDistrictColor",
                (r, g, b, a) =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    settings.DistrictColorEnabled = true;
                    settings.DistrictR = r;
                    settings.DistrictG = g;
                    settings.DistrictB = b;
                    settings.DistrictA = a;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding(
                Mod.ModId,
                "ResetToVanilla",
                () =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    UnityEngine.Color hovered = OutlineColorSystem.CapturedHoveredColor;
                    settings.OutlineR = hovered.r;
                    settings.OutlineG = hovered.g;
                    settings.OutlineB = hovered.b;
                    settings.OutlineA = OutlineColorSystem.CapturedOutlineA;
                    settings.FillA = OutlineColorSystem.CapturedFillA;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding(
                Mod.ModId,
                "ResetOutlineToVanilla",
                () =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    UnityEngine.Color hovered = OutlineColorSystem.CapturedHoveredColor;
                    settings.OutlineR = hovered.r;
                    settings.OutlineG = hovered.g;
                    settings.OutlineB = hovered.b;
                    settings.OutlineA = OutlineColorSystem.CapturedOutlineA;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding<bool>(
                Mod.ModId,
                "SetPanelOpen",
                SetPanelOpen));

            AddBinding(new TriggerBinding<bool>(
                Mod.ModId,
                "SetPanelTooltipsEnabled",
                enabled =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    settings.PanelTooltipsEnabled = enabled;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            AddBinding(new TriggerBinding(
                Mod.ModId,
                "ToggleSurfaceToolAreas",
                ToggleSurfaceToolAreas));

            // Apply a stored preset slot (slot = 1 or 2) — pushes that slot's color to live swatch.
            AddBinding(new TriggerBinding<int>(
                Mod.ModId,
                "ApplyPreset",
                slot =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    if (slot == 1)
                    {
                        settings.OutlineR = settings.Preset1R;
                        settings.OutlineG = settings.Preset1G;
                        settings.OutlineB = settings.Preset1B;
                        settings.OutlineA = settings.Preset1A;
                        settings.FillA = settings.Preset1FillA;
                        settings.GuidelineOpacityPercent = settings.Preset1GuidelinePercent;
                    }
                    else if (slot == 2)
                    {
                        settings.OutlineR = settings.Preset2R;
                        settings.OutlineG = settings.Preset2G;
                        settings.OutlineB = settings.Preset2B;
                        settings.OutlineA = settings.Preset2A;
                        settings.FillA = settings.Preset2FillA;
                        settings.GuidelineOpacityPercent = settings.Preset2GuidelinePercent;
                    }

                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            // Save the current live color into a preset slot. Persisted to .coc automatically.
            AddBinding(new TriggerBinding<int>(
                Mod.ModId,
                "SavePreset",
                slot =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    if (slot == 1)
                    {
                        settings.Preset1R = settings.OutlineR;
                        settings.Preset1G = settings.OutlineG;
                        settings.Preset1B = settings.OutlineB;
                        settings.Preset1A = settings.OutlineA;
                        settings.Preset1FillA = settings.FillA;
                        settings.Preset1GuidelinePercent = settings.GuidelineOpacityPercent;
                    }
                    else if (slot == 2)
                    {
                        settings.Preset2R = settings.OutlineR;
                        settings.Preset2G = settings.OutlineG;
                        settings.Preset2B = settings.OutlineB;
                        settings.Preset2A = settings.OutlineA;
                        settings.Preset2FillA = settings.FillA;
                        settings.Preset2GuidelinePercent = settings.GuidelineOpacityPercent;
                    }

                    settings.ApplyAndSave();
                    SyncValueBindings(); // updates Preset*Active + stored color bindings
                }));

            // Toggle preset slots between current player values and mod factory defaults.
            // First press: saves current slots as in-memory backup, applies defaults.
            // Second press: restores the backup.
            // If already at defaults with no backup (fresh install), does nothing.
            AddBinding(new TriggerBinding(
                Mod.ModId,
                "TogglePresetDefaults",
                () =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    if (!PresetsAtDefaults(settings))
                    {
                        // Save current player colors then apply defaults.
                        m_BkP1R = settings.Preset1R; m_BkP1G = settings.Preset1G; m_BkP1B = settings.Preset1B;
                        m_BkP1A = settings.Preset1A; m_BkP1FillA = settings.Preset1FillA; m_BkP1Guideline = settings.Preset1GuidelinePercent;
                        m_BkP2R = settings.Preset2R; m_BkP2G = settings.Preset2G; m_BkP2B = settings.Preset2B;
                        m_BkP2A = settings.Preset2A; m_BkP2FillA = settings.Preset2FillA; m_BkP2Guideline = settings.Preset2GuidelinePercent;
                        m_PresetBackupExists = true;

                        settings.Preset1R = DefaultPreset1R; settings.Preset1G = DefaultPreset1G; settings.Preset1B = DefaultPreset1B;
                        settings.Preset1A = DefaultPreset1A; settings.Preset1FillA = DefaultPreset1FillA;
                        settings.Preset1GuidelinePercent = HoverColorsSettings.DefaultGuidelineOpacityPercent;
                        settings.Preset2R = DefaultPreset2R; settings.Preset2G = DefaultPreset2G; settings.Preset2B = DefaultPreset2B;
                        settings.Preset2A = DefaultPreset2A; settings.Preset2FillA = DefaultPreset2FillA;
                        settings.Preset2GuidelinePercent = HoverColorsSettings.DefaultGuidelineOpacityPercent;
                    }
                    else if (m_PresetBackupExists)
                    {
                        // Restore backup.
                        settings.Preset1R = m_BkP1R; settings.Preset1G = m_BkP1G; settings.Preset1B = m_BkP1B;
                        settings.Preset1A = m_BkP1A; settings.Preset1FillA = m_BkP1FillA; settings.Preset1GuidelinePercent = m_BkP1Guideline;
                        settings.Preset2R = m_BkP2R; settings.Preset2G = m_BkP2G; settings.Preset2B = m_BkP2B;
                        settings.Preset2A = m_BkP2A; settings.Preset2FillA = m_BkP2FillA; settings.Preset2GuidelinePercent = m_BkP2Guideline;
                        m_PresetBackupExists = false;
                    }
                    // Already at defaults with no backup → no-op (nothing to restore).

                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));

            // Reset only the two city swatches. Dashed guide color/opacity are Options preferences.
            AddBinding(new TriggerBinding(
                Mod.ModId,
                "ResetGuidelines",
                () =>
                {
                    HoverColorsSettings? settings = Mod.Settings;
                    if (settings == null) return;

                    UnityEngine.Color lines = GuidelineColorSystem.CapturedVanillaGuidelineLinesColor;
                    UnityEngine.Color preview = GuidelineColorSystem.CapturedVanillaGuidelinePreviewColor;
                    settings.GuidelineLinesColorPreset = HoverColorsSettings.GuidelineColorPresetVanilla;
                    settings.GuidelineLinesR = lines.r;
                    settings.GuidelineLinesG = lines.g;
                    settings.GuidelineLinesB = lines.b;
                    settings.GuidelineLinesA = 1f;
                    settings.GuidelinePreviewColorPreset = HoverColorsSettings.GuidelineColorPresetVanilla;
                    settings.GuidelinePreviewR = preview.r;
                    settings.GuidelinePreviewG = preview.g;
                    settings.GuidelinePreviewB = preview.b;
                    settings.GuidelinePreviewA = 1f;
                    settings.ApplyAndSave();
                    SyncValueBindings();
                }));
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

        private void ApplyPresetSlot(int slot)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            if (slot == 1)
            {
                settings.OutlineR = settings.Preset1R; settings.OutlineG = settings.Preset1G;
                settings.OutlineB = settings.Preset1B; settings.OutlineA = settings.Preset1A;
                settings.FillA = settings.Preset1FillA;
                settings.GuidelineOpacityPercent = settings.Preset1GuidelinePercent;
            }
            else
            {
                settings.OutlineR = settings.Preset2R; settings.OutlineG = settings.Preset2G;
                settings.OutlineB = settings.Preset2B; settings.OutlineA = settings.Preset2A;
                settings.FillA = settings.Preset2FillA;
                settings.GuidelineOpacityPercent = settings.Preset2GuidelinePercent;
            }

            settings.ApplyAndSave();
            SyncValueBindings();
        }

        private void RegisterValueBindings()
        {
            HoverColorsSettings? settings = Mod.Settings;
            bool suppressSurfaceToolAreas = settings?.SurfaceToolAreasSuppressed ?? true;
            SurfaceToolOverlaySystem.SetSuppression(suppressSurfaceToolAreas);

            m_OutlineRBinding = AddValueBinding("OutlineR", settings?.OutlineR ?? 0.502f);
            m_OutlineGBinding = AddValueBinding("OutlineG", settings?.OutlineG ?? 0.869f);
            m_OutlineBBinding = AddValueBinding("OutlineB", settings?.OutlineB ?? 1f);
            m_OutlineABinding = AddValueBinding("OutlineA", settings?.OutlineA ?? 0.855f);
            m_FillABinding = AddValueBinding("FillA", settings?.FillA ?? 0f);
            m_DistrictRBinding = AddValueBinding("DistrictR", settings?.DistrictR ?? 128f / 255f);
            m_DistrictGBinding = AddValueBinding("DistrictG", settings?.DistrictG ?? 128f / 255f);
            m_DistrictBBinding = AddValueBinding("DistrictB", settings?.DistrictB ?? 128f / 255f);
            m_DistrictABinding = AddValueBinding("DistrictA", settings?.DistrictA ?? 64f / 255f);
            UnityEngine.Color guidelineLinesColor = GuidelineColorSystem.GetGuidelineLinesColor(settings);
            UnityEngine.Color guidelinePreviewColor = GuidelineColorSystem.GetGuidelinePreviewColor(settings);
            m_GuidelineLinesColorRBinding = AddValueBinding("GuidelineLinesColorR", guidelineLinesColor.r);
            m_GuidelineLinesColorGBinding = AddValueBinding("GuidelineLinesColorG", guidelineLinesColor.g);
            m_GuidelineLinesColorBBinding = AddValueBinding("GuidelineLinesColorB", guidelineLinesColor.b);
            m_GuidelineLinesColorABinding = AddValueBinding("GuidelineLinesColorA", settings?.GuidelineLinesA ?? 1f);
            m_GuidelinePreviewColorRBinding = AddValueBinding("GuidelinePreviewColorR", guidelinePreviewColor.r);
            m_GuidelinePreviewColorGBinding = AddValueBinding("GuidelinePreviewColorG", guidelinePreviewColor.g);
            m_GuidelinePreviewColorBBinding = AddValueBinding("GuidelinePreviewColorB", guidelinePreviewColor.b);
            m_GuidelinePreviewColorABinding = AddValueBinding("GuidelinePreviewColorA", settings?.GuidelinePreviewA ?? 1f);
            m_GuidelineOpacityBinding = AddValueBinding("GuidelineOpacityPercent", settings?.GuidelineOpacityPercent ?? HoverColorsSettings.DefaultGuidelineOpacityPercent);
            m_GuidelineDefaultBinding = AddValueBinding("GuidelineDefaultPercent", settings?.GuidelineDefaultPercent ?? HoverColorsSettings.DefaultGuidelineOpacityPercent);
            m_PanelOpenBinding = AddValueBinding("PanelOpen", s_PanelOpen);
            m_PanelTooltipsEnabledBinding = AddValueBinding("PanelTooltipsEnabled", settings?.PanelTooltipsEnabled ?? true);
            m_UseDarkerPanelBinding = AddValueBinding("UseDarkerPanel", settings?.UseDarkerPanel ?? false);
            m_SurfaceToolAreasSuppressedBinding = AddValueBinding("SurfaceToolAreasSuppressed", suppressSurfaceToolAreas);
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
            UpdateIfChanged(m_FillABinding, settings?.FillA ?? 0f);
            UpdateIfChanged(m_DistrictRBinding, settings?.DistrictR ?? 128f / 255f);
            UpdateIfChanged(m_DistrictGBinding, settings?.DistrictG ?? 128f / 255f);
            UpdateIfChanged(m_DistrictBBinding, settings?.DistrictB ?? 128f / 255f);
            UpdateIfChanged(m_DistrictABinding, settings?.DistrictA ?? 64f / 255f);
            UnityEngine.Color guidelineLinesColor = GuidelineColorSystem.GetGuidelineLinesColor(settings);
            UnityEngine.Color guidelinePreviewColor = GuidelineColorSystem.GetGuidelinePreviewColor(settings);
            UpdateIfChanged(m_GuidelineLinesColorRBinding, guidelineLinesColor.r);
            UpdateIfChanged(m_GuidelineLinesColorGBinding, guidelineLinesColor.g);
            UpdateIfChanged(m_GuidelineLinesColorBBinding, guidelineLinesColor.b);
            UpdateIfChanged(m_GuidelineLinesColorABinding, settings?.GuidelineLinesA ?? 1f);
            UpdateIfChanged(m_GuidelinePreviewColorRBinding, guidelinePreviewColor.r);
            UpdateIfChanged(m_GuidelinePreviewColorGBinding, guidelinePreviewColor.g);
            UpdateIfChanged(m_GuidelinePreviewColorBBinding, guidelinePreviewColor.b);
            UpdateIfChanged(m_GuidelinePreviewColorABinding, settings?.GuidelinePreviewA ?? 1f);
            UpdateIfChanged(m_GuidelineOpacityBinding, settings?.GuidelineOpacityPercent ?? HoverColorsSettings.DefaultGuidelineOpacityPercent);
            UpdateIfChanged(m_GuidelineDefaultBinding, settings?.GuidelineDefaultPercent ?? HoverColorsSettings.DefaultGuidelineOpacityPercent);
            UpdateIfChanged(m_PanelOpenBinding, s_PanelOpen);
            UpdateIfChanged(m_PanelTooltipsEnabledBinding, settings?.PanelTooltipsEnabled ?? true);
            UpdateIfChanged(m_UseDarkerPanelBinding, settings?.UseDarkerPanel ?? false);
            UpdateIfChanged(m_SurfaceToolAreasSuppressedBinding, SurfaceToolOverlaySystem.SuppressSurfaceToolAreas);
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

        private void ToggleSurfaceToolAreas()
        {
            bool enabled = !SurfaceToolOverlaySystem.SuppressSurfaceToolAreas;
            SurfaceToolOverlaySystem.SetSuppression(enabled);

            HoverColorsSettings? settings = Mod.Settings;
            if (settings != null)
            {
                settings.SurfaceToolAreasSuppressed = enabled;
                settings.ApplyAndSave();
            }

            UpdateIfChanged(m_SurfaceToolAreasSuppressedBinding, SurfaceToolOverlaySystem.SuppressSurfaceToolAreas);
        }

        private static bool IsVanillaOutlineActive()
        {
            HoverColorsSettings? settings = Mod.Settings;
            return settings != null
                && OutlineColorSystem.MatchesCapturedVanillaProfile(
                    settings.OutlineR,
                    settings.OutlineG,
                    settings.OutlineB,
                    settings.OutlineA,
                    settings.FillA);
        }

        // True when the live swatch exactly matches what's stored in that slot.
        private static bool IsPresetActive(int slot)
        {
            HoverColorsSettings? s = Mod.Settings;
            if (s == null) return false;

            if (slot == 1)
            {
                return ApproxEqual(s.OutlineR, s.Preset1R)
                    && ApproxEqual(s.OutlineG, s.Preset1G)
                    && ApproxEqual(s.OutlineB, s.Preset1B)
                    && ApproxEqual(s.OutlineA, s.Preset1A)
                    && ApproxEqual(s.FillA, s.Preset1FillA);
            }

            if (slot == 2)
            {
                return ApproxEqual(s.OutlineR, s.Preset2R)
                    && ApproxEqual(s.OutlineG, s.Preset2G)
                    && ApproxEqual(s.OutlineB, s.Preset2B)
                    && ApproxEqual(s.OutlineA, s.Preset2A)
                    && ApproxEqual(s.FillA, s.Preset2FillA);
            }

            return false;
        }

        private static bool ApproxEqual(float a, float b) => Math.Abs(a - b) < 0.0005f;

        private static bool PresetsAtDefaults(HoverColorsSettings s)
        {
            return ApproxEqual(s.Preset1R, DefaultPreset1R) && ApproxEqual(s.Preset1G, DefaultPreset1G)
                && ApproxEqual(s.Preset1B, DefaultPreset1B) && ApproxEqual(s.Preset1A, DefaultPreset1A)
                && ApproxEqual(s.Preset1FillA, DefaultPreset1FillA)
                && s.Preset1GuidelinePercent == HoverColorsSettings.DefaultGuidelineOpacityPercent
                && ApproxEqual(s.Preset2R, DefaultPreset2R) && ApproxEqual(s.Preset2G, DefaultPreset2G)
                && ApproxEqual(s.Preset2B, DefaultPreset2B) && ApproxEqual(s.Preset2A, DefaultPreset2A)
                && ApproxEqual(s.Preset2FillA, DefaultPreset2FillA)
                && s.Preset2GuidelinePercent == HoverColorsSettings.DefaultGuidelineOpacityPercent;
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
