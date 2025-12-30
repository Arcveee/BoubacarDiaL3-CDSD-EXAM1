using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BrasilBurger.Web.Services.Interfaces;
using BrasilBurger.Web.Models.Enums;
using BrasilBurger.Web.Models.ViewModels;
using System.Linq;

namespace BrasilBurger.Web.Controllers
{
    public class CommandeController : Controller
    {
        private readonly ICommandeService _commandeService;
        private readonly ICatalogueService _catalogueService;
        private readonly ILivraisonService _livraisonService;
        private readonly IPaiementService _paiementService;

        public CommandeController(
            ICommandeService commandeService,
            ICatalogueService catalogueService,
            ILivraisonService livraisonService,
            IPaiementService paiementService)
        {
            _commandeService = commandeService;
            _catalogueService = catalogueService;
            _livraisonService = livraisonService;
            _paiementService = paiementService;
        }

        // POST: Commande/AjouterAuPanier
        [HttpPost]
        public async Task<IActionResult> AjouterAuPanier(int produitId, TypeProduit typeProduit, int quantite = 1, List<int>? complementIds = null)
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (clientId == null)
            {
                return RedirectToAction("Login", "Client");
            }

            // Récupérer ou créer la commande en cours
            var commandeId = HttpContext.Session.GetInt32("CommandeEnCoursId");
            if (commandeId == null)
            {
                var commande = await _commandeService.CreerCommandeAsync(clientId.Value);
                commandeId = commande.IdCommande;
                HttpContext.Session.SetInt32("CommandeEnCoursId", commandeId.Value);
            }

            // Ajouter le produit
            int? idBurger = typeProduit == TypeProduit.BURGER ? produitId : null;
            int? idMenu = typeProduit == TypeProduit.MENU ? produitId : null;

            await _commandeService.AjouterLigneCommandeAsync(
                commandeId.Value,
                quantite,
                typeProduit,
                idBurger,
                idMenu,
                null
            );

            // Ajouter les compléments si présents
            if (complementIds != null && complementIds.Any())
            {
                foreach (var complementId in complementIds)
                {
                    await _commandeService.AjouterLigneCommandeAsync(
                        commandeId.Value,
                        1,
                        TypeProduit.COMPLEMENT,
                        null,
                        null,
                        complementId
                    );
                }
            }

            TempData["Success"] = "Produit ajouté au panier !";
            return RedirectToAction("Panier");
        }

        // GET: Commande/Panier
        public async Task<IActionResult> Panier()
        {
            var commandeId = HttpContext.Session.GetInt32("CommandeEnCoursId");
            if (commandeId == null)
            {
                ViewBag.Message = "Votre panier est vide";
                return View(new PanierViewModel());
            }

            var commande = await _commandeService.GetCommandeByIdAsync(commandeId.Value);
            if (commande == null || !commande.LignesCommande.Any())
            {
                ViewBag.Message = "Votre panier est vide";
                return View(new PanierViewModel());
            }

            var viewModel = new PanierViewModel
            {
                Commande = commande,
                Lignes = new List<LigneCommandeViewModel>(),
                SousTotal = await _commandeService.CalculerTotalAsync(commandeId.Value),
                ZonesDisponibles = (await _livraisonService.ListerZonesAsync()).ToList()
            };

            foreach (var lc in commande.LignesCommande)
            {
                var nomProduit = await _commandeService.GetNomProduitAsync(lc);
                string? imageUrl = null;
                
                if (lc.TypeProduit == TypeProduit.BURGER)
                {
                    var burger = await _catalogueService.GetBurgerByIdAsync(lc.IdProduit);
                    imageUrl = burger?.Image;
                }
                else if (lc.TypeProduit == TypeProduit.MENU)
                {
                    var menu = await _catalogueService.GetMenuByIdAsync(lc.IdProduit);
                    imageUrl = menu?.Image;
                }
                else if (lc.TypeProduit == TypeProduit.COMPLEMENT)
                {
                    var complement = await _catalogueService.GetComplementByIdAsync(lc.IdProduit);
                    imageUrl = complement?.Image;
                }
                
                viewModel.Lignes.Add(new LigneCommandeViewModel
                {
                    IdLigne = lc.IdLigne,
                    NomProduit = nomProduit,
                    Quantite = lc.Quantite,
                    PrixUnitaire = lc.SousTotal / lc.Quantite,
                    SousTotal = lc.SousTotal,
                    TypeProduit = lc.TypeProduit,
                    ImageUrl = imageUrl
                });
            }

            viewModel.Total = viewModel.SousTotal;

            return View(viewModel);
        }

        // POST: Commande/ValiderPanier
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ValiderPanier(ModeConsommation modeConsommation, string? adresseLivraison, int? zoneId)
        {
            var commandeId = HttpContext.Session.GetInt32("CommandeEnCoursId");
            if (commandeId == null)
            {
                TempData["Error"] = "Aucune commande en cours";
                return RedirectToAction("Panier");
            }

            try
            {
                await _commandeService.ValiderCommandeAsync(commandeId.Value, modeConsommation, adresseLivraison, zoneId);
                return RedirectToAction("Paiement", new { id = commandeId.Value });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Panier");
            }
        }

        // GET: Commande/Paiement/5
        public async Task<IActionResult> Paiement(int id)
        {
            var commande = await _commandeService.GetCommandeByIdAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            return View(commande);
        }

        // POST: Commande/EffectuerPaiement
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EffectuerPaiement(int commandeId, ModePaiement modePaiement)
        {
            try
            {
                var paiement = await _paiementService.EffectuerPaiementAsync(commandeId, modePaiement);
                
                // Incrémenter le compteur de commandes de la session
                var compteurCommandes = HttpContext.Session.GetInt32("CompteurCommandes") ?? 0;
                HttpContext.Session.SetInt32("CompteurCommandes", compteurCommandes + 1);
                
                // Supprimer la commande en cours de la session
                HttpContext.Session.Remove("CommandeEnCoursId");

                TempData["Success"] = "Paiement effectué avec succès !";
                return RedirectToAction("Confirmation", new { id = commandeId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Paiement", new { id = commandeId });
            }
        }

        // GET: Commande/Confirmation/5
        public async Task<IActionResult> Confirmation(int id)
        {
            var commande = await _commandeService.GetCommandeByIdAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            var numeroCommande = HttpContext.Session.GetInt32("CompteurCommandes") ?? 1;
            ViewBag.NumeroCommande = numeroCommande;
            ViewBag.CommandeService = _commandeService;

            return View(commande);
        }
    }
}
