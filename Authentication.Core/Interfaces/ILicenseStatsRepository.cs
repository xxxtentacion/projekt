using Authentication.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface ILicenseStatsRepository
    {
        Task<LicenseStats> GetByLicenseKeyAsync(string licenseKey);
        Task<IEnumerable<LicenseStats>> GetAllAsync();
        Task<LicenseStats> GetByIdAsync(int id);
        Task AddAsync(LicenseStats licenseStats);
        Task UpdateAsync(LicenseStats licenseStats);
        Task DeleteAsync(int id);
    }
}
