using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<Menu?> GetByIdAsync(int id);
        Task<IEnumerable<Menu>> GetAllAsync();
        Task<Menu> AddAsync(Menu menu);
        Task UpdateAsync(Menu menu);
        Task DeleteAsync(int id);
        Task<IEnumerable<Burger>> GetBurgersDuMenuAsync(int menuId);
    }
}
