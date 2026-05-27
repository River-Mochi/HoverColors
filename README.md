# Magic Highlights [MMH]

Recolor selection outlines and hover highlights in Cities: Skylines II.

- Pick custom **inner** (fill) and **outer** (outline) colors for the hovered/selected object highlight
- Independent **alpha** sliders for inner and outer
- Settings persist between sessions
- No Harmony patches — uses the game's own rendering ECS components and HDRP custom pass

---

## How it works

Two render surfaces are affected:

1. **Selection outline halo** (HDRP `OutlinesWorldUIPass` fullscreen pass)
   The full RGBA you choose flows through to the shader. Both inner and outer alpha sliders are honored.
2. **Lot fill pattern** (translucent tile fill under a hovered/placing building)
   The RGB you choose is applied via `Game.Prefabs.RenderingSettingsData.m_HoveredColor` / `m_OwnerColor`.
   Note: the game's `BuildingLotRenderJob` force-overrides this surface's alpha to 0.25 — that's intentional from
   Colossal/IFS and not something this mod fights.

So the inner/outer alpha sliders visibly change the outline halo, while the RGB changes also tint the lot pattern.

## API priority

Wherever possible this mod uses, in order:
1. **CO (Colossal) API** — `Game.*`, `Colossal.*`, ECS components like `RenderingSettingsData`
2. **Unity API** — HDRP `CustomPassVolume` + `OutlinesWorldUIPass` material properties
3. **System .NET** — only where neither of the above fits
4. **Custom code** — last resort

## Building

Requires the [CS2 Modding Toolchain](https://cs2.paradoxwikis.com/Modding) (`CSII_TOOLPATH` env var set).
Open `MagicHighlights.sln` in Visual Studio 2026 (or `dotnet build` from CLI). The MSBuild targets handle:
- The C# build into `${CSII_USERDATAPATH}\Mods\MagicHighlights\MagicHighlights.dll`
- `npm ci` (if `package-lock.json` changed) + `npm run build` for the UI bundle, writing directly into the same Mods folder
- `Scripts/Update-PublishConfig.ps1` syncs `<ModVersion>` and `mod.json` `"version"` from the csproj `<Version>` on every build

## License

MIT — see [LICENSE](LICENSE).
