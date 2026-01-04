using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Models.Enums;

namespace BrasilBurger.Web.Services.Interfaces
{
    public interface IPaiementService
    {
        Task<Paiement> EffectuerPaiementAsync(int commandeId, ModePaiement modePaiement);
        Task<bool> ValiderPaiementAsync(int paiementId);
    }
}
