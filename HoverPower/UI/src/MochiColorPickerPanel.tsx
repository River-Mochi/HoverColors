// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: Compact in-city hover-color panel anchored under the GameTopLeft icon button.
// Layout:
//   - Title bar: info toggle (tooltips on/off), draggable title, close button
//   - Outline row: icon (resets + shows vanilla-active indicator bar) + color swatch
//   - Fill / Guidelines rows: icon-led sliders
//   - Bottom: surface toggle | preset slots ① ② (tap=apply, hold 0.8s=save) + reset-presets

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { Color } from "cs2/bindings";
import { bindValue, trigger, useValue } from "cs2/api";
import { useLocalization } from "cs2/l10n";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import infoIconSrc from "../images/AdvisorInfoViewWhite.svg";
import lotToolIconSrc from "../images/LotTool.svg";
import surfaceIconSrc from "../images/Districts02.svg";
import fillIconSrc from "../images/MainElements-Fill2.svg";
import outlineIconSrc from "../images/MainElements.svg";
import guidelinesIconSrc from "../images/GuideLines4.svg";
import closeIconSrc from "../images/Close.svg";
import locale from "../../L10n/lang/en-US.json";
import styles from "./MochiColorPickerPanel.module.scss";

const CHANNEL = "HoverPower";
// How long the player must hold a preset button to save (ms). Increase if 0.8s still feels fast.
const PRESET_HOLD_MS = 800;
type LocaleKey = keyof typeof locale;

// Live color bindings
const outlineR$ = bindValue<number>(CHANNEL, "OutlineR", 0.502);
const outlineG$ = bindValue<number>(CHANNEL, "OutlineG", 0.869);
const outlineB$ = bindValue<number>(CHANNEL, "OutlineB", 1);
const outlineA$ = bindValue<number>(CHANNEL, "OutlineA", 0.855);
const fillA$ = bindValue<number>(CHANNEL, "FillA", 0);
const guidelineOpacity$ = bindValue<number>(CHANNEL, "GuidelineOpacityPercent", 40);
const surfaceToolAreasSuppressed$ = bindValue<boolean>(CHANNEL, "SurfaceToolAreasSuppressed", false);
const vanillaOutlineActive$ = bindValue<boolean>(CHANNEL, "VanillaOutlineActive", false);

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

export const MochiColorPickerPanel = () => {
    const boundOutline: Color = {
        r: useValue(outlineR$),
        g: useValue(outlineG$),
        b: useValue(outlineB$),
        a: useValue(outlineA$),
    };
    const boundFillA = useValue(fillA$);
    const boundGuideline = useValue(guidelineOpacity$);
    const surfaceToolAreasSuppressed = useValue(surfaceToolAreasSuppressed$);
    const vanillaOutlineActive = useValue(vanillaOutlineActive$);
    const preset1Active = useValue(preset1Active$);
    const preset2Active = useValue(preset2Active$);

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
            ariaClosePanel: l("HoverPower.UI.Aria.ClosePanel"),
            title: l("HoverPower.UI.Title"),
            tooltipClose: l("HoverPower.UI.Tooltip.Close"),
            tooltipDraggable: l("HoverPower.UI.Tooltip.Draggable"),
            tooltipFillOpacity: l("HoverPower.UI.Tooltip.FillOpacity"),
            tooltipGuidelinesOpacity: l("HoverPower.UI.Tooltip.GuidelinesOpacity"),
            tooltipInfo: l("HoverPower.UI.Tooltip.Info"),
            tooltipOutlineSwatch: l("HoverPower.UI.Tooltip.OutlineSwatch"),
            tooltipPreset1: l("HoverPower.UI.Tooltip.Preset1"),
            tooltipPreset2: l("HoverPower.UI.Tooltip.Preset2"),
            tooltipResetFill: l("HoverPower.UI.Tooltip.ResetFill"),
            tooltipResetGuidelines: l("HoverPower.UI.Tooltip.ResetGuidelines"),
            tooltipResetOutline: l("HoverPower.UI.Tooltip.ResetOutline"),
            tooltipResetPresets: l("HoverPower.UI.Tooltip.ResetPresets"),
            tooltipSurfaceToggle: l("HoverPower.UI.Tooltip.SurfaceToggle"),
        };
    }, [translate]);

    const [outline, setOutline] = React.useState<Color>(boundOutline);
    const [fillA, setFillA] = React.useState<number>(boundFillA);
    const [guidelineOpacity, setGuidelineOpacity] = React.useState<number>(boundGuideline);
    const [panelOffset, setPanelOffset] = React.useState({ x: 0, y: 0 });
    const [panelDragging, setPanelDragging] = React.useState(false);
    const [colorPickerDirection, setColorPickerDirection] = React.useState<"up" | "down">("down");
    // React-driven hover for the swatch shell — Cohtml doesn't reliably fire CSS :hover on a div
    // that contains an interactive button child (the ColorField button captures the pointer).
    const [swatchHovered, setSwatchHovered] = React.useState(false);

    // Hold-to-save state for preset buttons
    const [holdSlot, setHoldSlot] = React.useState<0 | 1 | 2>(0);
    const [holdProgress, setHoldProgress] = React.useState(0); // 0..1, drives holdBar scaleX
    const holdTimerRef = React.useRef<number | null>(null);
    const holdStartRef = React.useRef<number>(0);
    const holdRafRef = React.useRef<number | null>(null);

    const outlineSwatchRef = React.useRef<HTMLDivElement | null>(null);
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
    React.useEffect(() => { setGuidelineOpacity(boundGuideline); }, [boundGuideline]);

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
    const handleGuidelineChange = (v: number) => {
        const value = Math.max(0, Math.min(100, Math.round(v / 5) * 5));
        setGuidelineOpacity(value);
        trigger(CHANNEL, "SetGuidelineOpacity", value);
    };
    const handleClosePanel = () => trigger(CHANNEL, "SetPanelOpen", false);
    const handleResetOutline = () => trigger(CHANNEL, "ResetOutlineToVanilla");
    const handleResetFill = () => handleFillAChange(0);
    const handleResetGuidelines = () => handleGuidelineChange(40);
    const handleToggleSurfaceToolAreas = () => trigger(CHANNEL, "ToggleSurfaceToolAreas");
    const handleResetPresetsToDefault = () => trigger(CHANNEL, "ResetPresetsToDefault");

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
        const tick = () => {
            const p = Math.min((performance.now() - holdStartRef.current) / PRESET_HOLD_MS, 1);
            setHoldProgress(p);
            if (p < 1) holdRafRef.current = requestAnimationFrame(tick);
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
    const outlineFieldClass = styles.outlineField;
    const closeButtonClass = `${roundHighlightButtonTheme["button"] ?? ""} ${styles.closeButton}`;

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
                                <Tooltip tooltip={tt(text.tooltipOutlineSwatch)}>
                                    <div
                                        ref={outlineSwatchRef}
                                        className={`${styles.outlineFieldShell} ${swatchHovered ? styles.outlineFieldShellHovered : ""}`}
                                        onMouseEnter={() => { setSwatchHovered(true); updateColorPickerDirection(); }}
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
                                            onChange={handleOutlineChange}
                                            onOpenPicker={updateColorPickerDirection}
                                        />
                                    </div>
                                </Tooltip>
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

                        {/* Guidelines row */}
                        <div className={styles.controlRow}>
                            <Tooltip tooltip={tt(text.tooltipResetGuidelines)}>
                                <button type="button" className={styles.controlIconButton} onClick={handleResetGuidelines}>
                                    <img src={guidelinesIconSrc} className={`${styles.controlIcon} ${styles.idleIcon} ${styles.guidelinesIcon}`} alt="" />
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
                        {/* Left: surface toggle — LotTool icon primary, Districts02 secondary */}
                        <div className={styles.surfaceActions}>
                            <Tooltip tooltip={tt(text.tooltipSurfaceToggle)}>
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${surfaceToolAreasSuppressed ? styles.surfaceButtonActive : ""}`}
                                    onClick={handleToggleSurfaceToolAreas}
                                >
                                    <img src={lotToolIconSrc} className={styles.actionIcon} alt="" />
                                    {/* surfaceSecondIcon adds explicit margin — sibling combinators unreliable in Cohtml */}
                                    <img src={surfaceIconSrc} className={`${styles.actionIcon} ${styles.surfaceSecondIcon}`} alt="" />
                                </button>
                            </Tooltip>
                        </div>

                        {/* Right: preset slots + reset-to-defaults */}
                        <div className={styles.presetActions}>
                            {/* Slot 1 — tap=apply, hold 0.8s=save */}
                            <Tooltip tooltip={tt(text.tooltipPreset1)}>
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.presetButton} ${preset1Active ? styles.presetButtonActive : ""}`}
                                    onMouseDown={handlePresetMouseDown(1)}
                                    onMouseUp={handlePresetMouseUp(1)}
                                    onMouseLeave={cancelHold}
                                >
                                    {/* Top-right corner dot: shows the color stored in this slot */}
                                    <span className={styles.presetDot} style={dotStyle(p1)} />
                                    {/* Inline color forces white in Cohtml — CSS class alone is overridden by button's ButtonText default */}
                                    <span className={styles.presetGlyph} style={{ color: "rgba(255,255,255,0.82)" }}>1</span>
                                    {/* Hold progress bar sweeps left-to-right while saving; uses scaleX for reliable width */}
                                    {holdSlot === 1 && (
                                        <span className={styles.holdBar} style={holdBarStyle(holdProgress)} />
                                    )}
                                </button>
                            </Tooltip>

                            {/* Slot 2 — buttonGap adds margin-left since sibling CSS combinator loses to actionButton margin:0 */}
                            <Tooltip tooltip={tt(text.tooltipPreset2)}>
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.presetButton} ${styles.buttonGap} ${preset2Active ? styles.presetButtonActive : ""}`}
                                    onMouseDown={handlePresetMouseDown(2)}
                                    onMouseUp={handlePresetMouseUp(2)}
                                    onMouseLeave={cancelHold}
                                >
                                    <span className={styles.presetDot} style={dotStyle(p2)} />
                                    <span className={styles.presetGlyph} style={{ color: "rgba(255,255,255,0.82)" }}>2</span>
                                    {holdSlot === 2 && (
                                        <span className={styles.holdBar} style={holdBarStyle(holdProgress)} />
                                    )}
                                </button>
                            </Tooltip>

                            {/* Reset preset slots to factory defaults */}
                            <Tooltip tooltip={tt(text.tooltipResetPresets)}>
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.resetButton} ${styles.buttonGap}`}
                                    onClick={handleResetPresetsToDefault}
                                >
                                    <span className={styles.resetGlyph} style={{ color: "rgba(255,255,255,0.82)" }}>↺</span>
                                </button>
                            </Tooltip>
                        </div>
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
