// File: UI/src/panel/useMochiPanelText.ts
// Centralized panel text lookup so the main panel component stays focused on UI state.

import React from "react";
import { LocaleKey, usePanelLocalization } from "../localization";

export const useMochiPanelText = () => {
    const translatePanel = usePanelLocalization();

    return React.useMemo(() => {
        const l = (key: LocaleKey) => translatePanel(key);
        return {
            ariaClosePanel: l("HoverColors.UI.Aria.ClosePanel"),
            title: l("HoverColors.UI.Title"),
            tooltipClose: l("HoverColors.UI.Tooltip.Close"),
            tooltipDraggable: l("HoverColors.UI.Tooltip.Draggable"),
            tooltipFillOpacity: l("HoverColors.UI.Tooltip.FillOpacity"),
            tooltipGuidelinesColor: l("HoverColors.UI.Tooltip.GuidelinesColor"),
            tooltipGuidelinesPreviewColor: l("HoverColors.UI.Tooltip.GuidelinesPreviewColor"),
            tooltipGuidelinesDashedColor: l("HoverColors.UI.Tooltip.GuidelinesDashedColor"),
            tooltipGuidelinesOpacity: l("HoverColors.UI.Tooltip.GuidelinesOpacity"),
            tooltipInfo: l("HoverColors.UI.Tooltip.Info"),
            tooltipOutlineSwatch: l("HoverColors.UI.Tooltip.OutlineSwatch"),
            tooltipOwnerSwatch: l("HoverColors.UI.Tooltip.OwnerSwatch"),
            tooltipPreset1: l("HoverColors.UI.Tooltip.Preset1"),
            tooltipPreset2: l("HoverColors.UI.Tooltip.Preset2"),
            tooltipResetFill: l("HoverColors.UI.Tooltip.ResetFill"),
            tooltipResetGuidelines: l("HoverColors.UI.Tooltip.ResetGuidelines"),
            tooltipResetOutline: l("HoverColors.UI.Tooltip.ResetOutline"),
            tooltipResetPresets: l("HoverColors.UI.Tooltip.ResetPresets"),
            tooltipSurfaceToggle: l("HoverColors.UI.Tooltip.SurfaceToggle"),
            tooltipSpecializedIndustryToggle: l("HoverColors.UI.Tooltip.SpecializedIndustryToggle"),
            tooltipDistrictColors: l("HoverColors.UI.Tooltip.DistrictColors"),
            tooltipResetDistrictColors: l("HoverColors.UI.Tooltip.ResetDistrictColors"),
            districtMenuAllDistricts: l("HoverColors.UI.DistrictMenu.AllDistricts"),
            districtMenuResetAll: l("HoverColors.UI.DistrictMenu.ResetAll"),
        };
    }, [translatePanel]);
};
