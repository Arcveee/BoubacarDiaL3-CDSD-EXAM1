using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Repositories.Interfaces;
using BrasilBurger.Web.Services.Interfaces;

namespace BrasilBurger.Web.Services.Implementations
{
    public class CatalogueService : ICatalogueService
    {
        private readonly IBurgerRepository _burgerRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IComplementRepository _complementRepository;

        public CatalogueService(
            IBurgerRepository burgerRepository,
            IMenuRepository menuRepository,
            IComplementRepository complementRepository)
        {
            _burgerRepository = burgerRepository;
            _menuRepository = menuRepository;
            _complementRepository = complementRepository;
        }

        public async Task<IEnumerable<Burger>> GetBurgersActifsAsync()
        {
            return await _burgerRepository.GetActifsAsync();
        }

        public async Task<IEnumerable<Menu>> GetMenusAsync()
        {
            return await _menuRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Complement>> GetComplementsActifsAsync()
        {
            return await _complementRepository.GetActifsAsync();
        }

        public async Task<Burger?> GetBurgerByIdAsync(int id)
        {
            return await _burgerRepository.GetByIdAsync(id);
        }

        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            return await _menuRepository.GetByIdAsync(id);
        }

        public async Task<Complement?> GetComplementByIdAsync(int id)
        {
            return await _complementRepository.GetByIdAsync(id);
        }
    }
}
