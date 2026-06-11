// File: UI/src/MochiColorPickerPanel.tsx
// Purpose: Compact in-city hover-color panel anchored under the GameTopLeft icon button.
// Layout: title bar + color control rows + bottom action bar.

import React from "react";
import { Button, FormattedParagraphs, Tooltip } from "cs2/ui";
import { Color } from "cs2/bindings";
import { trigger, useValue } from "cs2/api";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import {
    CHANNEL,
    COMPACT_PICKER_BODY_CLASS,
    districtA$,
    districtB$,
    districtG$,
    districtR$,
    fillA$,
    guidelineLinesColorA$,
    guidelineLinesColorB$,
    guidelineLinesColorG$,
    guidelineLinesColorR$,
    guidelineDashedColorB$,
    guidelineDashedColorG$,
    guidelineDashedColorR$,
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
    specializedIndustryAreasSuppressed$,
    surfaceToolAreasSuppressed$,
    useDarkerPanel$,
    vanillaOutlineActive$,
} from "./panel/MochiPanelBindings";
import { DragGrip } from "./panel/MochiPanelPieces";
import { normalizeColorFieldValue } from "./panel/MochiPanelColorUtils";
import { MochiPanelActionBar } from "./panel/MochiPanelActionBar";
import { MochiPanelControlRows } from "./panel/MochiPanelControlRows";
import { useDistrictHold } from "./panel/useDistrictHold";
import { useDistrictToolPanel } from "./panel/useDistrictToolPanel";
import { useMochiPanelText } from "./panel/useMochiPanelText";
import { usePanelDrag } from "./panel/usePanelDrag";
import { usePresetHold } from "./panel/usePresetHold";
import infoIconSrc from "../images/AdvisorInfoViewWhite.svg";
import closeIconSrc from "../images/Close.svg";
import styles from "./MochiColorPickerPanel.module.scss";

export const MochiColorPickerPanel = () => {
    const boundOutline: Color = {
        r: useValue(outlineR$),
        g: useValue(outlineG$),
        b: useValue(outlineB$),
        a: useValue(outlineA$),
    };
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
    const boundGuidelineDashedColor: Color = {
        r: useValue(guidelineDashedColorR$),
        g: useValue(guidelineDashedColorG$),
        b: useValue(guidelineDashedColorB$),
        a: 1,
    };

    const boundFillA = useValue(fillA$);
    const boundGuideline = useValue(guidelineOpacity$);
    const useDarkerPanel = useValue(useDarkerPanel$);
    const surfaceToolAreasSuppressed = useValue(surfaceToolAreasSuppressed$);
    const specializedIndustryAreasSuppressed = useValue(specializedIndustryAreasSuppressed$);
    const vanillaOutlineActive = useValue(vanillaOutlineActive$);
    const preset1Active = useValue(preset1Active$);
    const preset2Active = useValue(preset2Active$);

    // Stored preset slots. Preview uses RGB for readability; alpha is still saved/applied by C#.
    const preset1Color: Color = { r: useValue(preset1R$), g: useValue(preset1G$), b: useValue(preset1B$), a: useValue(preset1A$) };
    const preset2Color: Color = { r: useValue(preset2R$), g: useValue(preset2G$), b: useValue(preset2B$), a: useValue(preset2A$) };

    const text = useMochiPanelText();
    const tooltipsEnabled = useValue(panelTooltipsEnabled$);

    // FormattedParagraphs lets vanilla Tooltip render JSON \n as real line breaks.
    const tt = React.useCallback(
        (s: string): React.ReactElement | undefined => (
            tooltipsEnabled
                ? <FormattedParagraphs>{s.split("\n")}</FormattedParagraphs>
                : undefined
        ),
        [tooltipsEnabled],
    );

    const [outline, setOutline] = React.useState<Color>(boundOutline);
    const [ownerColor, setOwnerColor] = React.useState<Color>(boundOwner);
    const [fillA, setFillA] = React.useState<number>(boundFillA);
    const [districtColor, setDistrictColor] = React.useState<Color>(boundDistrict);
    const [guidelineLinesColor, setGuidelineLinesColor] = React.useState<Color>(boundGuidelineLinesColor);
    const [guidelinePreviewColor, setGuidelinePreviewColor] = React.useState<Color>(boundGuidelinePreviewColor);
    const [guidelineDashedColor, setGuidelineDashedColor] = React.useState<Color>(boundGuidelineDashedColor);
    const [guidelineOpacity, setGuidelineOpacity] = React.useState<number>(boundGuideline);

    const [colorPickerDirection, setColorPickerDirection] = React.useState<"up" | "down">("down");
    const [ownerPickerDirection, setOwnerPickerDirection] = React.useState<"up" | "down">("down");
    const [guidelineLinesPickerDirection, setGuidelineLinesPickerDirection] = React.useState<"up" | "down">("up");
    const [guidelinePreviewPickerDirection, setGuidelinePreviewPickerDirection] = React.useState<"up" | "down">("up");
    const [guidelineDashedPickerDirection, setGuidelineDashedPickerDirection] = React.useState<"up" | "down">("up");
    const [districtPickerDirection, setDistrictPickerDirection] = React.useState<"up" | "down">("up");

    const [ownerPickerOpen, setOwnerPickerOpen] = React.useState(false);
    const [districtPickerOpen, setDistrictPickerOpen] = React.useState(false);
    const [districtMenuOpen, setDistrictMenuOpen] = React.useState(false);
    const [guidelineLinesPickerOpen, setGuidelineLinesPickerOpen] = React.useState(false);
    const [guidelinePreviewPickerOpen, setGuidelinePreviewPickerOpen] = React.useState(false);
    const [guidelineDashedPickerOpen, setGuidelineDashedPickerOpen] = React.useState(false);

    // ColorField can swallow hover events; React hover state keeps the visible rings reliable in COHTML.
    const [swatchHovered, setSwatchHovered] = React.useState(false);
    const [ownerSwatchHovered, setOwnerSwatchHovered] = React.useState(false);
    const [guidelineLinesHovered, setGuidelineLinesHovered] = React.useState(false);
    const [guidelinePreviewHovered, setGuidelinePreviewHovered] = React.useState(false);
    const [guidelineDashedHovered, setGuidelineDashedHovered] = React.useState(false);
    const [districtSwatchHovered, setDistrictSwatchHovered] = React.useState(false);
    const [preset1Hovered, setPreset1Hovered] = React.useState(false);
    const [preset2Hovered, setPreset2Hovered] = React.useState(false);

    const outlineSwatchRef = React.useRef<HTMLDivElement>(null);
    const ownerSwatchRef = React.useRef<HTMLDivElement>(null);
    const guidelineLinesPickerRef = React.useRef<HTMLDivElement>(null);
    const guidelinePreviewPickerRef = React.useRef<HTMLDivElement>(null);
    const guidelineDashedPickerRef = React.useRef<HTMLDivElement>(null);
    const districtPickerRef = React.useRef<HTMLDivElement>(null);
    const districtMenuRef = React.useRef<HTMLDivElement>(null);
    const districtColorSwatchRef = React.useRef<HTMLDivElement>(null);

    const {
        holdSlot,
        holdProgress,
        cancelHold,
        handlePresetMouseDown,
        handlePresetMouseUp,
    } = usePresetHold();
    const {
        panelOffset,
        panelDragging,
        panelElementRef,
        handlePanelDragStart,
    } = usePanelDrag();
    const { openAreasToolPanel } = useDistrictToolPanel();

    // Keep local controls synced when C# settings change through presets, reset buttons, or game reload.
    React.useEffect(() => { setOutline(boundOutline); }, [boundOutline.r, boundOutline.g, boundOutline.b, boundOutline.a]);
    React.useEffect(() => { setOwnerColor(boundOwner); }, [boundOwner.r, boundOwner.g, boundOwner.b, boundOwner.a]);
    React.useEffect(() => { setFillA(boundFillA); }, [boundFillA]);
    React.useEffect(() => { setDistrictColor(boundDistrict); }, [boundDistrict.r, boundDistrict.g, boundDistrict.b, boundDistrict.a]);
    React.useEffect(() => { setGuidelineLinesColor(boundGuidelineLinesColor); }, [boundGuidelineLinesColor.r, boundGuidelineLinesColor.g, boundGuidelineLinesColor.b, boundGuidelineLinesColor.a]);
    React.useEffect(() => { setGuidelinePreviewColor(boundGuidelinePreviewColor); }, [boundGuidelinePreviewColor.r, boundGuidelinePreviewColor.g, boundGuidelinePreviewColor.b, boundGuidelinePreviewColor.a]);
    React.useEffect(() => { setGuidelineDashedColor(boundGuidelineDashedColor); }, [boundGuidelineDashedColor.r, boundGuidelineDashedColor.g, boundGuidelineDashedColor.b]);
    React.useEffect(() => { setGuidelineOpacity(boundGuideline); }, [boundGuideline]);

    React.useEffect(() => {
        if (typeof document === "undefined") {
            return;
        }

        const compactPickerOpen = ownerPickerOpen || districtPickerOpen || guidelineLinesPickerOpen || guidelinePreviewPickerOpen || guidelineDashedPickerOpen;
        document.body.classList.toggle(COMPACT_PICKER_BODY_CLASS, compactPickerOpen);

        if (!compactPickerOpen) {
            return () => document.body.classList.remove(COMPACT_PICKER_BODY_CLASS);
        }

        const onMouseDown = (event: MouseEvent) => {
            const target = event.target as Element | null;
            if (target == null) {
                return;
            }

            // Vanilla ColorField closes on outside clicks but does not always call onClosePicker.
            if (
                districtMenuRef.current?.contains(target)
                || districtPickerRef.current?.contains(target)
                || districtColorSwatchRef.current?.contains(target)
                || ownerSwatchRef.current?.contains(target)
                || guidelineLinesPickerRef.current?.contains(target)
                || guidelinePreviewPickerRef.current?.contains(target)
                || guidelineDashedPickerRef.current?.contains(target)
                || target.closest(".color-picker-container_Sj5")
            ) {
                return;
            }

            setDistrictPickerOpen(false);
            setOwnerPickerOpen(false);
            setGuidelineLinesPickerOpen(false);
            setGuidelinePreviewPickerOpen(false);
            setGuidelineDashedPickerOpen(false);
        };

        document.addEventListener("mousedown", onMouseDown);
        return () => {
            document.removeEventListener("mousedown", onMouseDown);
            document.body.classList.remove(COMPACT_PICKER_BODY_CLASS);
        };
    }, [districtPickerOpen, guidelineDashedPickerOpen, guidelineLinesPickerOpen, guidelinePreviewPickerOpen, ownerPickerOpen]);

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

    const handleResetDistrict = React.useCallback(() => {
        trigger(CHANNEL, "ResetDistrictToVanilla");
        setDistrictPickerOpen(false);
    }, []);

    const handleOpenDistrictMenu = React.useCallback(() => {
        setDistrictMenuOpen(open => {
            const nextOpen = !open;
            if (nextOpen) {
                openAreasToolPanel();
            }

            return nextOpen;
        });
    }, [openAreasToolPanel]);

    const {
        districtHoldProgress,
        cancelDistrictHold,
        handleDistrictMouseDownCapture,
        handleDistrictMouseUpCapture,
        handleDistrictClickCapture,
    } = useDistrictHold({
        onReset: handleResetDistrict,
        onQuickClick: handleOpenDistrictMenu,
    });

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

    const handleGuidelineDashedColorChange = (value: Color) => {
        const syncedValue = normalizeColorFieldValue(value);
        const colorOnlyValue = { ...syncedValue, a: 1 };
        setGuidelineDashedColor(colorOnlyValue);
        trigger(CHANNEL, "SetGuidelineDashedColor", colorOnlyValue.r, colorOnlyValue.g, colorOnlyValue.b);
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

    const updatePickerDirection = React.useCallback((element: HTMLElement | null, setDirection: React.Dispatch<React.SetStateAction<"up" | "down">>) => {
        if (element == null) {
            return;
        }

        const rect = element.getBoundingClientRect();
        setDirection(rect.top + rect.height / 2 < window.innerHeight / 2 ? "down" : "up");
    }, []);

    const updateColorPickerDirection = React.useCallback(() => updatePickerDirection(outlineSwatchRef.current, setColorPickerDirection), [updatePickerDirection]);
    const updateOwnerPickerDirection = React.useCallback(() => updatePickerDirection(ownerSwatchRef.current, setOwnerPickerDirection), [updatePickerDirection]);
    const updateDistrictPickerDirection = React.useCallback(() => updatePickerDirection(districtColorSwatchRef.current ?? districtPickerRef.current, setDistrictPickerDirection), [updatePickerDirection]);
    const updateGuidelineLinesPickerDirection = React.useCallback(() => updatePickerDirection(guidelineLinesPickerRef.current, setGuidelineLinesPickerDirection), [updatePickerDirection]);
    const updateGuidelinePreviewPickerDirection = React.useCallback(() => updatePickerDirection(guidelinePreviewPickerRef.current, setGuidelinePreviewPickerDirection), [updatePickerDirection]);
    const updateGuidelineDashedPickerDirection = React.useCallback(() => updatePickerDirection(guidelineDashedPickerRef.current, setGuidelineDashedPickerDirection), [updatePickerDirection]);

    const resolver = VanillaComponentResolver.instance;
    const ColorField = resolver.ColorField;
    const Slider = resolver.Slider;
    const focusDisabled = resolver.FOCUS_DISABLED;
    const numberFieldClass = resolver.mouseToolOptionsTheme["number-field"];
    const roundHighlightButtonTheme = resolver.roundHighlightButtonTheme;
    const panelBaseTheme = resolver.panelBaseTheme;
    const panelTheme = resolver.panelTheme;
    const infoviewMenuTheme = resolver.infoviewMenuTheme;
    const closeButtonClass = `${roundHighlightButtonTheme["button"] ?? ""} ${styles.closeButton}`;
    const panelFrameClass = `${panelBaseTheme.panel ?? "panel_YqS"} ${infoviewMenuTheme.menu ?? "menu_O_M"} ${styles.panelFrame}`;
    const panelSurfaceClass = useDarkerPanel ? styles.panelDarker : styles.panelStandard;
    const panelContentClass = `${panelTheme.content ?? "content_XD5 content_AD7 child-opacity-transition_nkS"} ${infoviewMenuTheme.content ?? "content_Hzl"} ${styles.panelContent} ${panelSurfaceClass}`;

    return (
        <div
            className={styles.panelAnchor}
            style={{ transform: `translate(${panelOffset.x}px, ${panelOffset.y}px)` }}
        >
            <div ref={panelElementRef} className={panelFrameClass}>
                <div className={panelContentClass}>
                    <div className={styles.titleBar}>
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

                    <MochiPanelControlRows
                        text={text}
                        tt={tt}
                        ColorField={ColorField}
                        Slider={Slider}
                        focusDisabled={focusDisabled}
                        numberFieldClass={numberFieldClass}
                        useDarkerPanel={useDarkerPanel}
                        outline={outline}
                        ownerColor={ownerColor}
                        fillA={fillA}
                        guidelineLinesColor={guidelineLinesColor}
                        guidelinePreviewColor={guidelinePreviewColor}
                        guidelineDashedColor={guidelineDashedColor}
                        guidelineOpacity={guidelineOpacity}
                        preset1Color={preset1Color}
                        preset2Color={preset2Color}
                        colorPickerDirection={colorPickerDirection}
                        ownerPickerDirection={ownerPickerDirection}
                        guidelineLinesPickerDirection={guidelineLinesPickerDirection}
                        guidelinePreviewPickerDirection={guidelinePreviewPickerDirection}
                        guidelineDashedPickerDirection={guidelineDashedPickerDirection}
                        vanillaOutlineActive={vanillaOutlineActive}
                        preset1Active={preset1Active}
                        preset2Active={preset2Active}
                        swatchHovered={swatchHovered}
                        ownerSwatchHovered={ownerSwatchHovered}
                        guidelineLinesHovered={guidelineLinesHovered}
                        guidelinePreviewHovered={guidelinePreviewHovered}
                        guidelineDashedHovered={guidelineDashedHovered}
                        preset1Hovered={preset1Hovered}
                        preset2Hovered={preset2Hovered}
                        setSwatchHovered={setSwatchHovered}
                        setOwnerSwatchHovered={setOwnerSwatchHovered}
                        setGuidelineLinesHovered={setGuidelineLinesHovered}
                        setGuidelinePreviewHovered={setGuidelinePreviewHovered}
                        setGuidelineDashedHovered={setGuidelineDashedHovered}
                        setPreset1Hovered={setPreset1Hovered}
                        setPreset2Hovered={setPreset2Hovered}
                        setOwnerPickerOpen={setOwnerPickerOpen}
                        setGuidelineLinesPickerOpen={setGuidelineLinesPickerOpen}
                        setGuidelinePreviewPickerOpen={setGuidelinePreviewPickerOpen}
                        setGuidelineDashedPickerOpen={setGuidelineDashedPickerOpen}
                        holdSlot={holdSlot}
                        holdProgress={holdProgress}
                        cancelHold={cancelHold}
                        handlePresetMouseDown={handlePresetMouseDown}
                        handlePresetMouseUp={handlePresetMouseUp}
                        outlineSwatchRef={outlineSwatchRef}
                        ownerSwatchRef={ownerSwatchRef}
                        guidelineLinesPickerRef={guidelineLinesPickerRef}
                        guidelinePreviewPickerRef={guidelinePreviewPickerRef}
                        guidelineDashedPickerRef={guidelineDashedPickerRef}
                        handleOutlineChange={handleOutlineChange}
                        handleOwnerColorChange={handleOwnerColorChange}
                        handleFillAChange={handleFillAChange}
                        handleGuidelineLinesColorChange={handleGuidelineLinesColorChange}
                        handleGuidelinePreviewColorChange={handleGuidelinePreviewColorChange}
                        handleGuidelineDashedColorChange={handleGuidelineDashedColorChange}
                        handleGuidelineChange={handleGuidelineChange}
                        handleResetOutline={handleResetOutline}
                        handleResetFill={handleResetFill}
                        handleResetGuidelines={handleResetGuidelines}
                        handleTogglePresetDefaults={handleTogglePresetDefaults}
                        updateColorPickerDirection={updateColorPickerDirection}
                        updateOwnerPickerDirection={updateOwnerPickerDirection}
                        updateGuidelineLinesPickerDirection={updateGuidelineLinesPickerDirection}
                        updateGuidelinePreviewPickerDirection={updateGuidelinePreviewPickerDirection}
                        updateGuidelineDashedPickerDirection={updateGuidelineDashedPickerDirection}
                    />

                    <MochiPanelActionBar
                        text={text}
                        tt={tt}
                        ColorField={ColorField}
                        focusDisabled={focusDisabled}
                        useDarkerPanel={useDarkerPanel}
                        surfaceToolAreasSuppressed={surfaceToolAreasSuppressed}
                        specializedIndustryAreasSuppressed={specializedIndustryAreasSuppressed}
                        districtMenuOpen={districtMenuOpen}
                        districtColor={districtColor}
                        districtPickerDirection={districtPickerDirection}
                        districtSwatchHovered={districtSwatchHovered}
                        districtHoldProgress={districtHoldProgress}
                        districtPickerRef={districtPickerRef}
                        districtMenuRef={districtMenuRef}
                        districtColorSwatchRef={districtColorSwatchRef}
                        setDistrictPickerOpen={setDistrictPickerOpen}
                        setDistrictSwatchHovered={setDistrictSwatchHovered}
                        cancelDistrictHold={cancelDistrictHold}
                        openAreasToolPanel={openAreasToolPanel}
                        updateDistrictPickerDirection={updateDistrictPickerDirection}
                        handleToggleSurfaceToolAreas={handleToggleSurfaceToolAreas}
                        handleToggleSpecializedIndustryAreas={handleToggleSpecializedIndustryAreas}
                        handleDistrictMouseDownCapture={handleDistrictMouseDownCapture}
                        handleDistrictMouseUpCapture={handleDistrictMouseUpCapture}
                        handleDistrictClickCapture={handleDistrictClickCapture}
                        handleDistrictColorChange={handleDistrictColorChange}
                        handleResetDistrict={handleResetDistrict}
                    />

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
