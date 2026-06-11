// File: Systems/AreaToolOverlaySystem.cs
// Purpose: Optional Area-tool overlay suppression. Surface and Specialized Industry both write
// ToolBaseSystem.requireAreas, so one system owns the combined mask to avoid toggle conflicts.

namespace HoverColors.Systems
{
    using CS2Shared.RiverMochi;
    using Game;
    using Game.Areas;
    using Game.SceneFlow;
    using Game.Tools;
    using System;
    using System.Reflection;
    using Unity.Entities;

    public partial class AreaToolOverlaySystem : GameSystemBase
    {
        private static readonly PropertyInfo? s_RequireAreasProperty =
            typeof(ToolBaseSystem).GetProperty(
                nameof(ToolBaseSystem.requireAreas),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private ToolSystem? m_ToolSystem;
        private AreaToolSystem? m_AreaToolSystem;
        private AreaTypeMask m_OriginalMask;
        private AreaTypeMask m_LastSuppressedMask;
        private bool m_HasSuppressedMask;

        public static bool SuppressSurfaceToolAreas { get; private set; }
        public static bool SuppressSpecializedIndustryToolAreas { get; private set; }

        public static void ToggleSurfaceSuppression()
        {
            SuppressSurfaceToolAreas = !SuppressSurfaceToolAreas;
        }

        public static void SetSurfaceSuppression(bool enabled)
        {
            SuppressSurfaceToolAreas = enabled;
        }

        public static void ToggleSpecializedIndustrySuppression()
        {
            SuppressSpecializedIndustryToolAreas = !SuppressSpecializedIndustryToolAreas;
        }

        public static void SetSpecializedIndustrySuppression(bool enabled)
        {
            SuppressSpecializedIndustryToolAreas = enabled;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_ToolSystem = World.GetOrCreateSystemManaged<ToolSystem>();
            m_AreaToolSystem = World.GetOrCreateSystemManaged<AreaToolSystem>();
            Enabled = true;

            LogUtils.Info(() => $"{Mod.ModTag} AreaToolOverlaySystem created");
        }

        protected override void OnDestroy()
        {
            RestoreIfNeeded();
            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            if (!IsInGame() || m_ToolSystem == null || m_AreaToolSystem == null)
            {
                RestoreIfNeeded();
                return;
            }

            bool activeAreaTool = ReferenceEquals(m_ToolSystem.activeTool, m_AreaToolSystem);
            AreaTypeMask suppressMask = GetSuppressMask();
            if (suppressMask == AreaTypeMask.None || !activeAreaTool)
            {
                RestoreIfNeeded();
                return;
            }

            AreaTypeMask current = m_AreaToolSystem.requireAreas;
            if (m_HasSuppressedMask && current != (m_OriginalMask & ~m_LastSuppressedMask))
            {
                // Selected Area prefab changed; drop stale restore data before applying a new mask.
                m_HasSuppressedMask = false;
                m_LastSuppressedMask = AreaTypeMask.None;
            }

            AreaTypeMask applicableMask = current & suppressMask;
            if (applicableMask == AreaTypeMask.None)
            {
                return;
            }

            m_OriginalMask = current;
            m_LastSuppressedMask = applicableMask;
            AreaTypeMask stripped = current & ~applicableMask;
            if (SetRequireAreas(m_AreaToolSystem, stripped))
            {
                m_HasSuppressedMask = true;
            }
        }

        private void RestoreIfNeeded()
        {
            if (!m_HasSuppressedMask || m_AreaToolSystem == null)
            {
                m_HasSuppressedMask = false;
                return;
            }

            AreaTypeMask current = m_AreaToolSystem.requireAreas;
            AreaTypeMask expected = m_OriginalMask & ~m_LastSuppressedMask;
            if (current == expected)
            {
                SetRequireAreas(m_AreaToolSystem, m_OriginalMask);
            }

            m_HasSuppressedMask = false;
            m_LastSuppressedMask = AreaTypeMask.None;
        }

        private static AreaTypeMask GetSuppressMask()
        {
            AreaTypeMask mask = AreaTypeMask.None;
            if (SuppressSurfaceToolAreas)
            {
                mask |= AreaTypeMask.Surfaces;
            }

            if (SuppressSpecializedIndustryToolAreas)
            {
                mask |= AreaTypeMask.Lots;
            }

            return mask;
        }

        private static bool SetRequireAreas(ToolBaseSystem tool, AreaTypeMask mask)
        {
            if (s_RequireAreasProperty == null)
            {
                LogUtils.WarnOnce(
                    "surface-tool-require-areas-missing",
                    () => $"{Mod.ModTag} Cannot toggle Area tool overlays: requireAreas property not found.");
                return false;
            }

            try
            {
                s_RequireAreasProperty.SetValue(tool, mask);
                return true;
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "surface-tool-require-areas-set-failed",
                    () => $"{Mod.ModTag} Cannot toggle Area tool overlays: {ex.GetType().Name}: {ex.Message}",
                    ex);
                return false;
            }
        }

        private static bool IsInGame()
        {
            return GameManager.instance != null && GameManager.instance.gameMode == GameMode.Game;
        }
    }
}
