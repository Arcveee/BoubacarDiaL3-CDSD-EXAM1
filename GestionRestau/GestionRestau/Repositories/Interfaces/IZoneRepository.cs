using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Repositories.Interfaces
{
    public interface IZoneRepository
    {
        Task<Zone?> GetByIdAsync(int id);
        Task<IEnumerable<Zone>> GetAllAsync();
        Task<Zone> AddAsync(Zone zone);
        Task UpdateAsync(Zone zone);
        Task DeleteAsync(int id);
    }
}
