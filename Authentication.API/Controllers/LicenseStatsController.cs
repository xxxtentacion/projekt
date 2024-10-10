using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseStatsController : ControllerBase
    {
        private readonly ILicenseStatsRepository _licenseStatsRepository;

        public LicenseStatsController(ILicenseStatsRepository licenseStatsRepository)
        {
            _licenseStatsRepository = licenseStatsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLicenseStats()
        {
            var licenseStats = await _licenseStatsRepository.GetAllAsync();
            return Ok(licenseStats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenseStatById(int id)
        {
            var licenseStat = await _licenseStatsRepository.GetByIdAsync(id);
            if (licenseStat == null) return NotFound();
            return Ok(licenseStat);
        }

        [HttpGet("by-license/{licenseKey}")]
        public async Task<IActionResult> GetLicenseStatByLicenseKey(string licenseKey)
        {
            var licenseStat = await _licenseStatsRepository.GetByLicenseKeyAsync(licenseKey);
            if (licenseStat == null) return NotFound();
            return Ok(licenseStat);
        }


        [HttpPost]
        public async Task<IActionResult> CreateLicenseStat(LicenseStats licenseStat)
        {
            await _licenseStatsRepository.AddAsync(licenseStat);
            return CreatedAtAction(nameof(GetLicenseStatById), new { id = licenseStat.Id }, licenseStat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicenseStat(int id, LicenseStats licenseStat)
        {
            if (id != licenseStat.Id) return BadRequest();
            await _licenseStatsRepository.UpdateAsync(licenseStat);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicenseStat(int id)
        {
            await _licenseStatsRepository.DeleteAsync(id);
            return NoContent();
        }
    }

}
