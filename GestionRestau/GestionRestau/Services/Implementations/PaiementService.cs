using BrasilBurger.Web.Data;
using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Models.Enums;
using BrasilBurger.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BrasilBurger.Web.Services.Implementations
{
    public class PaiementService : IPaiementService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommandeService _commandeService;

        public PaiementService(ApplicationDbContext context, ICommandeService commandeService)
        {
            _context = context;
            _commandeService = commandeService;
        }

        public async Task<Paiement> EffectuerPaiementAsync(int commandeId, ModePaiement modePaiement)
        {
            var commande = await _commandeService.GetCommandeByIdAsync(commandeId);
            if (commande == null) throw new Exception("Commande introuvable");

            var paiementExistant = await _context.Paiements
                .FirstOrDefaultAsync(p => p.IdCommande == commandeId);

            if (paiementExistant != null)
            {
                throw new Exception("Cette commande a déjà été payée");
            }

            var paiement = new Paiement
            {
                IdCommande = commandeId,
                Montant = commande.Total,
                ModePaiement = modePaiement,
                DatePaiement = DateTime.UtcNow
            };

            _context.Paiements.Add(paiement);
            await _context.SaveChangesAsync();

            await _commandeService.ChangerEtatCommandeAsync(commandeId, EtatCommande.EN_COURS);

            return paiement;
        }

        public async Task<bool> ValiderPaiementAsync(int paiementId)
        {
            var paiement = await _context.Paiements.FindAsync(paiementId);
            return paiement != null;
        }
    }
}
