package views;

import utils.ConsoleUtils;

public class MainView {
    public void displayMenu() {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("BRASIL BURGER - GESTION");
        System.out.println("1. Gerer les Burgers");
        System.out.println("2. Gerer les Menus");
        System.out.println("3. Gerer les Complements");
        System.out.println("0. Quitter");
        ConsoleUtils.printLine();
        System.out.print("Votre choix: ");
    }

    public void displayGoodbye() {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("AU REVOIR!");
        System.out.println("Merci d'avoir utilise Brasil Burger.");
    }
}
