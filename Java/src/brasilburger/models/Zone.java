package brasilburger.models;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class Zone {
    private int idZone;
    private String nom;
    private BigDecimal prixLivraison;
    private List<Quartier> quartiers = new ArrayList<>();

    public Zone() {
    }

    public Zone(int idZone, String nom, BigDecimal prixLivraison) {
        this.idZone = idZone;
        this.nom = nom;
        this.prixLivraison = prixLivraison;
    }

    public int getIdZone() {
        return idZone;
    }

    public void setIdZone(int idZone) {
        this.idZone = idZone;
    }

    public String getNom() {
        return nom;
    }

    public void setNom(String nom) {
        this.nom = nom;
    }

    public BigDecimal getPrixLivraison() {
        return prixLivraison;
    }

    public void setPrixLivraison(BigDecimal prixLivraison) {
        this.prixLivraison = prixLivraison;
    }

    public List<Quartier> getQuartiers() {
        return quartiers;
    }

    public void setQuartiers(List<Quartier> quartiers) {
        this.quartiers = quartiers;
    }
}

