package views;

import models.Complement;
import utils.ConsoleUtils;
import java.util.List;

public class ComplementView {
    public void displayMenu() {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("GESTION DES COMPLEMENTS");
        System.out.println("1. Creer un complement");
        System.out.println("2. Lister les complements");
        System.out.println("3. Modifier un complement");
        System.out.println("4. Supprimer un complement");
        System.out.println("0. Retour");
        ConsoleUtils.printLine();
        System.out.print("Votre choix: ");
    }

    public void displayComplements(List<Complement> complements) {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("LISTE DES COMPLEMENTS");
        System.out.printf("%-5s %-30s %-10s %-10s%n", "ID", "Nom", "Prix", "Actif");
        ConsoleUtils.printLine();
        for (Complement c : complements) {
            System.out.printf("%-5d %-30s %-10.2f %-10s%n",
                c.getIdComplement(), c.getNom(), c.getPrix(), c.isActif() ? "Oui" : "Non");
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
