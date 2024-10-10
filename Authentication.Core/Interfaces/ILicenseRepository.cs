using Authentication.Core.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface ILicenseRepository : IGenericRepository<License>
    {
        Task<License> LoginAsync(string licenseKey,string hwid);
        Task<IEnumerable<License>> GetBannedLicensesAsync(string licenseKey);
        Task UpdateLicenseAsync(string adminLicenseKey, int licenseId, bool isBanned, bool isAdmin);
        Task<License> GenerateLicenseAsync(string adminLicenseKey);
        Task DeleteLicenseAsync(string adminLicenseKey, int licenseId);
    }
}
