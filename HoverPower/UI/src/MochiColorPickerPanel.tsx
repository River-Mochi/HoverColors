// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: Compact in-city hover-color panel anchored under the GameTopLeft icon button.
// Layout:
//   - Short draggable title bar with info tooltip, title, close button, and backup grip
//   - Outline row: icon + vanilla ColorField swatch launcher
//   - Fill / Guidelines rows: icon-led compact sliders with percent readouts
//   - Preset row: compact slot buttons + icon reset

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { Color } from "cs2/bindings";
import { bindValue, trigger, useValue } from "cs2/api";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import infoIconSrc from "../images/AdvisorInfoViewWhite.svg";
import surfaceIconSrc from "../images/Districts02.svg";
import fillIconSrc from "../images/MainElements-Fill2.svg";
import outlineIconSrc from "../images/MainElements.svg";
import guidelinesIconSrc from "../images/GuideLines.svg";
import resetIconSrc from "../images/Reset_Button.svg";
import saveIconSrc from "../images/Save.svg";
import styles from "./MochiColorPickerPanel.module.scss";

const CHANNEL = "HoverPower";
const closeIconSrc = "coui://uil/Standard/XClose.svg";

const outlineR$ = bindValue<number>(CHANNEL, "OutlineR", 0.502);
const outlineG$ = bindValue<number>(CHANNEL, "OutlineG", 0.869);
const outlineB$ = bindValue<number>(CHANNEL, "OutlineB", 1);
const outlineA$ = bindValue<number>(CHANNEL, "OutlineA", 0.855);
const fillA$ = bindValue<number>(CHANNEL, "FillA", 0);
const guidelineOpacity$ = bindValue<number>(CHANNEL, "GuidelineOpacityPercent", 40);
const surfaceToolAreasSuppressed$ = bindValue<boolean>(CHANNEL, "SurfaceToolAreasSuppressed", false);

// Gentle neutral preset for Mochi's preferred subtle highlight.
const PRESET_MOCHI_GRAY_OUTLINE: Color = { r: 140 / 255, g: 140 / 255, b: 171 / 255, a: 0.5 };
const PRESET_MOCHI_GRAY_FILL_A = 0;

// Purple-gray test preset inspired by yenyang's highlight experiments.
const PRESET_YENYANG_OUTLINE: Color = { r: 0.25, g: 0.15, b: 0.25, a: 0.5 };
const PRESET_YENYANG_FILL_A = 0;

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

    const [outline, setOutline] = React.useState<Color>(boundOutline);
    const [fillA, setFillA] = React.useState<number>(boundFillA);
    const [guidelineOpacity, setGuidelineOpacity] = React.useState<number>(boundGuideline);
    const [panelOffset, setPanelOffset] = React.useState({ x: 0, y: 0 });
    const [panelDragging, setPanelDragging] = React.useState(false);
    const [colorPickerDirection, setColorPickerDirection] = React.useState<"up" | "down">("down");
    const outlineSwatchRef = React.useRef<HTMLDivElement | null>(null);
    const panelDragRef = React.useRef<{
        pointerX: number;
        pointerY: number;
        originX: number;
        originY: number;
    } | null>(null);

    React.useEffect(() => {
        setOutline(boundOutline);
    }, [boundOutline.r, boundOutline.g, boundOutline.b, boundOutline.a]);

    React.useEffect(() => {
        setFillA(boundFillA);
    }, [boundFillA]);

    React.useEffect(() => {
        setGuidelineOpacity(boundGuideline);
    }, [boundGuideline]);

    React.useEffect(() => {
        if (!panelDragging) {
            return;
        }

        const handleMouseMove = (event: MouseEvent) => {
            const dragState = panelDragRef.current;
            if (dragState == null) {
                return;
            }

            setPanelOffset({
                x: dragState.originX + (event.clientX - dragState.pointerX),
                y: dragState.originY + (event.clientY - dragState.pointerY),
            });
        };

        const handleMouseUp = () => {
            panelDragRef.current = null;
            setPanelDragging(false);
        };

        window.addEventListener("mousemove", handleMouseMove);
        window.addEventListener("mouseup", handleMouseUp);

        return () => {
            window.removeEventListener("mousemove", handleMouseMove);
            window.removeEventListener("mouseup", handleMouseUp);
        };
    }, [panelDragging]);

    const handleOutlineChange = (value: Color) => {
        setOutline(value);
        trigger(CHANNEL, "SetOutlineColor", value.r, value.g, value.b, value.a);
    };

    const handleFillAChange = (sliderValue: number) => {
        const value = Math.max(0, Math.min(1, sliderValue));
        setFillA(value);
        trigger(CHANNEL, "SetFillAlpha", value);
    };

    const handleGuidelineChange = (percent: number) => {
        const value = Math.max(0, Math.min(100, Math.round(percent / 5) * 5));
        setGuidelineOpacity(value);
        trigger(CHANNEL, "SetGuidelineOpacity", value);
    };

    const applyPreset = (outlineColor: Color, fillAlpha: number) => {
        setOutline(outlineColor);
        setFillA(fillAlpha);
        trigger(CHANNEL, "SetOutlineColor", outlineColor.r, outlineColor.g, outlineColor.b, outlineColor.a);
        trigger(CHANNEL, "SetFillAlpha", fillAlpha);
    };

    const handleSet1 = () => applyPreset(PRESET_MOCHI_GRAY_OUTLINE, PRESET_MOCHI_GRAY_FILL_A);
    const handleSet2 = () => applyPreset(PRESET_YENYANG_OUTLINE, PRESET_YENYANG_FILL_A);
    const handleReset = () => trigger(CHANNEL, "ResetToVanilla");
    const handleClosePanel = () => trigger(CHANNEL, "SetPanelOpen", false);
    const handleResetOutline = () => trigger(CHANNEL, "ResetOutlineToVanilla");
    const handleResetFill = () => handleFillAChange(0);
    const handleResetGuidelines = () => handleGuidelineChange(40);
    const handleToggleSurfaceToolAreas = () => trigger(CHANNEL, "ToggleSurfaceToolAreas");

    const updateColorPickerDirection = React.useCallback(() => {
        const swatch = outlineSwatchRef.current;
        if (swatch == null) {
            return;
        }

        const rect = swatch.getBoundingClientRect();
        const swatchMiddleY = rect.top + rect.height / 2;
        setColorPickerDirection(swatchMiddleY < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const handlePanelDragStart = (event: React.MouseEvent<HTMLDivElement>) => {
        event.preventDefault();
        event.stopPropagation();
        panelDragRef.current = {
            pointerX: event.clientX,
            pointerY: event.clientY,
            originX: panelOffset.x,
            originY: panelOffset.y,
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

    return (
        <div
            className={styles.panelAnchor}
            style={{ transform: `translate(${panelOffset.x}px, ${panelOffset.y}px)` }}
        >
            <div className={`panel_YqS menu_O_M ${styles.panelFrame}`}>
                <div className={`content_XD5 content_AD7 child-opacity-transition_nkS content_Hzl ${styles.panelContent}`}>
                    <div className={styles.titleBar}>
                        <Tooltip tooltip="Mochi's color picker.">
                            <div className={styles.infoButton}>
                                <img src={infoIconSrc} className={styles.infoIcon} alt="" />
                            </div>
                        </Tooltip>

                        <Tooltip tooltip="Draggable">
                            <div
                                className={`${styles.titleDragHandle} ${panelDragging ? styles.titleDragHandleActive : ""}`}
                                onMouseDown={handlePanelDragStart}
                            >
                                <span className={styles.titleText}>Mochi&apos;s Blue Scrubber</span>
                            </div>
                        </Tooltip>

                        <Tooltip tooltip="Close this. You can also toggle it with hotkey H or click on Game top left icon.">
                            <Button
                                className={closeButtonClass}
                                variant="icon"
                                onClick={handleClosePanel}
                                focusKey={focusDisabled}
                                aria-label="Close panel"
                            >
                                <img src={closeIconSrc} className={styles.closeIcon} alt="" />
                            </Button>
                        </Tooltip>
                    </div>

                    <div className={styles.body}>
                        <div className={`${styles.controlRow} ${styles.outlineRow}`}>
                            <Tooltip tooltip="Reset Outline color and alpha to the game default. District and specialized-industry borders also use this path, so if those lines get faint, raise Outline alpha or use Reset.">
                                <button type="button" className={styles.controlIconButton} onClick={handleResetOutline}>
                                    <img src={outlineIconSrc} className={styles.controlIcon} alt="" />
                                </button>
                            </Tooltip>
                            <div className={`${styles.controlBody} ${styles.outlineControlBody}`}>
                                <Tooltip tooltip="Click this swatch to open the vanilla color picker. District and specialized-industry borders also follow this color and alpha path in vanilla.">
                                    <div
                                        ref={outlineSwatchRef}
                                        className={styles.outlineFieldShell}
                                        onMouseEnter={updateColorPickerDirection}
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

                        <div className={styles.controlRow}>
                            <Tooltip tooltip="Reset Fill to game defaults. Vanilla fill is 0% for normal building hover.">
                                <button type="button" className={styles.controlIconButton} onClick={handleResetFill}>
                                    <img src={fillIconSrc} className={styles.controlIcon} alt="" />
                                </button>
                            </Tooltip>
                            <Tooltip tooltip="Opacity of the fill inside the hovered outline. 0% = no inner fill, 100% = fully visible fill.">
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

                        <div className={styles.controlRow}>
                            <Tooltip tooltip="Reset Guidelines to the default HoverPower level. Both this panel and Options menu slider stay in sync.">
                                <button type="button" className={styles.controlIconButton} onClick={handleResetGuidelines}>
                                    <img src={guidelinesIconSrc} className={`${styles.controlIcon} ${styles.guidelinesIcon}`} alt="" />
                                </button>
                            </Tooltip>
                            <Tooltip tooltip="Guidelines opacity. Below 15% is not advised because the guides may become too hard to see.">
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

                    <div className={styles.actions}>
                        <div className={styles.surfaceActions}>
                            <Tooltip tooltip="Hide Surface tool boundary preview lines.\nDefault hotkey: L.">
                                <button
                                    type="button"
                                    className={`${styles.actionButton} ${styles.surfaceButton} ${surfaceToolAreasSuppressed ? styles.surfaceButtonActive : ""}`}
                                    onClick={handleToggleSurfaceToolAreas}
                                >
                                    <img src={surfaceIconSrc} className={styles.actionIcon} alt="" />
                                </button>
                            </Tooltip>
                        </div>
                        <div className={styles.presetActions}>
                            <Tooltip tooltip="Set 1: Mochi's gray-purple preset.">
                                <button type="button" className={`${styles.actionButton} ${styles.presetButton}`} onClick={handleSet1}>
                                    <span className={styles.slotBadge}>1</span>
                                    <img src={saveIconSrc} className={styles.actionIcon} alt="" />
                                </button>
                            </Tooltip>
                            <Tooltip tooltip="Set 2: yenyang purple-gray preset.">
                                <button type="button" className={`${styles.actionButton} ${styles.presetButton}`} onClick={handleSet2}>
                                    <span className={styles.slotBadge}>2</span>
                                    <img src={saveIconSrc} className={styles.actionIcon} alt="" />
                                </button>
                            </Tooltip>
                            <Tooltip tooltip="RESET OUTLINE back to game defaults.">
                                <button type="button" className={`${styles.actionButton} ${styles.resetButton}`} onClick={handleReset}>
                                    <img src={resetIconSrc} className={styles.actionIcon} alt="" />
                                </button>
                            </Tooltip>
                        </div>
                    </div>

                    <Tooltip tooltip="Draggable">
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
