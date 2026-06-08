// File: Localization/LocaleES.cs
// Purpose: Spanish (es-ES) strings for the Options Menu.
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.Actions), "Actions" },
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "Acerca de" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Comportamiento del color de herramienta" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Panel), "Panel" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Atajos de teclado" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Guías" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dedicatoria" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + carreteras" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Controla los colores temporales del contorno mientras están activos el bulldozer o las herramientas de carretera.\n\n**1. Recomendado** usa el color de advertencia del juego (amarillo) para demolición y un azul vanilla más suave para carreteras.\n**2. Colores vanilla de herramientas** restaura el azul vanilla normal del juego mientras están activos el bulldozer o las herramientas de carretera.\n**3. Mantener mi color personalizado** usa tu color elegido en todas partes.\n\nObjetivo: algunos usuarios/testers encuentran difícil ver su color personalizado al demoler.\nEsto ofrece opciones de colores de alta visibilidad durante el uso de herramientas.\nNo sobrescribe tu color personalizado guardado automáticamente en el selector de color." },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recomendado" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Colores vanilla de herramientas" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Mantener mi color personalizado" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Activar contorno de elementos superpuestos" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "<Se recomienda activarlo>\nMantiene visible el contorno vanilla rojo salmón del juego cuando la colocación de objetos o redes queda bloqueada por elementos superpuestos.\nLos límites de área, como las guías de radio de granjas de Industria especializada, no se modifican.\n\nFunciona con todos los modos Bulldozer + carreteras y no sobrescribe tu color personalizado guardado." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Permitir colores personalizados para NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "<Se recomienda activarlo>\nUsa tu color/transparencia HC guardado al colocar elementos de detalle NetLane como vallas, setos, marcas y herramientas similares basadas en carriles.\n\n- Las carreteras normales siguen usando el ajuste Bulldozer + carreteras elegido en la lista desplegable.\n- Desactívalo si quieres que esas herramientas usen el azul vanilla del juego.\n- El color de error por superposición sigue teniendo prioridad cuando está activado (color de error vanilla = rojo salmón)." },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Panel más oscuro" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Activado usa <Panel oscuro>: pensado para jugadores con la UI heredada; también puede usarse en la UI moderna si prefieres un panel más oscuro.\nDesactivado usa <Panel estándar>: estilo translúcido personalizado de Hover Colors.\n- Aspecto más claro y moderno.\n- Mejor para la mayoría de jugadores que usan la nueva UI moderna del juego.\n\nPrueba ambos y quédate con el que prefieras. Esto solo cambia el fondo de este panel del mod, no la UI del juego." },

                // Dashed alignment guide color
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Color de líneas guía discontinuas" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineDashedColorPreset)), "Define el color de las guías de alineación discontinuas usadas para ángulos de carretera, ayudas de 90 grados e indicaciones de conexión.\n\nLos dos controles de opacidad están sincronizados: este control de Opciones y el control del panel en ciudad ajustan la misma opacidad de las guías discontinuas." },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Vanilla"), "Blanco vanilla" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Green"), "Verde de alta visibilidad" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("MochiBlue"), "Mochi Blue" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("CyanBlue"), "Azul cian" },
                { m_Settings.GetGuidelineDashedColorPresetLocaleID("Yellow"), "Amarillo de alta visibilidad" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacidad de guías (alfa)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Controla la opacidad de las guías de alineación discontinuas, útil al colocar carreteras, vallas, props, etc.\n\n**100%** mantiene el aspecto vanilla predeterminado.\n**Más bajo** hace las guías más transparentes.\n**0%** las oculta por completo - <No recomendado>.\nSe recomienda mantenerse por encima de 15%, o será difícil ver qué ocurre.\nEl mismo control está en el panel del mod en la ciudad. Ambos están sincronizados;\nsi cambias este, el de la ciudad cambia también." },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Abrir/cerrar panel principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Atajo de teclado para abrir / cerrar el panel de color de objetos resaltados en la ciudad." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Alternar panel Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Activar/desactivar vistas previas de la herramienta Superficie" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Atajo de teclado para ocultar o restaurar las líneas de límite activas de la herramienta Superficie al colocar superficies." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Capa de vista previa de Superficie On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Alternar preajustes 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Atajo de teclado para cambiar entre el espacio de preajuste 1 y el espacio 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Alternar entre preajustes 1 y 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Abre la página de Paradox Mods del autor." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "En memoria cariñosa de Mochi." },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)), "Este mod está dedicado a Mochi. Fue una perrita muy querida, adoptada a los 7 años,\ny dio 13 años de amor y alegría. Este mod no habría sido posible sin Mochi." },
            };
        }

        public void Unload()
        {
        }
    }
}
