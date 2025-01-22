# Gestionnaire de Stock RFID

Ce projet est une application logicielle développée en C# utilisant le framework .NET. Elle permet de gérer les stocks en intégrant les données des étiquettes RFID détectées par un matériel spécifique dans une base de données. L'application est conçue pour fonctionner de manière efficace et simultanée grâce à l'utilisation de threading.

## Fonctionnalités

- **Détection RFID** : Le logiciel interagit avec des étiquettes RFID via des commandes spécifiques, qui sont des séquences de nombres hexadécimaux. Chaque séquence a une signification particulière, documentée par le fournisseur du matériel RFID.
  
- **Gestion des Stocks** : Les données extraites des étiquettes RFID sont intégrées dans la base de données du client, permettant une gestion efficace des stocks.

- **Threading** : L'application utilise le threading pour gérer simultanément plusieurs étiquettes RFID détectées, assurant ainsi une performance optimale.

- **Personnalisation des Requêtes SQL** : Les noms des tableaux et des attributs dans les requêtes SQL ont été modifiés pour s'adapter aux besoins spécifiques du client, garantissant une intégration fluide et personnalisée des données.

## Prérequis

- .NET Framework
- Accès à la documentation du fournisseur du matériel RFID pour comprendre les séquences de commandes hexadécimales.
- Serveur de base de données SQL pour l'intégration des données.

## Installation

1. **Cloner le dépôt** : 
   ```bash
   git clone https://github.com/Badr04-dev/RFID-Project.git
   ```

2. **Ouvrir le projet** : Utilisez Visual Studio ou tout autre IDE compatible avec C# et .NET pour ouvrir le projet.

3. **Configurer la base de données** : Assurez-vous que votre serveur SQL est configuré et que les noms des tableaux et des attributs correspondent à ceux définis dans le code.

4. **Compiler et exécuter** : Compilez le projet et exécutez l'application pour commencer à gérer les stocks via RFID.
