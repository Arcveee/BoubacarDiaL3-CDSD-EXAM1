package brasilburger;

import brasilburger.controllers.AuthController;
import brasilburger.controllers.ClientController;
import brasilburger.controllers.GestionnaireController;
import brasilburger.repositories.BurgerRepository;
import brasilburger.repositories.ClientRepository;
import brasilburger.repositories.CommandeRepository;
import brasilburger.repositories.ComplementRepository;
import brasilburger.repositories.GestionnaireRepository;
import brasilburger.repositories.MenuRepository;
import brasilburger.repositories.PaiementRepository;
import brasilburger.repositories.ZoneRepository;
import brasilburger.services.AuthService;
import brasilburger.services.CatalogueService;
import brasilburger.services.CommandeService;
import brasilburger.services.SessionService;
import brasilburger.services.StatService;

public class Main {
    public static void main(String[] args) {
        ClientRepository clientRepository = new ClientRepository();
        GestionnaireRepository gestionnaireRepository = new GestionnaireRepository();
        BurgerRepository burgerRepository = new BurgerRepository();
        MenuRepository menuRepository = new MenuRepository();
        ComplementRepository complementRepository = new ComplementRepository();
        CommandeRepository commandeRepository = new CommandeRepository();
        PaiementRepository paiementRepository = new PaiementRepository();
        ZoneRepository zoneRepository = new ZoneRepository();

        AuthService authService = new AuthService(clientRepository, gestionnaireRepository);
        CatalogueService catalogueService = new CatalogueService(burgerRepository, menuRepository, complementRepository);
        CommandeService commandeService = new CommandeService(commandeRepository, paiementRepository);
        StatService statService = new StatService();

        AuthController authController = new AuthController(authService);
        ClientController clientController = new ClientController(catalogueService, commandeService, zoneRepository);
        GestionnaireController gestionnaireController = new GestionnaireController(burgerRepository, commandeService, statService);

        boolean quitter = false;
        while (!quitter) {
            if (!SessionService.estConnecte()) {
                authController.afficherMenuAuth();
                if (!SessionService.estConnecte()) {
                    quitter = true;
                }
            } else {
                if (SessionService.estClientConnecte()) {
                    clientController.afficherMenuClient();
                } else if (SessionService.estGestionnaireConnecte()) {
                    gestionnaireController.afficherMenuGestionnaire();
                } else {
                    // fallback to auth if session state is inconsistent
                    SessionService.seDeconnecter();
                }
            }
        }
        System.out.println("Au revoir.");
    }
}


