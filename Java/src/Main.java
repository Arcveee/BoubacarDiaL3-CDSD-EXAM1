import config.DatabaseConfig;
import controllers.LoginController;
import controllers.MainController;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        try {
            LoginController loginController = new LoginController(scanner);
            if (loginController.login()) {
                MainController controller = new MainController();
                controller.start();
            }
        } catch (Exception e) {
            System.err.println("Erreur: " + e.getMessage());
        } finally {
            scanner.close();
            DatabaseConfig.close();
        }
    }
}
