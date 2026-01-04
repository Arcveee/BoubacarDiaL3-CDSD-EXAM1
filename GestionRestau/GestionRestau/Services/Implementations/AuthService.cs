using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Repositories.Interfaces;
using BrasilBurger.Web.Services.Interfaces;

namespace BrasilBurger.Web.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IClientRepository _clientRepository;

        public AuthService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<Client?> InscriptionClientAsync(string nom, string prenom, string telephone, string email, string password)
        {
            if (await _clientRepository.EmailExistsAsync(email))
            {
                return null;
            }

            var client = new Client
            {
                Nom = nom,
                Prenom = prenom,
                Telephone = telephone,
                Email = email,
                Password = password
            };

            return await _clientRepository.AddAsync(client);
        }

        public async Task<Client?> ConnexionClientAsync(string login, string password)
        {
            var client = await _clientRepository.GetByPrenomAsync(login);
            
            if (client == null || client.Password != password)
            {
                return null;
            }

            return client;
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _clientRepository.EmailExistsAsync(email);
        }
    }
}
