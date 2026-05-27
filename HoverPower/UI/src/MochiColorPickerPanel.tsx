// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: In-city panel anchored under the GameTopLeft icon button.
// Layout (current):
//   - Outline section:   vanilla ColorField swatch that opens the full vanilla picker popup
//   - Fill section:      vanilla Slider for fill alpha only (Fill RGB sliders dropped — they
//                        didn't visibly do anything because we route Outline RGB to all surfaces)
//   - Guidelines section: vanilla Slider mirroring the Options-UI Guidelines opacity
//                         (both surfaces write to Settings.GuidelineOpacityPercent; either works)
//   - Presets row:       compact preset buttons + bottom-right drag handle + top-right close button
//
// State sync model:
//   useValue() on each bound channel reads live from C# (Settings.OutlineR etc.).
//   We keep a local React copy so preset clicks update the panel instantly without waiting on
//   the next binding tick.

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { Color } from "cs2/bindings";
import { bindValue, trigger, useValue } from "cs2/api";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import styles from "./MochiColorPickerPanel.module.scss";

const CHANNEL = "HoverPower";

const outlineR$ = bindValue<number>(CHANNEL, "OutlineR", 0.502);
const outlineG$ = bindValue<number>(CHANNEL, "OutlineG", 0.869);
const outlineB$ = bindValue<number>(CHANNEL, "OutlineB", 1);
const outlineA$ = bindValue<number>(CHANNEL, "OutlineA", 0.855);
const fillA$ = bindValue<number>(CHANNEL, "FillA", 0);
const guidelineOpacity$ = bindValue<number>(CHANNEL, "GuidelineOpacityPercent", 40);
const closeIconSrc = "coui://uil/Standard/XClose.svg";

// -----------------------------------------------------------------------
// Presets (keep in sync with Settings/Setting.cs SetDefaults() for the Reset case)
// -----------------------------------------------------------------------

// Vanilla cyan-blue — exact values from the OutlinesWorldUIPass material defaults.
const PRESET_VANILLA_OUTLINE: Color = { r: 0.502, g: 0.869, b: 1, a: 0.855 };
const PRESET_VANILLA_FILL_A = 0;

// Set1: light gray with 10% halo alpha — a subtle ambient highlight.
const PRESET_LIGHT_GRAY_OUTLINE: Color = { r: 0.85, g: 0.85, b: 0.88, a: 0.10 };
const PRESET_LIGHT_GRAY_FILL_A = 0;

// Set2: purple-gray from yenyang's HighlightsAndGuidelinesTweaks POC.
// Yenyang's original m_HoveredColor was (0.25, 0.15, 0.25, 0.01). We keep the RGB and bump alpha
// to 0.50 so the halo is actually visible as a usable preset rather than near-invisible.
const PRESET_YENYANG_OUTLINE: Color = { r: 0.25, g: 0.15, b: 0.25, a: 0.5 };
const PRESET_YENYANG_FILL_A = 0;

export const MochiColorPickerPanel = () => {
    // Live values from C# settings via bindings.
    const boundOutline: Color = {
        r: useValue(outlineR$),
        g: useValue(outlineG$),
        b: useValue(outlineB$),
        a: useValue(outlineA$),
    };
    const boundFillA = useValue(fillA$);
    const boundGuideline = useValue(guidelineOpacity$);

    // Local mirrors so preset clicks snap state without waiting on the binding round-trip.
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

    const handleOutlineChange = (c: Color) => {
        setOutline(c);
        trigger(CHANNEL, "SetOutlineColor", c.r, c.g, c.b, c.a);
    };

    const handleFillAChange = (sliderValue: number) => {
        const a = Math.max(0, Math.min(1, sliderValue));
        setFillA(a);
        trigger(CHANNEL, "SetFillAlpha", a);
    };

    const handleGuidelineChange = (percent: number) => {
        const clamped = Math.max(0, Math.min(100, Math.round(percent / 5) * 5));
        setGuidelineOpacity(clamped);
        trigger(CHANNEL, "SetGuidelineOpacity", clamped);
    };

    const applyPreset = (outlineColor: Color, fillAlpha: number) => {
        setOutline(outlineColor);
        setFillA(fillAlpha);
        trigger(CHANNEL, "SetOutlineColor", outlineColor.r, outlineColor.g, outlineColor.b, outlineColor.a);
        trigger(CHANNEL, "SetFillAlpha", fillAlpha);
    };

    const handleSet1 = () => applyPreset(PRESET_LIGHT_GRAY_OUTLINE, PRESET_LIGHT_GRAY_FILL_A);
    const handleSet2 = () => applyPreset(PRESET_YENYANG_OUTLINE, PRESET_YENYANG_FILL_A);
    const handleReset = () => applyPreset(PRESET_VANILLA_OUTLINE, PRESET_VANILLA_FILL_A);
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
    const Section = VanillaComponentResolver.instance.Section;
    const focusDisabled = VanillaComponentResolver.instance.FOCUS_DISABLED;
    const numberFieldClass = VanillaComponentResolver.instance.mouseToolOptionsTheme["number-field"];
    const colorFieldTheme = VanillaComponentResolver.instance.colorFieldTheme;
    const roundHighlightButtonTheme = VanillaComponentResolver.instance.roundHighlightButtonTheme;
    const outlineFieldClass = `${colorFieldTheme["colorField"] ?? ""} ${styles.outlineField}`;
    const closeButtonClass = `${roundHighlightButtonTheme["button"] ?? ""} ${styles.closeButton}`;

    return (
        <div
            className={styles.panelAnchor}
            style={{ transform: `translate(${panelOffset.x}px, ${panelOffset.y}px)` }}
        >
            <div className={`panel_YqS menu_O_M ${styles.panelFrame}`}>
                <div className={` content_XD5 content_AD7 child-opacity-transition_nkS content_Hzl ${styles.panelContent}`}>
                    <div className={styles.panelUtilityRow}>
                        <Tooltip tooltip="Close this panel. You can also toggle it with the GTL icon or the H hotkey.">
                            <Button
                                className={closeButtonClass}
                                variant="icon"
                                focusKey={focusDisabled}
                                onSelect={handleClosePanel}
                            >
                                <img src={closeIconSrc} className={styles.closeIcon} alt="" />
                            </Button>
                        </Tooltip>
                    </div>

                    <div className={styles.body}>
                        <Section title="Outline">
                            <div className={styles.outlineControl}>
                                <Tooltip tooltip="Click this color box to open the full vanilla picker with wheel, RGB sliders, alpha, and hex input. This changes only the active hover/selection highlight, not the building permanently.">
                                    <div className={styles.outlineFieldAnchor}>
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
                                <div className={styles.helperText}>
                                    Click the box for the full vanilla color picker.
                                </div>
                            </div>
                        </Section>

                        <Section title="Fill">
                            <Tooltip tooltip="Opacity of the fill inside the hovered outline. 0% = no inner fill, 100% = fully visible fill.">
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
                            </Tooltip>
                        </Section>

                        <Section title="Guidelines">
                            <Tooltip tooltip="Guidelines opacity. This is the same setting used in the Options UI menu, so both places stay in sync.">
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
                            </Tooltip>
                        </Section>
                    </div>

                    <div className={styles.actions}>
                        <Tooltip tooltip="Preset 1: light gray with a subtle 10% outline halo.">
                            <Button className={styles.presetButton} variant="menu" focusKey={focusDisabled} onSelect={handleSet1}>
                                Set1
                            </Button>
                        </Tooltip>
                        <Tooltip tooltip="Preset 2: purple-gray test colors inspired by yenyang's highlight experiments.">
                            <Button className={styles.presetButton} variant="menu" focusKey={focusDisabled} onSelect={handleSet2}>
                                Set2
                            </Button>
                        </Tooltip>
                        <Tooltip tooltip="Reset to the Hover Power default blue outline and no fill.">
                            <Button className={styles.presetButton} variant="menu" focusKey={focusDisabled} onSelect={handleReset}>
                                Reset
                            </Button>
                        </Tooltip>
                    </div>

                    <Tooltip tooltip="Drag this little tab to move the panel.">
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
