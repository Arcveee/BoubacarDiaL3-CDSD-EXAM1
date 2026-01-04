using BrasilBurger.Web.Models.Entities;

namespace BrasilBurger.Web.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Client?> InscriptionClientAsync(string nom, string prenom, string telephone, string email, string password);
        Task<Client?> ConnexionClientAsync(string login, string password);
        Task<bool> EmailExisteAsync(string email);
    }
}
