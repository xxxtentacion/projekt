using Authentication.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface ILicenseStatsService
    {
        Task<LicenseStats> GetLicenseStatsByLicenseKeyAsync(string licenseKey);
        Task<IEnumerable<LicenseStats>> GetAllLicenseStatsAsync();
        Task AddLicenseStatsAsync(LicenseStats licenseStats);
        Task UpdateLicenseStatsAsync(LicenseStats licenseStats);
        Task DeleteLicenseStatsAsync(int id);
    }
}
