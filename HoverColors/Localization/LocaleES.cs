// File: Localization/LocaleES.cs
// Purpose: Spanish (es-ES) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/es-ES.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Comportamiento del color de las herramientas" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Atajos de teclado" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Guías" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Dedicatoria" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + carreteras" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Controla los colores temporales del contorno mientras el bulldozer o las herramientas de carretera están activos.\n" +
                    "\n" +
                    "**1. Recomendado** usa el color de advertencia del juego (amarillo) para demoler y un azul vanilla más suave para carreteras.\n" +
                    "**2. Colores vanilla de herramientas** restaura el azul vanilla normal del juego mientras el bulldozer o las herramientas de carretera están activos.\n" +
                    "**3. Mantener mi color personalizado** usa tu color elegido en todas partes.\n" +
                    "\n" +
                    "Propósito: algunos usuarios/testers encuentran difícil ver su color personalizado al demoler.\n" +
                    "Esto ofrece colores de alta visibilidad durante el uso de herramientas.\n" +
                    "No sobrescribe tu color personalizado guardado automáticamente en el selector de color."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Colores vanilla de herramientas" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Mantener mi color personalizado" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Activar contorno de elementos superpuestos" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Activado recomendado>\n" +
                    "Mantiene visible el contorno vanilla rojo salmón del juego cuando la colocación de objetos o redes está bloqueada por elementos superpuestos.\n" +
                    "Los límites de área, como las guías de radio de granjas de industria especializada, no se modifican.\n" +
                    "\n" +
                    "Funciona con todos los modos Bulldozer + carreteras y no sobrescribe tu color personalizado guardado."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Permitir colores personalizados para NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Activado recomendado>\n" +
                    "Usa tu color/transparencia HC guardado al colocar detalles NetLane como vallas, setos, marcas y herramientas similares basadas en carriles.\n" +
                    "\n" +
                    "- Las carreteras normales siguen usando el ajuste Bulldozer + carreteras elegido en la lista desplegable.\n" +
                    "- Desactiva esto si quieres que esas herramientas usen el azul vanilla del juego.\n" +
                    "- El color de error por superposición sigue teniendo prioridad cuando está activado (color de error vanilla = rojo salmón)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Panel más oscuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Activado = <Panel oscuro>: pensado para jugadores con Legacy UI; también se puede usar en Modern UI si prefieres un panel más oscuro.\n" +
                    "Desactivado = <Panel estándar>: estilo translúcido personalizado de Hover Colors.\n" +
                    "- Aspecto más claro y moderno.\n" +
                    "- Mejor para la mayoría de jugadores que usan la nueva Modern UI del juego.\n" +
                    "\n" +
                    "¡Prueba ambos y elige el que prefieras! Esto solo cambia el fondo de este panel del mod, no la UI del juego."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidad de guías (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Controla la opacidad de las guías de alineación discontinuas, útil al colocar carreteras, vallas, props, etc.\n" +
                    "\n" +
                    "**100%** mantiene el aspecto vanilla predeterminado.\n" +
                    "**Más bajo** hace que las guías sean más transparentes.\n" +
                    "**0%** las oculta por completo - <No recomendado>.\n" +
                    "Se recomienda mantenerse por encima de 15% o será difícil ver qué ocurre.\n" +
                    "El mismo deslizador vive en el panel del mod en la ciudad. Ambos están sincronizados;\n" +
                    "si cambias este, el de la ciudad también cambia."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/cerrar panel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Atajo de teclado para abrir / cerrar el panel de color de objetos bajo el cursor en la ciudad." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar panel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Activar/desactivar vistas previas de la herramienta Superficie" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Atajo de teclado para ocultar o restaurar las líneas de límite activas de la herramienta Superficie al colocar superficies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Capa de vista previa de Superficie On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar presets 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Atajo de teclado para cambiar entre el espacio de preset 1 y el 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Alternar entre presets 1 y 2" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Versión" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Abrir la página de Paradox Mods del autor." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "En memoria de Mochi."
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
