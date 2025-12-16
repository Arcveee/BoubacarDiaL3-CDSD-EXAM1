package brasilburger.models;

import java.math.BigDecimal;

public class LigneCommande {
    private int idLigne;
    private int quantite;
    private Burger burger;
    private Menu menu;
    private Complement complement;

    public LigneCommande() {
    }

    public LigneCommande(int idLigne, int quantite, Burger burger, Menu menu, Complement complement) {
        this.idLigne = idLigne;
        this.quantite = quantite;
        this.burger = burger;
        this.menu = menu;
        this.complement = complement;
    }

    public int getIdLigne() {
        return idLigne;
    }

    public void setIdLigne(int idLigne) {
        this.idLigne = idLigne;
    }

    public int getQuantite() {
        return quantite;
    }

    public void setQuantite(int quantite) {
        this.quantite = quantite;
    }

    public Burger getBurger() {
        return burger;
    }

    public void setBurger(Burger burger) {
        this.burger = burger;
    }

    public Menu getMenu() {
        return menu;
    }

    public void setMenu(Menu menu) {
        this.menu = menu;
    }

    public Complement getComplement() {
        return complement;
    }

    public void setComplement(Complement complement) {
        this.complement = complement;
    }

    public BigDecimal getSousTotal() {
        BigDecimal prix = BigDecimal.ZERO;
        if (burger != null) {
            prix = burger.getPrix();
        } else if (menu != null) {
            prix = menu.getPrix();
        } else if (complement != null) {
            prix = complement.getPrix();
        }
        return prix.multiply(BigDecimal.valueOf(quantite));
    }
}

