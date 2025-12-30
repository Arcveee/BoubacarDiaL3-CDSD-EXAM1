using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Models.Enums;
using BrasilBurger.Web.Repositories.Interfaces;

namespace BrasilBurger.Web.Repositories.Implementations
{
    public class CommandeRepository : ICommandeRepository
    {
        private readonly ApplicationDbContext _context;

        public CommandeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Commande?> GetByIdAsync(int id)
        {
            return await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.LignesCommande)
                .Include(c => c.Zone)
                .Include(c => c.Paiement)
                .FirstOrDefaultAsync(c => c.IdCommande == id);
        }

        public async Task<IEnumerable<Commande>> GetAllAsync()
        {
            return await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.LignesCommande)
                .Include(c => c.Zone)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetByClientIdAsync(int clientId)
        {
            return await _context.Commandes
                .Include(c => c.LignesCommande)
                .Include(c => c.Zone)
                .Include(c => c.Paiement)
                .Where(c => c.IdClient == clientId)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();
        }

        public async Task<int> GetNombreCommandesClientAsync(int clientId)
        {
            return await _context.Commandes
                .Where(c => c.IdClient == clientId)
                .CountAsync();
        }

        public async Task<IEnumerable<Commande>> GetByEtatAsync(EtatCommande etat)
        {
            return await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.LignesCommande)
                .Where(c => c.EtatCommande == etat)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetByDateAsync(DateTime date)
        {
            return await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.LignesCommande)
                .Where(c => c.DateCommande.Date == date.Date)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();
        }

        public async Task<IEnumerable<Commande>> GetByZoneAsync(int zoneId)
        {
            return await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.LignesCommande)
                .Include(c => c.Zone)
                .Where(c => c.IdZone == zoneId && c.ModeConsommation == ModeConsommation.LIVRAISON)
                .OrderByDescending(c => c.DateCommande)
                .ToListAsync();
        }

        public async Task<Commande> AddAsync(Commande commande)
        {
            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();
            return commande;
        }

        public async Task UpdateAsync(Commande commande)
        {
            _context.Commandes.Update(commande);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Commande>> FiltrerCommandesAsync(DateTime? dateDebut, DateTime? dateFin,
            EtatCommande? etat, int? clientId, TypeProduit? typeProduit)
        {
            var query = _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.LignesCommande)
                .Include(c => c.Zone)
                .AsQueryable();

            if (dateDebut.HasValue)
                query = query.Where(c => c.DateCommande.Date >= dateDebut.Value.Date);

            if (dateFin.HasValue)
                query = query.Where(c => c.DateCommande.Date <= dateFin.Value.Date);

            if (etat.HasValue)
                query = query.Where(c => c.EtatCommande == etat.Value);

            if (clientId.HasValue)
                query = query.Where(c => c.IdClient == clientId.Value);

            if (typeProduit.HasValue)
                query = query.Where(c => c.LignesCommande.Any(lc => lc.TypeProduit == typeProduit.Value));

            return await query.OrderByDescending(c => c.DateCommande).ToListAsync();
        }
    }
}
