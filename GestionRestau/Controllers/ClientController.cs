using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BrasilBurger.Web.Models.ViewModels;
using BrasilBurger.Web.Services.Interfaces;

namespace BrasilBurger.Web.Controllers
{
    public class ClientController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ICommandeService _commandeService;

        public ClientController(IAuthService authService, ICommandeService commandeService)
        {
            _authService = authService;
            _commandeService = commandeService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var client = await _authService.ConnexionClientAsync(model.Login, model.Password);

            if (client != null)
            {
                HttpContext.Session.SetInt32("ClientId", client.IdClient);
                HttpContext.Session.SetString("ClientNom", client.Prenom);
                TempData["Success"] = $"Bienvenue {client.Prenom} !";
                return RedirectToAction("Index", "Catalogue");
            }

            ModelState.AddModelError("", "Login ou mot de passe incorrect");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _authService.EmailExisteAsync(model.Email))
            {
                ModelState.AddModelError("Email", "Cet email est déjà utilisé");
                return View(model);
            }

            var client = await _authService.InscriptionClientAsync(
                model.Nom,
                model.Prenom,
                model.Telephone,
                model.Email,
                model.Password
            );

            if (client == null)
            {
                ModelState.AddModelError("", "Une erreur est survenue lors de l'inscription");
                return View(model);
            }

            TempData["Success"] = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> MesCommandes()
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (clientId == null)
            {
                return RedirectToAction("Login");
            }

            var commandes = await _commandeService.GetCommandesClientAsync(clientId.Value);
            return View(commandes);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsCommande(int id)
        {
            var clientId = HttpContext.Session.GetInt32("ClientId");
            if (clientId == null)
            {
                return RedirectToAction("Login");
            }

            var commande = await _commandeService.GetCommandeByIdAsync(id);

            if (commande == null || commande.IdClient != clientId.Value)
            {
                return NotFound();
            }

            var numeroCommande = await _commandeService.GetNumeroCommandeClientAsync(commande.IdClient, id);
            ViewBag.NumeroCommande = numeroCommande;
            ViewBag.CommandeService = _commandeService;

            return View(commande);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Déconnexion réussie";
            return RedirectToAction("Index", "Home");
        }
    }
}
