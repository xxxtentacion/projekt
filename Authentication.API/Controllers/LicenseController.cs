using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseController : ControllerBase
    {
        private readonly ILicenseRepository _licenseRepository;

        public LicenseController(ILicenseRepository licenseRepository)
        {
            _licenseRepository = licenseRepository;
        }

        // GET: api/License/GetBannedLicences
        [HttpGet("GetBannedLicences")]
        public async Task<IActionResult> GetBannedLicenses(string licenseKey)
        {
            var licenses = await _licenseRepository.GetBannedLicensesAsync(licenseKey);
            return Ok(licenses);
        }

        // GET: api/License/GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllLicenses()
        {
            var licenses = await _licenseRepository.GetAllAsync();
            return Ok(licenses);
        }

        // PUT: api/License/Update
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateLicense([FromQuery] string adminLicenseKey, [FromQuery] int licenseId, [FromForm] bool isBanned, [FromForm] bool isAdmin)
        {
            try
            {
                // Ensure only admins can update licenses
                await _licenseRepository.UpdateLicenseAsync(adminLicenseKey, licenseId, isBanned, isAdmin);
                return Ok(new { Message = "License updated successfully." });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { Message = "Admin privileges are required to update licenses." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "License not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        // POST: api/License/Generate
        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateLicense([FromQuery] string adminLicenseKey)
        {
            try
            {
                // Call the repository method to generate the license
                var newLicense = await _licenseRepository.GenerateLicenseAsync(adminLicenseKey);

                // Return success response with the newly generated license key
                return Ok(new { Message = "License generated successfully.", LicenseKey = newLicense.LicenseKey });
            }
            catch (UnauthorizedAccessException ex)
            {
                // If admin validation fails, return Unauthorized response
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                // Catch any other errors and return a server error response
                return StatusCode(500, new { Message = "An error occurred while generating the license.", Details = ex.Message });
            }
        }

        // DELETE: api/License/Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteLicense([FromQuery] string adminLicenseKey, int licenseId)
        {
            try
            {
                await _licenseRepository.DeleteLicenseAsync(adminLicenseKey, licenseId);
                return Ok(new { Message = "License deleted successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "License not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred.", Details = ex.Message });
            }
        }

        // POST: api/License/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromQuery] string licenseKey, [FromQuery] string hwid)
        {
            var license = await _licenseRepository.LoginAsync(licenseKey, hwid);
            if (license == null)
            {
                return Unauthorized(new { Message = "HWID Mismatch, Invalid or banned license key." });
            }

            return Ok(new
            {
                Message = "Login successful.",
                LicenseKey = license.LicenseKey,
                IsAdmin = license.IsAdmin
            });
        }
    }
}
