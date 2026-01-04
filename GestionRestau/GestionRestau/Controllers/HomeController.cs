using Microsoft.AspNetCore.Mvc;
using BrasilBurger.Web.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace BrasilBurger.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICatalogueService _catalogueService;

        public HomeController(ICatalogueService catalogueService)
        {
            _catalogueService = catalogueService;
        }

        // Endpoint temporaire pour générer les hash
        public IActionResult Hash(string password)
        {
            if (string.IsNullOrEmpty(password))
                return Content("Usage: /Home/Hash?password=votremotdepasse");
            
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = Convert.ToBase64String(hashedBytes);
            return Content($"Password: {password}\nHash: {hash}");
        }

        public async Task<IActionResult> Index()
        {
            var burgers = await _catalogueService.GetBurgersActifsAsync();
            var menus = await _catalogueService.GetMenusAsync();

            ViewBag.Burgers = burgers;
            ViewBag.Menus = menus;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
