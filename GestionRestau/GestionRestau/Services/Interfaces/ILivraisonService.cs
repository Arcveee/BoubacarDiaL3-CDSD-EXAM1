using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Services.Interfaces
{
    public interface ILivraisonService
    {
        Task<Zone> AjouterZoneAsync(string nom, decimal prixLivraison);
        Task ModifierZoneAsync(int id, string nom, decimal prixLivraison);
        Task AjouterQuartierAsync(int zoneId, string nomQuartier);
        Task<IEnumerable<Zone>> ListerZonesAsync();
        Task<IEnumerable<Commande>> GetCommandesParZoneAsync(int zoneId);
    }
}
