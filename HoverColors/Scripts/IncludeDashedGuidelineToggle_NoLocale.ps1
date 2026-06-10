$ErrorActionPreference = "Stop"

$repoRoot = (Resolve-Path ".").Path
$settingsPath = Join-Path $repoRoot "HoverColors/Settings/Setting.cs"
$helpersPath = Join-Path $repoRoot "HoverColors/Systems/HoverColorsUISystem.Helpers.cs"
$triggersPath = Join-Path $repoRoot "HoverColors/Systems/HoverColorsUISystem.Triggers.cs"

function Write-Utf8NoBom($Path, $Text) {
    $utf8 = New-Object System.Text.UTF8Encoding($false)
    [System.IO.File]::WriteAllText($Path, $Text, $utf8)
}

function Replace-CSharpMethod {
    param(
        [string]$Text,
        [string]$SignaturePattern,
        [string]$Replacement,
        [string]$Label
    )

    $match = [regex]::Match($Text, $SignaturePattern, [System.Text.RegularExpressions.RegexOptions]::Singleline)
    if (-not $match.Success) {
        throw "Could not find method: $Label"
    }

    $start = $match.Index
    $braceIndex = $Text.IndexOf('{', $match.Index)
    if ($braceIndex -lt 0) {
        throw "Could not find opening brace for: $Label"
    }

    $depth = 0
    $end = -1
    for ($i = $braceIndex; $i -lt $Text.Length; $i++) {
        $ch = $Text[$i]
        if ($ch -eq '{') {
            $depth++
        }
        elseif ($ch -eq '}') {
            $depth--
            if ($depth -eq 0) {
                $end = $i + 1
                break
            }
        }
    }

    if ($end -lt 0) {
        throw "Could not find closing brace for: $Label"
    }

    return $Text.Substring(0, $start) + $Replacement.TrimEnd() + $Text.Substring($end)
}

# -----------------------------------------------------------------------------
# Setting.cs: add hidden backup fields for dashed guide color.
# -----------------------------------------------------------------------------
$settings = Get-Content -Raw -Encoding UTF8 $settingsPath

$settings = $settings -replace 'Guidelines icon toggle backup\. First click saves S1/S2 here and applies vanilla;\r?\n\s*// next click restores these values\. Dashed guide color/opacity are separate\.', 'Guidelines icon toggle backup. First click saves S1/S2/dashed color and applies vanilla;`r`n        // next click restores these values. Opacity stays separate.'

if ($settings -notmatch 'GuidelineBackupDashedColorPreset') {
    $anchor = @'
        [SettingsUIHidden]
        public float GuidelineBackupPreviewA { get; set; }
'@

    $insert = @'
        [SettingsUIHidden]
        public float GuidelineBackupPreviewA { get; set; }

        [SettingsUIHidden]
        public int GuidelineBackupDashedColorPreset { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupDashedR { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupDashedG { get; set; }

        [SettingsUIHidden]
        public float GuidelineBackupDashedB { get; set; }
'@

    if (-not $settings.Contains($anchor)) {
        throw "Could not find Setting.cs anchor: GuidelineBackupPreviewA"
    }

    $settings = $settings.Replace($anchor, $insert)
    Write-Host "Patched Setting.cs backup fields"
}
else {
    Write-Host "Setting.cs backup fields already present"
}

Write-Utf8NoBom $settingsPath $settings

# -----------------------------------------------------------------------------
# HoverColorsUISystem.Helpers.cs: include dashed color in save/restore/vanilla.
# -----------------------------------------------------------------------------
$helpers = Get-Content -Raw -Encoding UTF8 $helpersPath

$saveMethod = @'
        private static void SaveGuidelineToggleBackup(HoverColorsSettings settings)
        {
            settings.GuidelineBackupLinesColorPreset = settings.GuidelineLinesColorPreset;
            settings.GuidelineBackupLinesR = settings.GuidelineLinesR;
            settings.GuidelineBackupLinesG = settings.GuidelineLinesG;
            settings.GuidelineBackupLinesB = settings.GuidelineLinesB;
            settings.GuidelineBackupLinesA = settings.GuidelineLinesA;
            settings.GuidelineBackupPreviewColorPreset = settings.GuidelinePreviewColorPreset;
            settings.GuidelineBackupPreviewR = settings.GuidelinePreviewR;
            settings.GuidelineBackupPreviewG = settings.GuidelinePreviewG;
            settings.GuidelineBackupPreviewB = settings.GuidelinePreviewB;
            settings.GuidelineBackupPreviewA = settings.GuidelinePreviewA;
            settings.GuidelineBackupDashedColorPreset = settings.GuidelineDashedColorPreset;
            settings.GuidelineBackupDashedR = settings.GuidelineDashedR;
            settings.GuidelineBackupDashedG = settings.GuidelineDashedG;
            settings.GuidelineBackupDashedB = settings.GuidelineDashedB;
            settings.GuidelineVanillaToggleHasBackup = true;
        }
'@

$restoreMethod = @'
        private static void RestoreGuidelineToggleBackup(HoverColorsSettings settings)
        {
            settings.GuidelineLinesColorPreset = settings.GuidelineBackupLinesColorPreset;
            settings.GuidelineLinesR = settings.GuidelineBackupLinesR;
            settings.GuidelineLinesG = settings.GuidelineBackupLinesG;
            settings.GuidelineLinesB = settings.GuidelineBackupLinesB;
            settings.GuidelineLinesA = settings.GuidelineBackupLinesA;
            settings.GuidelinePreviewColorPreset = settings.GuidelineBackupPreviewColorPreset;
            settings.GuidelinePreviewR = settings.GuidelineBackupPreviewR;
            settings.GuidelinePreviewG = settings.GuidelineBackupPreviewG;
            settings.GuidelinePreviewB = settings.GuidelineBackupPreviewB;
            settings.GuidelinePreviewA = settings.GuidelineBackupPreviewA;
            settings.GuidelineDashedColorPreset = settings.GuidelineBackupDashedColorPreset;
            settings.GuidelineDashedR = settings.GuidelineBackupDashedR;
            settings.GuidelineDashedG = settings.GuidelineBackupDashedG;
            settings.GuidelineDashedB = settings.GuidelineBackupDashedB;
        }
'@

$vanillaMethod = @'
        private static void ApplyVanillaGuidelineSwatches(HoverColorsSettings settings)
        {
            UnityEngine.Color lines = GuidelineColorSystem.CapturedVanillaGuidelineLinesColor;
            UnityEngine.Color preview = GuidelineColorSystem.CapturedVanillaGuidelinePreviewColor;
            UnityEngine.Color dashed = GuidelineColorSystem.CapturedVanillaGuidelineDashedColor;
            settings.GuidelineLinesColorPreset = HoverColorsSettings.kGuidelineColorPresetVanilla;
            settings.GuidelineLinesR = lines.r;
            settings.GuidelineLinesG = lines.g;
            settings.GuidelineLinesB = lines.b;
            settings.GuidelineLinesA = 1f;
            settings.GuidelinePreviewColorPreset = HoverColorsSettings.kGuidelineColorPresetVanilla;
            settings.GuidelinePreviewR = preview.r;
            settings.GuidelinePreviewG = preview.g;
            settings.GuidelinePreviewB = preview.b;
            settings.GuidelinePreviewA = 1f;
            settings.GuidelineDashedColorPreset = HoverColorsSettings.kGuidelineDashedColorPresetVanilla;
            settings.GuidelineDashedR = dashed.r;
            settings.GuidelineDashedG = dashed.g;
            settings.GuidelineDashedB = dashed.b;
        }
'@

$helpers = Replace-CSharpMethod $helpers '        private static void SaveGuidelineToggleBackup\s*\(\s*HoverColorsSettings settings\s*\)\s*\{' $saveMethod 'SaveGuidelineToggleBackup'
$helpers = Replace-CSharpMethod $helpers '        private static void RestoreGuidelineToggleBackup\s*\(\s*HoverColorsSettings settings\s*\)\s*\{' $restoreMethod 'RestoreGuidelineToggleBackup'
$helpers = Replace-CSharpMethod $helpers '        private static void ApplyVanillaGuidelineSwatches\s*\(\s*HoverColorsSettings settings\s*\)\s*\{' $vanillaMethod 'ApplyVanillaGuidelineSwatches'
Write-Utf8NoBom $helpersPath $helpers
Write-Host "Patched HoverColorsUISystem.Helpers.cs"

# -----------------------------------------------------------------------------
# HoverColorsUISystem.Triggers.cs: choosing dashed color exits vanilla-toggle mode.
# -----------------------------------------------------------------------------
$triggers = Get-Content -Raw -Encoding UTF8 $triggersPath

$dashedMethod = @'
        private void SetGuidelineDashedColor(float r, float g, float b)
        {
            HoverColorsSettings? settings = Mod.Settings;
            if (settings == null) return;

            r = Clamp01(r);
            g = Clamp01(g);
            b = Clamp01(b);

            bool changed = settings.GuidelineDashedColorPreset != HoverColorsSettings.kGuidelineDashedColorPresetCustom
                || settings.GuidelineVanillaToggleActive
                || !ApproxEqual(settings.GuidelineDashedR, r)
                || !ApproxEqual(settings.GuidelineDashedG, g)
                || !ApproxEqual(settings.GuidelineDashedB, b);

            if (!changed)
            {
                return;
            }

            // Dashed line opacity is controlled separately by GuidelineOpacityPercent.
            settings.GuidelineDashedColorPreset = HoverColorsSettings.kGuidelineDashedColorPresetCustom;
            settings.GuidelineDashedR = r;
            settings.GuidelineDashedG = g;
            settings.GuidelineDashedB = b;
            settings.GuidelineVanillaToggleActive = false;
            ApplySaveAndSync(settings);
        }
'@

$triggers = Replace-CSharpMethod $triggers '        private void SetGuidelineDashedColor\s*\(\s*float r,\s*float g,\s*float b\s*\)\s*\{' $dashedMethod 'SetGuidelineDashedColor'
Write-Utf8NoBom $triggersPath $triggers
Write-Host "Patched HoverColorsUISystem.Triggers.cs"

Write-Host "Done. No locale JSON files were touched."
