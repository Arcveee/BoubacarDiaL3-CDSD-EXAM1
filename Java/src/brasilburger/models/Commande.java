package brasilburger.models;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.List;
import brasilburger.models.enums.EtatCommande;
import brasilburger.models.enums.ModeConsommation;

public class Commande {
    private int idCommande;
    private LocalDateTime dateCommande;
    private BigDecimal total;
    private String adresseLivraison;
    private ModeConsommation modeConsommation;
    private EtatCommande etatCommande;
    private Client client;
    private Zone zone;
    private List<LigneCommande> lignes = new ArrayList<>();
    private Paiement paiement;

    public Commande() {
    }

    public Commande(int idCommande, LocalDateTime dateCommande, String adresseLivraison, ModeConsommation modeConsommation, EtatCommande etatCommande, Client client, Zone zone) {
        this.idCommande = idCommande;
        this.dateCommande = dateCommande;
        this.adresseLivraison = adresseLivraison;
        this.modeConsommation = modeConsommation;
        this.etatCommande = etatCommande;
        this.client = client;
        this.zone = zone;
    }

    public int getIdCommande() {
        return idCommande;
    }

    public void setIdCommande(int idCommande) {
        this.idCommande = idCommande;
    }

    public LocalDateTime getDateCommande() {
        return dateCommande;
    }

    public void setDateCommande(LocalDateTime dateCommande) {
        this.dateCommande = dateCommande;
    }

    public BigDecimal getTotal() {
        return total;
    }

    public void setTotal(BigDecimal total) {
        this.total = total;
    }

    public String getAdresseLivraison() {
        return adresseLivraison;
    }

    public void setAdresseLivraison(String adresseLivraison) {
        this.adresseLivraison = adresseLivraison;
    }

    public ModeConsommation getModeConsommation() {
        return modeConsommation;
    }

    public void setModeConsommation(ModeConsommation modeConsommation) {
        this.modeConsommation = modeConsommation;
    }

    public EtatCommande getEtatCommande() {
        return etatCommande;
    }

    public void setEtatCommande(EtatCommande etatCommande) {
        this.etatCommande = etatCommande;
    }

    public Client getClient() {
        return client;
    }

    public void setClient(Client client) {
        this.client = client;
    }

    public Zone getZone() {
        return zone;
    }

    public void setZone(Zone zone) {
        this.zone = zone;
    }

    public List<LigneCommande> getLignes() {
        return lignes;
    }

    public void setLignes(List<LigneCommande> lignes) {
        this.lignes = lignes;
    }

    public Paiement getPaiement() {
        return paiement;
    }

    public void setPaiement(Paiement paiement) {
        this.paiement = paiement;
    }

    public void ajouterLigne(LigneCommande ligne) {
        this.lignes.add(ligne);
    }

    public BigDecimal calculerTotal() {
        BigDecimal somme = lignes.stream()
                .map(LigneCommande::getSousTotal)
                .reduce(BigDecimal.ZERO, BigDecimal::add);
        if (modeConsommation == ModeConsommation.LIVRAISON && zone != null && zone.getPrixLivraison() != null) {
            somme = somme.add(zone.getPrixLivraison());
        }
        this.total = somme;
        return somme;
    }
}

