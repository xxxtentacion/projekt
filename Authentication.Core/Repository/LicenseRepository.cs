using Authentication.Core.Database;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Authentication.Core.Repository
{
    public class LicenseRepository : IRepository<License>, ILicenseRepository
    {
        private readonly DatabaseContext _context;

        public LicenseRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public async Task<License> LoginAsync(string licenseKey, string hwid)
        {
            var license = await _context.Licenses
                                        .FirstOrDefaultAsync(l => l.LicenseKey == licenseKey && !l.IsBanned);

            if (license == null)
            {
                // License banned eller findes ikke
                return null;
            }

            if (string.IsNullOrEmpty(license.HWID))
            {
                // First login, assign the HWID
                license.HWID = hwid;
                _context.Licenses.Update(license);
                await _context.SaveChangesAsync();
            }
            else if (license.HWID != hwid)
            {
                // HWID doesn't match, reject login
                return null;
            }

            // Successful login
            return license;
        }

        public async Task<IEnumerable<License>> GetBannedLicensesAsync(string licenseKey)
        {
            // Verify if the provided license key is an admin
            var adminLicense = await _context.Licenses.FirstOrDefaultAsync(l => l.LicenseKey == licenseKey && l.IsAdmin == true);

            if (adminLicense == null)
            {
                // The provided license key does not belong to an admin
                throw new UnauthorizedAccessException("Admin privileges are required to update license details.");
            }
            else
            {
                return await _context.Licenses
                                 .Where(l => l.IsBanned)
                                 .ToListAsync();
            }
        }

        public async Task UpdateLicenseAsync(string adminLicenseKey, int licenseId, bool isBanned, bool isAdmin)
        {
            // Verify if the provided license key is an admin
            var adminLicense = await _context.Licenses.FirstOrDefaultAsync(l => l.LicenseKey == adminLicenseKey && l.IsAdmin == true);

            if (adminLicense == null)
            {
                // The provided license key does not belong to an admin
                throw new UnauthorizedAccessException("Admin privileges are required to update license details.");
            }

            // Fetch the license that needs to be updated
            var licenseToUpdate = await _context.Licenses.FirstOrDefaultAsync(l => l.Id == licenseId);

            if (licenseToUpdate != null)
            {
                // Update the license details
                licenseToUpdate.IsBanned = isBanned;
                licenseToUpdate.IsAdmin = isAdmin;

                _context.Entry(licenseToUpdate).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("License not found.");
            }
        }

        public async Task<License> GenerateLicenseAsync(string adminLicenseKey)
        {
            // Verify if the provided license key is an admin
            var adminLicense = await _context.Licenses.FirstOrDefaultAsync(l => l.LicenseKey == adminLicenseKey && l.IsAdmin == true);

            if (adminLicense == null)
            {
                // The provided license key does not belong to an admin
                throw new UnauthorizedAccessException("Admin privileges are required to generate new licenses.");
            }




            var newLicense = new License
            {
                LicenseKey = Guid.NewGuid().ToString(), // Generate a new unique license key
                IsBanned = false,
                IsAdmin = false,
                HWID = null
                
            };

            await _context.Licenses.AddAsync(newLicense);
            await _context.SaveChangesAsync();

            return newLicense;
        }

        public async Task DeleteLicenseAsync(string adminLicenseKey, int licenseId)
        {
            // Verify if the provided license key is an admin
            var adminLicense = await _context.Licenses.FirstOrDefaultAsync(l => l.LicenseKey == adminLicenseKey && l.IsAdmin == true);

            if (adminLicense == null)
            {
                // The provided license key does not belong to an admin
                throw new UnauthorizedAccessException("Admin privileges are required to delete licenses.");
            }

            // Fetch the license that needs to be deleted
            var licenseToRemove = await _context.Licenses.FirstOrDefaultAsync(l => l.Id == licenseId);

            if (licenseToRemove != null)
            {
                _context.Licenses.Remove(licenseToRemove);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("License not found.");
            }
        }
    }
}
