using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Repositories.Interfaces
{
    public interface IBurgerRepository
    {
        Task<Burger?> GetByIdAsync(int id);
        Task<IEnumerable<Burger>> GetAllAsync();
        Task<IEnumerable<Burger>> GetActifsAsync();
        Task<Burger> AddAsync(Burger burger);
        Task UpdateAsync(Burger burger);
        Task DeleteAsync(int id);
        Task<bool> ArchiverAsync(int id);
    }
}
