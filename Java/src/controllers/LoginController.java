package controllers;

import services.AuthService;
import views.LoginView;
import java.util.Scanner;

public class LoginController {
    private AuthService authService = new AuthService();
    private LoginView view = new LoginView();
    private Scanner scanner;

    public LoginController(Scanner scanner) {
        this.scanner = scanner;
    }

    public boolean login() {
        int attempts = 0;
        while (attempts < 3) {
            view.displayLoginScreen();
            System.out.print("Login: ");
            String login = scanner.nextLine();
            System.out.print("Password: ");
            String password = scanner.nextLine();

            try {
                if (authService.authenticate(login, password)) {
                    view.displaySuccess("Connexion reussie!");
                    Thread.sleep(1000);
                    return true;
                } else {
                    attempts++;
                    view.displayError("Login ou mot de passe incorrect. Tentatives restantes: " + (3 - attempts));
                    if (attempts < 3) {
                        Thread.sleep(2000);
                    }
                }
            } catch (Exception e) {
                view.displayError(e.getMessage());
                return false;
            }
        }
        view.displayError("Trop de tentatives echouees. Application fermee.");
        return false;
    }
}
