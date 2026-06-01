// File: Localization/LocaleFR.cs
// Purpose: French (fr-FR) strings for the Options UI (ESC -> Options -> Hover Colors).
// Registered in Mod.OnLoad via GameManager.instance.localizationManager.AddSource("fr-FR", ...).
// Strings for the in-city cohtml panel live separately in L10n/lang/fr-FR.json.

namespace HoverColors.Localization
{
    using Colossal;
    using HoverColors.Settings;
    using System.Collections.Generic;

    public sealed class LocaleFR : IDictionarySource
    {
        private readonly HoverColorsSettings m_Settings;

        public LocaleFR(HoverColorsSettings settings)
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
                { m_Settings.GetOptionTabLocaleID(HoverColorsSettings.About), "À propos" },

                // Groups
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.ToolColors), "Comportement des couleurs d’outil" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.KeyBindings), "Raccourcis clavier" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.Guidelines), "Repères" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.AboutDedication), "Dédicace" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + routes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Contrôle les couleurs temporaires de contour quand le bulldozer ou les outils de route sont actifs.\n\n" +
                    "**1. Recommandé** utilise la couleur d’avertissement du jeu pour la démolition et un bleu vanilla plus doux pour les routes.\n" +
                    "**2. Couleurs vanilla des outils** remet le bleu vanilla normal du jeu quand ces outils sont actifs.\n" +
                    "**3. Garder ma couleur perso** utilise la couleur choisie partout.\n\n" +
                    "Cela n’écrase pas la couleur perso automatiquement enregistrée dans le sélecteur de couleur.\n"+
                    "Certains joueurs trouvent leur couleur perso difficile à voir pendant la démolition et voulaient retrouver automatiquement des contours plus visibles pendant l’utilisation des outils."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recommandé" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Couleurs vanilla des outils" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Garder ma couleur perso" },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacité des repères (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Ajuste les repères du jeu (les flèches/lignes colorées visibles en plaçant routes, props, etc.)\n\n" +
                    "**100%** garde l’apparence vanilla par défaut.\n" +
                    "**Plus bas** rend les repères plus transparents.\n" +
                    "**0%** les masque complètement - <Non recommandé>.\n" +           
                    "Il est conseillé de rester au-dessus de 15%, sinon c’est difficile à suivre.\n" +
                    "Le même curseur existe dans le panneau du mod en ville. Les deux sont synchronisés ;\n" +
                    "si celui-ci change, celui en ville change aussi automatiquement."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Ouvrir/fermer le panneau principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Raccourci clavier pour ouvrir / fermer le panneau Hover Colors en ville." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Afficher/masquer le panneau Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Activer/désactiver les aperçus Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Raccourci clavier pour masquer ou rétablir les lignes de limite actives de l’outil Surface pendant le placement des surfaces." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Activer/désactiver les lignes Surface" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Basculer les préréglages 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Raccourci clavier pour passer du slot de préréglage 1 au slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName),
                    "Basculer entre les préréglages 1 et 2" },

                // About — name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About — Paradox Mods link button (matches CityWatchdog phrasing)
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Ouvre la page Paradox Mods de l’auteur." },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "En tendre mémoire de Mochi."
                    },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.MochiDedicationText)),
                    "Ce mod est dédié à Mochi. C’était une chienne adorée, adoptée à l’âge de 7 ans,\n" +
                    "et elle a donné 13 ans d’amour et de joie. Ce mod n’aurait pas été possible sans Mochi."
                    },
            };
        }

        public void Unload()
        {
        }
    }
}
