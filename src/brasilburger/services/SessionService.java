package brasilburger.services;

import brasilburger.models.Client;
import brasilburger.models.Gestionnaire;

public class SessionService {
    private static Client clientConnecte;
    private static Gestionnaire gestionnaireConnecte;
    private static int sessionVersion = 0;

    public static Client getClientConnecte() {
        return clientConnecte;
    }

    public static void setClientConnecte(Client client) {
        clientConnecte = client;
        gestionnaireConnecte = null;
        sessionVersion++;
    }

    public static Gestionnaire getGestionnaireConnecte() {
        return gestionnaireConnecte;
    }

    public static void setGestionnaireConnecte(Gestionnaire gestionnaire) {
        gestionnaireConnecte = gestionnaire;
        clientConnecte = null;
        sessionVersion++;
    }

    public static void seDeconnecter() {
        clientConnecte = null;
        gestionnaireConnecte = null;
        sessionVersion++;
    }

    public static boolean estClientConnecte() {
        return clientConnecte != null;
    }

    public static boolean estGestionnaireConnecte() {
        return gestionnaireConnecte != null;
    }

    public static boolean estConnecte() {
        return estClientConnecte() || estGestionnaireConnecte();
    }

    public static int getSessionVersion() {
        return sessionVersion;
    }

    public static void bumpSessionVersion() {
        sessionVersion++;
    }
}


