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
    public class LicenseStatsRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly LicenseStatsRepository _licenseStatsRepository;

        public LicenseStatsRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "LicenseStatsRepositoryTests")
                .Options;

            _context = new(_options);
            _licenseStatsRepository = new(_context);
        }

        [Fact]
        public async Task AddLicenseStats_Should_AddNewLicenseStatsToDatabase()
        {
            // Arrange
            var newLicenseStats = new LicenseStats
            {
                LicenseKey = "TestLicense123",
                GamesPlayed = 20,
                GamesWon = 10,
                GamesLost = 10
            };

            // Act
            await _licenseStatsRepository.AddAsync(newLicenseStats);
            await _context.SaveChangesAsync();

            // Assert
            var statsInDb = await _licenseStatsRepository.GetByIdAsync(newLicenseStats.Id);
            Assert.NotNull(statsInDb);
            Assert.Equal(20, statsInDb.GamesPlayed);
            Assert.Equal(10, statsInDb.GamesWon);
        }

        [Fact]
        public async Task UpdateLicenseStats_Should_UpdateExistingLicenseStats()
        {
            // Arrange
            var licenseStats = new LicenseStats
            {
                LicenseKey = "TestLicense123",
                GamesPlayed = 20,
                GamesWon = 10,
                GamesLost = 10
            };

            _context.LicenseStats.Add(licenseStats);
            await _context.SaveChangesAsync();

            // Act
            licenseStats.GamesPlayed = 30; // Update games played
            licenseStats.GamesWon = 15;    // Update games won
            await _licenseStatsRepository.UpdateAsync(licenseStats);
            await _context.SaveChangesAsync();

            // Assert
            var updatedStats = await _licenseStatsRepository.GetByIdAsync(licenseStats.Id);
            Assert.Equal(30, updatedStats.GamesPlayed);
            Assert.Equal(15, updatedStats.GamesWon);
        }

        [Fact]
        public async Task DeleteLicenseStats_Should_RemoveLicenseStatsFromDatabase()
        {
            // Arrange
            var licenseStats = new LicenseStats
            {
                LicenseKey = "TestLicense123",
                GamesPlayed = 20,
                GamesWon = 10,
                GamesLost = 10
            };

            _context.LicenseStats.Add(licenseStats);
            await _context.SaveChangesAsync();

            // Act
            await _licenseStatsRepository.DeleteAsync(licenseStats.Id);
            await _context.SaveChangesAsync();

            // Assert
            var deletedStats = await _licenseStatsRepository.GetByIdAsync(licenseStats.Id);
            Assert.Null(deletedStats);  // Assert that the record is deleted
        }

        [Fact]
        public async Task GetLicenseStatsByLicenseKey_Should_ReturnCorrectStats()
        {
            // Arrange
            var licenseStats = new LicenseStats
            {
                LicenseKey = "TestLicenseKey456",
                GamesPlayed = 15,
                GamesWon = 7,
                GamesLost = 8
            };

            _context.LicenseStats.Add(licenseStats);
            await _context.SaveChangesAsync();

            // Act
            var result = await _licenseStatsRepository.GetByLicenseKeyAsync("TestLicenseKey456");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(15, result.GamesPlayed);
            Assert.Equal(7, result.GamesWon);
        }

        [Fact]
        public async Task GetAllLicenseStats_Should_ReturnAllStats()
        {
            // Arrange
            _context.LicenseStats.RemoveRange(_context.LicenseStats);  // Clear existing data
            await _context.SaveChangesAsync();

            var stats1 = new LicenseStats { LicenseKey = "LicenseKey1", GamesPlayed = 10, GamesWon = 5, GamesLost = 5 };
            var stats2 = new LicenseStats { LicenseKey = "LicenseKey2", GamesPlayed = 20, GamesWon = 15, GamesLost = 5 };

            _context.LicenseStats.AddRange(stats1, stats2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _licenseStatsRepository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());  // Check that both records are retrieved
        }

        [Fact]
        public async Task GetLicenseStatsById_Should_ReturnNull_WhenLicenseStatsDoesNotExist()
        {
            // Act
            var result = await _licenseStatsRepository.GetByIdAsync(999); // Non-existent ID

            // Assert
            Assert.Null(result);
        }
    }
}
