// File: UI/src/ModIconButton.tsx
// GameTopLeft launcher for the Hover Power in-city panel. Same as CWD + EasyZoning.

import React from "react";
import { Button, Tooltip } from "cs2/ui";
import { bindValue, trigger, useValue } from "cs2/api";
import { useLocalization } from "cs2/l10n";
import { MochiColorPickerPanel } from "./MochiColorPickerPanel";
import locale from "../../L10n/lang/en-US.json";
import styles from "./ModIconButton.module.scss";

// SVG passed via Button.src so its own fills render (single color today, multi-color later).
import ModIconPath from "../images/MainElements.svg";

const CHANNEL = "HoverPower";
const panelOpen$ = bindValue<boolean>(CHANNEL, "PanelOpen", false);

export default () => {
    const isOpen = useValue(panelOpen$);
    const { translate } = useLocalization();
    const tooltip = translate("HoverPower.UI.TopLeft.Tooltip", locale["HoverPower.UI.TopLeft.Tooltip"])
        ?? locale["HoverPower.UI.TopLeft.Tooltip"];

    return (
        // .anchor is position:relative only — lets the panel below absolute-position under the button.
        <div className={styles.anchor}>
            <Tooltip tooltip={tooltip}>
                <Button
                    variant="floating"
                    src={ModIconPath}                                            // SVG colors render as-is, no tinting
                    selected={isOpen}                                            // vanilla light-blue overlay when active
                    onSelect={() => trigger(CHANNEL, "SetPanelOpen", !isOpen)}   // C# owns the toggle so H hotkey shares state
                />
            </Tooltip>

            {isOpen && <MochiColorPickerPanel />}
        </div>
    );
};
