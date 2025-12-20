package brasilburger.services;

import brasilburger.models.*;
import brasilburger.models.enums.EtatCommande;
import brasilburger.models.enums.ModeConsommation;
import brasilburger.repositories.CommandeRepository;
import brasilburger.repositories.PaiementRepository;

import java.math.BigDecimal;
import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

public class CommandeService {
    private final CommandeRepository commandeRepository;
    private final PaiementRepository paiementRepository;

    public CommandeService(CommandeRepository commandeRepository, PaiementRepository paiementRepository) {
        this.commandeRepository = commandeRepository;
        this.paiementRepository = paiementRepository;
    }

    public List<Commande> listerToutes() {
        return commandeRepository.listerToutes();
    }

    public List<Commande> filtrer(LocalDate debut, LocalDate fin, EtatCommande etat, Integer idClient, String typeProduit) {
        return commandeRepository.filtrer(debut, fin, etat, idClient, typeProduit);
    }

    public Commande details(int id) {
        return commandeRepository.trouverAvecLignes(id);
    }

    public void changerEtat(int idCommande, EtatCommande nouvelEtat) {
        commandeRepository.changerEtat(idCommande, nouvelEtat);
    }

    public void annuler(int idCommande) {
        commandeRepository.annuler(idCommande);
    }

    public void payer(Commande commande, BigDecimal montant, brasilburger.models.enums.ModePaiement mode) {
        if (paiementRepository.existePourCommande(commande.getIdCommande())) {
            throw new IllegalStateException("Commande déjà payée");
        }
        Paiement paiement = new Paiement();
        paiement.setCommande(commande);
        paiement.setDatePaiement(LocalDateTime.now());
        paiement.setMontant(montant);
        paiement.setModePaiement(mode);
        paiementRepository.creer(paiement);
        commandeRepository.changerEtat(commande.getIdCommande(), EtatCommande.VALIDEE);
        // bump session version so client menus are reset after validation
        SessionService.bumpSessionVersion();
    }

    public Commande creerDepuisPanier(List<LigneCommande> lignes, ModeConsommation modeConsommation, Zone zone, String adresse) {
        if (lignes == null || lignes.isEmpty()) {
            throw new IllegalStateException("Panier vide");
        }
        if (modeConsommation == ModeConsommation.LIVRAISON && zone == null) {
            throw new IllegalArgumentException("Zone obligatoire pour la livraison");
        }
        if (!brasilburger.services.SessionService.estClientConnecte()) {
            throw new IllegalStateException("Client non connecté");
        }
        Commande commande = new Commande();
        commande.setDateCommande(LocalDateTime.now());
        commande.setModeConsommation(modeConsommation);
        commande.setEtatCommande(EtatCommande.EN_COURS);
        commande.setAdresseLivraison(adresse);
        commande.setClient(brasilburger.services.SessionService.getClientConnecte());
        commande.setZone(zone);
        for (LigneCommande lc : lignes) {
            commande.ajouterLigne(lc);
        }
        commande.calculerTotal();
        return commandeRepository.creer(commande, lignes);
    }
}


