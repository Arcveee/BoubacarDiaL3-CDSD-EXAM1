package brasilburger.controllers;

import brasilburger.models.*;
import brasilburger.models.enums.ModeConsommation;
import brasilburger.models.enums.ModePaiement;
import brasilburger.services.CatalogueService;
import brasilburger.services.CommandeService;
import brasilburger.services.PanierService;
import brasilburger.services.SessionService;
import brasilburger.repositories.ZoneRepository;
import brasilburger.views.ConsoleUtils;

import java.util.List;

public class ClientController {
    private final CatalogueService catalogueService;
    private final CommandeService commandeService;
    private final ZoneRepository zoneRepository;

    public ClientController(CatalogueService catalogueService, CommandeService commandeService, ZoneRepository zoneRepository) {
        this.catalogueService = catalogueService;
        this.commandeService = commandeService;
        this.zoneRepository = zoneRepository;
    }

    public void afficherMenuClient() {
        if (!SessionService.estClientConnecte()) {
            System.out.println("Connexion client obligatoire.");
            return;
        }
        boolean quitter = false;
        while (!quitter && SessionService.estClientConnecte()) {
            System.out.println("=========== Menu Client ==========");
            System.out.println("1. Voir tous les burgers");
            System.out.println("2. Voir tous les menus");
            System.out.println("3. Voir tous les compléments");
            System.out.println("4. Détails burger");
            System.out.println("5. Détails menu");
            System.out.println("6. Ajouter burger au panier");
            System.out.println("7. Ajouter menu au panier");
            System.out.println("8. Voir panier");
            System.out.println("9. Valider commande");
            System.out.println("10. Mes commandes");
            System.out.println("0. Se déconnecter");
            int choix = ConsoleUtils.lireEntier("Votre choix : ");
            switch (choix) {
                case 1:
                    afficherBurgers();
                    break;
                case 2:
                    afficherMenus();
                    break;
                case 3:
                    catalogueService.listerComplementsActifs().forEach(c -> System.out.println(c.getIdComplement() + " - " + c.getNom() + " - " + c.getPrix()));
                    break;
                case 4:
                    afficherDetailsBurger();
                    break;
                case 5:
                    afficherDetailsMenu();
                    break;
                case 6:
                    ajouterBurgerPanier();
                    break;
                case 7:
                    ajouterMenuPanier();
                    break;
                case 8:
                    afficherPanier();
                    break;
                case 9:
                    validerCommande();
                    break;
                case 10:
                    listerCommandesClient();
                    break;
                case 0:
                    SessionService.seDeconnecter();
                    quitter = true;
                    break;
                default:
                    System.out.println("Choix invalide.");
            }
        }
    }

    private void afficherBurgers() {
        List<Burger> burgers = catalogueService.listerBurgersActifs();
        for (Burger burger : burgers) {
            System.out.println(burger.getIdBurger() + " - " + burger.getNom() + " - " + burger.getPrix());
        }
    }

    private void afficherMenus() {
        List<Menu> menus = catalogueService.listerMenusActifs();
        for (Menu menu : menus) {
            System.out.println(menu.getIdMenu() + " - " + menu.getNom() + " - " + menu.getPrix());
        }
    }

    private void afficherDetailsBurger() {
        int id = ConsoleUtils.lireEntier("Id burger : ");
        Burger burger = catalogueService.trouverBurgerParId(id);
        if (burger == null) {
            System.out.println("Burger introuvable.");
        } else {
            System.out.println("Nom : " + burger.getNom());
            System.out.println("Prix : " + burger.getPrix());
            System.out.println("Image : " + burger.getImage());
        }
    }

    private void afficherDetailsMenu() {
        int id = ConsoleUtils.lireEntier("Id menu : ");
        Menu menu = catalogueService.trouverMenuParId(id);
        if (menu == null) {
            System.out.println("Menu introuvable.");
        } else {
            System.out.println("Nom : " + menu.getNom());
            System.out.println("Prix : " + menu.getPrix());
            System.out.println("Burgers :");
            for (Burger burger : menu.getBurgers()) {
                System.out.println("- " + burger.getNom() + " (" + burger.getPrix() + ")");
            }
        }
    }

    private void ajouterBurgerPanier() {
        int id = ConsoleUtils.lireEntier("Id burger : ");
        Burger burger = catalogueService.trouverBurgerParId(id);
        if (burger == null) {
            System.out.println("Burger introuvable.");
            return;
        }
        int quantite = ConsoleUtils.lireEntier("Quantité : ");
        if (quantite <= 0) {
            System.out.println("Quantité invalide.");
            return;
        }
        PanierService.ajouterBurger(burger, quantite);
        System.out.println("Burger ajouté au panier.");
    }

    private void ajouterMenuPanier() {
        int id = ConsoleUtils.lireEntier("Id menu : ");
        Menu menu = catalogueService.trouverMenuParId(id);
        if (menu == null) {
            System.out.println("Menu introuvable.");
            return;
        }
        int quantite = ConsoleUtils.lireEntier("Quantité : ");
        if (quantite <= 0) {
            System.out.println("Quantité invalide.");
            return;
        }
        PanierService.ajouterMenu(menu, quantite);
        System.out.println("Menu ajouté au panier.");
    }

    private void afficherPanier() {
        List<LigneCommande> lignes = PanierService.getLignes();
        if (lignes.isEmpty()) {
            System.out.println("Panier vide.");
            return;
        }
        int index = 0;
        for (LigneCommande lc : lignes) {
            String libelle = "";
            if (lc.getBurger() != null) {
                libelle = "Burger " + lc.getBurger().getNom();
            } else if (lc.getMenu() != null) {
                libelle = "Menu " + lc.getMenu().getNom();
            } else if (lc.getComplement() != null) {
                libelle = "Complement " + lc.getComplement().getNom();
            }
            System.out.println(index + " - " + libelle + " x" + lc.getQuantite() + " = " + lc.getSousTotal());
            index++;
        }
    }

    private void validerCommande() {
        List<LigneCommande> lignes = PanierService.getLignes();
        if (lignes.isEmpty()) {
            System.out.println("Panier vide.");
            return;
        }
        System.out.println("Mode de consommation : 1-SUR_PLACE 2-EMPORTER 3-LIVRAISON");
        int choix = ConsoleUtils.lireEntier("Votre choix : ");
        ModeConsommation mode;
        Zone zone = null;
        String adresse = null;
        if (choix == 1) {
            mode = ModeConsommation.SUR_PLACE;
        } else if (choix == 2) {
            mode = ModeConsommation.EMPORTER;
        } else if (choix == 3) {
            mode = ModeConsommation.LIVRAISON;
            adresse = ConsoleUtils.lireTexte("Adresse de livraison : ");
            List<Zone> zones = zoneRepository.lister();
            for (Zone z : zones) {
                System.out.println(z.getIdZone() + " - " + z.getNom() + " (" + z.getPrixLivraison() + ")");
            }
            int idZone = ConsoleUtils.lireEntier("Id zone : ");
            zone = zoneRepository.trouverParId(idZone);
            if (zone == null) {
                System.out.println("Zone invalide.");
                return;
            }
        } else {
            System.out.println("Choix invalide.");
            return;
        }
        try {
            Commande commande = commandeService.creerDepuisPanier(lignes, mode, zone, adresse);
            PanierService.vider();
            System.out.println("Commande créée sous le numéro " + commande.getIdCommande() + ". Total : " + commande.getTotal());
            System.out.println("Payer maintenant ? 1-WAVE 2-OM 3-Plus tard");
            int p = ConsoleUtils.lireEntier("Votre choix : ");
            if (p == 1 || p == 2) {
                ModePaiement modePaiement = p == 1 ? ModePaiement.WAVE : ModePaiement.OM;
                commandeService.payer(commande, commande.getTotal(), modePaiement);
                System.out.println("Paiement effectué, commande validée.");
            }
        } catch (Exception e) {
            System.out.println("Erreur lors de la création de la commande : " + e.getMessage());
        }
    }

    private void listerCommandesClient() {
        if (!SessionService.estClientConnecte()) {
            System.out.println("Connexion client obligatoire.");
            return;
        }
        int idClient = SessionService.getClientConnecte().getIdClient();
        List<Commande> commandes = commandeService.filtrer(null, null, null, idClient, null);
        for (Commande c : commandes) {
            System.out.println(c.getIdCommande() + " - " + c.getDateCommande() + " - " + c.getEtatCommande() + " - " + c.getTotal());
        }
    }
}


