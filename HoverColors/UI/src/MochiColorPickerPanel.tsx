// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: Compact in-city hover-color panel anchored under the GameTopLeft icon button.
// Layout:
//   - Title bar: info toggle (tooltips on/off), draggable title, close button
//   - Outline row: icon (resets + shows vanilla-active indicator bar) + color swatch
//     + preset slots 1/2 (tap=apply, hold 0.7s=save) + preset reset icon
//   - Fill row: icon-led slider
//   - Guidelines row: reset icon + 2 compact color swatches + opacity slider
//   - Bottom: Surface/Specialized overlay toggles + District color menu

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { Color, toolbar } from "cs2/bindings";
import { trigger, useMapValue, useValue } from "cs2/api";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import { LocaleKey, usePanelLocalization } from "./localization";
import {
    AREA_MENU_NAME_TOKENS,
    CHANNEL,
    COMPACT_PICKER_BODY_CLASS,
    DISTRICT_AREA_NAME_TOKENS,
    DISTRICT_RESET_HOLD_MS,
    PRESET_HOLD_MS,
    districtA$,
    districtB$,
    districtG$,
    districtR$,
    fillA$,
    guidelineLinesColorA$,
    guidelineLinesColorB$,
    guidelineLinesColorG$,
    guidelineLinesColorR$,
    guidelineOpacity$,
    guidelinePreviewColorA$,
    guidelinePreviewColorB$,
    guidelinePreviewColorG$,
    guidelinePreviewColorR$,
    outlineA$,
    outlineB$,
    outlineG$,
    outlineR$,
    ownerA$,
    ownerB$,
    ownerG$,
    ownerR$,
    panelTooltipsEnabled$,
    preset1A$,
    preset1Active$,
    preset1B$,
    preset1G$,
    preset1R$,
    preset2A$,
    preset2Active$,
    preset2B$,
    preset2G$,
    preset2R$,
    sameEntity,
    specializedIndustryAreasSuppressed$,
    surfaceToolAreasSuppressed$,
    useDarkerPanel$,
    vanillaOutlineActive$,
} from "./panel/MochiPanelBindings";

import { DragGrip, PresetSlotButton } from "./panel/MochiPanelPieces";
import infoIconSrc from "../images/AdvisorInfoViewWhite.svg";
import lotToolIconSrc from "../images/LotTool03.svg";
import specializedIndustryIconSrc from "../images/LotToolSpecializedIndustry.svg";
import surfaceIconSrc from "../images/Districts03.svg";
import fillIconSrc from "../images/MainElements-Fill3.svg";
import outlineIconSrc from "../images/MainElements_short_bigTriangle.svg";
import guidelinesIconSrc from "../images/GuideLines4.svg";
import closeIconSrc from "../images/Close.svg";
import resetIconSrc from "../images/Reset_Button2.svg";
import styles from "./MochiColorPickerPanel.module.scss";


export const MochiColorPickerPanel = () => {
    const boundOutline: Color = {
        r: useValue(outlineR$),
        g: useValue(outlineG$),
        b: useValue(outlineB$),
        a: useValue(outlineA$),
    };
    const boundFillA = useValue(fillA$);
    const boundOwner: Color = {
        r: useValue(ownerR$),
        g: useValue(ownerG$),
        b: useValue(ownerB$),
        a: useValue(ownerA$),
    };
    const boundDistrict: Color = {
        r: useValue(districtR$),
        g: useValue(districtG$),
        b: useValue(districtB$),
        a: useValue(districtA$),
    };
    const boundGuideline = useValue(guidelineOpacity$);
    const boundGuidelineLinesColor: Color = {
        r: useValue(guidelineLinesColorR$),
        g: useValue(guidelineLinesColorG$),
        b: useValue(guidelineLinesColorB$),
        a: useValue(guidelineLinesColorA$),
    };
    const boundGuidelinePreviewColor: Color = {
        r: useValue(guidelinePreviewColorR$),
        g: useValue(guidelinePreviewColorG$),
        b: useValue(guidelinePreviewColorB$),
        a: useValue(guidelinePreviewColorA$),
    };
    const useDarkerPanel = useValue(useDarkerPanel$);
    const surfaceToolAreasSuppressed = useValue(surfaceToolAreasSuppressed$);
    const specializedIndustryAreasSuppressed = useValue(specializedIndustryAreasSuppressed$);
    const vanillaOutlineActive = useValue(vanillaOutlineActive$);
    const preset1Active = useValue(preset1Active$);
    const preset2Active = useValue(preset2Active$);
    const toolbarGroups = useValue(toolbar.toolbarGroups$);
    const selectedAssetMenu = useValue(toolbar.selectedAssetMenu$);
    const selectedAssetCategory = useValue(toolbar.selectedAssetCategory$);
    const selectedAsset = useValue(toolbar.selectedAsset$);
    const normalizeToolbarName = React.useCallback((value: string) => value.toUpperCase(), []);
    const areasMenu = React.useMemo(() => (
        toolbarGroups
            ?.flatMap(group => group.children ?? [])
            .find(item =>
                item.type === toolbar.ToolbarItemType.menu
                && AREA_MENU_NAME_TOKENS.some(token => normalizeToolbarName(item.name).includes(token)))
    ), [normalizeToolbarName, toolbarGroups]);
    const areasCategories = useMapValue(toolbar.assetCategories$, areasMenu?.entity);
    const districtCategory = React.useMemo(() => (
        areasCategories
            ?.find(item => DISTRICT_AREA_NAME_TOKENS.some(token => normalizeToolbarName(item.name).includes(token)))
    ), [areasCategories, normalizeToolbarName]);
    const districtAssets = useMapValue(toolbar.assets$, districtCategory?.entity);
    const districtAsset = React.useMemo(() => (
        districtAssets
            ?.find(item => DISTRICT_AREA_NAME_TOKENS.some(token => normalizeToolbarName(item.name).includes(token)))
        ?? districtAssets?.[0]
    ), [districtAssets, normalizeToolbarName]);

    // Stored slot colors. Swatch preview ignores alpha below for readability.
    const p1: Color = { r: useValue(preset1R$), g: useValue(preset1G$), b: useValue(preset1B$), a: useValue(preset1A$) };
    const p2: Color = { r: useValue(preset2R$), g: useValue(preset2G$), b: useValue(preset2B$), a: useValue(preset2A$) };

    const translatePanel = usePanelLocalization();

    // Tooltip toggle is persisted in ModsSettings/HoverColors/HoverColors.coc via HoverColorsUISystem.
    const tooltipsEnabled = useValue(panelTooltipsEnabled$);
    // Tooltips return undefined when disabled. Info tooltip stays visible for re-enable.
    const tt = React.useCallback(
        (s: string): string | undefined => (tooltipsEnabled ? s : undefined),
        [tooltipsEnabled],
    );

    const text = React.useMemo(() => {
        const l = (key: LocaleKey) => translatePanel(key);
        return {
            ariaClosePanel: l("HoverColors.UI.Aria.ClosePanel"),
            title: l("HoverColors.UI.Title"),
            tooltipClose: l("HoverColors.UI.Tooltip.Close"),
            tooltipDraggable: l("HoverColors.UI.Tooltip.Draggable"),
            tooltipFillOpacity: l("HoverColors.UI.Tooltip.FillOpacity"),
            tooltipGuidelinesColor: l("HoverColors.UI.Tooltip.GuidelinesColor"),
            tooltipGuidelinesPreviewColor: l("HoverColors.UI.Tooltip.GuidelinesPreviewColor"),
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
            districtMenuAllDistricts: l("HoverColors.UI.DistrictMenu.AllDistricts"),
            districtMenuResetAll: l("HoverColors.UI.DistrictMenu.ResetAll"),
        };
    }, [translatePanel]);

    const [outline, setOutline] = React.useState<Color>(boundOutline);
    const [ownerColor, setOwnerColor] = React.useState<Color>(boundOwner);
    const [fillA, setFillA] = React.useState<number>(boundFillA);
    const [districtColor, setDistrictColor] = React.useState<Color>(boundDistrict);
    const [guidelineLinesColor, setGuidelineLinesColor] = React.useState<Color>(boundGuidelineLinesColor);
    const [guidelinePreviewColor, setGuidelinePreviewColor] = React.useState<Color>(boundGuidelinePreviewColor);
    const [guidelineOpacity, setGuidelineOpacity] = React.useState<number>(boundGuideline);
    const [panelOffset, setPanelOffset] = React.useState({ x: 0, y: 0 });
    const [panelDragging, setPanelDragging] = React.useState(false);
    const [colorPickerDirection, setColorPickerDirection] = React.useState<"up" | "down">("down");
    const [ownerPickerDirection, setOwnerPickerDirection] = React.useState<"up" | "down">("down");
    const [guidelineLinesPickerDirection, setGuidelineLinesPickerDirection] = React.useState<"up" | "down">("up");
    const [guidelinePreviewPickerDirection, setGuidelinePreviewPickerDirection] = React.useState<"up" | "down">("up");
    const [districtPickerDirection, setDistrictPickerDirection] = React.useState<"up" | "down">("up");
    const [guidelineLinesPickerOpen, setGuidelineLinesPickerOpen] = React.useState(false);
    const [guidelinePreviewPickerOpen, setGuidelinePreviewPickerOpen] = React.useState(false);
    const [ownerPickerOpen, setOwnerPickerOpen] = React.useState(false);
    const [districtPickerOpen, setDistrictPickerOpen] = React.useState(false);
    const [districtMenuOpen, setDistrictMenuOpen] = React.useState(false);
    const [pendingDistrictToolOpen, setPendingDistrictToolOpen] = React.useState(false);
    // ColorField can swallow hover events; React state keeps the swatch ring reliable.
    const [swatchHovered, setSwatchHovered] = React.useState(false);
    const [ownerSwatchHovered, setOwnerSwatchHovered] = React.useState(false);
    const [guidelineLinesHovered, setGuidelineLinesHovered] = React.useState(false);
    const [guidelinePreviewHovered, setGuidelinePreviewHovered] = React.useState(false);
    const [districtSwatchHovered, setDistrictSwatchHovered] = React.useState(false);

    // Preset numbers use inline color, so hover color also needs React state.
    const [p1Hovered, setP1Hovered] = React.useState(false);
    const [p2Hovered, setP2Hovered] = React.useState(false);

    // Hold-to-save state for preset buttons
    const [holdSlot, setHoldSlot] = React.useState<0 | 1 | 2>(0);
    const [holdProgress, setHoldProgress] = React.useState(0); // 0..1, drives holdBar scaleX
    const holdTimerRef = React.useRef<number | null>(null);
    const holdStartRef = React.useRef<number>(0);
    const holdRafRef = React.useRef<number | null>(null);
    const [districtHoldProgress, setDistrictHoldProgress] = React.useState(0);
    const districtHoldTimerRef = React.useRef<number | null>(null);
    const districtHoldStartRef = React.useRef<number>(0);
    const districtHoldRafRef = React.useRef<number | null>(null);
    const districtHoldCompletedRef = React.useRef(false);

    const outlineSwatchRef = React.useRef<HTMLDivElement | null>(null);
    const ownerSwatchRef = React.useRef<HTMLDivElement | null>(null);
    const guidelineLinesPickerRef = React.useRef<HTMLDivElement | null>(null);
    const guidelinePreviewPickerRef = React.useRef<HTMLDivElement | null>(null);
    const districtPickerRef = React.useRef<HTMLDivElement | null>(null);
    const districtMenuRef = React.useRef<HTMLDivElement | null>(null);
    const districtColorSwatchRef = React.useRef<HTMLDivElement | null>(null);
    const areaPanelOpenTimerRef = React.useRef<number | null>(null);
    const districtToolOpenTimeoutRef = React.useRef<number | null>(null);
    const districtToolSelectRetryRef = React.useRef<number | null>(null);
    const panelElementRef = React.useRef<HTMLDivElement | null>(null);
    const panelDragFrameRef = React.useRef<number | null>(null);
    const panelDragPendingOffsetRef = React.useRef(panelOffset);
    const panelDragRef = React.useRef<{
        pointerX: number; pointerY: number;
        originX: number; originY: number;
        originLeft: number; originTop: number;
        originWidth: number; originHeight: number;
    } | null>(null);

    React.useEffect(() => { setOutline(boundOutline); },
        [boundOutline.r, boundOutline.g, boundOutline.b, boundOutline.a]);
    React.useEffect(() => { setOwnerColor(boundOwner); },
        [boundOwner.r, boundOwner.g, boundOwner.b, boundOwner.a]);
    React.useEffect(() => { setFillA(boundFillA); }, [boundFillA]);
    React.useEffect(() => { setDistrictColor(boundDistrict); },
        [boundDistrict.r, boundDistrict.g, boundDistrict.b, boundDistrict.a]);
    React.useEffect(() => { setGuidelineLinesColor(boundGuidelineLinesColor); },
        [boundGuidelineLinesColor.r, boundGuidelineLinesColor.g, boundGuidelineLinesColor.b, boundGuidelineLinesColor.a]);
    React.useEffect(() => { setGuidelinePreviewColor(boundGuidelinePreviewColor); },
        [boundGuidelinePreviewColor.r, boundGuidelinePreviewColor.g, boundGuidelinePreviewColor.b, boundGuidelinePreviewColor.a]);
    React.useEffect(() => { setGuidelineOpacity(boundGuideline); }, [boundGuideline]);

    React.useEffect(() => {
        if (typeof document === "undefined") {
            return;
        }

        const compactPickerOpen = ownerPickerOpen || districtPickerOpen || guidelineLinesPickerOpen || guidelinePreviewPickerOpen;
        document.body.classList.toggle(COMPACT_PICKER_BODY_CLASS, compactPickerOpen);

        if (!compactPickerOpen) {
            return () => document.body.classList.remove(COMPACT_PICKER_BODY_CLASS);
        }

        const onMouseDown = (event: MouseEvent) => {
            const target = event.target as Element | null;
            if (target == null) {
                return;
            }

            // Vanilla ColorField closes on outside clicks but does not call onClosePicker there,
            // Keep scoped CSS mode in sync without touching picker internals.
            if (
                districtMenuRef.current?.contains(target)
                || districtPickerRef.current?.contains(target)
                || districtColorSwatchRef.current?.contains(target)
                || ownerSwatchRef.current?.contains(target)
                || guidelineLinesPickerRef.current?.contains(target)
                || guidelinePreviewPickerRef.current?.contains(target)
                || target.closest(".color-picker-container_Sj5")
            ) {
                return;
            }

            setDistrictPickerOpen(false);
            setOwnerPickerOpen(false);
            setGuidelineLinesPickerOpen(false);
            setGuidelinePreviewPickerOpen(false);
        };

        document.addEventListener("mousedown", onMouseDown);
        return () => {
            document.removeEventListener("mousedown", onMouseDown);
            document.body.classList.remove(COMPACT_PICKER_BODY_CLASS);
        };
    }, [districtPickerOpen, guidelineLinesPickerOpen, guidelinePreviewPickerOpen, ownerPickerOpen]);

    React.useEffect(() => {
        if (!districtMenuOpen || typeof document === "undefined") {
            return;
        }

        const onMouseDown = (event: MouseEvent) => {
            const target = event.target as Element | null;
            if (
                target == null
                || districtPickerRef.current?.contains(target)
                || districtMenuRef.current?.contains(target)
                || target.closest(".color-picker-container_Sj5")
            ) {
                return;
            }

            setDistrictMenuOpen(false);
        };

        document.addEventListener("mousedown", onMouseDown);
        return () => document.removeEventListener("mousedown", onMouseDown);
    }, [districtMenuOpen]);

    React.useEffect(() => () => {
        if (areaPanelOpenTimerRef.current != null) {
            clearTimeout(areaPanelOpenTimerRef.current);
            areaPanelOpenTimerRef.current = null;
        }
        if (districtToolOpenTimeoutRef.current != null) {
            clearTimeout(districtToolOpenTimeoutRef.current);
            districtToolOpenTimeoutRef.current = null;
        }
        if (districtToolSelectRetryRef.current != null) {
            clearTimeout(districtToolSelectRetryRef.current);
            districtToolSelectRetryRef.current = null;
        }
        if (districtHoldTimerRef.current != null) {
            clearTimeout(districtHoldTimerRef.current);
            districtHoldTimerRef.current = null;
        }
        if (districtHoldRafRef.current != null) {
            cancelAnimationFrame(districtHoldRafRef.current);
            districtHoldRafRef.current = null;
        }
    }, []);

    React.useEffect(() => {
        if (!pendingDistrictToolOpen) {
            return;
        }

        if (areasMenu == null) {
            setPendingDistrictToolOpen(false);
            return;
        }

        if (!sameEntity(selectedAssetMenu, areasMenu.entity)) {
            toolbar.selectAssetMenu(areasMenu.entity);
            return;
        }

        if (districtCategory == null) {
            return;
        }

        if (!sameEntity(selectedAssetCategory, districtCategory.entity)) {
            toolbar.selectAssetCategory(districtCategory.entity);
            return;
        }

        if (districtAsset == null) {
            return;
        }

        // Category opens the Areas panel; asset selection enters the District tool.
        // Vanilla can restore the previous subtool, so retry once after opening.
        toolbar.clearAssetSelection();
        toolbar.selectAssetMenu(areasMenu.entity);
        toolbar.selectAssetCategory(districtCategory.entity);
        toolbar.selectAsset(districtAsset.entity, true);

        if (districtToolSelectRetryRef.current != null) {
            clearTimeout(districtToolSelectRetryRef.current);
        }
        const areasMenuEntity = areasMenu.entity;
        const districtCategoryEntity = districtCategory.entity;
        const districtEntity = districtAsset.entity;
        districtToolSelectRetryRef.current = window.setTimeout(() => {
            toolbar.clearAssetSelection();
            toolbar.selectAssetMenu(areasMenuEntity);
            toolbar.selectAssetCategory(districtCategoryEntity);
            toolbar.selectAsset(districtEntity, true);
            districtToolSelectRetryRef.current = null;
        }, 250);

        if (districtToolOpenTimeoutRef.current != null) {
            clearTimeout(districtToolOpenTimeoutRef.current);
            districtToolOpenTimeoutRef.current = null;
        }
        setPendingDistrictToolOpen(false);
    }, [
        areasMenu,
        districtAsset,
        districtCategory,
        pendingDistrictToolOpen,
        selectedAsset,
        selectedAssetCategory,
        selectedAssetMenu,
    ]);

    // Panel drag
    React.useEffect(() => {
        if (!panelDragging) return;
        const onMove = (e: MouseEvent) => {
            const d = panelDragRef.current;
            if (d == null) return;
            const dx = e.clientX - d.pointerX;
            const dy = e.clientY - d.pointerY;
            let nx = d.originX + dx;
            let ny = d.originY + dy;
            const nl = d.originLeft + dx, nt = d.originTop + dy;
            const nr = nl + d.originWidth, nb = nt + d.originHeight;
            if (nl < 0) nx -= nl;
            if (nt < 0) ny -= nt;
            if (nr > window.innerWidth) nx -= nr - window.innerWidth;
            if (nb > window.innerHeight) ny -= nb - window.innerHeight;
            panelDragPendingOffsetRef.current = { x: nx, y: ny };
            if (panelDragFrameRef.current == null) {
                panelDragFrameRef.current = window.requestAnimationFrame(() => {
                    panelDragFrameRef.current = null;
                    setPanelOffset(panelDragPendingOffsetRef.current);
                });
            }
        };
        const onUp = () => {
            if (panelDragFrameRef.current != null) {
                window.cancelAnimationFrame(panelDragFrameRef.current);
                panelDragFrameRef.current = null;
            }
            panelDragRef.current = null;
            setPanelDragging(false);
            setPanelOffset(panelDragPendingOffsetRef.current);
        };
        window.addEventListener("mousemove", onMove);
        window.addEventListener("mouseup", onUp);
        return () => { window.removeEventListener("mousemove", onMove); window.removeEventListener("mouseup", onUp); };
    }, [panelDragging]);

    const normalizeColorFieldValue = (value: Color) => {
        const alpha = typeof value.a === "number" && Number.isFinite(value.a) ? value.a : 1;
        return {
            ...value,
            a: Math.max(0, Math.min(1, alpha)),
        };
    };

    // Live color handlers
    const handleOutlineChange = (value: Color) => {
        setOutline(value);
        trigger(CHANNEL, "SetOutlineColor", value.r, value.g, value.b, value.a);
    };
    const handleOwnerColorChange = (value: Color) => {
        const syncedValue = normalizeColorFieldValue(value);
        setOwnerColor(syncedValue);
        trigger(CHANNEL, "SetOwnerColor", syncedValue.r, syncedValue.g, syncedValue.b, syncedValue.a);
    };
    const handleFillAChange = (v: number) => {
        const value = Math.max(0, Math.min(1, v));
        setFillA(value);
        trigger(CHANNEL, "SetFillAlpha", value);
    };
    const handleDistrictColorChange = (value: Color) => {
        cancelDistrictHold();
        setDistrictColor(value);
        trigger(CHANNEL, "SetDistrictColor", value.r, value.g, value.b, value.a);
    };
    const handleGuidelineLinesColorChange = (value: Color) => {
        const syncedValue = normalizeColorFieldValue(value);
        setGuidelineLinesColor(syncedValue);
        trigger(CHANNEL, "SetGuidelineLinesColor", syncedValue.r, syncedValue.g, syncedValue.b, syncedValue.a);
    };
    const handleGuidelinePreviewColorChange = (value: Color) => {
        const syncedValue = normalizeColorFieldValue(value);
        setGuidelinePreviewColor(syncedValue);
        trigger(CHANNEL, "SetGuidelinePreviewColor", syncedValue.r, syncedValue.g, syncedValue.b, syncedValue.a);
    };
    const handleGuidelineChange = (v: number) => {
        const value = Math.max(0, Math.min(100, Math.round(v / 5) * 5));
        setGuidelineOpacity(value);
        trigger(CHANNEL, "SetGuidelineOpacity", value);
    };
    const handleClosePanel = () => trigger(CHANNEL, "SetPanelOpen", false);
    const handleResetOutline = () => trigger(CHANNEL, "ResetOutlineToVanilla");
    const handleResetFill = () => handleFillAChange(0);
    const handleResetGuidelines = () => trigger(CHANNEL, "ResetGuidelines");
    const handleToggleSurfaceToolAreas = () => trigger(CHANNEL, "ToggleSurfaceToolAreas");
    const handleToggleSpecializedIndustryAreas = () => trigger(CHANNEL, "ToggleSpecializedIndustryAreas");
    const handleTogglePresetDefaults = () => trigger(CHANNEL, "TogglePresetDefaults");
    const handleResetDistrict = () => {
        trigger(CHANNEL, "ResetDistrictToVanilla");
        setDistrictPickerOpen(false);
    };

    // Preset hold-to-save
    const cancelHold = React.useCallback(() => {
        if (holdTimerRef.current != null) { clearTimeout(holdTimerRef.current); holdTimerRef.current = null; }
        if (holdRafRef.current != null) { cancelAnimationFrame(holdRafRef.current); holdRafRef.current = null; }
        setHoldSlot(0);
        setHoldProgress(0);
    }, []);

    const handlePresetMouseDown = (slot: 1 | 2) => (e: React.MouseEvent) => {
        e.preventDefault();
        cancelHold();
        holdStartRef.current = performance.now();
        setHoldSlot(slot);
        setHoldProgress(0);
        const SWEEP_DELAY = 150; // avoids a save-sweep flash on quick tap
        const tick = () => {
            const elapsed = performance.now() - holdStartRef.current;
            if (elapsed >= SWEEP_DELAY) {
                // Sweep uses only the visible portion of the hold window.
                const p = Math.min((elapsed - SWEEP_DELAY) / (PRESET_HOLD_MS - SWEEP_DELAY), 1);
                setHoldProgress(p);
                if (p < 1) holdRafRef.current = requestAnimationFrame(tick);
            } else {
                holdRafRef.current = requestAnimationFrame(tick);
            }
        };
        holdRafRef.current = requestAnimationFrame(tick);
        holdTimerRef.current = window.setTimeout(() => {
            holdTimerRef.current = null;
            if (holdRafRef.current != null) { cancelAnimationFrame(holdRafRef.current); holdRafRef.current = null; }
            trigger(CHANNEL, "SavePreset", slot);
            setHoldSlot(0);
            setHoldProgress(0);
        }, PRESET_HOLD_MS);
    };

    const handlePresetMouseUp = (slot: 1 | 2) => () => {
        if (holdTimerRef.current != null) {
            clearTimeout(holdTimerRef.current);
            holdTimerRef.current = null;
            trigger(CHANNEL, "ApplyPreset", slot);
        }
        if (holdRafRef.current != null) { cancelAnimationFrame(holdRafRef.current); holdRafRef.current = null; }
        setHoldSlot(0);
        setHoldProgress(0);
    };

    const cancelDistrictHold = React.useCallback(() => {
        if (districtHoldTimerRef.current != null) {
            clearTimeout(districtHoldTimerRef.current);
            districtHoldTimerRef.current = null;
        }
        if (districtHoldRafRef.current != null) {
            cancelAnimationFrame(districtHoldRafRef.current);
            districtHoldRafRef.current = null;
        }
        districtHoldCompletedRef.current = false;
        setDistrictHoldProgress(0);
    }, []);

    const handleDistrictMouseDownCapture = (event: React.MouseEvent<HTMLDivElement>) => {
        if (event.button !== 0) {
            return;
        }

        cancelDistrictHold();
        districtHoldCompletedRef.current = false;
        districtHoldStartRef.current = performance.now();
        setDistrictHoldProgress(0);

        const SWEEP_DELAY = 150;
        const tick = () => {
            const elapsed = performance.now() - districtHoldStartRef.current;
            if (elapsed >= SWEEP_DELAY) {
                const p = Math.min((elapsed - SWEEP_DELAY) / (DISTRICT_RESET_HOLD_MS - SWEEP_DELAY), 1);
                setDistrictHoldProgress(p);
                if (p < 1) {
                    districtHoldRafRef.current = requestAnimationFrame(tick);
                }
            } else {
                districtHoldRafRef.current = requestAnimationFrame(tick);
            }
        };

        districtHoldRafRef.current = requestAnimationFrame(tick);
        districtHoldTimerRef.current = window.setTimeout(() => {
            districtHoldTimerRef.current = null;
            if (districtHoldRafRef.current != null) {
                cancelAnimationFrame(districtHoldRafRef.current);
                districtHoldRafRef.current = null;
            }
            districtHoldCompletedRef.current = true;
            handleResetDistrict();
            setDistrictHoldProgress(0);
        }, DISTRICT_RESET_HOLD_MS);
    };

    const handleDistrictMouseUpCapture = () => {
        if (!districtHoldCompletedRef.current) {
            cancelDistrictHold();
        }
    };

    const handleDistrictClickCapture = (event: React.MouseEvent<HTMLDivElement>) => {
        if (!districtHoldCompletedRef.current) {
            event.preventDefault();
            event.stopPropagation();
            setDistrictMenuOpen(open => !open);
            openAreasToolPanel();
            return;
        }

        event.preventDefault();
        event.stopPropagation();
        districtHoldCompletedRef.current = false;
    };

    const updateColorPickerDirection = React.useCallback(() => {
        const swatch = outlineSwatchRef.current;
        if (swatch == null) return;
        const rect = swatch.getBoundingClientRect();
        setColorPickerDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const updateOwnerPickerDirection = React.useCallback(() => {
        const swatch = ownerSwatchRef.current;
        if (swatch == null) return;
        const rect = swatch.getBoundingClientRect();
        setOwnerPickerDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const updateDistrictPickerDirection = React.useCallback(() => {
        const swatch = districtColorSwatchRef.current ?? districtPickerRef.current;
        if (swatch == null) return;
        const rect = swatch.getBoundingClientRect();
        setDistrictPickerDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const updateGuidelineLinesPickerDirection = React.useCallback(() => {
        const swatch = guidelineLinesPickerRef.current;
        if (swatch == null) return;
        const rect = swatch.getBoundingClientRect();
        setGuidelineLinesPickerDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const updateGuidelinePreviewPickerDirection = React.useCallback(() => {
        const swatch = guidelinePreviewPickerRef.current;
        if (swatch == null) return;
        const rect = swatch.getBoundingClientRect();
        setGuidelinePreviewPickerDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const openAreasToolPanel = React.useCallback(() => {
        if (areaPanelOpenTimerRef.current != null) {
            clearTimeout(areaPanelOpenTimerRef.current);
        }
        if (districtToolOpenTimeoutRef.current != null) {
            clearTimeout(districtToolOpenTimeoutRef.current);
        }
        if (districtToolSelectRetryRef.current != null) {
            clearTimeout(districtToolSelectRetryRef.current);
            districtToolSelectRetryRef.current = null;
        }

        // Defer until after picker click finishes; toolbar bindings arrive in steps.
        areaPanelOpenTimerRef.current = window.setTimeout(() => {
            toolbar.clearAssetSelection();
            setPendingDistrictToolOpen(true);
            areaPanelOpenTimerRef.current = null;
        }, 80);

        districtToolOpenTimeoutRef.current = window.setTimeout(() => {
            setPendingDistrictToolOpen(false);
            districtToolOpenTimeoutRef.current = null;
        }, 1500);
    }, []);

    const handlePanelDragStart = (e: React.MouseEvent<HTMLDivElement>) => {
        e.preventDefault(); e.stopPropagation();
        const rect = panelElementRef.current?.getBoundingClientRect();
        panelDragPendingOffsetRef.current = panelOffset;
        panelDragRef.current = {
            pointerX: e.clientX, pointerY: e.clientY,
            originX: panelOffset.x, originY: panelOffset.y,
            originLeft: rect?.left ?? 0, originTop: rect?.top ?? 0,
            originWidth: rect?.width ?? 0, originHeight: rect?.height ?? 0,
        };
        setPanelDragging(true);
    };

    const resolver = VanillaComponentResolver.instance;
    const ColorField = resolver.ColorField;
    const Slider = resolver.Slider;
    const focusDisabled = resolver.FOCUS_DISABLED;
    const numberFieldClass = resolver.mouseToolOptionsTheme["number-field"];
    const roundHighlightButtonTheme = resolver.roundHighlightButtonTheme;
    const panelBaseTheme = resolver.panelBaseTheme;
    const panelTheme = resolver.panelTheme;
    const infoviewMenuTheme = resolver.infoviewMenuTheme;
    const outlineFieldClass = styles.outlineField;
    const ownerFieldClass = styles.ownerField;
    const closeButtonClass = `${roundHighlightButtonTheme["button"] ?? ""} ${styles.closeButton}`;
    const panelFrameClass = `${panelBaseTheme.panel ?? "panel_YqS"} ${infoviewMenuTheme.menu ?? "menu_O_M"} ${styles.panelFrame}`;
    const panelSurfaceClass = useDarkerPanel ? styles.panelDarker : styles.panelStandard;
    const panelContentClass = `${panelTheme.content ?? "content_XD5 content_AD7 child-opacity-transition_nkS"} ${infoviewMenuTheme.content ?? "content_Hzl"} ${styles.panelContent} ${panelSurfaceClass}`;

    // Preview ignores alpha for legibility; saved preset still applies alpha in-game.
    const presetPreviewStyle = (c: Color) => ({
        backgroundColor: `rgb(${Math.round(c.r * 255)},${Math.round(c.g * 255)},${Math.round(c.b * 255)})`,
    });

    // Small swatches show RGB clearly; alpha still applies in-game.
    const compactSwatchStyle = (c: Color, hovered = false) => {
        const channel = (value: unknown, fallback: number) => {
            const n = Number(value);
            return Math.round(Math.max(0, Math.min(1, Number.isFinite(n) ? n : fallback)) * 255);
        };
        const r = channel(c.r, 0.7);
        const g = channel(c.g, 0.7);
        const b = channel(c.b, 1);
        const idleShadow = useDarkerPanel ? "inset 0 0 0 1rem rgba(7, 13, 18, 0.32)" : "none";
        const hoverShadow = useDarkerPanel
            ? "inset 0 0 0 1rem rgba(7, 13, 18, 0.32), 0 0 0 1.15rem rgba(255, 255, 255, 0.76)"
            : "0 0 0 1.15rem rgba(255, 255, 255, 0.76)";
        return {
            backgroundColor: `rgb(${r},${g},${b})`,
            // Standard stays clean; Dark keeps the extra edge for contrast.
            boxShadow: hovered ? hoverShadow : idleShadow,
        };
    };

    // Swatch boxes for guidelines and owner color borders on hover.
    const guidelineShellStyle = (c: Color, hovered: boolean) => compactSwatchStyle(c, hovered);

    // Compact swatches: hidden vanilla ColorField owns picker; shell/preview own hover.
    const ownerShellStyle = (c: Color, hovered: boolean) => guidelineShellStyle(c, hovered);

    const presetNumberColor = (active: boolean, hovered: boolean) => {
        if (hovered) {
            return "rgba(255, 255, 255, 1)";
        }
        // Active number stays opaque; softer cyan comes from RGB, not alpha.
        return active ? "rgba(150, 235, 255, 0.96)" : "rgba(255, 255, 255, 0.78)";
    };

    // scaleX keeps hold progress independent of button width.
    const holdBarStyle = (progress: number) => ({ transform: `scaleX(${progress})` });

    return (
        <div
            className={styles.panelAnchor}
            style={{ transform: `translate(${panelOffset.x}px, ${panelOffset.y}px)` }}
        >
            <div ref={panelElementRef} className={panelFrameClass}>
                <div className={panelContentClass}>

                    {/* Title bar */}
                    <div className={styles.titleBar}>
                        {/* Info toggles panel tooltips. Its own tooltip always stays visible. */}
                        <Tooltip tooltip={text.tooltipInfo}>
                            <button
                                type="button"
                                className={`${styles.infoButton} ${!tooltipsEnabled ? styles.infoButtonActive : ""}`}
                                onClick={() => trigger(CHANNEL, "SetPanelTooltipsEnabled", !tooltipsEnabled)}
                            >
                                <img src={infoIconSrc} className={`${styles.infoIcon} ${styles.idleIcon}`} alt="" />
                            </button>
                        </Tooltip>

                        <Tooltip tooltip={tt(text.tooltipDraggable)}>
                            <div
                                className={`${styles.titleDragHandle} ${panelDragging ? styles.titleDragHandleActive : ""}`}
                                onMouseDown={handlePanelDragStart}
                            >
                                <span className={styles.titleText}>{text.title}</span>
                            </div>
                        </Tooltip>

                        <Tooltip tooltip={tt(text.tooltipClose)}>
                            <Button
                                className={closeButtonClass}
                                variant="icon"
                                onClick={handleClosePanel}
                                focusKey={focusDisabled}
                                aria-label={text.ariaClosePanel}
                            >
                                <img src={closeIconSrc} className={styles.closeIcon} alt="" />
                            </Button>
                        </Tooltip>
                    </div>

                    {/* Control rows */}
                    <div className={styles.body}>
                        {/* Outline row — click icon to reset to vanilla; indicator bar appears when vanilla is active */}
                        <div className={`${styles.controlRow} ${styles.outlineRow}`}>
                            <Tooltip tooltip={tt(text.tooltipResetOutline)}>
                                <button
                                    type="button"
                                    className={`${styles.controlIconButton} ${vanillaOutlineActive ? styles.vanillaActiveButton : ""}`}
                                    onClick={handleResetOutline}
                                >
                                    <img src={outlineIconSrc} className={`${styles.controlIcon} ${styles.idleIcon}`} alt="" />
                                </button>
                            </Tooltip>
                            <div className={`${styles.controlBody} ${styles.outlineControlBody}`}>
                                {/* Left group: swatch + preset slots. */}
                                <div className={styles.outlineLeft}>
                                    <Tooltip tooltip={tt(text.tooltipOutlineSwatch)}>
                                        <div
                                            ref={outlineSwatchRef}
                                            className={`${styles.outlineFieldShell} ${swatchHovered ? styles.outlineFieldShellHovered : ""}`}
                                            // CSS :hover plus bubbled events keeps Cohtml hover reliable.
                                            onMouseOver={() => { if (!swatchHovered) { setSwatchHovered(true); updateColorPickerDirection(); }}}
                                            onMouseMove={() => { if (!swatchHovered) { setSwatchHovered(true); updateColorPickerDirection(); }}}
                                            onMouseLeave={() => setSwatchHovered(false)}
                                            onMouseDown={updateColorPickerDirection}
                                        >
                                            <ColorField
                                                focusKey={focusDisabled}
                                                className={outlineFieldClass}
                                                value={outline}
                                                alpha={true}
                                                popupDirection={colorPickerDirection}
                                                hideHint={true}
                                                hexInput={true}
                                                colorWheel={true}
                                                // Extra hover events from ColorField root.
                                                onMouseEnter={() => setSwatchHovered(true)}
                                                onMouseLeave={() => setSwatchHovered(false)}
                                                onChange={handleOutlineChange}
                                                onOpenPicker={updateColorPickerDirection}
                                            />
                                            <span className={styles.outlineFieldHoverRing} aria-hidden="true" />
                                        </div>
                                    </Tooltip>
                                    <Tooltip tooltip={tt(text.tooltipOwnerSwatch)}>
                                        <div
                                            ref={ownerSwatchRef}
                                            className={`${styles.ownerFieldShell} ${ownerSwatchHovered ? styles.ownerFieldShellHovered : ""}`}
                                            style={ownerShellStyle(ownerColor, ownerSwatchHovered)}
                                            onMouseOver={() => { if (!ownerSwatchHovered) { setOwnerSwatchHovered(true); updateOwnerPickerDirection(); }}}
                                            onMouseMove={() => { if (!ownerSwatchHovered) { setOwnerSwatchHovered(true); updateOwnerPickerDirection(); }}}
                                            onMouseLeave={() => setOwnerSwatchHovered(false)}
                                            onMouseDown={updateOwnerPickerDirection}
                                        >
                                            <ColorField
                                                focusKey={focusDisabled}
                                                className={ownerFieldClass}
                                                value={ownerColor}
                                                alpha={true}
                                                popupDirection={ownerPickerDirection}
                                                hideHint={true}
                                                hexInput={true}
                                                colorWheel={false}
                                                onChange={handleOwnerColorChange}
                                                onOpenPicker={() => {
                                                    setOwnerPickerOpen(true);
                                                    updateOwnerPickerDirection();
                                                }}
                                                onClosePicker={() => setOwnerPickerOpen(false)}
                                            />
                                            <span
                                                className={styles.ownerColorPreview}
                                                style={compactSwatchStyle(ownerColor, ownerSwatchHovered)}
                                                aria-hidden="true"
                                            />
                                        </div>
                                    </Tooltip>

                                    {/* Presets: tap applies; hold saves current color/alpha. */}
                                    <PresetSlotButton
                                        slot={1}
                                        color={p1}
                                        active={preset1Active}
                                        holdActive={holdSlot === 1}
                                        holdProgress={holdProgress}
                                        tooltip={tt(text.tooltipPreset1)}
                                        marginLeft="10rem"
                                        numberColor={presetNumberColor(preset1Active, p1Hovered)}
                                        presetPreviewStyle={presetPreviewStyle}
                                        holdBarStyle={holdBarStyle}
                                        onMouseEnter={() => setP1Hovered(true)}
                                        onMouseDown={handlePresetMouseDown(1)}
                                        onMouseUp={handlePresetMouseUp(1)}
                                        onMouseLeave={() => { setP1Hovered(false); cancelHold(); }}
                                    />

                                    <PresetSlotButton
                                        slot={2}
                                        color={p2}
                                        active={preset2Active}
                                        holdActive={holdSlot === 2}
                                        holdProgress={holdProgress}
                                        tooltip={tt(text.tooltipPreset2)}
                                        marginLeft="5rem"
                                        numberColor={presetNumberColor(preset2Active, p2Hovered)}
                                        presetPreviewStyle={presetPreviewStyle}
                                        holdBarStyle={holdBarStyle}
                                        onMouseEnter={() => setP2Hovered(true)}
                                        onMouseDown={handlePresetMouseDown(2)}
                                        onMouseUp={handlePresetMouseUp(2)}
                                        onMouseLeave={() => { setP2Hovered(false); cancelHold(); }}
                                    />

                                </div>

                                {/* SVG reset avoids missing glyph boxes in CJK fonts. */}
                                <div className={styles.outlineRight}>
                                    <Tooltip tooltip={tt(text.tooltipResetPresets)}>
                                        <button
                                            type="button"
                                            className={styles.presetResetBare}
                                            onClick={handleTogglePresetDefaults}
                                        >
                                            <img src={resetIconSrc} className={styles.resetIcon} alt="" />
                                        </button>
                                    </Tooltip>
                                </div>
                            </div>
                        </div>

                        {/* Fill row */}
                        <div className={styles.controlRow}>
                            <Tooltip tooltip={tt(text.tooltipResetFill)}>
                                <button type="button" className={styles.controlIconButton} onClick={handleResetFill}>
                                    <img src={fillIconSrc} className={`${styles.controlIcon} ${styles.idleIcon}`} alt="" />
                                </button>
                            </Tooltip>
                            <Tooltip tooltip={tt(text.tooltipFillOpacity)}>
                                <div className={styles.controlBody}>
                                    <div className={styles.sliderRow}>
                                        <Slider
                                            focusKey={focusDisabled}
                                            className={styles.slider}
                                            value={fillA}
                                            start={0}
                                            end={1}
                                            gamepadStep={0.01}
                                            onChange={handleFillAChange}
                                        />
                                        <div className={`${styles.valueField} ${numberFieldClass}`}>
                                            {`${Math.round(fillA * 100)}%`}
                                        </div>
                                    </div>
                                </div>
                            </Tooltip>
                        </div>

                        {/* Guidelines: icon resets guideline colors; opacity slider stays as-is. */}
                        <div className={styles.controlRow}>
                            <Tooltip tooltip={tt(text.tooltipResetGuidelines)}>
                                <button
                                    type="button"
                                    className={styles.controlIconButton}
                                    onClick={handleResetGuidelines}
                                >
                                    <img src={guidelinesIconSrc} className={`${styles.controlIcon} ${styles.idleIcon} ${styles.guidelinesIcon}`} alt="" />
                                </button>
                            </Tooltip>
                            <div className={`${styles.controlBody} ${styles.guidelineControlBody}`}>
                                <div className={styles.guidelineSwatches}>
                                    <Tooltip tooltip={tt(text.tooltipGuidelinesColor)}>
                                        <div
                                            ref={guidelineLinesPickerRef}
                                            className={`${styles.guidelineColorShell} ${guidelineLinesHovered ? styles.guidelineColorShellHovered : ""}`}
                                            style={guidelineShellStyle(guidelineLinesColor, guidelineLinesHovered)}
                                            onMouseOver={() => { if (!guidelineLinesHovered) { setGuidelineLinesHovered(true); } updateGuidelineLinesPickerDirection(); }}
                                            onMouseMove={() => { if (!guidelineLinesHovered) { setGuidelineLinesHovered(true); } }}
                                            onMouseLeave={() => setGuidelineLinesHovered(false)}
                                            onMouseDown={updateGuidelineLinesPickerDirection}
                                        >
                                            {/* Hidden ColorField opens picker only; shell/preview own color + hover. */}
                                            <ColorField
                                                focusKey={focusDisabled}
                                                className={styles.guidelineColorField}
                                                value={guidelineLinesColor}
                                                alpha={true}
                                                popupDirection={guidelineLinesPickerDirection}
                                                hideHint={true}
                                                hexInput={true}
                                                colorWheel={false}
                                                onChange={handleGuidelineLinesColorChange}
                                                onOpenPicker={() => {
                                                    setGuidelineLinesPickerOpen(true);
                                                    updateGuidelineLinesPickerDirection();
                                                }}
                                                onClosePicker={() => setGuidelineLinesPickerOpen(false)}
                                            />
                                            <span
                                                className={styles.guidelineColorPreview}
                                                style={compactSwatchStyle(guidelineLinesColor, guidelineLinesHovered)}
                                                aria-hidden="true"
                                            />
                                        </div>
                                    </Tooltip>
                                    <Tooltip tooltip={tt(text.tooltipGuidelinesPreviewColor)}>
                                        <div
                                            ref={guidelinePreviewPickerRef}
                                            className={`${styles.guidelineColorShell} ${styles.guidelinePreviewColorShell} ${guidelinePreviewHovered ? styles.guidelineColorShellHovered : ""}`}
                                            style={guidelineShellStyle(guidelinePreviewColor, guidelinePreviewHovered)}
                                            onMouseOver={() => { if (!guidelinePreviewHovered) { setGuidelinePreviewHovered(true); } updateGuidelinePreviewPickerDirection(); }}
                                            onMouseMove={() => { if (!guidelinePreviewHovered) { setGuidelinePreviewHovered(true); } }}
                                            onMouseLeave={() => setGuidelinePreviewHovered(false)}
                                            onMouseDown={updateGuidelinePreviewPickerDirection}
                                        >
                                            {/* Hidden ColorField opens picker only; shell/preview own color + hover. */}
                                            <ColorField
                                                focusKey={focusDisabled}
                                                className={styles.guidelineColorField}
                                                value={guidelinePreviewColor}
                                                alpha={true}
                                                popupDirection={guidelinePreviewPickerDirection}
                                                hideHint={true}
                                                hexInput={true}
                                                colorWheel={false}
                                                onChange={handleGuidelinePreviewColorChange}
                                                onOpenPicker={() => {
                                                    setGuidelinePreviewPickerOpen(true);
                                                    updateGuidelinePreviewPickerDirection();
                                                }}
                                                onClosePicker={() => setGuidelinePreviewPickerOpen(false)}
                                            />
                                            <span
                                                className={styles.guidelineColorPreview}
                                                style={compactSwatchStyle(guidelinePreviewColor, guidelinePreviewHovered)}
                                                aria-hidden="true"
                                            />
                                        </div>
                                    </Tooltip>
                                </div>
                                <Tooltip tooltip={tt(text.tooltipGuidelinesOpacity)}>
                                    <div className={`${styles.sliderRow} ${styles.guidelineSliderRow}`}>
                                        <Slider
                                            focusKey={focusDisabled}
                                            className={styles.slider}
                                            value={guidelineOpacity}
                                            start={0}
                                            end={100}
                                            gamepadStep={5}
                                            onChange={handleGuidelineChange}
                                        />
                                        <div className={`${styles.valueField} ${numberFieldClass}`}>
                                            {`${guidelineOpacity}%`}
                                        </div>
                                    </div>
                                </Tooltip>
                            </div>
                        </div>
                    </div>

                    {/* Bottom action bar */}
                    <div className={styles.actions}>
                        {/* Area-tool preview toggles: each button has its own active indicator. */}
                        <div className={styles.surfaceActions}>
                            {/* LotTool toggles surface preview suppression. */}
                            <Tooltip tooltip={tt(text.tooltipSurfaceToggle)}>
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${surfaceToolAreasSuppressed ? styles.surfaceButtonActive : ""}`}
                                    onClick={handleToggleSurfaceToolAreas}
                                >
                                    <img src={lotToolIconSrc} className={`${styles.controlIcon} ${styles.idleIcon}`} alt="" />
                                </button>
                            </Tooltip>
                            {/* Specialized Industry uses AreaTypeMask.Lots, so it is separate from Surfaces. */}
                            <Tooltip tooltip={tt(text.tooltipSpecializedIndustryToggle)}>
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${styles.buttonGap} ${specializedIndustryAreasSuppressed ? styles.surfaceButtonActive : ""}`}
                                    onClick={handleToggleSpecializedIndustryAreas}
                                >
                                    <img src={specializedIndustryIconSrc} className={`${styles.controlIcon} ${styles.idleIcon}`} alt="" />
                                </button>
                            </Tooltip>
                            {/* District button opens a tiny menu; the menu swatch owns the picker. */}
                            <Tooltip tooltip={tt(text.tooltipDistrictColors)}>
                                <div
                                    ref={districtPickerRef}
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${styles.buttonGap} ${styles.districtPickerButton} ${districtMenuOpen ? styles.districtPickerButtonActive : ""}`}
                                    onMouseOver={updateDistrictPickerDirection}
                                    // Hold resets District colors; quick click opens the mini menu.
                                    onMouseDownCapture={handleDistrictMouseDownCapture}
                                    onMouseUpCapture={handleDistrictMouseUpCapture}
                                    onMouseLeave={cancelDistrictHold}
                                    onClickCapture={handleDistrictClickCapture}
                                >
                                    {districtHoldProgress > 0 && <span className={styles.holdBar} style={holdBarStyle(districtHoldProgress)} />}
                                    <img src={surfaceIconSrc} className={`${styles.controlIcon} ${styles.idleIcon} ${styles.districtPickerIcon}`} alt="" />
                                </div>
                            </Tooltip>
                        </div>

                        {districtMenuOpen && (
                            <div ref={districtMenuRef} className={styles.districtMenu}>
                                <div className={styles.districtMenuRow}>
                                    <div
                                        ref={districtColorSwatchRef}
                                        className={styles.districtMenuSwatch}
                                        style={guidelineShellStyle(districtColor, districtSwatchHovered)}
                                        onMouseOver={() => { if (!districtSwatchHovered) { setDistrictSwatchHovered(true); } updateDistrictPickerDirection(); }}
                                        onMouseMove={() => { if (!districtSwatchHovered) { setDistrictSwatchHovered(true); } }}
                                        onMouseLeave={() => setDistrictSwatchHovered(false)}
                                        onMouseDown={updateDistrictPickerDirection}
                                    >
                                        {/* Future per-district rows can reuse this swatch + reset pattern. */}
                                        <span
                                            className={styles.districtMenuSwatchPreview}
                                            style={compactSwatchStyle(districtColor, false)}
                                            aria-hidden="true"
                                        />
                                    <ColorField
                                        focusKey={focusDisabled}
                                            className={styles.districtColorField}
                                        value={districtColor}
                                        alpha={true}
                                        popupDirection={districtPickerDirection}
                                        hideHint={true}
                                        hexInput={true}
                                        colorWheel={false}
                                        onChange={handleDistrictColorChange}
                                        onOpenPicker={() => {
                                            cancelDistrictHold();
                                            setDistrictPickerOpen(true);
                                            openAreasToolPanel();
                                            updateDistrictPickerDirection();
                                        }}
                                        onClosePicker={() => setDistrictPickerOpen(false)}
                                    />
                                    </div>
                                    <span className={styles.districtMenuName}>{text.districtMenuAllDistricts}</span>
                                    <button
                                        type="button"
                                        className={styles.districtMenuReset}
                                        onClick={handleResetDistrict}
                                    >
                                        {text.districtMenuResetAll}
                                    </button>
                                </div>
                            </div>
                        )}

                        {/* Reset moved to the outline row. */}
                    </div>

                    <DragGrip
                        active={panelDragging}
                        tooltip={tt(text.tooltipDraggable)}
                        onMouseDown={handlePanelDragStart}
                    />

                </div>
            </div>
        </div>
    );
};
