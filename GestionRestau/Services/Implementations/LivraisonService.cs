using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Repositories.Interfaces;
using BrasilBurger.Web.Services.Interfaces;
using BrasilBurger.Web.Data;

namespace BrasilBurger.Web.Services.Implementations
{
    public class LivraisonService : ILivraisonService
    {
        private readonly IZoneRepository _zoneRepository;
        private readonly ICommandeRepository _commandeRepository;
        private readonly ApplicationDbContext _context;

        public LivraisonService(
            IZoneRepository zoneRepository,
            ICommandeRepository commandeRepository,
            ApplicationDbContext context)
        {
            _zoneRepository = zoneRepository;
            _commandeRepository = commandeRepository;
            _context = context;
        }

        public async Task<Zone> AjouterZoneAsync(string nom, decimal prixLivraison)
        {
            var zone = new Zone
            {
                Nom = nom,
                PrixLivraison = prixLivraison
            };

            return await _zoneRepository.AddAsync(zone);
        }

        public async Task ModifierZoneAsync(int id, string nom, decimal prixLivraison)
        {
            var zone = await _zoneRepository.GetByIdAsync(id);
            if (zone == null) throw new Exception("Zone introuvable");

            zone.Nom = nom;
            zone.PrixLivraison = prixLivraison;

            await _zoneRepository.UpdateAsync(zone);
        }

        public async Task AjouterQuartierAsync(int zoneId, string nomQuartier)
        {
            var zone = await _zoneRepository.GetByIdAsync(zoneId);
            if (zone == null) throw new Exception("Zone introuvable");

            var quartier = new Quartier
            {
                Nom = nomQuartier,
                IdZone = zoneId
            };

            _context.Quartiers.Add(quartier);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Zone>> ListerZonesAsync()
        {
            return await _zoneRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Commande>> GetCommandesParZoneAsync(int zoneId)
        {
            return await _commandeRepository.GetByZoneAsync(zoneId);
        }
    }
}
