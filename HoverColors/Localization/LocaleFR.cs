// File: Localization/LocaleFR.cs
// Purpose: French (fr-FR) strings for the Options Menu.
// Strings for the in-city cohtml panel live separately in L10n/lang/fr-FR.json.

namespace HoverColors.Localization
{
    using System.Collections.Generic;

    using Colossal;

    using HoverColors.Settings;

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
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kToolColors), "Comportement des couleurs d’outil" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kPanel), "Panneau" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kKeyBindings), "Raccourcis clavier" },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kGuidelines), "Repères" },
                // AboutInfo + AboutLinks intentionally have empty group headers.
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutInfo), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutLinks), string.Empty },
                { m_Settings.GetOptionGroupLocaleID(HoverColorsSettings.kAboutDedication), "Dédicace" },

                // Tool color behavior
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToolColorMode)), "Bulldozer + routes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToolColorMode)),
                    "Contrôle les couleurs temporaires du contour lorsque le bulldozer ou les outils de route sont actifs.\n" +
                    "\n" +
                    "**1. Recommandé** utilise la couleur d’avertissement du jeu (jaune) pour la démolition et un bleu vanilla plus doux pour les routes.\n" +
                    "**2. Couleurs vanilla des outils** restaure le bleu vanilla normal du jeu lorsque le bulldozer ou les outils de route sont actifs.\n" +
                    "**3. Garder ma couleur personnalisée** utilise partout la couleur que vous avez choisie.\n" +
                    "\n" +
                    "Objectif : certains utilisateurs/testeurs trouvent leur couleur personnalisée difficile à voir pendant la démolition.\n" +
                    "Ces options offrent des couleurs très visibles pendant l’utilisation des outils.\n" +
                    "Cela n’écrase pas la couleur personnalisée enregistrée automatiquement dans le sélecteur de couleur."
                },
                { m_Settings.GetToolColorModeLocaleID("Recommended"), "1. Recommandé" },
                { m_Settings.GetToolColorModeLocaleID("Vanilla"), "2. Couleurs vanilla des outils" },
                { m_Settings.GetToolColorModeLocaleID("Custom"), "3. Garder ma couleur personnalisée" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)), "Activer le contour des éléments qui se chevauchent" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseOverlapWarningColor)),
                    "<Activé est recommandé>\n" +
                    "Conserve le contour vanilla saumon rouge du jeu lorsque le placement d’un objet ou d’un réseau est bloqué par des éléments qui se chevauchent.\n" +
                    "Les limites de zone, comme les guides de rayon des fermes d’industrie spécialisée, ne sont pas modifiées.\n" +
                    "\n" +
                    "Fonctionne avec tous les modes Bulldozer + routes et n’écrase pas votre couleur personnalisée enregistrée."
                },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)), "Autoriser les couleurs personnalisées pour NetLanes" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseCustomColorsForNetLanes)),
                    "<Activé est recommandé>\n" +
                    "Utilise votre couleur/transparence HC enregistrée lors du placement d’éléments de détail NetLane comme les clôtures, haies, marquages et autres outils similaires basés sur les voies.\n" +
                    "\n" +
                    "- Les routes normales suivent toujours le réglage Bulldozer + routes choisi dans la liste déroulante.\n" +
                    "- Désactivez cette option si vous voulez que ces outils utilisent le bleu vanilla du jeu à la place.\n" +
                    "- La couleur d’erreur de chevauchement reste prioritaire quand elle est activée (couleur d’erreur vanilla = saumon rouge)."
                },

                // Darker panel
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)), "Panneau plus sombre" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.UseDarkerPanel)),
                    "Activé = <Panneau sombre> : conçu pour les joueurs avec l’ancienne UI ; peut aussi être utilisé avec l’UI moderne si vous préférez un panneau plus sombre.\n" +
                    "Désactivé = <Panneau standard> : style Hover Colors translucide personnalisé.\n" +
                    "- Aspect plus clair et plus moderne.\n" +
                    "- Idéal pour la plupart des joueurs utilisant la nouvelle Modern UI du jeu.\n" +
                    "\n" +
                    "Essayez les deux et gardez celui que vous préférez ! Cela change seulement l’arrière-plan de ce panneau du mod, pas l’UI du jeu."
                },

                // Guidelines opacity slider
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)), "Opacité des repères (alpha)" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.GuidelineOpacityPercent)),
                    "Contrôle l’opacité des guides d’alignement en pointillés, utile pendant le placement de routes, clôtures, props, etc.\n" +
                    "\n" +
                    "**100%** conserve l’apparence vanilla par défaut.\n" +
                    "**Plus bas** rend les guides plus transparents.\n" +
                    "**0%** les masque complètement - <Non recommandé>.\n" +
                    "Il est conseillé de rester au-dessus de 15%, sinon il devient difficile de voir ce qui se passe.\n" +
                    "Le même curseur existe dans le panneau du mod en ville. Les deux sont synchronisés ;\n" +
                    "si vous changez celui-ci, celui en ville change aussi automatiquement."
                },

                // Keybinds
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)), "Ouvrir/fermer le panneau principal" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePanelBinding)),
                    "Raccourci clavier pour ouvrir / fermer le panneau de couleur des objets survolés en ville." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePanelActionName), "Afficher/masquer le panneau Hover Colors" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)), "Activer/désactiver les aperçus de l’outil Surface" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.ToggleSurfaceToolAreasBinding)),
                    "Raccourci clavier pour masquer ou rétablir les lignes de limite actives de l’outil Surface pendant le placement des surfaces." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kToggleSurfaceToolAreasActionName), "Couche d’aperçu Surface On/Off" },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)), "Basculer les préréglages 1+2" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.TogglePresetBinding)),
                    "Raccourci clavier pour passer du slot de préréglage 1 au slot 2." },
                { m_Settings.GetBindingKeyLocaleID(Mod.kTogglePresetActionName), "Basculer entre les préréglages 1 et 2" },

                // About name + version
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.NameText)), "Mod" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.NameText)), string.Empty },

                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.VersionText)), "Version" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.VersionText)), string.Empty },

                // About Paradox Mods link button
                { m_Settings.GetOptionLabelLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Paradox Mods" },
                { m_Settings.GetOptionDescLocaleID(nameof(HoverColorsSettings.OpenParadox)), "Ouvrir la page Paradox Mods de l’auteur." },

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
