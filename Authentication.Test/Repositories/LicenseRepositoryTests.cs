using Authentication.Core.Database;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Authentication.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Test.Repositories
{
    public class LicenseRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly LicenseRepository _licenseRepository;

        public LicenseRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "LicenseRepositoryTests")
                .Options;

            _context = new(_options);

            _licenseRepository = new(_context);
        }

        // Create
        [Fact]
        public async Task AddLicense_Should_AddNewAdminLicenseToDatabase()
        {
            // Arrange
            var newLicense = new License
            {
                LicenseKey = "TestLicense123",
                IsBanned = false,
                IsAdmin = true
            };

            // Act
            await _licenseRepository.AddAsync(newLicense);
            await _context.SaveChangesAsync();

            // Assert
            var licenseInDb = await _licenseRepository.GetByIdAsync(newLicense.Id);
            Assert.NotNull(licenseInDb);
            Assert.Equal("TestLicense123", licenseInDb.LicenseKey);
        }

        // Read
        [Fact]
        public async Task GetByIdAsync_Should_ReturnCorrectLicense()
        {
            // Arrange
            var newLicense = new License
            {
                LicenseKey = "TestLicense456",
                IsBanned = false,
                IsAdmin = false
            };
            await _licenseRepository.AddAsync(newLicense);
            await _context.SaveChangesAsync();

            // Act
            var retrievedLicense = await _licenseRepository.GetByIdAsync(newLicense.Id);

            // Assert
            Assert.NotNull(retrievedLicense);
            Assert.Equal("TestLicense456", retrievedLicense.LicenseKey);
        }

        // Update
        [Fact]
        public async Task UpdateLicense_Should_UpdateLicenseInDatabase()
        {
            // Arrange
            var newLicense = new License
            {
                LicenseKey = "TestLicense789",
                IsBanned = false,
                IsAdmin = false
            };
            await _licenseRepository.AddAsync(newLicense);
            await _context.SaveChangesAsync();

            // Act
            newLicense.IsBanned = true;
            await _licenseRepository.UpdateAsync(newLicense);
            await _context.SaveChangesAsync();

            // Assert
            var updatedLicense = await _licenseRepository.GetByIdAsync(newLicense.Id);
            Assert.True(updatedLicense.IsBanned);
        }

        // Delete
        [Fact]
        public async Task DeleteLicense_Should_RemoveLicenseFromDatabase()
        {
            // Arrange
            var newLicense = new License
            {
                LicenseKey = "TestLicense987",
                IsBanned = false,
                IsAdmin = false
            };
            await _licenseRepository.AddAsync(newLicense);
            await _context.SaveChangesAsync();

            // Act
            await _licenseRepository.DeleteAsync(newLicense.Id);
            await _context.SaveChangesAsync();

            // Assert
            var deletedLicense = await _licenseRepository.GetByIdAsync(newLicense.Id);
            Assert.Null(deletedLicense);
        }

        [Fact]
        public async Task GetAllLicenses_Should_ReturnAllLicenses()
        {
            // Clear existing data in the Licenses table
            _context.Licenses.RemoveRange(_context.Licenses);
            await _context.SaveChangesAsync();

            // Arrange
            var license1 = new License { LicenseKey = "LicenseKey1", IsBanned = false, IsAdmin = false };
            var license2 = new License { LicenseKey = "LicenseKey2", IsBanned = true, IsAdmin = false };

            _context.Licenses.AddRange(license1, license2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _licenseRepository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());  // Check that both records are retrieved
            Assert.Contains(result, l => l.LicenseKey == "LicenseKey1");
            Assert.Contains(result, l => l.LicenseKey == "LicenseKey2");
        }
    }
}
