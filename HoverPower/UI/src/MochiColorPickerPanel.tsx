// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: Compact in-city hover-color panel anchored under the GameTopLeft icon button.
// Layout:
//   - Title bar: info toggle (tooltips on/off), draggable title, close button
//   - Outline row: icon (resets + shows vanilla-active indicator bar) + color swatch
//   - Fill / Guidelines rows: icon-led sliders
//   - Bottom: surface toggle | preset slots ① ② (tap=apply, hold 0.8s=save) + reset-presets

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { Color, tool, toolbar } from "cs2/bindings";
import { bindValue, trigger, useValue } from "cs2/api";
import { useLocalization } from "cs2/l10n";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import infoIconSrc from "../images/AdvisorInfoViewWhite.svg";
import lotToolIconSrc from "../images/LotTool03.svg";
import surfaceIconSrc from "../images/Districts03.svg";
import fillIconSrc from "../images/MainElements-Fill2.svg";
import outlineIconSrc from "../images/MainElements.svg";
import guidelinesIconSrc from "../images/GuideLines4.svg";
import closeIconSrc from "../images/Close.svg";
import locale from "../../L10n/lang/en-US.json";
import styles from "./MochiColorPickerPanel.module.scss";

const CHANNEL = "HoverColors";
// How long to hold down a preset button to save (ms). Increase if 0.8s feels fast.
const PRESET_HOLD_MS = 700;
type LocaleKey = keyof typeof locale;

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
const surfaceToolAreasSuppressed$ = bindValue<boolean>(CHANNEL, "SurfaceToolAreasSuppressed", false);
const vanillaOutlineActive$ = bindValue<boolean>(CHANNEL, "VanillaOutlineActive", false);
const AREA_MENU_NAME_TOKENS = ["SERVICES.NAMES[AREAS]", "SERVICES.NAME[AREAS]", "AREAS"];

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

type HsvaColor = { h: number; s: number; v: number; a: number };

const rgbaToHsva = (color: Color, fallbackHue: number): HsvaColor => {
    const r = Math.max(0, Math.min(1, color.r));
    const g = Math.max(0, Math.min(1, color.g));
    const b = Math.max(0, Math.min(1, color.b));
    const max = Math.max(r, g, b);
    const min = Math.min(r, g, b);
    const delta = max - min;
    let h = fallbackHue;

    if (delta > 0) {
        if (max === r) {
            h = ((g - b) / delta + (g < b ? 6 : 0)) / 6;
        } else if (max === g) {
            h = ((b - r) / delta + 2) / 6;
        } else {
            h = ((r - g) / delta + 4) / 6;
        }
    }

    return {
        h,
        s: max === 0 ? 0 : delta / max,
        v: max,
        a: Math.max(0, Math.min(1, color.a)),
    };
};

const hsvaToRgba = (color: HsvaColor): Color => {
    const h = ((color.h % 1) + 1) % 1;
    const s = Math.max(0, Math.min(1, color.s));
    const v = Math.max(0, Math.min(1, color.v));
    const i = Math.floor(h * 6);
    const f = h * 6 - i;
    const p = v * (1 - s);
    const q = v * (1 - f * s);
    const t = v * (1 - (1 - f) * s);

    switch (i % 6) {
        case 0: return { r: v, g: t, b: p, a: color.a };
        case 1: return { r: q, g: v, b: p, a: color.a };
        case 2: return { r: p, g: v, b: t, a: color.a };
        case 3: return { r: p, g: q, b: v, a: color.a };
        case 4: return { r: t, g: p, b: v, a: color.a };
        default: return { r: v, g: p, b: q, a: color.a };
    }
};

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
    const surfaceToolAreasSuppressed = useValue(surfaceToolAreasSuppressed$);
    const vanillaOutlineActive = useValue(vanillaOutlineActive$);
    const preset1Active = useValue(preset1Active$);
    const preset2Active = useValue(preset2Active$);
    const toolbarGroups = useValue(toolbar.toolbarGroups$);

    // Stored slot colors — used for the corner dot badge on each preset button.
    // Just plain CSS inline style; not a special React feature.
    const p1: Color = { r: useValue(preset1R$), g: useValue(preset1G$), b: useValue(preset1B$), a: useValue(preset1A$) };
    const p2: Color = { r: useValue(preset2R$), g: useValue(preset2G$), b: useValue(preset2B$), a: useValue(preset2A$) };

    const { translate } = useLocalization();

    // Tooltip toggle — persists for this panel session only; resets when panel re-mounts.
    const [tooltipsEnabled, setTooltipsEnabled] = React.useState(true);
    // tt() wraps every tooltip string; returns undefined when tooltips are off (no tooltip shown).
    // The info button always gets its tooltip so players can re-enable.
    const tt = React.useCallback(
        (s: string): string | undefined => (tooltipsEnabled ? s : undefined),
        [tooltipsEnabled],
    );

    const text = React.useMemo(() => {
        const l = (key: LocaleKey) => translate(key, locale[key]) ?? locale[key];
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
    }, [translate]);

    const [outline, setOutline] = React.useState<Color>(boundOutline);
    const [fillA, setFillA] = React.useState<number>(boundFillA);
    const [districtColor, setDistrictColor] = React.useState<Color>(boundDistrict);
    const [guidelineOpacity, setGuidelineOpacity] = React.useState<number>(boundGuideline);
    const [panelOffset, setPanelOffset] = React.useState({ x: 0, y: 0 });
    const [panelDragging, setPanelDragging] = React.useState(false);
    const [colorPickerDirection, setColorPickerDirection] = React.useState<"up" | "down">("down");
    const [districtPickerDirection, setDistrictPickerDirection] = React.useState<"up" | "down">("up");
    const [districtPickerOpen, setDistrictPickerOpen] = React.useState(false);
    // React-driven hover for the swatch shell — Cohtml doesn't reliably fire CSS :hover on a div
    // that contains an interactive button child (the ColorField button captures the pointer).
    const [swatchHovered, setSwatchHovered] = React.useState(false);

    // Hover state for preset buttons — CSS :hover cannot change inline styles (inline wins cascade),
    // so React state is the only way to brighten the number glyph on hover.
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
    const districtPickerPopupRef = React.useRef<HTMLDivElement | null>(null);
    const districtPickerHueRef = React.useRef(0);
    const areaPanelOpenTimerRef = React.useRef<number | null>(null);
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
        if (!districtPickerOpen) return;
        const onMouseDown = (e: MouseEvent) => {
            const target = e.target as Node | null;
            if (target == null) return;
            if (districtPickerRef.current?.contains(target) || districtPickerPopupRef.current?.contains(target)) {
                return;
            }

            setDistrictPickerOpen(false);
        };

        document.addEventListener("mousedown", onMouseDown);
        return () => document.removeEventListener("mousedown", onMouseDown);
    }, [districtPickerOpen]);

    React.useEffect(() => () => {
        if (areaPanelOpenTimerRef.current != null) {
            clearTimeout(areaPanelOpenTimerRef.current);
            areaPanelOpenTimerRef.current = null;
        }
    }, []);

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
    const handleDistrictPickerColorChange = (value: HsvaColor) => {
        districtPickerHueRef.current = value.h;
        handleDistrictColorChange(hsvaToRgba(value));
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

    // Guideline button: tap = apply player's saved default, hold 0.8s = save current % as new default.
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
            // Save current slider value as the player's new personal default.
            trigger(CHANNEL, "SaveGuidelineDefault", guidelineOpacity);
            setGuidelineHolding(false);
            setGuidelineHoldProgress(0);
        }, HOLD_MS);
    };

    const handleGuidelineMouseUp = () => {
        if (guidelineHoldTimerRef.current != null) {
            // Released before save threshold → tap: apply saved default.
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
        const SWEEP_DELAY = 150; // ms before the fill sweep becomes visible — prevents flash on quick tap
        const tick = () => {
            const elapsed = performance.now() - holdStartRef.current;
            if (elapsed >= SWEEP_DELAY) {
                // Remap progress so sweep goes 0→1 over the remaining hold window after the delay.
                const p = Math.min((elapsed - SWEEP_DELAY) / (PRESET_HOLD_MS - SWEEP_DELAY), 1);
                setHoldProgress(p);
                if (p < 1) holdRafRef.current = requestAnimationFrame(tick);
            } else {
                holdRafRef.current = requestAnimationFrame(tick); // tick but keep progress at 0
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
            trigger(CHANNEL, "ApplyPreset", slot); // tap = apply
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
        const sameEntity = (
            a: { index: number; version: number } | null | undefined,
            b: { index: number; version: number } | null | undefined,
        ) => a != null && b != null && a.index === b.index && a.version === b.version;
        const normalize = (value: string) => value.toUpperCase();
        const areasMenu = toolbarGroups
            ?.flatMap(group => group.children ?? [])
            .find(item =>
                item.type === toolbar.ToolbarItemType.menu
                && AREA_MENU_NAME_TOKENS.some(token => normalize(item.name).includes(token)));

        if (areaPanelOpenTimerRef.current != null) {
            clearTimeout(areaPanelOpenTimerRef.current);
        }

        // Defer until after the picker click finishes. Vanilla toolbar menu actions can
        // behave like toggles, so use the live binding value at execution time to avoid
        // reopening then immediately closing the Areas panel.
        areaPanelOpenTimerRef.current = window.setTimeout(() => {
            tool.selectTool(tool.AREA_TOOL);
            if (areasMenu != null && !sameEntity(toolbar.selectedAssetMenu$.value, areasMenu.entity)) {
                toolbar.selectAssetMenu(areasMenu.entity);
            }
            areaPanelOpenTimerRef.current = null;
        }, 80);
    }, [toolbarGroups]);

    const handleDistrictPickerOpen = React.useCallback(() => {
        openAreasToolPanel();
        updateDistrictPickerDirection();
        setDistrictPickerOpen(prev => !prev);
    }, [openAreasToolPanel, updateDistrictPickerDirection]);

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
    const ColorPicker = resolver.ColorPicker;
    const Slider = resolver.Slider;
    const colorPickerSliderMode = resolver.ColorPickerSliderMode;
    const focusDisabled = resolver.FOCUS_DISABLED;
    const numberFieldClass = resolver.mouseToolOptionsTheme["number-field"];
    const roundHighlightButtonTheme = resolver.roundHighlightButtonTheme;
    const outlineFieldClass = styles.outlineField;
    const closeButtonClass = `${roundHighlightButtonTheme["button"] ?? ""} ${styles.closeButton}`;
    const districtPickerColor = rgbaToHsva(districtColor, districtPickerHueRef.current);

    // Corner dot color: inline style sets the dot background to the stored preset RGBA.
    // This is just a CSS background-color set via React's style prop — no special React feature.
    const dotStyle = (c: Color) => ({
        backgroundColor: `rgba(${Math.round(c.r * 255)},${Math.round(c.g * 255)},${Math.round(c.b * 255)},${c.a.toFixed(2)})`,
    });

    // holdBar uses transform: scaleX so percentage works regardless of button width
    const holdBarStyle = (progress: number) => ({ transform: `scaleX(${progress})` });

    return (
        <div
            className={styles.panelAnchor}
            style={{ transform: `translate(${panelOffset.x}px, ${panelOffset.y}px)` }}
        >
            <div ref={panelElementRef} className={`panel_YqS menu_O_M ${styles.panelFrame}`}>
                <div className={`content_XD5 content_AD7 child-opacity-transition_nkS content_Hzl ${styles.panelContent}`}>

                    {/* Title bar */}
                    <div className={styles.titleBar}>
                        {/* Info button toggles panel tooltips. Always shows its own tooltip so player can re-enable. */}
                        <Tooltip tooltip={text.tooltipInfo}>
                            <button
                                type="button"
                                className={`${styles.infoButton} ${!tooltipsEnabled ? styles.infoButtonActive : ""}`}
                                onClick={() => setTooltipsEnabled(prev => !prev)}
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
                                {/* Left group: swatch + preset circles — space-between pushes outlineRight to far right */}
                                <div className={styles.outlineLeft}>
                                    <Tooltip tooltip={tt(text.tooltipOutlineSwatch)}>
                                        <div
                                            ref={outlineSwatchRef}
                                            className={`${styles.outlineFieldShell} ${swatchHovered ? styles.outlineFieldShellHovered : ""}`}
                                            // onMouseMove bubbles from the ColorField button child → reliable in Cohtml.
                                            // onMouseEnter/onMouseOver are swallowed by the interactive child in Cohtml;
                                            // onMouseMove is not swallowed because it needs to bubble for drag tracking.
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
                                                // belt-and-suspenders: ColorField forwards onMouseEnter/Leave
                                                // to its root element; combined with onMouseMove on the shell.
                                                onMouseEnter={() => setSwatchHovered(true)}
                                                onMouseLeave={() => setSwatchHovered(false)}
                                                onChange={handleOutlineChange}
                                                onOpenPicker={updateColorPickerDirection}
                                            />
                                        </div>
                                    </Tooltip>

                                    {/* Preset slots: left half = stored colour, right half = number.
                                        Tap = apply. Hold 0.8s = save current live color to this slot.
                                        holdBar sweeps left-to-right (z:1) over the colour half, under the number (z:2). */}
                                    {/* onMouseEnter/Leave on the button for number brightness — buttons reliably
                                        receive hover events in Cohtml. CSS :hover cannot override inline styles. */}
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
                                            <span className={styles.presetColorHalf} style={dotStyle(p1)} />
                                            {holdSlot === 1 && holdProgress > 0 && <span className={styles.holdBar} style={holdBarStyle(holdProgress)} />}
                                            <span className={styles.presetNumberHalf} style={{ color: p1Hovered ? "rgba(255,255,255,1)" : "rgba(255,255,255,0.72)" }}>1</span>
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
                                            <span className={styles.presetColorHalf} style={dotStyle(p2)} />
                                            {holdSlot === 2 && holdProgress > 0 && <span className={styles.holdBar} style={holdBarStyle(holdProgress)} />}
                                            <span className={styles.presetNumberHalf} style={{ color: p2Hovered ? "rgba(255,255,255,1)" : "rgba(255,255,255,0.72)" }}>2</span>
                                        </button>
                                    </Tooltip>
                                </div>

                                {/* Right group: ↺ bare icon (no dark box) — same feel as the info button */}
                                <div className={styles.outlineRight}>
                                    <Tooltip tooltip={tt(text.tooltipResetPresets)}>
                                        <button
                                            type="button"
                                            className={styles.presetResetBare}
                                            onClick={handleTogglePresetDefaults}
                                        >
                                            <span className={styles.resetGlyph} style={{ color: "rgba(255,255,255,0.72)" }}>↺</span>
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

                        {/* Guidelines row — tap to apply saved default, hold 0.8s to save current % as new default */}
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
                        {/* Left: two separate surface-area buttons, each with its own indicator */}
                        <div className={styles.surfaceActions}>
                            {/* LotTool: toggles surface tool area suppression, owns the active indicator */}
                            <Tooltip tooltip={tt(text.tooltipSurfaceToggle)}>
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${surfaceToolAreasSuppressed ? styles.surfaceButtonActive : ""}`}
                                    onClick={handleToggleSurfaceToolAreas}
                                >
                                    <img src={lotToolIconSrc} className={`${styles.controlIcon} ${styles.idleIcon}`} alt="" />
                                </button>
                            </Tooltip>
                            {/* Districts: small vanilla slider/hex picker (colorWheel=false). */}
                            <Tooltip tooltip={tt(text.tooltipDistrictColors)}>
                                <div
                                    ref={districtPickerRef}
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${styles.buttonGap} ${styles.districtPickerButton} ${districtPickerOpen ? styles.districtPickerButtonActive : ""}`}
                                    onMouseOver={updateDistrictPickerDirection}
                                    onMouseDown={handleDistrictPickerOpen}
                                >
                                    <img src={surfaceIconSrc} className={`${styles.controlIcon} ${styles.idleIcon} ${styles.districtPickerIcon}`} alt="" />
                                    {districtPickerOpen && (
                                        <div
                                            ref={districtPickerPopupRef}
                                            className={`${styles.districtPickerPopup} ${districtPickerDirection === "up" ? styles.districtPickerPopupUp : styles.districtPickerPopupDown}`}
                                            onMouseDown={e => e.stopPropagation()}
                                        >
                                            <ColorPicker
                                                focusKey={focusDisabled}
                                                color={districtPickerColor}
                                                alpha={true}
                                                colorWheel={false}
                                                sliderTextInput={false}
                                                mode={colorPickerSliderMode.Hsv}
                                                hexInput={true}
                                                allowFocusExit={false}
                                                onChange={handleDistrictPickerColorChange}
                                            />
                                            <div className={styles.districtPickerLabel}>Districts</div>
                                        </div>
                                    )}
                                </div>
                            </Tooltip>
                        </div>

                        {/* ↺ is now in the outline row — nothing here */}
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
