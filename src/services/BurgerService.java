package services;

import config.DatabaseConfig;
import models.Burger;
import java.math.BigDecimal;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class BurgerService {
    public void create(String nom, BigDecimal prix, String image, boolean actif) throws SQLException {
        String sql = "INSERT INTO burgers (nom, prix, image, actif) VALUES (?, ?, ?, ?)";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, nom);
            stmt.setBigDecimal(2, prix);
            stmt.setString(3, image);
            stmt.setBoolean(4, actif);
            stmt.executeUpdate();
        }
    }

    public List<Burger> findAll() throws SQLException {
        List<Burger> burgers = new ArrayList<>();
        String sql = "SELECT * FROM burgers WHERE actif = true ORDER BY id_burger";
        try (Connection conn = DatabaseConfig.getConnection();
             Statement stmt = conn.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) {
                burgers.add(new Burger(
                    rs.getInt("id_burger"),
                    rs.getString("nom"),
                    rs.getBigDecimal("prix"),
                    rs.getString("image"),
                    rs.getBoolean("actif")
                ));
            }
        }
        return burgers;
    }

    public void update(int id, String nom, BigDecimal prix, String image, boolean actif) throws SQLException {
        String sql = "UPDATE burgers SET nom = ?, prix = ?, image = ?, actif = ? WHERE id_burger = ?";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, nom);
            stmt.setBigDecimal(2, prix);
            stmt.setString(3, image);
            stmt.setBoolean(4, actif);
            stmt.setInt(5, id);
            stmt.executeUpdate();
        }
    }

    public void delete(int id) throws SQLException {
        String sql = "DELETE FROM burgers WHERE id_burger = ?";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, id);
            stmt.executeUpdate();
        }
    }
}
