using Authentication.Core.Database;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Repository
{
    public class LicenseStatsRepository : ILicenseStatsRepository
    {
        private readonly DatabaseContext _context;

        public LicenseStatsRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<LicenseStats> GetByLicenseKeyAsync(string licenseKey)
        {
            // Find the LicenseStats by LicenseKey
            return await _context.LicenseStats
                .FirstOrDefaultAsync(ls => ls.LicenseKey == licenseKey);
        }

        public async Task<IEnumerable<LicenseStats>> GetAllAsync()
        {
            return await _context.LicenseStats.ToListAsync();
        }

        public async Task<LicenseStats> GetByIdAsync(int id)
        {
            return await _context.LicenseStats.FindAsync(id);
        }

        public async Task AddAsync(LicenseStats entity)
        {
            await _context.LicenseStats.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LicenseStats entity)
        {
            _context.LicenseStats.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var licenseStat = await _context.LicenseStats.FindAsync(id);
            if (licenseStat != null)
            {
                _context.LicenseStats.Remove(licenseStat);
                await _context.SaveChangesAsync();
            }
        }
    }
}
