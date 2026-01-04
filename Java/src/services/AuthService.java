package services;

import config.DatabaseConfig;
import java.sql.*;

public class AuthService {
    public boolean authenticate(String login, String password) throws SQLException {
        String sql = "SELECT * FROM gestionnaires WHERE login = ? AND password = ?";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, login);
            stmt.setString(2, password);
            try (ResultSet rs = stmt.executeQuery()) {
                return rs.next();
            }
        }
    }
}
