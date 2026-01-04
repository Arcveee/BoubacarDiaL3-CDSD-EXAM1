using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Services.Interfaces
{
    public interface ICatalogueService
    {
        Task<IEnumerable<Burger>> GetBurgersActifsAsync();
        Task<IEnumerable<Menu>> GetMenusAsync();
        Task<IEnumerable<Complement>> GetComplementsActifsAsync();
        Task<Burger?> GetBurgerByIdAsync(int id);
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<Complement?> GetComplementByIdAsync(int id);
    }
}
