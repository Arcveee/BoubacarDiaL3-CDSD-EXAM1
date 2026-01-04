using Microsoft.EntityFrameworkCore;
using BrasilBurger.Web.Data;
using BrasilBurger.Web.Models.Entities;
using BrasilBurger.Web.Repositories.Interfaces;

namespace BrasilBurger.Web.Repositories.Implementations
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly ApplicationDbContext _context;

        public ZoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Zone?> GetByIdAsync(int id)
        {
            return await _context.Zones
                .Include(z => z.Quartiers)
                .FirstOrDefaultAsync(z => z.IdZone == id);
        }

        public async Task<IEnumerable<Zone>> GetAllAsync()
        {
            return await _context.Zones
                .Include(z => z.Quartiers)
                .ToListAsync();
        }

        public async Task<Zone> AddAsync(Zone zone)
        {
            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();
            return zone;
        }

        public async Task UpdateAsync(Zone zone)
        {
            _context.Zones.Update(zone);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var zone = await _context.Zones.FindAsync(id);
            if (zone != null)
            {
                _context.Zones.Remove(zone);
                await _context.SaveChangesAsync();
            }
        }
    }
}
