// File: UI/src/index.tsx
// Purpose: Mod entry point registered with the cs2/modding registry.
//   - Wires up the VanillaComponentResolver so all vanilla components resolve once on load.
//   - Appends ModIconButton to the GameTopLeft (GTL) toolbar.
// This file is the webpack entry point; only add module-level side effects here.

import { ModRegistrar } from "cs2/modding";
import { VanillaComponentResolver } from "./utils/vanilla/VanillaComponentResolver";
import "./MochiColorPickerPanel.global.scss";

import ModIconButton from "./entry/ModIconButton";

const register: ModRegistrar = (moduleRegistry) => {
    VanillaComponentResolver.setRegistry(moduleRegistry);

    moduleRegistry.append(
        "GameTopLeft",
        ModIconButton
    );
};

export default register;
