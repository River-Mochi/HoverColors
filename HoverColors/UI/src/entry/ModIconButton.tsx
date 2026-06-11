// File: UI/src/entry/ModIconButton.tsx
// GameTopLeft launcher for the Hover Colors in-city panel. Same as CWD + EasyZoning.

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { bindValue, trigger, useValue } from "cs2/api";
import { usePanelLocalization } from "../localization";
import { MochiColorPickerPanel } from "../MochiColorPickerPanel";
import styles from "./ModIconButton.module.scss";

// SVG passed via Button.src so its own fills render (single color today, multi-color later).
import ModIconPath from "../../images/MainElements_short_bigTriangle.svg";

const CHANNEL = "HoverColors";
const panelOpen$ = bindValue<boolean>(CHANNEL, "PanelOpen", false);

export default () => {
    const isOpen = useValue(panelOpen$);
    const translatePanel = usePanelLocalization();
    const tooltip = translatePanel("HoverColors.UI.TopLeft.Tooltip");

    return (
        // .anchor is position:relative only — lets the panel below absolute-position under the button.
        <div className={styles.anchor}>
            <Tooltip tooltip={tooltip}>
                <Button
                    variant="floating"
                    src={ModIconPath}                                            // SVG colors render as-is, no tinting
                    // No selected prop: hover lightens, but open panel does not keep the GTL icon tinted.
                    onSelect={() => trigger(CHANNEL, "SetPanelOpen", !isOpen)}   // C# owns the toggle so J hotkey shares state
                />
            </Tooltip>

            {isOpen && <MochiColorPickerPanel />}
        </div>
    );
};
