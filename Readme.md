# Rendu TD6

## Notre solution
Nous avons fait un client et un serveur, qui met à disposition notre service.
Le client prend en input le point de départ et d'arrivé qu'il envoie au serveur.
Il recevra l'itinéraire (format Google Map) du trajet à suivre en passant par deux stations de vélib.

## Côté serveur
Le serveur fait d'abord deux appels à l'api de Google Map, pour le point de départ et d'arrivé afin d'avoir leurs coordonées exactes.
Il fait ensuite un appel à l'api des vélibs pour avoir toutes les stations.
Ensuite il calcul pour chaque station la distance entre celle-ci et le point d'arrivé, de même pour le point de départ.
Si elle est inférieure à celle stockée, il y a un appel pour avoir les détails de cette station et ainsi savoir si il y a des vélibs de disponibles / places de libres.
Ensuite une dernière requête est faite à l'api de Google Map afin d'avoir le trajet entre le départ et l'arrivé tout en passant par les deux stations de vélibs calculées.

## Déploiement
L'archive contient deux dossiers, un pour le client et l'autre pour le service.
Les deux dossiers contiennent le fichier de projet pour Visual Studio, il suffit de lancer deux instances de Visual Studio, ouvrir le serveur et le client, et lancer les deux.

**FILIOL DE RAIMOND-MICHEL Guillaume, GONNIN Thibaut**

