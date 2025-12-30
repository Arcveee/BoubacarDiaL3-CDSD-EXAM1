using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Models.Enums;
using BrasilBurger.Web.Repositories.Interfaces;
using BrasilBurger.Web.Services.Interfaces;
using BrasilBurger.Web.Data;

namespace BrasilBurger.Web.Services.Implementations
{
    public class CommandeService : ICommandeService
    {
        private readonly ICommandeRepository _commandeRepository;
        private readonly IBurgerRepository _burgerRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IComplementRepository _complementRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly ApplicationDbContext _context;

        public CommandeService(
            ICommandeRepository commandeRepository,
            IBurgerRepository burgerRepository,
            IMenuRepository menuRepository,
            IComplementRepository complementRepository,
            IZoneRepository zoneRepository,
            ApplicationDbContext context)
        {
            _commandeRepository = commandeRepository;
            _burgerRepository = burgerRepository;
            _menuRepository = menuRepository;
            _complementRepository = complementRepository;
            _zoneRepository = zoneRepository;
            _context = context;
        }

        public async Task<Commande> CreerCommandeAsync(int clientId)
        {
            var commande = new Commande
            {
                IdClient = clientId,
                DateCommande = DateTime.UtcNow,
                EtatCommande = EtatCommande.EN_COURS,
                Total = 0
            };

            return await _commandeRepository.AddAsync(commande);
        }

        public async Task AjouterLigneCommandeAsync(int commandeId, int quantite, TypeProduit typeProduit, 
            int? idBurger, int? idMenu, int? idComplement)
        {
            int idProduit = idBurger ?? idMenu ?? idComplement ?? 0;
            decimal prixUnitaire = 0;

            if (typeProduit == TypeProduit.BURGER && idBurger.HasValue)
            {
                var burger = await _burgerRepository.GetByIdAsync(idBurger.Value);
                prixUnitaire = burger?.Prix ?? 0;
            }
            else if (typeProduit == TypeProduit.MENU && idMenu.HasValue)
            {
                var menu = await _menuRepository.GetByIdAsync(idMenu.Value);
                prixUnitaire = menu?.Prix ?? 0;
            }
            else if (typeProduit == TypeProduit.COMPLEMENT && idComplement.HasValue)
            {
                var complement = await _complementRepository.GetByIdAsync(idComplement.Value);
                prixUnitaire = complement?.Prix ?? 0;
            }

            var ligneCommande = new LigneCommande
            {
                IdCommande = commandeId,
                Quantite = quantite,
                TypeProduit = typeProduit,
                IdProduit = idProduit,
                SousTotal = prixUnitaire * quantite
            };

            _context.LignesCommande.Add(ligneCommande);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalculerTotalAsync(int commandeId)
        {
            var commande = await _commandeRepository.GetByIdAsync(commandeId);
            if (commande == null) return 0;

            decimal total = commande.LignesCommande.Sum(l => l.SousTotal);

            if (commande.ModeConsommation == ModeConsommation.LIVRAISON && commande.IdZone.HasValue)
            {
                var zone = await _zoneRepository.GetByIdAsync(commande.IdZone.Value);
                if (zone != null)
                {
                    total += zone.PrixLivraison;
                }
            }

            commande.Total = total;
            await _commandeRepository.UpdateAsync(commande);

            return total;
        }

        public async Task ValiderCommandeAsync(int commandeId, ModeConsommation modeConsommation, 
            string? adresseLivraison, int? zoneId)
        {
            var commande = await _commandeRepository.GetByIdAsync(commandeId);
            if (commande == null) throw new Exception("Commande introuvable");

            if (modeConsommation == ModeConsommation.LIVRAISON && !zoneId.HasValue)
            {
                throw new Exception("La zone de livraison est obligatoire pour une livraison");
            }

            commande.ModeConsommation = modeConsommation;
            commande.AdresseLivraison = adresseLivraison;
            commande.IdZone = zoneId;
            commande.EtatCommande = EtatCommande.EN_COURS;

            await CalculerTotalAsync(commandeId);
        }

        public async Task<IEnumerable<Commande>> GetCommandesClientAsync(int clientId)
        {
            return await _commandeRepository.GetByClientIdAsync(clientId);
        }

        public async Task<IEnumerable<Commande>> GetToutesCommandesAsync()
        {
            return await _commandeRepository.GetAllAsync();
        }

        public async Task<Commande?> GetCommandeByIdAsync(int id)
        {
            return await _commandeRepository.GetByIdAsync(id);
        }

        public async Task ChangerEtatCommandeAsync(int commandeId, EtatCommande nouvelEtat)
        {
            var commande = await _commandeRepository.GetByIdAsync(commandeId);
            if (commande == null) throw new Exception("Commande introuvable");

            commande.EtatCommande = nouvelEtat;
            await _commandeRepository.UpdateAsync(commande);
        }

        public async Task AnnulerCommandeAsync(int commandeId)
        {
            await ChangerEtatCommandeAsync(commandeId, EtatCommande.ANNULEE);
        }

        public async Task<IEnumerable<Commande>> FiltrerCommandesAsync(DateTime? dateDebut, DateTime? dateFin, 
            EtatCommande? etat, int? clientId, TypeProduit? typeProduit)
        {
            return await _commandeRepository.FiltrerCommandesAsync(dateDebut, dateFin, etat, clientId, typeProduit);
        }

        public async Task<string> GetNomProduitAsync(LigneCommande ligne)
        {
            switch (ligne.TypeProduit)
            {
                case TypeProduit.BURGER:
                    var burger = await _burgerRepository.GetByIdAsync(ligne.IdProduit);
                    return burger?.Nom ?? "Burger inconnu";
                case TypeProduit.MENU:
                    var menu = await _menuRepository.GetByIdAsync(ligne.IdProduit);
                    return menu?.Nom ?? "Menu inconnu";
                case TypeProduit.COMPLEMENT:
                    var complement = await _complementRepository.GetByIdAsync(ligne.IdProduit);
                    return complement?.Nom ?? "Complément inconnu";
                default:
                    return "Produit inconnu";
            }
        }

        public async Task<int> GetNumeroCommandeClientAsync(int clientId, int commandeId)
        {
            // Récupérer toutes les commandes du client pour cette session
            var commandesClient = await _commandeRepository.GetByClientIdAsync(clientId);
            var commandesOrdered = commandesClient.OrderBy(c => c.DateCommande).ToList();
            var index = commandesOrdered.FindIndex(c => c.IdCommande == commandeId);
            return index >= 0 ? index + 1 : 1;
        }
    }
}
