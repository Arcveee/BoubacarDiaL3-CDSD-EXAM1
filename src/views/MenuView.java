package views;

import models.Menu;
import utils.ConsoleUtils;
import java.util.List;

public class MenuView {
    public void displayMenu() {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("GESTION DES MENUS");
        System.out.println("1. Creer un menu");
        System.out.println("2. Lister les menus");
        System.out.println("3. Modifier un menu");
        System.out.println("4. Supprimer un menu");
        System.out.println("0. Retour");
        ConsoleUtils.printLine();
        System.out.print("Votre choix: ");
    }

    public void displayMenus(List<Menu> menus) {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("LISTE DES MENUS");
        System.out.printf("%-5s %-30s %-10s%n", "ID", "Nom", "Actif");
        ConsoleUtils.printLine();
        for (Menu m : menus) {
            System.out.printf("%-5d %-30s %-10s%n",
                m.getIdMenu(), m.getNom(), m.isActif() ? "Oui" : "Non");
        }
        ConsoleUtils.printLine();
    }

    public void displaySuccess(String message) {
        System.out.println("\n✓ " + message);
    }

    public void displayError(String message) {
        System.out.println("\n✗ Erreur: " + message);
    }
}
