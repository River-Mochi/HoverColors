// File: UI/src/panel/MochiPanelActionBar.tsx
// Purpose: Bottom action bar: Surface, Specialized Industry, and District color menu.

import React from "react";
import { Tooltip } from "cs2/ui";
import { Color } from "cs2/bindings";
import { compactSwatchStyle, holdBarStyle } from "./MochiPanelColorUtils";
import { useMochiPanelText } from "./useMochiPanelText";
import lotToolIconSrc from "../../images/LotTool03.svg";
import specializedIndustryIconSrc from "../../images/LotToolSpecializedIndustry.svg";
import surfaceIconSrc from "../../images/Districts03.svg";
import styles from "../MochiColorPickerPanel.module.scss";

type PickerDirection = "up" | "down";
type TooltipFn = (text: string) => React.ReactNode | undefined;
type ColorFieldComponent = React.ComponentType<any>;
type PanelText = ReturnType<typeof useMochiPanelText>;

interface MochiPanelActionBarProps {
    text: PanelText;
    tt: TooltipFn;
    ColorField: ColorFieldComponent;
    focusDisabled: any;
    useDarkerPanel: boolean;

    surfaceToolAreasSuppressed: boolean;
    specializedIndustryAreasSuppressed: boolean;
    districtMenuOpen: boolean;
    districtColor: Color;
    districtPickerDirection: PickerDirection;
    districtSwatchHovered: boolean;
    districtHoldProgress: number;

    districtPickerRef: React.RefObject<HTMLDivElement>;
    districtMenuRef: React.RefObject<HTMLDivElement>;
    districtColorSwatchRef: React.RefObject<HTMLDivElement>;

    setDistrictPickerOpen: React.Dispatch<React.SetStateAction<boolean>>;
    setDistrictSwatchHovered: React.Dispatch<React.SetStateAction<boolean>>;

    cancelDistrictHold: () => void;
    openAreasToolPanel: () => void;
    updateDistrictPickerDirection: () => void;

    handleToggleSurfaceToolAreas: () => void;
    handleToggleSpecializedIndustryAreas: () => void;
    handleDistrictMouseDownCapture: React.MouseEventHandler;
    handleDistrictMouseUpCapture: React.MouseEventHandler;
    handleDistrictClickCapture: React.MouseEventHandler;
    handleDistrictColorChange: (value: Color) => void;
    handleResetDistrict: () => void;
}

export const MochiPanelActionBar = ({
    text,
    tt,
    ColorField,
    focusDisabled,
    useDarkerPanel,
    surfaceToolAreasSuppressed,
    specializedIndustryAreasSuppressed,
    districtMenuOpen,
    districtColor,
    districtPickerDirection,
    districtSwatchHovered,
    districtHoldProgress,
    districtPickerRef,
    districtMenuRef,
    districtColorSwatchRef,
    setDistrictPickerOpen,
    setDistrictSwatchHovered,
    cancelDistrictHold,
    openAreasToolPanel,
    updateDistrictPickerDirection,
    handleToggleSurfaceToolAreas,
    handleToggleSpecializedIndustryAreas,
    handleDistrictMouseDownCapture,
    handleDistrictMouseUpCapture,
    handleDistrictClickCapture,
    handleDistrictColorChange,
    handleResetDistrict,
}: MochiPanelActionBarProps) => {
    const districtShellStyle = React.useCallback(
        (color: Color, hovered: boolean) => compactSwatchStyle(color, hovered, useDarkerPanel),
        [useDarkerPanel],
    );

    return (
        <div className={styles.actions}>
            <div className={styles.surfaceActions}>
                <Tooltip tooltip={tt(text.tooltipSurfaceToggle)}>
                    <button
                        type="button"
                        className={`${styles.actionButton} ${styles.surfaceButton} ${surfaceToolAreasSuppressed ? styles.surfaceButtonActive : ""}`}
                        onClick={handleToggleSurfaceToolAreas}
                    >
                        <img src={lotToolIconSrc} className={`${styles.controlIcon} ${styles.idleIcon}`} alt="" />
                    </button>
                </Tooltip>

                <Tooltip tooltip={tt(text.tooltipSpecializedIndustryToggle)}>
                    <button
                        type="button"
                        className={`${styles.actionButton} ${styles.surfaceButton} ${styles.buttonGap} ${specializedIndustryAreasSuppressed ? styles.surfaceButtonActive : ""}`}
                        onClick={handleToggleSpecializedIndustryAreas}
                    >
                        <img src={specializedIndustryIconSrc} className={`${styles.controlIcon} ${styles.idleIcon}`} alt="" />
                    </button>
                </Tooltip>

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
                            style={districtShellStyle(districtColor, districtSwatchHovered)}
                            onMouseOver={() => {
                                if (!districtSwatchHovered) {
                                    setDistrictSwatchHovered(true);
                                }
                                updateDistrictPickerDirection();
                            }}
                            onMouseMove={() => {
                                if (!districtSwatchHovered) {
                                    setDistrictSwatchHovered(true);
                                }
                            }}
                            onMouseLeave={() => setDistrictSwatchHovered(false)}
                            onMouseDown={updateDistrictPickerDirection}
                        >
                            {/* Phase 2 can render one of these rows per District. */}
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

                        <span className={styles.districtMenuName}>
                            {text.districtMenuAllDistricts}
                        </span>

                        <Tooltip tooltip={tt(text.tooltipResetDistrictColors)}>
                            <button
                                type="button"
                                className={styles.districtMenuReset}
                                onClick={handleResetDistrict}
                            >
                                {text.districtMenuResetAll}
                            </button>
                        </Tooltip>
                    </div>
                </div>
            )}
        </div>
    );
};
