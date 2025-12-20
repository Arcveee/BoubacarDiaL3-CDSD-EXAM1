package brasilburger.services;

import brasilburger.DatabaseConnection;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.time.LocalDate;
import java.util.LinkedHashMap;
import java.util.Map;

public class StatService {

    public Map<String, Object> statsDuJour() {
        LocalDate today = LocalDate.now();
        Map<String, Object> stats = new LinkedHashMap<>();
        stats.put("commandes_en_cours", compter("SELECT COUNT(*) FROM commandes WHERE date_commande::date = ? AND etat_commande = 'EN_COURS'", today));
        stats.put("commandes_validees", compter("SELECT COUNT(*) FROM commandes WHERE date_commande::date = ? AND etat_commande = 'VALIDEE'", today));
        stats.put("commandes_preparees", compter("SELECT COUNT(*) FROM commandes WHERE date_commande::date = ? AND etat_commande = 'PREPAREE'", today));
        stats.put("commandes_terminees", compter("SELECT COUNT(*) FROM commandes WHERE date_commande::date = ? AND etat_commande = 'TERMINEE'", today));
        stats.put("commandes_annulees", compter("SELECT COUNT(*) FROM commandes WHERE date_commande::date = ? AND etat_commande = 'ANNULEE'", today));
        stats.put("recettes_journalieres", somme("SELECT COALESCE(SUM(total),0) FROM commandes WHERE date_commande::date = ? AND etat_commande IN ('VALIDEE','PREPAREE','TERMINEE')", today));
        stats.put("top_produits", topProduits(today));
        return stats;
    }

    public Map<String, Long> topProduitsDuJour() {
        return topProduits(LocalDate.now());
    }

    private long compter(String sql, LocalDate day) {
        try (Connection connection = DatabaseConnection.getConnection();
             PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setDate(1, java.sql.Date.valueOf(day));
            try (ResultSet rs = statement.executeQuery()) {
                if (rs.next()) {
                    return rs.getLong(1);
                }
            }
            return 0;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private java.math.BigDecimal somme(String sql, LocalDate day) {
        try (Connection connection = DatabaseConnection.getConnection();
             PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setDate(1, java.sql.Date.valueOf(day));
            try (ResultSet rs = statement.executeQuery()) {
                if (rs.next()) {
                    return rs.getBigDecimal(1);
                }
            }
            return java.math.BigDecimal.ZERO;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }

    private Map<String, Long> topProduits(LocalDate day) {
        // Count sold quantities per product (burgers, menus, complements)
        // Only consider orders that are validated/prepared/terminated (real purchases)
        String sql = "SELECT nom, SUM(total_q) AS total_vendu FROM (" +
                " SELECT b.nom AS nom, SUM(lc.quantite) AS total_q FROM lignes_commandes lc " +
                " JOIN burgers b ON b.id_burger = lc.id_burger " +
                " JOIN commandes c ON c.id_commande = lc.id_commande " +
                " WHERE c.date_commande::date = ? AND c.etat_commande IN ('VALIDEE','PREPAREE','TERMINEE') GROUP BY b.nom " +
                " UNION ALL " +
                " SELECT m.nom AS nom, SUM(lc.quantite) AS total_q FROM lignes_commandes lc " +
                " JOIN menus m ON m.id_menu = lc.id_menu " +
                " JOIN commandes c ON c.id_commande = lc.id_commande " +
                " WHERE c.date_commande::date = ? AND c.etat_commande IN ('VALIDEE','PREPAREE','TERMINEE') GROUP BY m.nom " +
                " UNION ALL " +
                " SELECT co.nom AS nom, SUM(lc.quantite) AS total_q FROM lignes_commandes lc " +
                " JOIN complements co ON co.id_complement = lc.id_complement " +
                " JOIN commandes c ON c.id_commande = lc.id_commande " +
                " WHERE c.date_commande::date = ? AND c.etat_commande IN ('VALIDEE','PREPAREE','TERMINEE') GROUP BY co.nom " +
                ") x GROUP BY nom ORDER BY total_vendu DESC LIMIT 10";
        Map<String, Long> top = new LinkedHashMap<>();
        try (Connection connection = DatabaseConnection.getConnection();
             PreparedStatement statement = connection.prepareStatement(sql)) {
            statement.setDate(1, java.sql.Date.valueOf(day));
            statement.setDate(2, java.sql.Date.valueOf(day));
            statement.setDate(3, java.sql.Date.valueOf(day));
            try (ResultSet rs = statement.executeQuery()) {
                while (rs.next()) {
                    top.put(rs.getString("nom"), rs.getLong("total_vendu"));
                }
            }
            return top;
        } catch (SQLException e) {
            throw new RuntimeException(e);
        }
    }
}


