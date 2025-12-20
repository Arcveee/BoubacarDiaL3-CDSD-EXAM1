package brasilburger;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class DatabaseConnection {
    private static final String URL = "jdbc:postgresql://ep-long-shadow-a8gibx5f-pooler.eastus2.azure.neon.tech/neondb?sslmode=require&channel_binding=require";
    private static final String USERNAME = "neondb_owner";
    private static final String PASSWORD = "npg_NKMDBu4Zn3Ik";

    public static Connection getConnection() {
        try {
            return DriverManager.getConnection(URL, USERNAME, PASSWORD);
        } catch (SQLException e) {
            throw new RuntimeException("Erreur de connexion à la base de données", e);
        }
    }
}
