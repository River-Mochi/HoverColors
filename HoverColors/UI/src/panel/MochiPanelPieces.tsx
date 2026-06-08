// File: UI/src/panel/MochiPanelPieces.tsx
// Purpose: Small reusable pieces for MochiColorPickerPanel.tsx.

import React from "react";
import { Tooltip } from "cs2/ui";
import { Color } from "cs2/bindings";
import styles from "../MochiColorPickerPanel.module.scss";

type PresetSlotButtonProps = {
    slot: 1 | 2;
    color: Color;
    active: boolean;
    holdActive: boolean;
    holdProgress: number;
    tooltip?: string;
    marginLeft: string;
    numberColor: string;
    presetPreviewStyle: (color: Color) => React.CSSProperties;
    holdBarStyle: (progress: number) => React.CSSProperties;
    onMouseEnter: () => void;
    onMouseDown: (event: React.MouseEvent<HTMLButtonElement>) => void;
    onMouseUp: () => void;
    onMouseLeave: () => void;
};

export const PresetSlotButton = ({
    slot,
    color,
    active,
    holdActive,
    holdProgress,
    tooltip,
    marginLeft,
    numberColor,
    presetPreviewStyle,
    holdBarStyle,
    onMouseEnter,
    onMouseDown,
    onMouseUp,
    onMouseLeave,
}: PresetSlotButtonProps) => (
    <Tooltip tooltip={tooltip}>
        <button
            type="button"
            className={`${styles.presetSlot} ${active ? styles.presetSlotActive : ""}`}
            style={{ marginLeft }}
            onMouseEnter={onMouseEnter}
            onMouseDown={onMouseDown}
            onMouseUp={onMouseUp}
            onMouseLeave={onMouseLeave}
        >
            {holdActive && holdProgress > 0 && (
                <span className={styles.holdBar} style={holdBarStyle(holdProgress)} />
            )}
            <span className={styles.presetNumber} style={{ color: numberColor }}>
                {slot}
            </span>
            <span className={styles.presetSwatch} style={presetPreviewStyle(color)} />
        </button>
    </Tooltip>
);

type DragGripProps = {
    active: boolean;
    tooltip?: string;
    onMouseDown: (event: React.MouseEvent<HTMLDivElement>) => void;
};

export const DragGrip = ({ active, tooltip, onMouseDown }: DragGripProps) => (
    <Tooltip tooltip={tooltip}>
        <div
            className={`${styles.dragGrip} ${active ? styles.dragGripActive : ""}`}
            onMouseDown={onMouseDown}
        >
            <span className={styles.dragGripDot} />
            <span className={styles.dragGripDot} />
            <span className={styles.dragGripDot} />
        </div>
    </Tooltip>
);
