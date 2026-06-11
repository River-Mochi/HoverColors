# File: HoverColors/Scripts/PatchGuidelineDashedAlphaSync.ps1
# Purpose: Make guideline reset toggle S1/S2/S3 + dashed opacity, and sync dashed swatch alpha with guideline opacity.
# Run from repository root:
#   powershell -NoProfile -ExecutionPolicy Bypass -File .\HoverColors\Scripts\PatchGuidelineDashedAlphaSync.ps1

$ErrorActionPreference = "Stop"

function Read-Utf8([string]$Path) {
    if (-not (Test-Path $Path)) {
        throw "Missing file: $Path"
    }

    return [System.IO.File]::ReadAllText($Path, [System.Text.UTF8Encoding]::new($false))
}

function Write-Utf8([string]$Path, [string]$Text) {
    [System.IO.File]::WriteAllText($Path, $Text, [System.Text.UTF8Encoding]::new($false))
}

function Replace-Required([string]$Text, [string]$Pattern, [string]$Replacement, [string]$Label) {
    $newText = [System.Text.RegularExpressions.Regex]::Replace(
        $Text,
        $Pattern,
        $Replacement,
        [System.Text.RegularExpressions.RegexOptions]::Singleline)

    if ($newText -eq $Text) {
        throw "Could not patch: $Label"
    }

    return $newText
}

function Insert-After-Pattern([string]$Text, [string]$Pattern, [string]$Insert, [string]$AlreadyPattern, [string]$Label) {
    if ($Text -match $AlreadyPattern) {
        return $Text
    }

    $replacement = ('$0' + $Insert)
    return Replace-Required $Text $Pattern $replacement $Label
}

$settingPath = "HoverColors/Settings/Setting.cs"
$helpersPath = "HoverColors/Systems/HoverColorsUISystem.Helpers.cs"
$triggersPath = "HoverColors/Systems/HoverColorsUISystem.Triggers.cs"
$panelPath = "HoverColors/UI/src/MochiColorPickerPanel.tsx"
$rowsPath = "HoverColors/UI/src/panel/MochiPanelControlRows.tsx"

# ---------------------------------------------------------------------------
# Setting.cs
# ---------------------------------------------------------------------------

$setting = Read-Utf8 $settingPath

$setting = $setting.Replace(
    "// Guidelines icon toggle backup. First click saves S1/S2 + dashed color and applies vanilla;`r`n        // next click restores those colors. Opacity stays independent.",
    "// Guidelines icon toggle backup. First click saves S1/S2/S3 + opacity and applies vanilla;`r`n        // next click restores those values."
)
$setting = $setting.Replace(
    "// Guidelines icon toggle backup. First click saves S1/S2 + dashed color and applies vanilla;`n        // next click restores those colors. Opacity stays independent.",
    "// Guidelines icon toggle backup. First click saves S1/S2/S3 + opacity and applies vanilla;`n        // next click restores those values."
)

$backupOpacityBlock = @"

        [SettingsUIHidden]
        public int GuidelineBackupOpacityPercent { get; set; }
"@

$setting = Insert-After-Pattern `
    $setting `
    "        public float GuidelineBackupDashedB \{ get; set; \}\r?\n" `
    $backupOpacityBlock `
    "GuidelineBackupOpacityPercent" `
    "Setting.cs GuidelineBackupOpacityPercent"

Write-Utf8 $settingPath $setting
Write-Host "Patched Setting.cs"

# ---------------------------------------------------------------------------
# HoverColorsUISystem.Helpers.cs
# ---------------------------------------------------------------------------

$helpers = Read-Utf8 $helpersPath

$helpers = Insert-After-Pattern `
    $helpers `
    "            settings\.GuidelineBackupDashedB = settings\.GuidelineDashedB;\r?\n" `
    "            settings.GuidelineBackupOpacityPercent = settings.GuidelineOpacityPercent;`n" `
    "GuidelineBackupOpacityPercent = settings\.GuidelineOpacityPercent" `
    "SaveGuidelineToggleBackup opacity"

$helpers = Insert-After-Pattern `
    $helpers `
    "            settings\.GuidelineDashedB = settings\.GuidelineBackupDashedB;\r?\n" `
    "            settings.GuidelineOpacityPercent = settings.GuidelineBackupOpacityPercent;`n" `
    "GuidelineOpacityPercent = settings\.GuidelineBackupOpacityPercent" `
    "RestoreGuidelineToggleBackup opacity"

$helpers = Insert-After-Pattern `
    $helpers `
    "            settings\.GuidelineDashedB = dashed\.b;\r?\n" `
    "            settings.GuidelineOpacityPercent = 100;`n" `
    "GuidelineOpacityPercent = 100" `
    "ApplyVanillaGuidelineSwatches opacity"

Write-Utf8 $helpersPath $helpers
Write-Host "Patched HoverColorsUISystem.Helpers.cs"

# ---------------------------------------------------------------------------
# HoverColorsUISystem.Triggers.cs
# ---------------------------------------------------------------------------

$triggers = Read-Utf8 $triggersPath

$triggers = $triggers.Replace(
    'AddBinding(new TriggerBinding<float, float, float>(Mod.ModId, "SetGuidelineDashedColor", SetGuidelineDashedColor));',
    'AddBinding(new TriggerBinding<float, float, float, float>(Mod.ModId, "SetGuidelineDashedColor", SetGuidelineDashedColor));'
)

$newDashedMethod = @'
        private void SetGuidelineDashedColor(float r, float g, float b, float a)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            r = Clamp01(r);
            g = Clamp01(g);
            b = Clamp01(b);
            a = Clamp01(a);
            int percent = Math.Max(0, Math.Min(100, (int)(Math.Round(a * 20f) * 5f)));

            bool changed = settings.GuidelineDashedColorPreset != HoverColorsSettings.kGuidelineDashedColorPresetCustom
                || settings.GuidelineVanillaToggleActive
                || !ApproxEqual(settings.GuidelineDashedR, r)
                || !ApproxEqual(settings.GuidelineDashedG, g)
                || !ApproxEqual(settings.GuidelineDashedB, b)
                || settings.GuidelineOpacityPercent != percent;

            if (!changed)
            {
                return;
            }

            // Dashed swatch alpha and the guideline opacity slider represent the same value.
            settings.GuidelineDashedColorPreset = HoverColorsSettings.kGuidelineDashedColorPresetCustom;
            settings.GuidelineDashedR = r;
            settings.GuidelineDashedG = g;
            settings.GuidelineDashedB = b;
            settings.GuidelineOpacityPercent = percent;
            settings.GuidelineVanillaToggleActive = false;
            ApplySaveAndSync(settings);
        }

'@

$triggers = Replace-Required `
    $triggers `
    "        private void SetGuidelineDashedColor\(float r, float g, float b(?:, float a)?\).*?\r?\n        private void SetDistrictColor" `
    ($newDashedMethod + "        private void SetDistrictColor") `
    "SetGuidelineDashedColor four-arg method"

Write-Utf8 $triggersPath $triggers
Write-Host "Patched HoverColorsUISystem.Triggers.cs"

# ---------------------------------------------------------------------------
# MochiColorPickerPanel.tsx
# ---------------------------------------------------------------------------

$panel = Read-Utf8 $panelPath

$oldDashedBlock = @'
    const boundGuidelineDashedColor: Color = {
        r: useValue(guidelineDashedColorR$),
        g: useValue(guidelineDashedColorG$),
        b: useValue(guidelineDashedColorB$),
        a: 1,
    };

    const boundFillA = useValue(fillA$);
    const boundGuideline = useValue(guidelineOpacity$);
'@

$newDashedBlock = @'
    const boundFillA = useValue(fillA$);
    const boundGuideline = useValue(guidelineOpacity$);
    const boundGuidelineDashedColor: Color = {
        r: useValue(guidelineDashedColorR$),
        g: useValue(guidelineDashedColorG$),
        b: useValue(guidelineDashedColorB$),
        a: Math.max(0, Math.min(1, boundGuideline / 100)),
    };
'@

if ($panel.Contains($oldDashedBlock)) {
    $panel = $panel.Replace($oldDashedBlock, $newDashedBlock)
} elseif ($panel -notmatch "a: Math\.max\(0, Math\.min\(1, boundGuideline / 100\)\)") {
    throw "Could not patch MochiColorPickerPanel.tsx dashed binding block"
}

$panel = $panel.Replace(
    'React.useEffect(() => { setGuidelineDashedColor(boundGuidelineDashedColor); }, [boundGuidelineDashedColor.r, boundGuidelineDashedColor.g, boundGuidelineDashedColor.b]);',
    'React.useEffect(() => { setGuidelineDashedColor(boundGuidelineDashedColor); }, [boundGuidelineDashedColor.r, boundGuidelineDashedColor.g, boundGuidelineDashedColor.b, boundGuidelineDashedColor.a]);'
)

$oldDashedHandler = @'
    const handleGuidelineDashedColorChange = (value: Color) => {
        const syncedValue = normalizeColorFieldValue(value);
        const colorOnlyValue = { ...syncedValue, a: 1 };
        setGuidelineDashedColor(colorOnlyValue);
        trigger(CHANNEL, "SetGuidelineDashedColor", colorOnlyValue.r, colorOnlyValue.g, colorOnlyValue.b);
    };
'@

$newDashedHandler = @'
    const handleGuidelineDashedColorChange = (value: Color) => {
        const syncedValue = normalizeColorFieldValue(value);
        const alpha = typeof syncedValue.a === "number" ? syncedValue.a : 1;
        const percent = Math.max(0, Math.min(100, Math.round((alpha * 100) / 5) * 5));
        const dashedValue = { ...syncedValue, a: percent / 100 };
        setGuidelineDashedColor(dashedValue);
        setGuidelineOpacity(percent);
        trigger(CHANNEL, "SetGuidelineDashedColor", dashedValue.r, dashedValue.g, dashedValue.b, dashedValue.a);
    };
'@

if ($panel.Contains($oldDashedHandler)) {
    $panel = $panel.Replace($oldDashedHandler, $newDashedHandler)
} elseif ($panel -notmatch 'trigger\(CHANNEL, "SetGuidelineDashedColor", dashedValue\.r, dashedValue\.g, dashedValue\.b, dashedValue\.a\)') {
    throw "Could not patch MochiColorPickerPanel.tsx dashed change handler"
}

$oldGuidelineSliderHandler = @'
    const handleGuidelineChange = (v: number) => {
        const value = Math.max(0, Math.min(100, Math.round(v / 5) * 5));
        setGuidelineOpacity(value);
        trigger(CHANNEL, "SetGuidelineOpacity", value);
    };
'@

$newGuidelineSliderHandler = @'
    const handleGuidelineChange = (v: number) => {
        const value = Math.max(0, Math.min(100, Math.round(v / 5) * 5));
        setGuidelineOpacity(value);
        setGuidelineDashedColor(prev => ({ ...prev, a: value / 100 }));
        trigger(CHANNEL, "SetGuidelineOpacity", value);
    };
'@

if ($panel.Contains($oldGuidelineSliderHandler)) {
    $panel = $panel.Replace($oldGuidelineSliderHandler, $newGuidelineSliderHandler)
} elseif ($panel -notmatch 'setGuidelineDashedColor\(prev => \(\{ \.\.\.prev, a: value / 100 \}\)\)') {
    throw "Could not patch MochiColorPickerPanel.tsx guideline slider handler"
}

Write-Utf8 $panelPath $panel
Write-Host "Patched MochiColorPickerPanel.tsx"

# ---------------------------------------------------------------------------
# MochiPanelControlRows.tsx
# ---------------------------------------------------------------------------

$rows = Read-Utf8 $rowsPath

$rows = $rows.Replace(
    "{/* Dashed guideline color only; opacity stays on the slider. */}",
    "{/* Dashed alpha syncs with the guideline opacity slider. */}"
)

$rows = $rows.Replace(
    "                                    value={guidelineDashedColor}`r`n                                    alpha={false}",
    "                                    value={guidelineDashedColor}`r`n                                    alpha={true}"
)
$rows = $rows.Replace(
    "                                    value={guidelineDashedColor}`n                                    alpha={false}",
    "                                    value={guidelineDashedColor}`n                                    alpha={true}"
)

if ($rows -match "value=\{guidelineDashedColor\}\s+alpha=\{false\}") {
    throw "Could not change dashed ColorField alpha to true"
}

Write-Utf8 $rowsPath $rows
Write-Host "Patched MochiPanelControlRows.tsx"

Write-Host "Done. Build C# and run npm run build."
