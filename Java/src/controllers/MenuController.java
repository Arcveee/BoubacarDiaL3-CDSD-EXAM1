package controllers;

import services.MenuService;
import views.MenuView;
import java.util.Scanner;

public class MenuController {
    private MenuService service = new MenuService();
    private MenuView view = new MenuView();
    private Scanner scanner;

    public MenuController(Scanner scanner) {
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
            System.out.print("Actif (true/false): ");
            boolean actif = scanner.nextBoolean();
            scanner.nextLine();

            service.create(nom, actif);
            view.displaySuccess("Menu cree avec succes!");
            scanner.nextLine();
        } catch (Exception e) {
            view.displayError(e.getMessage());
            scanner.nextLine();
        }
    }

    private void list() {
        try {
            view.displayMenus(service.findAll());
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
            System.out.print("ID du menu a modifier: ");
            int id = scanner.nextInt();
            scanner.nextLine();
            System.out.print("Nouveau nom: ");
            String nom = scanner.nextLine();
            System.out.print("Actif (true/false): ");
            boolean actif = scanner.nextBoolean();
            scanner.nextLine();

            service.update(id, nom, actif);
            view.displaySuccess("Menu modifie avec succes!");
            scanner.nextLine();
        } catch (Exception e) {
            view.displayError(e.getMessage());
            scanner.nextLine();
        }
    }

    private void delete() {
        try {
            list();
            System.out.print("ID du menu a supprimer: ");
            int id = scanner.nextInt();
            scanner.nextLine();
            
            service.delete(id);
            view.displaySuccess("Menu supprime avec succes!");
            scanner.nextLine();
        } catch (Exception e) {
            view.displayError(e.getMessage());
            scanner.nextLine();
        }
    }
}
