package brasilburger.models;

import java.math.BigDecimal;

public class Complement {
    private int idComplement;
    private String nom;
    private BigDecimal prix;
    private String image;
    private boolean actif;

    public Complement() {
    }

    public Complement(int idComplement, String nom, BigDecimal prix, String image, boolean actif) {
        this.idComplement = idComplement;
        this.nom = nom;
        this.prix = prix;
        this.image = image;
        this.actif = actif;
    }

    public int getIdComplement() {
        return idComplement;
    }

    public void setIdComplement(int idComplement) {
        this.idComplement = idComplement;
    }

    public String getNom() {
        return nom;
    }

    public void setNom(String nom) {
        this.nom = nom;
    }

    public BigDecimal getPrix() {
        return prix;
    }

    public void setPrix(BigDecimal prix) {
        this.prix = prix;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }

    public boolean isActif() {
        return actif;
    }

    public void setActif(boolean actif) {
        this.actif = actif;
    }
}

