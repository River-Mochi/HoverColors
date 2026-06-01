// File: Systems/SurfaceToolOverlaySystem.cs
// Purpose: Optional Surface-tool overlay suppression for creators who want to preview layered surfaces
// without the active tool's blue boundary preview staying on top of the texture work.

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

    public partial class SurfaceToolOverlaySystem : GameSystemBase
    {
        private static readonly PropertyInfo? s_RequireAreasProperty =
            typeof(ToolBaseSystem).GetProperty(
                nameof(ToolBaseSystem.requireAreas),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private ToolSystem? m_ToolSystem;
        private AreaToolSystem? m_AreaToolSystem;
        private AreaTypeMask m_OriginalMask;
        private bool m_HasSuppressedMask;

        public static bool SuppressSurfaceToolAreas { get; private set; }

        public static void ToggleSuppression()
        {
            SuppressSurfaceToolAreas = !SuppressSurfaceToolAreas;
        }

        public static void SetSuppression(bool enabled)
        {
            SuppressSurfaceToolAreas = enabled;
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            m_ToolSystem = World.GetOrCreateSystemManaged<ToolSystem>();
            m_AreaToolSystem = World.GetOrCreateSystemManaged<AreaToolSystem>();
            Enabled = true;

            LogUtils.Info(() => $"{Mod.ModTag} SurfaceToolOverlaySystem created");
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
            if (!SuppressSurfaceToolAreas || !activeAreaTool)
            {
                RestoreIfNeeded();
                return;
            }

            AreaTypeMask current = m_AreaToolSystem.requireAreas;
            if ((current & AreaTypeMask.Surfaces) == 0)
            {
                if (m_HasSuppressedMask && current != (m_OriginalMask & ~AreaTypeMask.Surfaces))
                {
                    m_HasSuppressedMask = false;
                }

                return;
            }

            m_OriginalMask = current;
            AreaTypeMask stripped = current & ~AreaTypeMask.Surfaces;
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
            if ((m_OriginalMask & AreaTypeMask.Surfaces) != 0 && (current & AreaTypeMask.Surfaces) == 0)
            {
                SetRequireAreas(m_AreaToolSystem, m_OriginalMask);
            }

            m_HasSuppressedMask = false;
        }

        private static bool SetRequireAreas(ToolBaseSystem tool, AreaTypeMask mask)
        {
            if (s_RequireAreasProperty == null)
            {
                LogUtils.WarnOnce(
                    "surface-tool-require-areas-missing",
                    () => $"{Mod.ModTag} Cannot toggle Surface tool overlay: requireAreas property not found.");
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
                    () => $"{Mod.ModTag} Cannot toggle Surface tool overlay: {ex.GetType().Name}: {ex.Message}",
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
