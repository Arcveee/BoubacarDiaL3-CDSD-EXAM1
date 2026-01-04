using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<Client?> GetByIdAsync(int id);
        Task<Client?> GetByEmailAsync(string email);
        Task<Client?> GetByPrenomAsync(string prenom);
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client> AddAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(int id);
        Task<bool> EmailExistsAsync(string email);
    }
}
