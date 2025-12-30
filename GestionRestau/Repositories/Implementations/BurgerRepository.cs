using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Repositories.Interfaces;

namespace BrasilBurger.Web.Repositories.Implementations
{
    public class BurgerRepository : IBurgerRepository
    {
        private readonly ApplicationDbContext _context;

        public BurgerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Burger?> GetByIdAsync(int id)
        {
            return await _context.Burgers
                .Include(b => b.MenuBurgers)
                .FirstOrDefaultAsync(b => b.IdBurger == id);
        }

        public async Task<IEnumerable<Burger>> GetAllAsync()
        {
            return await _context.Burgers.ToListAsync();
        }

        public async Task<IEnumerable<Burger>> GetActifsAsync()
        {
            return await _context.Burgers
                .Where(b => b.Actif)
                .ToListAsync();
        }

        public async Task<Burger> AddAsync(Burger burger)
        {
            _context.Burgers.Add(burger);
            await _context.SaveChangesAsync();
            return burger;
        }

        public async Task UpdateAsync(Burger burger)
        {
            _context.Burgers.Update(burger);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var burger = await _context.Burgers.FindAsync(id);
            if (burger != null)
            {
                _context.Burgers.Remove(burger);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ArchiverAsync(int id)
        {
            var burger = await _context.Burgers.FindAsync(id);
            if (burger != null)
            {
                burger.Actif = !burger.Actif;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
