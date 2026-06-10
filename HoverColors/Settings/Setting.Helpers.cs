// Copyright (c) River Mochi.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// File: Settings/Setting.Helpers.cs
// Purpose: Small Options UI and external-link helpers for HoverColorsSettings.

namespace HoverColors.Settings
{
    using System; // Exception handling for external link open failures.

    using CS2Shared.RiverMochi; // WarnOnce for link failure logging.

    using Game.UI.Widgets; // DropdownItem for Options UI dropdown rows.

    using UnityEngine; // Application.OpenURL.

    public partial class HoverColorsSettings
    {
        public DropdownItem<int>[] GetToolColorModeItems()
        {
            return new[]
            {
                new DropdownItem<int>
                {
                    value = kToolColorModeRecommended,
                    displayName = GetToolColorModeLocaleID("Recommended"),
                },
                new DropdownItem<int>
                {
                    value = kToolColorModeVanilla,
                    displayName = GetToolColorModeLocaleID("Vanilla"),
                },
                new DropdownItem<int>
                {
                    value = kToolColorModeCustom,
                    displayName = GetToolColorModeLocaleID("Custom"),
                },
            };
        }

        public string GetToolColorModeLocaleID(string valueName)
        {
            return "Options[" + id + ".ToolColorMode." + valueName + "]";
        }

        private static void TryOpenUrl(string url)
        {
            try
            {
                Application.OpenURL(url);
            }
            catch (Exception ex)
            {
                LogUtils.WarnOnce(
                    "open-url-" + url,
                    () => $"Failed to open URL '{url}': {ex.GetType().Name}: {ex.Message}",
                    ex);
            }
        }
    }
}
