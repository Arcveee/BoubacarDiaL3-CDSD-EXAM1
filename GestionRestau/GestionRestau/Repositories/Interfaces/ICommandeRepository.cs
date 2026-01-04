using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Models.Enums;

namespace BrasilBurger.Web.Repositories.Interfaces
{
    public interface ICommandeRepository
    {
        Task<Commande?> GetByIdAsync(int id);
        Task<IEnumerable<Commande>> GetAllAsync();
        Task<IEnumerable<Commande>> GetByClientIdAsync(int clientId);
        Task<IEnumerable<Commande>> GetByEtatAsync(EtatCommande etat);
        Task<IEnumerable<Commande>> GetByDateAsync(DateTime date);
        Task<IEnumerable<Commande>> GetByZoneAsync(int zoneId);
        Task<Commande> AddAsync(Commande commande);
        Task UpdateAsync(Commande commande);
        Task DeleteAsync(int id);
        Task<int> GetNombreCommandesClientAsync(int clientId);
        Task<IEnumerable<Commande>> FiltrerCommandesAsync(DateTime? dateDebut, DateTime? dateFin, 
            EtatCommande? etat, int? clientId, TypeProduit? typeProduit);
    }
}
