using Microsoft.AspNetCore.Mvc;
using BrasilBurger.Web.Services.Interfaces;

namespace BrasilBurger.Web.Controllers
{
    public class CatalogueController : Controller
    {
        private readonly ICatalogueService _catalogueService;

        public CatalogueController(ICatalogueService catalogueService)
        {
            _catalogueService = catalogueService;
        }

        // GET: Catalogue
        public async Task<IActionResult> Index(string? filtre)
        {
            ViewBag.Filtre = filtre;

            if (filtre == "burgers")
            {
                var burgers = await _catalogueService.GetBurgersActifsAsync();
                ViewBag.Produits = burgers;
                ViewBag.TypeProduit = "Burgers";
            }
            else if (filtre == "menus")
            {
                var menus = await _catalogueService.GetMenusAsync();
                ViewBag.Produits = menus;
                ViewBag.TypeProduit = "Menus";
            }
            else
            {
                var burgers = await _catalogueService.GetBurgersActifsAsync();
                var menus = await _catalogueService.GetMenusAsync();
                ViewBag.Burgers = burgers;
                ViewBag.Menus = menus;
                ViewBag.TypeProduit = "Tous";
            }

            return View();
        }

        // GET: Catalogue/DetailsBurger/5
        public async Task<IActionResult> DetailsBurger(int id)
        {
            var burger = await _catalogueService.GetBurgerByIdAsync(id);
            if (burger == null)
            {
                return NotFound();
            }

            var complements = await _catalogueService.GetComplementsActifsAsync();
            ViewBag.Complements = complements;

            return View(burger);
        }

        // GET: Catalogue/DetailsMenu/5
        public async Task<IActionResult> DetailsMenu(int id)
        {
            var menu = await _catalogueService.GetMenuByIdAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }
    }
}
