package views;

import utils.ConsoleUtils;

public class LoginView {
    public void displayLoginScreen() {
        ConsoleUtils.clear();
        ConsoleUtils.printHeader("BRASIL BURGER - CONNEXION");
    }

    public void displayError(String message) {
        System.out.println("\n✗ " + message);
    }

    public void displaySuccess(String message) {
        System.out.println("\n✓ " + message);
    }
}
