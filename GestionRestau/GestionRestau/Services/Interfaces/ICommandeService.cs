using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Models.Enums;

namespace BrasilBurger.Web.Services.Interfaces
{
    public interface ICommandeService
    {
        Task<Commande> CreerCommandeAsync(int clientId);
        Task AjouterLigneCommandeAsync(int commandeId, int quantite, TypeProduit typeProduit, int? idBurger, int? idMenu, int? idComplement);
        Task<decimal> CalculerTotalAsync(int commandeId);
        Task ValiderCommandeAsync(int commandeId, ModeConsommation modeConsommation, string? adresseLivraison, int? zoneId);
        Task<IEnumerable<Commande>> GetCommandesClientAsync(int clientId);
        Task<IEnumerable<Commande>> GetToutesCommandesAsync();
        Task<Commande?> GetCommandeByIdAsync(int id);
        Task ChangerEtatCommandeAsync(int commandeId, EtatCommande nouvelEtat);
        Task AnnulerCommandeAsync(int commandeId);
        Task<IEnumerable<Commande>> FiltrerCommandesAsync(DateTime? dateDebut, DateTime? dateFin, EtatCommande? etat, int? clientId, TypeProduit? typeProduit);
        Task<string> GetNomProduitAsync(LigneCommande ligne);
        Task<int> GetNumeroCommandeClientAsync(int clientId, int commandeId);
    }
}
