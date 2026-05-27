import type { FocusKey, UniqueFocusKey } from "cs2/bindings";
import type { ModuleRegistry } from "cs2/modding";
import type { FC } from "react";
import type {
    VanillaCheckboxProps,
    VanillaColorFieldProps,
    VanillaDescriptionTooltipProps,
    VanillaDropdownItemComponentProps,
    VanillaInfoLinkProps,
    VanillaInfoRowComponentProps,
    VanillaInfoSectionComponentProps,
    VanillaPageSelectorProps,
    VanillaPageSwitcherProps,
    VanillaScrollableComponentProps,
    VanillaSectionProps,
    VanillaSliderProps,
    VanillaStepToolButtonProps,
    VanillaTextInputProps,
    VanillaThemeValue,
    VanillaToolButtonProps,
    VanillaUseUniqueFocusKey,
} from "./types";

// Shared vanilla-module library for HoverPower and future CS2 mods.
//
// This combines two good patterns from community resolvers:
// - yenyang's "how to discover modules in the UI bundle" comments
// - Luca's scalable grouped registries for components/themes/focus helpers
//
// Practical notes for future you:
// 1. Open the in-game UI dev tools at http://localhost:9444/
// 2. In Sources -> index.js, pretty-print if needed
// 3. Search for the TSX/SCSS path you want
// 4. Add it to one of the maps below and expose a typed getter
//
// Also remember: `UI/types/bindings.d.ts` is helpful for binding/widget payloads,
// but the vanilla React module props can differ from the binding model.

export const VANILLA_COMPONENT_MODULES = {
    Section: ["game-ui/game/components/tool-options/mouse-tool-options/mouse-tool-options.tsx", "Section"],
    ToolButton: ["game-ui/game/components/tool-options/tool-button/tool-button.tsx", "ToolButton"],
    StepToolButton: ["game-ui/game/components/tool-options/tool-button/tool-button.tsx", "StepToolButton"],
    Checkbox: ["game-ui/common/input/toggle/checkbox/checkbox.tsx", "Checkbox"],
    DescriptionTooltip: ["game-ui/common/tooltip/description-tooltip/description-tooltip.tsx", "DescriptionTooltip"],
    ColorField: ["game-ui/common/input/color-picker/color-field/color-field.tsx", "ColorField"],
    Slider: ["game-ui/common/input/slider/slider.tsx", "Slider"],
    InfoSection: ["game-ui/game/components/selected-info-panel/shared-components/info-section/info-section.tsx", "InfoSection"],
    InfoRow: ["game-ui/game/components/selected-info-panel/shared-components/info-row/info-row.tsx", "InfoRow"],
    InfoLink: ["game-ui/game/components/selected-info-panel/shared-components/info-link/info-link.tsx", "InfoLink"],
    PageSelector: ["game-ui/menu/components/whats-new-panel/page-selector/page-selector.tsx", "PageSelector"],
    PageSwitcher: ["game-ui/common/animations/paging/page-switcher.tsx", "PageSwitcher"],
    Page: ["game-ui/common/animations/paging/page-switcher.tsx", "Page"],
    Scrollable: ["game-ui/common/scrolling/scrollable.tsx", "Scrollable"],
    TextInput: ["game-ui/common/input/text/text-input.tsx", "TextInput"],
    DropdownItem: ["game-ui/common/input/dropdown/items/dropdown-item.tsx", "DropdownItem"],
} as const;

export const VANILLA_THEME_MODULES = {
    toolButtonTheme: ["game-ui/game/components/tool-options/tool-button/tool-button.module.scss", "classes"],
    mouseToolOptionsTheme: ["game-ui/game/components/tool-options/mouse-tool-options/mouse-tool-options.module.scss", "classes"],
    descriptionTooltipTheme: ["game-ui/common/tooltip/description-tooltip/description-tooltip.module.scss", "classes"],
    colorFieldTheme: ["game-ui/common/input/color-picker/color-field/color-field.module.scss", "classes"],
    commonDropdownTheme: ["game-ui/common/input/dropdown/dropdown.module.scss", "classes"],
    menuDropdownTheme: ["game-ui/menu/themes/dropdown.module.scss", "classes"],
    checkboxTheme: ["game-ui/common/input/toggle/checkbox/checkbox.module.scss", "classes"],
    statisticsCheckboxTheme: ["game-ui/game/components/statistics-panel/menu/item/statistics-item.module.scss", "classes"],
    toolbarFeatureButtonTheme: ["game-ui/game/components/toolbar/components/feature-button/toolbar-feature-button.module.scss", "classes"],
    roundHighlightButtonTheme: ["game-ui/common/input/button/themes/round-highlight-button.module.scss", "classes"],
    infoRowTheme: ["game-ui/game/components/selected-info-panel/shared-components/info-row/info-row.module.scss", "classes"],
    panelTheme: ["game-ui/common/panel/themes/default.module.scss", "classes"],
    pageSelectorTheme: ["game-ui/menu/components/whats-new-panel/page-selector/page-selector.module.scss", "classes"],
    whatsNewPageTheme: ["game-ui/menu/components/whats-new-panel/whats-new-page/whats-new-page.module.scss", "classes"],
    horizontalTransitionTheme: ["game-ui/common/animations/paging/transitions/horizontal-transition.module.scss", "classes"],
    whatsNewTabTheme: ["game-ui/menu/components/whats-new-panel/whats-new-tab/whats-new-tab.module.scss", "classes"],
    toolOptionsPanelTheme: ["game-ui/game/components/tool-options/tool-options-panel.module.scss", "classes"],
    assetCategoryTabItemTheme: ["game-ui/game/components/asset-menu/asset-category-tab-bar/category-item.module.scss", "classes"],
    assetCategoryTabBarTheme: ["game-ui/game/components/asset-menu/asset-category-tab-bar/asset-category-tab-bar.module.scss", "classes"],
    itemGridTheme: ["game-ui/game/components/item-grid/item-grid.module.scss", "classes"],
    textInputTheme: ["game-ui/editor/widgets/item/editor-item.module.scss", "classes"],
} as const;

export const VANILLA_FOCUS_MODULES = {
    FOCUS_DISABLED: ["game-ui/common/focus/focus-key.ts", "FOCUS_DISABLED"],
    FOCUS_AUTO: ["game-ui/common/focus/focus-key.ts", "FOCUS_AUTO"],
    useUniqueFocusKey: ["game-ui/common/focus/focus-key.ts", "useUniqueFocusKey"],
} as const;

export type VanillaComponentModuleName = keyof typeof VANILLA_COMPONENT_MODULES;
export type VanillaThemeModuleName = keyof typeof VANILLA_THEME_MODULES;
export type VanillaFocusModuleName = keyof typeof VANILLA_FOCUS_MODULES;

type VanillaModuleName =
    | VanillaComponentModuleName
    | VanillaThemeModuleName
    | VanillaFocusModuleName;

export class VanillaComponentResolver {
    public static get instance(): VanillaComponentResolver {
        return this._instance!!;
    }

    private static _instance?: VanillaComponentResolver;

    public static setRegistry(moduleRegistry: ModuleRegistry) {
        this._instance = new VanillaComponentResolver(moduleRegistry);
    }

    private readonly registryData: ModuleRegistry;
    private readonly cache: Partial<Record<VanillaModuleName, unknown>> = {};

    private constructor(moduleRegistry: ModuleRegistry) {
        this.registryData = moduleRegistry;
    }

    private resolve<T>(entry: VanillaModuleName, path: string, exportName: string): T {
        const cached = this.cache[entry];
        if (cached !== undefined) {
            return cached as T;
        }

        const moduleValue = this.registryData.registry.get(path);
        if (moduleValue == null || !(exportName in moduleValue)) {
            throw new Error(
                `[VanillaComponentResolver] Failed to resolve ${entry} from ${path} -> ${exportName}.`,
            );
        }

        const resolved = (moduleValue as Record<string, unknown>)[exportName] as T;
        this.cache[entry] = resolved;
        return resolved;
    }

    public resolveComponent<T = FC<any>>(entry: VanillaComponentModuleName): T {
        const [path, exportName] = VANILLA_COMPONENT_MODULES[entry];
        return this.resolve<T>(entry, path, exportName);
    }

    public resolveTheme<T = VanillaThemeValue>(entry: VanillaThemeModuleName): T {
        const [path, exportName] = VANILLA_THEME_MODULES[entry];
        return this.resolve<T>(entry, path, exportName);
    }

    public resolveFocus<T = unknown>(entry: VanillaFocusModuleName): T {
        const [path, exportName] = VANILLA_FOCUS_MODULES[entry];
        return this.resolve<T>(entry, path, exportName);
    }

    public get Section(): FC<VanillaSectionProps> {
        return this.resolveComponent("Section");
    }

    public get ToolButton(): FC<VanillaToolButtonProps> {
        return this.resolveComponent("ToolButton");
    }

    public get StepToolButton(): FC<VanillaStepToolButtonProps> {
        return this.resolveComponent("StepToolButton");
    }

    public get Checkbox(): FC<VanillaCheckboxProps> {
        return this.resolveComponent("Checkbox");
    }

    public get DescriptionTooltip(): FC<VanillaDescriptionTooltipProps> {
        return this.resolveComponent("DescriptionTooltip");
    }

    public get ColorField(): FC<VanillaColorFieldProps> {
        return this.resolveComponent("ColorField");
    }

    public get Slider(): FC<VanillaSliderProps> {
        return this.resolveComponent("Slider");
    }

    public get InfoSection(): FC<VanillaInfoSectionComponentProps> {
        return this.resolveComponent("InfoSection");
    }

    public get InfoRow(): FC<VanillaInfoRowComponentProps> {
        return this.resolveComponent("InfoRow");
    }

    public get InfoLink(): FC<VanillaInfoLinkProps> {
        return this.resolveComponent("InfoLink");
    }

    public get PageSelector(): FC<VanillaPageSelectorProps> {
        return this.resolveComponent("PageSelector");
    }

    public get PageSwitcher(): FC<VanillaPageSwitcherProps> {
        return this.resolveComponent("PageSwitcher");
    }

    public get Page(): FC<any> {
        return this.resolveComponent("Page");
    }

    public get Scrollable(): FC<VanillaScrollableComponentProps> {
        return this.resolveComponent("Scrollable");
    }

    public get TextInput(): FC<VanillaTextInputProps> {
        return this.resolveComponent("TextInput");
    }

    public get DropdownItem(): FC<VanillaDropdownItemComponentProps> {
        return this.resolveComponent("DropdownItem");
    }

    public get toolButtonTheme(): VanillaThemeValue {
        return this.resolveTheme("toolButtonTheme");
    }

    public get mouseToolOptionsTheme(): VanillaThemeValue {
        return this.resolveTheme("mouseToolOptionsTheme");
    }

    public get descriptionTooltipTheme(): VanillaThemeValue {
        return this.resolveTheme("descriptionTooltipTheme");
    }

    public get colorFieldTheme(): VanillaThemeValue {
        return this.resolveTheme("colorFieldTheme");
    }

    public get commonDropdownTheme(): VanillaThemeValue {
        return this.resolveTheme("commonDropdownTheme");
    }

    public get menuDropdownTheme(): VanillaThemeValue {
        return this.resolveTheme("menuDropdownTheme");
    }

    public get checkboxTheme(): VanillaThemeValue {
        return this.resolveTheme("checkboxTheme");
    }

    public get statisticsCheckboxTheme(): VanillaThemeValue {
        return this.resolveTheme("statisticsCheckboxTheme");
    }

    public get toolbarFeatureButtonTheme(): VanillaThemeValue {
        return this.resolveTheme("toolbarFeatureButtonTheme");
    }

    public get roundHighlightButtonTheme(): VanillaThemeValue {
        return this.resolveTheme("roundHighlightButtonTheme");
    }

    public get infoRowTheme(): VanillaThemeValue {
        return this.resolveTheme("infoRowTheme");
    }

    public get panelTheme(): VanillaThemeValue {
        return this.resolveTheme("panelTheme");
    }

    public get pageSelectorTheme(): VanillaThemeValue {
        return this.resolveTheme("pageSelectorTheme");
    }

    public get whatsNewPageTheme(): VanillaThemeValue {
        return this.resolveTheme("whatsNewPageTheme");
    }

    public get horizontalTransitionTheme(): VanillaThemeValue {
        return this.resolveTheme("horizontalTransitionTheme");
    }

    public get whatsNewTabTheme(): VanillaThemeValue {
        return this.resolveTheme("whatsNewTabTheme");
    }

    public get toolOptionsPanelTheme(): VanillaThemeValue {
        return this.resolveTheme("toolOptionsPanelTheme");
    }

    public get assetCategoryTabItemTheme(): VanillaThemeValue {
        return this.resolveTheme("assetCategoryTabItemTheme");
    }

    public get assetCategoryTabBarTheme(): VanillaThemeValue {
        return this.resolveTheme("assetCategoryTabBarTheme");
    }

    public get itemGridTheme(): VanillaThemeValue {
        return this.resolveTheme("itemGridTheme");
    }

    public get textInputTheme(): VanillaThemeValue {
        return this.resolveTheme("textInputTheme");
    }

    public get FOCUS_DISABLED(): UniqueFocusKey {
        return this.resolveFocus("FOCUS_DISABLED");
    }

    public get FOCUS_AUTO(): UniqueFocusKey {
        return this.resolveFocus("FOCUS_AUTO");
    }

    public get useUniqueFocusKey(): VanillaUseUniqueFocusKey {
        return this.resolveFocus("useUniqueFocusKey");
    }
}

// Short alias kept because a lot of CS2 UI examples use this name.
export const VanillaResolver = VanillaComponentResolver;
