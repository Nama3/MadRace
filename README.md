Mad Race - Madbox Home Assignment
--------------------------------------------------------------------------------------------------------------------------------------------

Durée du test :
--------------------------------------------------------------------------------------------------------------------------------------------
26/11/2020 :
- 17:00 -> 19:00
- 21:30 -> 22:00

27/11/2020 :
- 9:00 -> 11:30

Total: 5h
--------------------------------------------------------------------------------------------------------------------------------------------

Difficultés :
--------------------------------------------------------------------------------------------------------------------------------------------
Aucune tâche de cette mission m'a paru réellement difficile, mis à part la durée limitée qui implique une bonne gestion du temps et des priorités.
Pour ce qui est de la technique, tout ce que j'ai produis dans cette version n'était pas nouveau pour moi, donc je n'ai pas vraiment eu de difficultés.

Ceci dit, la gestion des bots aurait sans doute été le point le plus délicat si j'avais eu le temps de l'approfondir plus en détails, chose que j'ai déjà planifié au cas où je décide de faire une seconde version.
Autrement, l'aspect le plus complexe de ma version actuelle est probablement la génération des chemins pour les personnages.
Puisque j'ai utilisé l'asset Path Creator (https://assetstore.unity.com/packages/tools/utilities/b-zier-path-creator-136082) pour créer les levels, j'ai décidé de générer les chemins des personnages à partir du path qui permet de dessiner la track.
Ça me permet d'avoir un chemin scripté facile à suivre pour les personnages, pour plus de flexibilités je peux, si les levels devenaient plus complexes, modifier manuellement ce chemin.

Bien que le code pour la génération des chemins soit légèrement complexe, il ne m'a pas pris trop de temps à produire, puisque j'avais déjà codé quelque chose de similaire auparavant.
La démarche était donc encore assez clair dans ma tête.
--------------------------------------------------------------------------------------------------------------------------------------------

Améliorations :
--------------------------------------------------------------------------------------------------------------------------------------------
J'aurais aimé avoir le temps de faire des bots qui s'arrêtent devant les obstacles et tentent de passer au bon moment.
J'avais une idée assez précise de comment m'y prendre, mais c'était quelque chose d'assez chronophage et je n'ai donc pas eu le temps de le faire.
J'ai donc préféré prendre un peu de temps pour avoir un rendu visuel agréable, bien que ma version soit loin d'être suffisante graphiquement, je pense qu'elle est correcte pour le temps imparti.
Les quelques éléments graphiques (shader d'eau, skybox, personnage animé) que j'ai mis en place me sont assez familiés puisque je les ai déjà utilisés dans d'autres projets. 

J'aurais également aimé avoir le temps de créer d'autres types d'obstacles, d'autant que la base que j'ai créé pour les obstacles est assez facilement extensible.
Finalement, un simple écran de fin de niveau aurait pu être intéressant à mettre en place, puisqu'actuellement il y a très peu de feel quand on en termine un.

Comme précisé au dessus, si j'avais plus de temps j'améliorerais les bots, j'approfondirais le level design (nouveaux obstacles et variations de la piste).
J'ajouterais aussi plus d'interface graphique pour informer le joueur de sa progression, ainsi que des feedbacks visuels pour améliorer le game feel.
Je ferais également un système de checkpoints dans les niveaux pour ne pas avoir à tout recommencer en cas d'échec. 
Bien entendu, si j'avais la chance de travailler avec un(e) graphiste, je mettrais en place un système d'achat de skins avec une currency remportée dans les courses (et pourquoi pas le système de danse de victoire comme dans Fun Race 3D).
--------------------------------------------------------------------------------------------------------------------------------------------

Opinion :
--------------------------------------------------------------------------------------------------------------------------------------------
Ce test était très intéressant, quelque peu stressant, c'est toujours un challenge d'effectuer quelque chose de convaincant en une durée très limitée (bien que j'ai connu pire que 5h).
Le concept a recréer est relativement simple techniquement, les mouvements étant très scryptés, ça laisse assez peu de hasard au gameplay.
De ce fait, il est possible de faire des choses très variées sur le level design, ça rend le développement du jeu d'autant plus ludique.
--------------------------------------------------------------------------------------------------------------------------------------------

Commentaires :
--------------------------------------------------------------------------------------------------------------------------------------------
Vous remarquerez peut-être que mon projet est quelque peu architecturé.
Je n'ai pas vraiment pris de temps en plus pour faire ça, car j'ai travaillé sur un petit projet avec une structure de gameplay assez similaire récemment.
J'ai donc repris la base de cet autre projet (surtout en ce qui concerne la gestion des niveaux, le chargement de ceux-ci), pour pouvoir travailler sur une base relativement solide.

J'ai également implémenté dans ce projet des outils que j'utilises régulièrement, notamment le système de sauvegarde (via mon script SavedDataServices et mon plugin Encryption Tool).
J'aurais très bien pu faire sans puisque je n'ai pas vraiment exploité ces outils, mais j'ai préféré les intégrer directement, dans l'évantualité où je prolongerais le projet après la durée du test, pour rendre les intérations plus simple (et surtout ne pas avoir à faire marche arrière).

J'ai aussi ajouté au projet mon package Mighty Attributes, qui est une collection de différents attributs C# plutôt pratiques pour faire des inspectors plus intéressants que ceux par défaut de Unity.
Là aussi je me suis assez peu servis de ce plugin, et j'aurais pu faire sans. Il s'agit simplement d'une habitude de travail pour aller un peu plus vite.

Je pense avoir fais le tour de ce que j'avais à dire. Si d'autres choses me reviennent je vous en ferais part par email.

Merci d'avoir pris le temps de me lire.
--------------------------------------------------------------------------------------------------------------------------------------------