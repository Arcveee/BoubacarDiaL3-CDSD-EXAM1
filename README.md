# Brasil Burger - Système de Gestion Restaurant

## 🍔 Description

Brasil Burger est une application web de gestion de restaurant développée avec Symfony 8, conçue pour gérer les commandes, produits, zones de livraison et statistiques d'un restaurant de burgers.

## ✨ Fonctionnalités

### 🔐 Authentification & Sécurité
- Connexion obligatoire avec email + mot de passe
- Déconnexion sécurisée
- Accès restreint aux vues gestionnaire

### 📊 Dashboard
- Résumé du jour (commandes par état, recettes)
- Top produits vendus avec graphiques
- Alertes pour commandes en retard
- Actions rapides

### 🍔 Gestion des Produits
- CRUD complet (Burgers, Menus, Compléments)
- Gestion des images
- Archivage des produits
- Association menus ↔ burgers

### 📦 Gestion des Commandes
- Liste avec filtres avancés
- Workflow d'états (EN_COURS → VALIDÉE → PRÉPARÉE → TERMINÉE)
- Gestion des paiements (WAVE/OM/Espèces)
- Annulation de commandes

### 🗺️ Gestion des Zones de Livraison
- Création/modification de zones
- Prix de livraison par zone
- Association quartiers ↔ zones

### 📈 Statistiques & Rapports
- Recettes par période (jour/semaine/mois)
- Statistiques par zone
- Top produits vendus
- Taux d'annulation

## 🛠️ Technologies

- **Backend**: Symfony 8.0
- **Base de données**: PostgreSQL (Neon)
- **ORM**: Doctrine
- **Frontend**: Bootstrap 5 + Font Awesome
- **Sécurité**: Symfony Security Bundle

## 🎨 Design System

### Couleurs Brasil Burger
- Rouge principal: `#E74C3C`
- Orange: `#F39C12`
- Vert: `#27AE60`
- Bleu foncé: `#2C3E50`

### Composants
- Cards avec ombres
- Badges colorés par type
- Boutons avec animations
- Alertes personnalisées

## 📋 Installation

### Prérequis
- PHP 8.4+
- Composer
- PostgreSQL
- Compte Neon (ou autre PostgreSQL)

### Étapes d'installation

1. **Cloner le projet** (déjà fait)
```bash
cd "c:\Users\drago\OneDrive\Documents\EXAM CODE\Symfony\GestionRestau"
```

2. **Installer les dépendances**
```bash
composer install
```

3. **Configurer la base de données**
Modifier le fichier `.env` avec vos informations Neon :
```env
DATABASE_URL="postgresql://username:password@host:5432/database_name?serverVersion=15&charset=utf8"
```

4. **Créer les migrations et la base**
```bash
php bin/console doctrine:migrations:diff
php bin/console doctrine:migrations:migrate
```

5. **Charger les données de test** (optionnel)
```bash
php bin/console doctrine:fixtures:load
```

6. **Créer un utilisateur admin**
```bash
php bin/console app:create-admin admin@brasilburger.com motdepasse Admin Brasil
```

7. **Démarrer le serveur**
```bash
symfony server:start
```

## 🚀 Utilisation

### Connexion
- URL: `http://localhost:8000/login`
- Email: `admin@brasilburger.com`
- Mot de passe: `motdepasse` (ou celui défini)

### Navigation
- **Dashboard**: Vue d'ensemble des activités
- **Produits**: Gestion des burgers, menus, compléments
- **Commandes**: Suivi et gestion des commandes
- **Zones**: Configuration des zones de livraison
- **Statistiques**: Rapports et analyses

## 📁 Structure du Projet

```
src/
├── Controller/          # Contrôleurs MVC
├── Entity/             # Entités Doctrine
├── Repository/         # Repositories
├── Command/            # Commandes console
└── DataFixtures/       # Données de test

templates/
├── base.html.twig      # Template principal
├── dashboard/          # Templates dashboard
├── produit/           # Templates produits
├── commande/          # Templates commandes
├── zone/              # Templates zones
├── statistiques/      # Templates statistiques
└── security/          # Templates authentification

public/
└── css/
    └── site.css       # CSS personnalisé Brasil Burger
```

## 🔧 Configuration Base de Données

### Entités principales
- **User**: Utilisateurs/gestionnaires
- **Produit**: Burgers, menus, compléments
- **Zone**: Zones de livraison
- **Commande**: Commandes clients
- **CommandeItem**: Items de commande

### Relations
- Commande ↔ Zone (ManyToOne)
- Commande ↔ CommandeItem (OneToMany)
- CommandeItem ↔ Produit (ManyToOne)
- Produit ↔ Produit (ManyToMany pour menus/burgers)

## 🎯 Workflow des Commandes

1. **EN_COURS**: Commande reçue
2. **VALIDÉE**: Commande confirmée
3. **PRÉPARÉE**: Commande prête
4. **TERMINÉE**: Commande livrée/récupérée
5. **ANNULÉE**: Commande annulée

## 💳 Modes de Paiement

- **WAVE**: Paiement mobile Wave
- **OM**: Orange Money
- **ESPECES**: Paiement en espèces

## 📱 Responsive Design

L'interface est entièrement responsive et s'adapte aux :
- Ordinateurs de bureau
- Tablettes
- Smartphones

## 🔒 Sécurité

- Authentification obligatoire
- Protection CSRF
- Hashage des mots de passe
- Validation des données
- Contrôle d'accès par rôles

## 🚀 Prochaines Fonctionnalités

- [ ] Export Excel/PDF des statistiques
- [ ] Notifications en temps réel
- [ ] API REST pour application mobile
- [ ] Gestion des stocks
- [ ] Programme de fidélité
- [ ] Intégration paiements en ligne

## 📞 Support

Pour toute question ou problème :
1. Vérifier la configuration de la base de données
2. S'assurer que toutes les dépendances sont installées
3. Vérifier les logs dans `var/log/`

## 🎉 Félicitations !

Votre système de gestion Brasil Burger est maintenant prêt à l'emploi ! 🍔✨