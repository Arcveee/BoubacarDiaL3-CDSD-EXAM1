package brasilburger.models;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import brasilburger.models.enums.ModePaiement;

public class Paiement {
    private int idPaiement;
    private LocalDateTime datePaiement;
    private BigDecimal montant;
    private ModePaiement modePaiement;
    private Commande commande;

    public Paiement() {
    }

    public Paiement(int idPaiement, LocalDateTime datePaiement, BigDecimal montant, ModePaiement modePaiement, Commande commande) {
        this.idPaiement = idPaiement;
        this.datePaiement = datePaiement;
        this.montant = montant;
        this.modePaiement = modePaiement;
        this.commande = commande;
    }

    public int getIdPaiement() {
        return idPaiement;
    }

    public void setIdPaiement(int idPaiement) {
        this.idPaiement = idPaiement;
    }

    public LocalDateTime getDatePaiement() {
        return datePaiement;
    }

    public void setDatePaiement(LocalDateTime datePaiement) {
        this.datePaiement = datePaiement;
    }

    public BigDecimal getMontant() {
        return montant;
    }

    public void setMontant(BigDecimal montant) {
        this.montant = montant;
    }

    public ModePaiement getModePaiement() {
        return modePaiement;
    }

    public void setModePaiement(ModePaiement modePaiement) {
        this.modePaiement = modePaiement;
    }

    public Commande getCommande() {
        return commande;
    }

    public void setCommande(Commande commande) {
        this.commande = commande;
    }
}

