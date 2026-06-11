// File: UI/src/panel/MochiPanelColorUtils.ts
// Small color/style helpers used by the compact panel controls.

import { Color } from "cs2/bindings";

export const normalizeColorFieldValue = (value: Color): Color => {
    const alpha = typeof value.a === "number" && Number.isFinite(value.a) ? value.a : 1;
    return {
        ...value,
        a: Math.max(0, Math.min(1, alpha)),
    };
};

// Preview ignores alpha for legibility; saved preset still applies alpha in-game.
export const presetPreviewStyle = (c: Color) => ({
    backgroundColor: `rgb(${Math.round(c.r * 255)},${Math.round(c.g * 255)},${Math.round(c.b * 255)})`,
});

// Small swatches show RGB clearly; alpha still applies in-game.
export const compactSwatchStyle = (c: Color, hovered = false, useDarkerPanel = false) => {
    const channel = (value: unknown, fallback: number) => {
        const n = Number(value);
        return Math.round(Math.max(0, Math.min(1, Number.isFinite(n) ? n : fallback)) * 255);
    };

    const r = channel(c.r, 0.7);
    const g = channel(c.g, 0.7);
    const b = channel(c.b, 1);
    const idleShadow = useDarkerPanel ? "inset 0 0 0 1rem rgba(7, 13, 18, 0.32)" : "none";
    const hoverShadow = useDarkerPanel
        ? "inset 0 0 0 1rem rgba(7, 13, 18, 0.32), 0 0 0 1.15rem rgba(255, 255, 255, 0.76)"
        : "0 0 0 1.15rem rgba(255, 255, 255, 0.76)";

    return {
        backgroundColor: `rgb(${r},${g},${b})`,
        // Standard stays clean; Dark keeps the extra edge for contrast.
        boxShadow: hovered ? hoverShadow : idleShadow,
    };
};

export const presetNumberColor = (active: boolean, hovered: boolean) => {
    if (hovered) {
        return "rgba(255, 255, 255, 1)";
    }

    // Active number stays opaque; softer cyan comes from RGB, not alpha.
    return active ? "rgba(150, 235, 255, 0.96)" : "rgba(255, 255, 255, 0.78)";
};

// scaleX keeps hold progress independent of button width.
export const holdBarStyle = (progress: number) => ({ transform: `scaleX(${progress})` });
