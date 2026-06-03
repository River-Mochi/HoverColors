// File: Localization/LocaleES.cs
// Purpose: Spanish (es-ES) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("es-ES", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/es-ES.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleES : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleES(HoverColorsSettings settings)
        {
            m_Settings = settings;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            string title = Mod.ModName;
            if (!string.IsNullOrEmpty(Mod.ModVersion))
            {
                title += " (" + Mod.ModVersion + ")";
            }

            return new Dictionary<string, string>
            {
                // Mod title in the left rail of the Options menu.
                { m_Settings.GetSettingsLocaleID(), title },

                // Tabs
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Acciones" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Acerca de" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Comportamiento del color de herramientas" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Atajos de teclado" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guías" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedicatoria" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Demolición + carreteras" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controla los colores temporales del contorno cuando el bulldozer o las herramientas de carretera están activos.\n\n" +
                    "**1. Recomendado** usa el color de advertencia del juego para demoler y un azul vanilla más suave para carreteras.\n" +
                    "**2. Colores vanilla de herramienta** restaura el azul vanilla normal del juego mientras esas herramientas están activas.\n" +
                    "**3. Mantener mi color personalizado** usa el color elegido en todas partes.\n\n" +
                    "Esto no sobrescribe el color personalizado guardado automáticamente en el selector de color.\n"+
                    "Algunos jugadores encuentran difícil ver su color personalizado al demoler y querían que los contornos fuertes volvieran automáticamente durante el uso de herramientas."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Colores vanilla de herramienta" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Mantener mi color personalizado" },
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Activar contorno de objetos superpuestos" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "Activa el comportamiento vanilla normal al colocar objetos que se superponen con otros.\n" +
                    "Usa el contorno de error del juego (color salmón) cuando intentas superponer.\n\n" +
                    "Funciona con todos los modos de Bulldozer + carreteras y no sobrescribe tu color personalizado guardado."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Panel más oscuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "<Panel oscuro>: pensado para jugadores con Legacy UI; también sirve en la UI moderna si prefieres algo más oscuro.\n" +
                    "<Panel estándar>: estilo translúcido propio de Hover Colors.\n" +
                    "Aspecto más claro y moderno.\n" +
                    "Recomendado para la mayoría de jugadores con la nueva UI moderna del juego.\n" +
                    "Prueba ambos y quédate con el que prefieras. Solo cambia el fondo de este panel del mod, no la UI del juego."
                },
                
                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidad de guías (alfa)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Ajusta las guías del juego (las flechas/líneas de color al colocar carreteras, props, etc.)\n\n" +
                    "**100%** mantiene el aspecto vanilla predeterminado.\n" +
                    "**Más bajo** hace que las guías sean más transparentes.\n" +
                    "**0%** las oculta por completo - <No recomendado>.\n" +           
                    "Se recomienda mantenerse por encima de 15%, o será difícil ver qué está pasando.\n" +
                    "El mismo control está en el panel del mod dentro de la ciudad. Ambos están sincronizados;\n" +
                    "si cambias este, el de la ciudad también cambia cómodamente."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/cerrar panel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Atajo de teclado para abrir / cerrar el panel Hover Colors dentro de la ciudad." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar panel de Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Activar/desactivar vistas previas de Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Atajo de teclado para ocultar o restaurar las líneas de límite activas de la herramienta Surface al colocar superficies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Activar/desactivar líneas de Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar presets 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Atajo de teclado para cambiar entre el slot de preset 1 y el slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Alternar entre presets 1 y 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Versión" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Abre la página de Paradox Mods del autor." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "En memoria de Mochi, con cariño."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Este mod está dedicado a Mochi. Fue una perrita muy querida, adoptada a los 7 años,\n" +
                    "y dio 13 años de amor y alegría. Este mod no habría sido posible sin Mochi."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
