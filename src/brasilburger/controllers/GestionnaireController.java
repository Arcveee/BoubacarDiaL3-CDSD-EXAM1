package brasilburger.controllers;

import brasilburger.models.Burger;
import brasilburger.models.Commande;
import brasilburger.models.enums.EtatCommande;
import brasilburger.repositories.BurgerRepository;
import brasilburger.services.CommandeService;
import brasilburger.services.SessionService;
import brasilburger.services.StatService;
import brasilburger.views.ConsoleUtils;

import java.time.LocalDate;
import java.util.List;
import java.util.Map;
import java.util.HashSet;
import java.util.Set;
import java.util.HashMap;

public class GestionnaireController {
    private final BurgerRepository burgerRepository;
    private final CommandeService commandeService;
    private final StatService statService;
    private final Set<Integer> choixAffiches = new HashSet<>();
    private Integer lastViewedCommandeId = null;
    private boolean lastActionWasDetails = false;
    private int lastSessionVersion = -1;

    public GestionnaireController(BurgerRepository burgerRepository, CommandeService commandeService, StatService statService) {
        this.burgerRepository = burgerRepository;
        this.commandeService = commandeService;
        this.statService = statService;
    }

    public void afficherMenuGestionnaire() {
        if (!SessionService.estGestionnaireConnecte()) {
            System.out.println("Connexion gestionnaire obligatoire.");
            return;
        }
        boolean quitter = false;
        while (!quitter && SessionService.estGestionnaireConnecte()) {
            int currentVersion = SessionService.getSessionVersion();
            if (currentVersion != lastSessionVersion) {
                choixAffiches.clear();
                lastSessionVersion = currentVersion;
            }
            System.out.println("=========== Menu Gestionnaire ==========");
            Map<Integer, Integer> affichageVersOriginal = new HashMap<>();
            int idx = 1;
            if (!choixAffiches.contains(1)) { System.out.println(idx + ". Dashboard du jour"); affichageVersOriginal.put(idx++, 1); }
            if (!choixAffiches.contains(2)) { System.out.println(idx + ". Lister burgers actifs"); affichageVersOriginal.put(idx++, 2); }
            if (!choixAffiches.contains(3)) { System.out.println(idx + ". Lister toutes les commandes"); affichageVersOriginal.put(idx++, 3); }
            if (!choixAffiches.contains(4)) { System.out.println(idx + ". Détails d'une commande"); affichageVersOriginal.put(idx++, 4); }
            if (!choixAffiches.contains(5)) { System.out.println(idx + ". Changer l'état d'une commande"); affichageVersOriginal.put(idx++, 5); }
            if (!choixAffiches.contains(6)) { System.out.println(idx + ". Annuler une commande"); affichageVersOriginal.put(idx++, 6); }
            System.out.println("0. Se déconnecter");

            int choixAffichage = ConsoleUtils.lireEntier("Votre choix : ");
            if (choixAffichage == 0) {
                SessionService.seDeconnecter();
                quitter = true;
                break;
            }
            Integer original = affichageVersOriginal.get(choixAffichage);
            if (original == null) {
                System.out.println("Choix invalide.");
                continue;
            }
            switch (original) {
                case 1:
                    afficherDashboardJour();
                    choixAffiches.add(1);
                    break;
                case 2:
                    listerBurgers();
                    choixAffiches.add(2);
                    break;
                case 3:
                    listerCommandes();
                    choixAffiches.add(3);
                    break;
                case 4:
                    afficherDetailsCommande();
                    choixAffiches.add(4);
                    break;
                case 5:
                    changerEtatCommande();
                    choixAffiches.add(5);
                    break;
                case 6:
                    annulerCommande();
                    choixAffiches.add(6);
                    break;
                default:
                    System.out.println("Choix invalide.");
            }
        }
    }

    private void afficherDashboardJour() {
        Map<String, Object> stats = statService.statsDuJour();
        System.out.println("===== Statistiques du " + LocalDate.now() + " =====");
        System.out.println("Commandes en cours : " + stats.get("commandes_en_cours"));
        System.out.println("Commandes validées : " + stats.get("commandes_validees"));
        System.out.println("Commandes préparées : " + stats.get("commandes_preparees"));
        System.out.println("Commandes terminées : " + stats.get("commandes_terminees"));
        System.out.println("Commandes annulées : " + stats.get("commandes_annulees"));
        System.out.println("Recettes journalières : " + stats.get("recettes_journalieres") + " FCFA");
        System.out.println("Top produits du jour :");
        Map<String, Long> top = statService.topProduitsDuJour();
        for (Map.Entry<String, Long> entry : top.entrySet()) {
            System.out.println("- " + entry.getKey() + " : " + entry.getValue());
        }
    }

    private void listerBurgers() {
        List<Burger> burgers = burgerRepository.trouverTousActifs();
        for (Burger burger : burgers) {
            System.out.println(burger.getIdBurger() + " - " + burger.getNom() + " - " + burger.getPrix());
        }
    }

    private void listerCommandes() {
        List<Commande> commandes = commandeService.listerToutes();
        for (Commande c : commandes) {
            System.out.println(c.getIdCommande() + " - " + c.getDateCommande() + " - " + c.getEtatCommande() + " - " + c.getTotal());
        }
    }

    private void afficherDetailsCommande() {
        int id = ConsoleUtils.lireEntier("Id commande : ");
        Commande commande = commandeService.details(id);
        if (commande == null) {
            System.out.println("Commande introuvable.");
            return;
        }
        // mark that user just viewed details for this commande
        lastViewedCommandeId = id;
        lastActionWasDetails = true;

        System.out.println("Commande " + commande.getIdCommande());
        System.out.println("Date : " + commande.getDateCommande());
        System.out.println("Client id : " + (commande.getClient() != null ? commande.getClient().getIdClient() : ""));
        System.out.println("Mode consommation : " + commande.getModeConsommation());
        System.out.println("Adresse : " + commande.getAdresseLivraison());
        System.out.println("Etat : " + commande.getEtatCommande());
        System.out.println("Total : " + commande.getTotal());
    }

    private void changerEtatCommande() {
        int id = ConsoleUtils.lireEntier("Id commande : ");
        // prevent changing state immediately after viewing details of the same commande
        if (lastActionWasDetails && lastViewedCommandeId != null && lastViewedCommandeId.equals(id)) {
            System.out.println("Modification interdite : vous avez récemment consulté les détails de cette commande.\nUtilisez l'option depuis la liste des commandes si nécessaire.");
            // reset flag so subsequent attempts are allowed
            lastActionWasDetails = false;
            lastViewedCommandeId = null;
            return;
        }
        // reset detail flag when attempting other state changes
        lastActionWasDetails = false;
        lastViewedCommandeId = null;

        System.out.println("Nouveaux états possibles : EN_COURS, VALIDEE, PREPAREE, TERMINEE, ANNULEE");
        String etatTexte = ConsoleUtils.lireTexte("Nouvel état : ");
        try {
            EtatCommande nouvelEtat = EtatCommande.valueOf(etatTexte);
            commandeService.changerEtat(id, nouvelEtat);
            System.out.println("Etat mis à jour.");
        } catch (IllegalArgumentException e) {
            System.out.println("Etat invalide.");
        }
    }

    private void annulerCommande() {
        int id = ConsoleUtils.lireEntier("Id commande : ");
        String confirmation = ConsoleUtils.lireTexte("Confirmer annulation (o/n) : ");
        if ("o".equalsIgnoreCase(confirmation)) {
            commandeService.annuler(id);
            System.out.println("Commande annulée.");
        } else {
            System.out.println("Annulation abandonnée.");
        }
    }
}
