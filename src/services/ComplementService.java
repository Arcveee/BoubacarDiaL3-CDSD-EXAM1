package services;

import config.DatabaseConfig;
import models.Complement;
import java.math.BigDecimal;
import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class ComplementService {
    public void create(String nom, BigDecimal prix, boolean actif) throws SQLException {
        String sql = "INSERT INTO complements (nom, prix, actif) VALUES (?, ?, ?)";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, nom);
            stmt.setBigDecimal(2, prix);
            stmt.setBoolean(3, actif);
            stmt.executeUpdate();
        }
    }

    public List<Complement> findAll() throws SQLException {
        List<Complement> complements = new ArrayList<>();
        String sql = "SELECT * FROM complements WHERE actif = true ORDER BY id_complement";
        try (Connection conn = DatabaseConfig.getConnection();
             Statement stmt = conn.createStatement();
             ResultSet rs = stmt.executeQuery(sql)) {
            while (rs.next()) {
                complements.add(new Complement(
                    rs.getInt("id_complement"),
                    rs.getString("nom"),
                    rs.getBigDecimal("prix"),
                    rs.getBoolean("actif")
                ));
            }
        }
        return complements;
    }

    public void update(int id, String nom, BigDecimal prix, boolean actif) throws SQLException {
        String sql = "UPDATE complements SET nom = ?, prix = ?, actif = ? WHERE id_complement = ?";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setString(1, nom);
            stmt.setBigDecimal(2, prix);
            stmt.setBoolean(3, actif);
            stmt.setInt(4, id);
            stmt.executeUpdate();
        }
    }

    public void delete(int id) throws SQLException {
        String sql = "DELETE FROM complements WHERE id_complement = ?";
        try (Connection conn = DatabaseConfig.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, id);
            stmt.executeUpdate();
        }
    }
}
