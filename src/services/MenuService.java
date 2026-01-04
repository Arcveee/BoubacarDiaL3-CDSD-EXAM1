package services;

import config.DatabaseConfig;
import models.Menu;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class MenuService {
    public void create(String nom, boolean actif) throws SQLException {
        String sql = "INSERT INTO menus (nom, actif) VALUES (?, ?)";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, nom);
            stmt.setBoolean(2, actif);
            stmt.executeUpdate();
        }
    }

    public List<Menu> findAll() throws SQLException {
        List<Menu> menus = new ArrayList<>();
        String sql = "SELECT * FROM menus WHERE actif = true ORDER BY id_menu";
        try (Connection conn = DatabaseConfig.getConnection();
             Statement stmt = conn.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) {
                menus.add(new Menu(
                    rs.getInt("id_menu"),
                    rs.getString("nom"),
                    rs.getBoolean("actif")
                ));
            }
        }
        return menus;
    }

    public void update(int id, String nom, boolean actif) throws SQLException {
        String sql = "UPDATE menus SET nom = ?, actif = ? WHERE id_menu = ?";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, nom);
            stmt.setBoolean(2, actif);
            stmt.setInt(3, id);
            stmt.executeUpdate();
        }
    }

    public void delete(int id) throws SQLException {
        String sql = "DELETE FROM menus WHERE id_menu = ?";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, id);
            stmt.executeUpdate();
        }
    }
}
