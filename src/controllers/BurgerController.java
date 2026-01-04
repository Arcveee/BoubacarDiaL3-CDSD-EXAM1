package controllers;

import services.BurgerService;
import views.BurgerView;
import java.math.BigDecimal;
import java.util.Scanner;

public class BurgerController {
    private BurgerService service = new BurgerService();
    private BurgerView view = new BurgerView();
    private Scanner scanner;

    public BurgerController(Scanner scanner) {
        this.scanner = scanner;
    }

    public void start() {
        boolean running = true;
        while (running) {
            view.displayMenu();
            int choice = scanner.nextInt();
            scanner.nextLine();

            switch (choice) {
                case 1: create(); break;
                case 2: list(); break;
                case 3: update(); break;
                case 4: delete(); break;
                case 0: running = false; break;
                default: view.displayError("Choix invalide");
            }
        }
    }

    private void create() {
        try {
            System.out.print("Nom: ");
            String nom = scanner.nextLine();
            System.out.print("Prix: ");
            BigDecimal prix = new BigDecimal(scanner.nextLine());
            System.out.print("Image: ");
            String image = scanner.nextLine();
            System.out.print("Actif (true/false): ");
            boolean actif = scanner.nextBoolean();
            scanner.nextLine();

            service.create(nom, prix, image, actif);
            view.displaySuccess("Burger cree avec succes!");
            scanner.nextLine();
        } catch (Exception e) {
            view.displayError(e.getMessage());
            scanner.nextLine();
        }
    }

    private void list() {
        try {
            view.displayBurgers(service.findAll());
            System.out.print("\nAppuyez sur Entree pour continuer...");
            scanner.nextLine();
        } catch (Exception e) {
            view.displayError(e.getMessage());
            scanner.nextLine();
        }
    }

    private void update() {
        try {
            list();
            System.out.print("ID du burger a modifier: ");
            int id = scanner.nextInt();
            scanner.nextLine();
            System.out.print("Nouveau nom: ");
            String nom = scanner.nextLine();
            System.out.print("Nouveau prix: ");
            BigDecimal prix = new BigDecimal(scanner.nextLine());
            System.out.print("Nouvelle image: ");
            String image = scanner.nextLine();
            System.out.print("Actif (true/false): ");
            boolean actif = scanner.nextBoolean();
            scanner.nextLine();

            service.update(id, nom, prix, image, actif);
            view.displaySuccess("Burger modifie avec succes!");
            scanner.nextLine();
        } catch (Exception e) {
            view.displayError(e.getMessage());
            scanner.nextLine();
        }
    }

    private void delete() {
        try {
            list();
            System.out.print("ID du burger a supprimer: ");
            int id = scanner.nextInt();
            scanner.nextLine();
            
            service.delete(id);
            view.displaySuccess("Burger supprime avec succes!");
            scanner.nextLine();
        } catch (Exception e) {
            view.displayError(e.getMessage());
            scanner.nextLine();
        }
    }
}
