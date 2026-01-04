using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Repositories.Interfaces;

namespace BrasilBurger.Web.Repositories.Implementations
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _context;

        public MenuRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Menu?> GetByIdAsync(int id)
        {
            return await _context.Menus
                .Include(m => m.MenuBurgers)
                    .ThenInclude(mb => mb.Burger)
                .FirstOrDefaultAsync(m => m.IdMenu == id);
        }

        public async Task<IEnumerable<Menu>> GetAllAsync()
        {
            return await _context.Menus
                .Include(m => m.MenuBurgers)
                    .ThenInclude(mb => mb.Burger)
                .ToListAsync();
        }

        public async Task<Menu> AddAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task UpdateAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu != null)
            {
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Burger>> GetBurgersDuMenuAsync(int menuId)
        {
            return await _context.MenuBurgers
                .Where(mb => mb.IdMenu == menuId)
                .Include(mb => mb.Burger)
                .Select(mb => mb.Burger)
                .ToListAsync();
        }
    }
}
