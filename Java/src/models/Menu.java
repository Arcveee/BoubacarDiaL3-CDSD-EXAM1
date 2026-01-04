package models;

public class Menu {
    private int idMenu;
    private String nom;
    private boolean actif;

    public Menu() {}

    public Menu(int idMenu, String nom, boolean actif) {
        this.idMenu = idMenu;
        this.nom = nom;
        this.actif = actif;
    }

    public int getIdMenu() { return idMenu; }
    public void setIdMenu(int idMenu) { this.idMenu = idMenu; }
    public String getNom() { return nom; }
    public void setNom(String nom) { this.nom = nom; }
    public boolean isActif() { return actif; }
    public void setActif(boolean actif) { this.actif = actif; }
}
