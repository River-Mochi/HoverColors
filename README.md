# Hover Colors [HC]

Recolor hover highlights, reduce visual clutter, and make area tools easier to see in Cities: Skylines II.

- Pick outline, owner, guideline, and District colors with the game's vanilla color picker.
- Adjust hover fill opacity and guideline opacity from a compact in-game panel.
- Save two quick outline presets, including opacity, for fast color switching.
- Keep vanilla-safe colors for bulldozer, roads, NetLanes, and placement errors when desired.
- Change District overlay/border color and opacity, including turning the overlay off.
- Hide Surface tool preview fill so layered surfaces can be judged without the cloudy overlay.
- Use a darker panel mode for Legacy UI or players who prefer a more solid panel.
- Keep player colors and presets saved between sessions in `ModsSettings/HoverColors/HoverColors.coc`.
- No Harmony patches.

## How It Works

Hover Colors uses the game's own systems where possible: ECS systems, prefab color data, `RenderingSettingsData`, `GuideLineSettingsData`, and the HDRP `OutlinesWorldUIPass` material values. The visible vanilla hover look is not one RGBA field, so the mod captures/restores the relevant vanilla profile instead of guessing a single hard-coded color.

The Surface preview toggle adjusts the Area tool's required area mask while active, then restores it when disabled or when the tool changes. District colors are applied to the vanilla `District Area` prefab only, so custom area assets are not accidentally recolored.

## License

MIT - see [LICENSE](LICENSE).
