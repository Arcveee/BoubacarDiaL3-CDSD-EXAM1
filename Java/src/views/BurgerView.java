package views;

import models.Burger;
import utils.ConsoleUtils;
import java.util.List;

public class BurgerView {
    public void displayMenu() {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("GESTION DES BURGERS");
        System.out.println("1. Creer un burger");
        System.out.println("2. Lister les burgers");
        System.out.println("3. Modifier un burger");
        System.out.println("4. Supprimer un burger");
        System.out.println("0. Retour");
        ConsoleUtils.printLine();
        System.out.print("Votre choix: ");
    }

    public void displayBurgers(List<Burger> burgers) {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("LISTE DES BURGERS");
        System.out.printf("%-5s %-20s %-10s %-20s %-10s%n", "ID", "Nom", "Prix", "Image", "Actif");
        ConsoleUtils.printLine();
        for (Burger b : burgers) {
            System.out.printf("%-5d %-20s %-10.2f %-20s %-10s%n",
                b.getIdBurger(), b.getNom(), b.getPrix(), b.getImage(), b.isActif() ? "Oui" : "Non");
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
