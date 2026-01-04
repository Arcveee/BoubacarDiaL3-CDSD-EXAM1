using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Repositories.Interfaces;

namespace BrasilBurger.Web.Repositories.Implementations
{
    public class ComplementRepository : IComplementRepository
    {
        private readonly ApplicationDbContext _context;

        public ComplementRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Complement?> GetByIdAsync(int id)
        {
            return await _context.Complements.FindAsync(id);
        }

        public async Task<IEnumerable<Complement>> GetAllAsync()
        {
            return await _context.Complements.ToListAsync();
        }

        public async Task<IEnumerable<Complement>> GetActifsAsync()
        {
            return await _context.Complements
                .Where(c => c.Actif)
                .ToListAsync();
        }

        public async Task<Complement> AddAsync(Complement complement)
        {
            _context.Complements.Add(complement);
            await _context.SaveChangesAsync();
            return complement;
        }

        public async Task UpdateAsync(Complement complement)
        {
            _context.Complements.Update(complement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var complement = await _context.Complements.FindAsync(id);
            if (complement != null)
            {
                _context.Complements.Remove(complement);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ArchiverAsync(int id)
        {
            var complement = await _context.Complements.FindAsync(id);
            if (complement != null)
            {
                complement.Actif = !complement.Actif;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
