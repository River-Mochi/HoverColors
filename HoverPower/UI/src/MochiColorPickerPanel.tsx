// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: Compact in-city hover-color panel anchored under the GameTopLeft icon button.
// Layout:
//   - Short draggable title bar with info tooltip, title, close button, and backup grip
//   - Outline row: icon + vanilla ColorField swatch launcher
//   - Fill / Guidelines rows: icon-led compact sliders with percent readouts
//   - Preset row: compact slot buttons + icon reset

import React from "react";
import { Tooltip } from "cs2/ui";
import { Color } from "cs2/bindings";
import { bindValue, trigger, useValue } from "cs2/api";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import infoIconSrc from "../images/AdvisorInfoViewWhite.svg";
import fillIconSrc from "../images/Building_PrimaryColor.svg";
import outlineIconSrc from "../images/Building_TertiaryColor.svg";
import guidelinesIconSrc from "../images/GuideLines.svg";
import resetIconSrc from "../images/Reset_Button.svg";
import saveIconSrc from "../images/Save.svg";
import styles from "./MochiColorPickerPanel.module.scss";

const CHANNEL = "HoverPower";

const outlineR$ = bindValue<number>(CHANNEL, "OutlineR", 0.502);
const outlineG$ = bindValue<number>(CHANNEL, "OutlineG", 0.869);
const outlineB$ = bindValue<number>(CHANNEL, "OutlineB", 1);
const outlineA$ = bindValue<number>(CHANNEL, "OutlineA", 0.855);
const fillA$ = bindValue<number>(CHANNEL, "FillA", 0);
const guidelineOpacity$ = bindValue<number>(CHANNEL, "GuidelineOpacityPercent", 40);

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

    const [outline, setOutline] = React.useState<Color>(boundOutline);
    const [fillA, setFillA] = React.useState<number>(boundFillA);
    const [guidelineOpacity, setGuidelineOpacity] = React.useState<number>(boundGuideline);
    const [panelOffset, setPanelOffset] = React.useState({ x: 0, y: 0 });
    const [panelDragging, setPanelDragging] = React.useState(false);
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

    const ColorField = VanillaComponentResolver.instance.ColorField;
    const Slider = VanillaComponentResolver.instance.Slider;
    const focusDisabled = VanillaComponentResolver.instance.FOCUS_DISABLED;
    const numberFieldClass = VanillaComponentResolver.instance.mouseToolOptionsTheme["number-field"];
    const colorFieldTheme = VanillaComponentResolver.instance.colorFieldTheme;
    const outlineFieldClass = `${colorFieldTheme["colorField"] ?? ""} ${styles.outlineField}`;

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
                                <span className={styles.titleText}>Mochi&apos;s Blue Buster</span>
                            </div>
                        </Tooltip>

                        <Tooltip tooltip="Close this panel. You can also toggle it with the GTL icon or the H hotkey.">
                            <button
                                type="button"
                                className={styles.closeButton}
                                onClick={handleClosePanel}
                                aria-label="Close panel"
                            >
                                <span className={styles.closeGlyph}>X</span>
                            </button>
                        </Tooltip>
                    </div>

                    <div className={styles.body}>
                        <div className={styles.controlRow}>
                            <Tooltip tooltip="Outline color for the active hover and selection highlight. Click the swatch to open the vanilla color picker.">
                                <div className={styles.controlIconButton}>
                                    <img src={outlineIconSrc} className={styles.controlIcon} alt="" />
                                </div>
                            </Tooltip>
                            <div className={styles.controlBody}>
                                <Tooltip tooltip="Click this color box to open the full vanilla picker with wheel, RGB sliders, alpha, and hex input. This changes only the active hover/selection highlight, not the building permanently.">
                                    <div className={styles.outlineFieldShell}>
                                        <ColorField
                                            focusKey={focusDisabled}
                                            className={outlineFieldClass}
                                            value={outline}
                                            alpha={true}
                                            popupDirection="right"
                                            hideHint={true}
                                            hexInput={true}
                                            colorWheel={true}
                                            onChange={handleOutlineChange}
                                        />
                                    </div>
                                </Tooltip>
                            </div>
                        </div>

                        <div className={styles.controlRow}>
                            <Tooltip tooltip="Opacity of the fill inside the hovered outline. 0% = no inner fill, 100% = fully visible fill.">
                                <div className={styles.controlIconButton}>
                                    <img src={fillIconSrc} className={styles.controlIcon} alt="" />
                                </div>
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
                            <Tooltip tooltip="Guidelines opacity. This is the same setting used in the Options menu, so both places stay in sync.">
                                <div className={styles.controlIconButton}>
                                    <img src={guidelinesIconSrc} className={styles.controlIcon} alt="" />
                                </div>
                            </Tooltip>
                            <Tooltip tooltip="Guidelines opacity. This is the same setting used in the Options menu, so both places stay in sync.">
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
                        <Tooltip tooltip="Preset 1: Mochi-gray.">
                            <button type="button" className={`${styles.actionButton} ${styles.presetButton}`} onClick={handleSet1}>
                                <span className={styles.slotBadge}>1</span>
                                <img src={saveIconSrc} className={styles.actionIcon} alt="" />
                            </button>
                        </Tooltip>
                        <Tooltip tooltip="Preset 2: yenyang purple-gray.">
                            <button type="button" className={`${styles.actionButton} ${styles.presetButton}`} onClick={handleSet2}>
                                <span className={styles.slotBadge}>2</span>
                                <img src={saveIconSrc} className={styles.actionIcon} alt="" />
                            </button>
                        </Tooltip>
                        <Tooltip tooltip="Reset to vanilla hover colors and no fill.">
                            <button type="button" className={`${styles.actionButton} ${styles.resetButton}`} onClick={handleReset}>
                                <img src={resetIconSrc} className={styles.actionIcon} alt="" />
                            </button>
                        </Tooltip>
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
