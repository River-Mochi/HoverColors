// File: UI/src/ModIconButton.tsx
// Purpose: Floating GameTopLeft launcher button for the Magic Highlights in-city panel.
// Matches the CityWatchdog/EasyZoning EntryButton pattern:
//   - Vanilla <Button variant="floating" src={iconPath} selected={isOpen} /> handles hover +
//     selected styling natively. No <img maskImage> trick (that was the cause of "all white" bug).
//   - One SVG (the icon keeps its own colors). No active-state alt SVG.
//   - Clicking toggles the panel.

import React, { useState } from "react";
import { Button, Tooltip } from "cs2/ui";
import { MochiColorPickerPanel } from "./MochiColorPickerPanel";
import styles from "./ModIconButton.module.scss";

// Webpack emits this to coui://ui-mods/images/.
import ModIconPath from "../images/OutlineColors.svg";

export default () => {
    const [isOpen, setIsOpen] = useState(false);

    return (
        <div className={styles.anchor}>
            <Tooltip tooltip="Magic Highlights">
                <Button
                    variant="floating"
                    src={ModIconPath}
                    selected={isOpen}
                    onSelect={() => setIsOpen(v => !v)}
                />
            </Tooltip>

            {isOpen && <MochiColorPickerPanel />}
        </div>
    );
};
