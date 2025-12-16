package brasilburger.models;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

public class Menu {
    private int idMenu;
    private String nom;
    private String image;
    private boolean actif;
    private List<Burger> burgers = new ArrayList<>();

    public Menu() {
    }

    public Menu(int idMenu, String nom, String image, boolean actif) {
        this.idMenu = idMenu;
        this.nom = nom;
        this.image = image;
        this.actif = actif;
    }

    public int getIdMenu() {
        return idMenu;
    }

    public void setIdMenu(int idMenu) {
        this.idMenu = idMenu;
    }

    public String getNom() {
        return nom;
    }

    public void setNom(String nom) {
        this.nom = nom;
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

    public List<Burger> getBurgers() {
        return burgers;
    }

    public void setBurgers(List<Burger> burgers) {
        this.burgers = burgers;
    }

    public BigDecimal getPrix() {
        return burgers.stream()
                .map(Burger::getPrix)
                .reduce(BigDecimal.ZERO, BigDecimal::add);
    }
}

