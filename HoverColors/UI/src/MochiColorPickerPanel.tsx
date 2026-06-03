// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: Compact in-city hover-color panel anchored under the GameTopLeft icon button.
// Layout:
//   - Title bar: info toggle (tooltips on/off), draggable title, close button
//   - Outline row: icon (resets + shows vanilla-active indicator bar) + color swatch
//     + preset slots 1/2 (tap=apply, hold 0.7s=save) + preset reset icon
//   - Fill / Guidelines rows: icon-led sliders
//   - Bottom: surface overlay toggle + District color picker

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { Color, toolbar } from "cs2/bindings";
import { bindValue, trigger, useMapValue, useValue } from "cs2/api";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import { LocaleKey, usePanelLocalization } from "./localization";
import infoIconSrc from "../images/AdvisorInfoViewWhite.svg";
import lotToolIconSrc from "../images/LotTool03.svg";
import surfaceIconSrc from "../images/Districts03.svg";
import fillIconSrc from "../images/MainElements-Fill2.svg";
import outlineIconSrc from "../images/MainElements2.svg";
import guidelinesIconSrc from "../images/GuideLines4.svg";
import closeIconSrc from "../images/Close.svg";
import resetIconSrc from "../images/Reset_Button2.svg";
import styles from "./MochiColorPickerPanel.module.scss";

const CHANNEL = "HoverColors";
// Hold time for saving preset slots. Increase if quick taps save too often.
const PRESET_HOLD_MS = 700;

// Live color bindings
const outlineR$ = bindValue<number>(CHANNEL, "OutlineR", 0.502);
const outlineG$ = bindValue<number>(CHANNEL, "OutlineG", 0.869);
const outlineB$ = bindValue<number>(CHANNEL, "OutlineB", 1);
const outlineA$ = bindValue<number>(CHANNEL, "OutlineA", 0.855);
const fillA$ = bindValue<number>(CHANNEL, "FillA", 0);
const districtR$ = bindValue<number>(CHANNEL, "DistrictR", 128 / 255);
const districtG$ = bindValue<number>(CHANNEL, "DistrictG", 128 / 255);
const districtB$ = bindValue<number>(CHANNEL, "DistrictB", 128 / 255);
const districtA$ = bindValue<number>(CHANNEL, "DistrictA", 64 / 255);
const guidelineOpacity$ = bindValue<number>(CHANNEL, "GuidelineOpacityPercent", 30);
const panelTooltipsEnabled$ = bindValue<boolean>(CHANNEL, "PanelTooltipsEnabled", true);
const useDarkerPanel$ = bindValue<boolean>(CHANNEL, "UseDarkerPanel", false);
const surfaceToolAreasSuppressed$ = bindValue<boolean>(CHANNEL, "SurfaceToolAreasSuppressed", true);
const vanillaOutlineActive$ = bindValue<boolean>(CHANNEL, "VanillaOutlineActive", false);
const AREA_MENU_NAME_TOKENS = ["SERVICES.NAMES[AREAS]", "SERVICES.NAME[AREAS]", "AREAS"];
const DISTRICT_AREA_NAME_TOKENS = [
    "ASSETS.NAME[DISTRICT AREA]",
    "ASSETS.DESCRIPTION[DISTRICT AREA]",
    "DISTRICT AREA",
    "DISTRICT",
];
type ToolbarEntity = { index: number; version: number };

const sameEntity = (
    a: ToolbarEntity | null | undefined,
    b: ToolbarEntity | null | undefined,
) => a != null && b != null && a.index === b.index && a.version === b.version;

// Preset stored-color bindings
const preset1R$ = bindValue<number>(CHANNEL, "Preset1R", 140 / 255);
const preset1G$ = bindValue<number>(CHANNEL, "Preset1G", 140 / 255);
const preset1B$ = bindValue<number>(CHANNEL, "Preset1B", 171 / 255);
const preset1A$ = bindValue<number>(CHANNEL, "Preset1A", 0.5);
const preset2R$ = bindValue<number>(CHANNEL, "Preset2R", 0.25);
const preset2G$ = bindValue<number>(CHANNEL, "Preset2G", 0.15);
const preset2B$ = bindValue<number>(CHANNEL, "Preset2B", 0.25);
const preset2A$ = bindValue<number>(CHANNEL, "Preset2A", 0.5);
const preset1Active$ = bindValue<boolean>(CHANNEL, "Preset1Active", false);
const preset2Active$ = bindValue<boolean>(CHANNEL, "Preset2Active", false);
const DISTRICT_PICKER_BODY_CLASS = "mochiDistrictPickerOpen";

export const MochiColorPickerPanel = () => {
    const boundOutline: Color = {
        r: useValue(outlineR$),
        g: useValue(outlineG$),
        b: useValue(outlineB$),
        a: useValue(outlineA$),
    };
    const boundFillA = useValue(fillA$);
    const boundDistrict: Color = {
        r: useValue(districtR$),
        g: useValue(districtG$),
        b: useValue(districtB$),
        a: useValue(districtA$),
    };
    const boundGuideline = useValue(guidelineOpacity$);
    const useDarkerPanel = useValue(useDarkerPanel$);
    const surfaceToolAreasSuppressed = useValue(surfaceToolAreasSuppressed$);
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
            tooltipGuidelinesOpacity: l("HoverColors.UI.Tooltip.GuidelinesOpacity"),
            tooltipInfo: l("HoverColors.UI.Tooltip.Info"),
            tooltipOutlineSwatch: l("HoverColors.UI.Tooltip.OutlineSwatch"),
            tooltipPreset1: l("HoverColors.UI.Tooltip.Preset1"),
            tooltipPreset2: l("HoverColors.UI.Tooltip.Preset2"),
            tooltipResetFill: l("HoverColors.UI.Tooltip.ResetFill"),
            tooltipResetGuidelines: l("HoverColors.UI.Tooltip.ResetGuidelines"),
            tooltipResetOutline: l("HoverColors.UI.Tooltip.ResetOutline"),
            tooltipResetPresets: l("HoverColors.UI.Tooltip.ResetPresets"),
            tooltipSurfaceToggle: l("HoverColors.UI.Tooltip.SurfaceToggle"),
            tooltipDistrictColors: l("HoverColors.UI.Tooltip.DistrictColors"),
        };
    }, [translatePanel]);

    const [outline, setOutline] = React.useState<Color>(boundOutline);
    const [fillA, setFillA] = React.useState<number>(boundFillA);
    const [districtColor, setDistrictColor] = React.useState<Color>(boundDistrict);
    const [guidelineOpacity, setGuidelineOpacity] = React.useState<number>(boundGuideline);
    const [panelOffset, setPanelOffset] = React.useState({ x: 0, y: 0 });
    const [panelDragging, setPanelDragging] = React.useState(false);
    const [colorPickerDirection, setColorPickerDirection] = React.useState<"up" | "down">("down");
    const [districtPickerDirection, setDistrictPickerDirection] = React.useState<"up" | "down">("up");
    const [districtPickerOpen, setDistrictPickerOpen] = React.useState(false);
    const [pendingDistrictToolOpen, setPendingDistrictToolOpen] = React.useState(false);
    // ColorField can swallow hover events; React state keeps the swatch ring reliable.
    const [swatchHovered, setSwatchHovered] = React.useState(false);

    // Preset numbers use inline color, so hover color also needs React state.
    const [p1Hovered, setP1Hovered] = React.useState(false);
    const [p2Hovered, setP2Hovered] = React.useState(false);

    // Hold-to-save state for preset buttons
    const [holdSlot, setHoldSlot] = React.useState<0 | 1 | 2>(0);
    const [holdProgress, setHoldProgress] = React.useState(0); // 0..1, drives holdBar scaleX
    const holdTimerRef = React.useRef<number | null>(null);
    const holdStartRef = React.useRef<number>(0);
    const holdRafRef = React.useRef<number | null>(null);

    const outlineSwatchRef = React.useRef<HTMLDivElement | null>(null);
    const districtPickerRef = React.useRef<HTMLDivElement | null>(null);
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
    React.useEffect(() => { setFillA(boundFillA); }, [boundFillA]);
    React.useEffect(() => { setDistrictColor(boundDistrict); },
        [boundDistrict.r, boundDistrict.g, boundDistrict.b, boundDistrict.a]);
    React.useEffect(() => { setGuidelineOpacity(boundGuideline); }, [boundGuideline]);

    React.useEffect(() => {
        if (typeof document === "undefined") {
            return;
        }

        document.body.classList.toggle(DISTRICT_PICKER_BODY_CLASS, districtPickerOpen);

        if (!districtPickerOpen) {
            return () => document.body.classList.remove(DISTRICT_PICKER_BODY_CLASS);
        }

        const onMouseDown = (event: MouseEvent) => {
            const target = event.target as Element | null;
            if (target == null) {
                return;
            }

            // Vanilla ColorField closes on outside clicks but does not call onClosePicker there,
            // Keep scoped CSS mode in sync without touching picker internals.
            if (districtPickerRef.current?.contains(target) || target.closest(".color-picker-container_Sj5")) {
                return;
            }

            setDistrictPickerOpen(false);
        };

        document.addEventListener("mousedown", onMouseDown);
        return () => {
            document.removeEventListener("mousedown", onMouseDown);
            document.body.classList.remove(DISTRICT_PICKER_BODY_CLASS);
        };
    }, [districtPickerOpen]);

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

    // Live color handlers
    const handleOutlineChange = (value: Color) => {
        setOutline(value);
        trigger(CHANNEL, "SetOutlineColor", value.r, value.g, value.b, value.a);
    };
    const handleFillAChange = (v: number) => {
        const value = Math.max(0, Math.min(1, v));
        setFillA(value);
        trigger(CHANNEL, "SetFillAlpha", value);
    };
    const handleDistrictColorChange = (value: Color) => {
        setDistrictColor(value);
        trigger(CHANNEL, "SetDistrictColor", value.r, value.g, value.b, value.a);
    };
    const handleGuidelineChange = (v: number) => {
        const value = Math.max(0, Math.min(100, Math.round(v / 5) * 5));
        setGuidelineOpacity(value);
        trigger(CHANNEL, "SetGuidelineOpacity", value);
    };
    const handleClosePanel = () => trigger(CHANNEL, "SetPanelOpen", false);
    const handleResetOutline = () => trigger(CHANNEL, "ResetOutlineToVanilla");
    const handleResetFill = () => handleFillAChange(0);
    const handleToggleSurfaceToolAreas = () => trigger(CHANNEL, "ToggleSurfaceToolAreas");
    const handleTogglePresetDefaults = () => trigger(CHANNEL, "TogglePresetDefaults");

    // Guidelines: tap applies saved default; hold 0.7s saves current % as default.
    const [guidelineHoldProgress, setGuidelineHoldProgress] = React.useState(0);
    const guidelineHoldTimerRef = React.useRef<number | null>(null);
    const guidelineHoldStartRef = React.useRef<number>(0);
    const guidelineHoldRafRef = React.useRef<number | null>(null);
    const [guidelineHolding, setGuidelineHolding] = React.useState(false);

    const cancelGuidelineHold = React.useCallback(() => {
        if (guidelineHoldTimerRef.current != null) { clearTimeout(guidelineHoldTimerRef.current); guidelineHoldTimerRef.current = null; }
        if (guidelineHoldRafRef.current != null) { cancelAnimationFrame(guidelineHoldRafRef.current); guidelineHoldRafRef.current = null; }
        setGuidelineHolding(false);
        setGuidelineHoldProgress(0);
    }, []);

    const handleGuidelineMouseDown = (e: React.MouseEvent) => {
        e.preventDefault();
        cancelGuidelineHold();
        guidelineHoldStartRef.current = performance.now();
        setGuidelineHolding(true);
        setGuidelineHoldProgress(0);
        const SWEEP_DELAY = 150;
        const HOLD_MS = 700;
        const tick = () => {
            const elapsed = performance.now() - guidelineHoldStartRef.current;
            if (elapsed >= SWEEP_DELAY) {
                const p = Math.min((elapsed - SWEEP_DELAY) / (HOLD_MS - SWEEP_DELAY), 1);
                setGuidelineHoldProgress(p);
                if (p < 1) guidelineHoldRafRef.current = requestAnimationFrame(tick);
            } else {
                guidelineHoldRafRef.current = requestAnimationFrame(tick);
            }
        };
        guidelineHoldRafRef.current = requestAnimationFrame(tick);
        guidelineHoldTimerRef.current = window.setTimeout(() => {
            guidelineHoldTimerRef.current = null;
            if (guidelineHoldRafRef.current != null) { cancelAnimationFrame(guidelineHoldRafRef.current); guidelineHoldRafRef.current = null; }
            // Long press saves the current value as the personal default.
            trigger(CHANNEL, "SaveGuidelineDefault", guidelineOpacity);
            setGuidelineHolding(false);
            setGuidelineHoldProgress(0);
        }, HOLD_MS);
    };

    const handleGuidelineMouseUp = () => {
        if (guidelineHoldTimerRef.current != null) {
            // Short press applies the saved default.
            clearTimeout(guidelineHoldTimerRef.current);
            guidelineHoldTimerRef.current = null;
            trigger(CHANNEL, "ApplyGuidelineDefault");
        }
        cancelGuidelineHold();
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

    const updateColorPickerDirection = React.useCallback(() => {
        const swatch = outlineSwatchRef.current;
        if (swatch == null) return;
        const rect = swatch.getBoundingClientRect();
        setColorPickerDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const updateDistrictPickerDirection = React.useCallback(() => {
        const swatch = districtPickerRef.current;
        if (swatch == null) return;
        const rect = swatch.getBoundingClientRect();
        setDistrictPickerDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
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
    const closeButtonClass = `${roundHighlightButtonTheme["button"] ?? ""} ${styles.closeButton}`;
    const panelFrameClass = `${panelBaseTheme.panel ?? "panel_YqS"} ${infoviewMenuTheme.menu ?? "menu_O_M"} ${styles.panelFrame}`;
    const panelSurfaceClass = useDarkerPanel ? styles.panelDarker : styles.panelStandard;
    const panelContentClass = `${panelTheme.content ?? "content_XD5 content_AD7 child-opacity-transition_nkS"} ${infoviewMenuTheme.content ?? "content_Hzl"} ${styles.panelContent} ${panelSurfaceClass}`;

    // Preview ignores alpha for legibility; saved preset still applies alpha in-game.
    const presetPreviewStyle = (c: Color) => ({
        backgroundColor: `rgb(${Math.round(c.r * 255)},${Math.round(c.g * 255)},${Math.round(c.b * 255)})`,
    });

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

                                    {/* Presets: tap applies; hold saves current color/alpha. */}
                                    <Tooltip tooltip={tt(text.tooltipPreset1)}>
                                        <button
                                            type="button"
                                            className={`${styles.presetSlot} ${preset1Active ? styles.presetSlotActive : ""}`}
                                            style={{ marginLeft: "10rem" }}
                                            onMouseEnter={() => setP1Hovered(true)}
                                            onMouseDown={handlePresetMouseDown(1)}
                                            onMouseUp={handlePresetMouseUp(1)}
                                            onMouseLeave={() => { setP1Hovered(false); cancelHold(); }}
                                        >
                                            {holdSlot === 1 && holdProgress > 0 && <span className={styles.holdBar} style={holdBarStyle(holdProgress)} />}
                                            <span className={styles.presetNumber} style={{ color: presetNumberColor(preset1Active, p1Hovered) }}>1</span>
                                            <span className={styles.presetSwatch} style={presetPreviewStyle(p1)} />
                                        </button>
                                    </Tooltip>
                                    <Tooltip tooltip={tt(text.tooltipPreset2)}>
                                        <button
                                            type="button"
                                            className={`${styles.presetSlot} ${preset2Active ? styles.presetSlotActive : ""}`}
                                            style={{ marginLeft: "5rem" }}
                                            onMouseEnter={() => setP2Hovered(true)}
                                            onMouseDown={handlePresetMouseDown(2)}
                                            onMouseUp={handlePresetMouseUp(2)}
                                            onMouseLeave={() => { setP2Hovered(false); cancelHold(); }}
                                        >
                                            {holdSlot === 2 && holdProgress > 0 && <span className={styles.holdBar} style={holdBarStyle(holdProgress)} />}
                                            <span className={styles.presetNumber} style={{ color: presetNumberColor(preset2Active, p2Hovered) }}>2</span>
                                            <span className={styles.presetSwatch} style={presetPreviewStyle(p2)} />
                                        </button>
                                    </Tooltip>
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

                        {/* Guidelines: tap applies saved default; hold saves current %. */}
                        <div className={styles.controlRow}>
                            <Tooltip tooltip={tt(text.tooltipResetGuidelines)}>
                                <button
                                    type="button"
                                    className={styles.controlIconButton}
                                    onMouseDown={handleGuidelineMouseDown}
                                    onMouseUp={handleGuidelineMouseUp}
                                    onMouseLeave={cancelGuidelineHold}
                                >
                                    <img src={guidelinesIconSrc} className={`${styles.controlIcon} ${styles.idleIcon} ${styles.guidelinesIcon}`} alt="" />
                                    {guidelineHolding && guidelineHoldProgress > 0 && (
                                        <span className={styles.guidelineHoldBar} style={holdBarStyle(guidelineHoldProgress)} />
                                    )}
                                </button>
                            </Tooltip>
                            <Tooltip tooltip={tt(text.tooltipGuidelinesOpacity)}>
                                <div className={styles.controlBody}>
                                    <div className={styles.sliderRow}>
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
                                </div>
                            </Tooltip>
                        </div>
                    </div>

                    {/* Bottom action bar */}
                    <div className={styles.actions}>
                        {/* Surface tools: each button has its own active indicator. */}
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
                            {/* Districts uses hidden ColorField so vanilla owns the popup. */}
                            <Tooltip tooltip={tt(text.tooltipDistrictColors)}>
                                <div
                                    ref={districtPickerRef}
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${styles.buttonGap} ${styles.districtPickerButton} ${districtPickerOpen ? styles.districtPickerButtonActive : ""}`}
                                    onMouseOver={updateDistrictPickerDirection}
                                    onMouseDown={() => {
                                        openAreasToolPanel();
                                        updateDistrictPickerDirection();
                                    }}
                                >
                                    <img src={surfaceIconSrc} className={`${styles.controlIcon} ${styles.idleIcon} ${styles.districtPickerIcon}`} alt="" />
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
                                            setDistrictPickerOpen(true);
                                            openAreasToolPanel();
                                            updateDistrictPickerDirection();
                                        }}
                                        onClosePicker={() => setDistrictPickerOpen(false)}
                                    />
                                </div>
                            </Tooltip>
                        </div>

                        {/* Reset moved to the outline row. */}
                    </div>

                    <Tooltip tooltip={tt(text.tooltipDraggable)}>
                        <div
                            className={`${styles.dragGrip} ${panelDragging ? styles.dragGripActive : ""}`}
                            onMouseDown={handlePanelDragStart}
                        >
                            <span className={styles.dragGripDot}></span>
                            <span className={styles.dragGripDot}></span>
                            <span className={styles.dragGripDot}></span>
                            <span className={styles.dragGripDot}></span>
                        </div>
                    </Tooltip>
                </div>
            </div>
        </div>
    );
};
