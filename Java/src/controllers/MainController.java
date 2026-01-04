package controllers;

import views.MainView;
import java.util.Scanner;

public class MainController {
    private MainView view = new MainView();
    private Scanner scanner = new Scanner(System.in);

    public void start() {
        boolean running = true;
        while (running) {
            view.displayMenu();
            int choice = scanner.nextInt();
            scanner.nextLine();

            switch (choice) {
                case 1:
                    new BurgerController(scanner).start();
                    break;
                case 2:
                    new MenuController(scanner).start();
                    break;
                case 3:
                    new ComplementController(scanner).start();
                    break;
                case 0:
                    running = false;
                    view.displayGoodbye();
                    break;
                default:
                    System.out.println("Choix invalide");
            }
        }
        scanner.close();
    }
}
