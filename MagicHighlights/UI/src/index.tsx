import { ModRegistrar } from "cs2/modding";
import { VanillaResolver } from "./utils/VanilliaResolver";

import ModIconButton from "./ModIconButton";

const register: ModRegistrar = (moduleRegistry) => {
    VanillaResolver.setRegistry(moduleRegistry);

    moduleRegistry.append(
        "GameTopLeft",
        ModIconButton
    );
};

export default register;