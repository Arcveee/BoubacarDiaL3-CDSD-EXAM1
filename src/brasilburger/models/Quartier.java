package brasilburger.models;

public class Quartier {
    private int idQuartier;
    private String nom;
    private Zone zone;

    public Quartier() {
    }

    public Quartier(int idQuartier, String nom, Zone zone) {
        this.idQuartier = idQuartier;
        this.nom = nom;
        this.zone = zone;
    }

    public int getIdQuartier() {
        return idQuartier;
    }

    public void setIdQuartier(int idQuartier) {
        this.idQuartier = idQuartier;
    }

    public String getNom() {
        return nom;
    }

    public void setNom(String nom) {
        this.nom = nom;
    }

    public Zone getZone() {
        return zone;
    }

    public void setZone(Zone zone) {
        this.zone = zone;
    }
}

