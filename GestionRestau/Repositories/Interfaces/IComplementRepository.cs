using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Repositories.Interfaces
{
    public interface IComplementRepository
    {
        Task<Complement?> GetByIdAsync(int id);
        Task<IEnumerable<Complement>> GetAllAsync();
        Task<IEnumerable<Complement>> GetActifsAsync();
        Task<Complement> AddAsync(Complement complement);
        Task UpdateAsync(Complement complement);
        Task DeleteAsync(int id);
        Task<bool> ArchiverAsync(int id);
    }
}
