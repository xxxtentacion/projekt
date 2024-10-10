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
    public class GameStatsRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly GameStatsRepository _gameStatsRepository;

        public GameStatsRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "GameStatsRepositoryTests")
                .Options;

            _context = new(_options);
            _gameStatsRepository = new(_context);
        }

        [Fact]
        public async Task AddGameStats_Should_AddNewGameStatsToDatabase()
        {
            // Arrange
            var newGameStats = new GameStats
            {
                GamesPlayed = 10,
                GamesWon = 5,
                GamesLost = 5
            };

            // Act
            await _gameStatsRepository.AddAsync(newGameStats);
            await _context.SaveChangesAsync();

            // Assert
            var statsInDb = await _gameStatsRepository.GetByIdAsync(newGameStats.Id);
            Assert.NotNull(statsInDb);
            Assert.Equal(10, statsInDb.GamesPlayed);
            Assert.Equal(5, statsInDb.GamesWon);
        }

        [Fact]
        public async Task GetAllGameStats_Should_ReturnAllStats()
        {
            // Arrange
            var stats1 = new GameStats { GamesPlayed = 10, GamesWon = 5, GamesLost = 5 };
            var stats2 = new GameStats { GamesPlayed = 20, GamesWon = 15, GamesLost = 5 };

            _context.GameStats.AddRange(stats1, stats2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _gameStatsRepository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());  // Check that both records are retrieved
        }

        [Fact]
        public async Task UpdateGameStats_Should_UpdateExistingGameStats()
        {
            // Arrange
            var gameStats = new GameStats { GamesPlayed = 10, GamesWon = 5, GamesLost = 5 };
            _context.GameStats.Add(gameStats);
            await _context.SaveChangesAsync();

            gameStats.GamesPlayed = 20;
            gameStats.GamesWon = 15;

            // Act
            await _gameStatsRepository.UpdateAsync(gameStats);
            await _context.SaveChangesAsync();

            // Assert
            var updatedStats = await _gameStatsRepository.GetByIdAsync(gameStats.Id);
            Assert.Equal(20, updatedStats.GamesPlayed);
            Assert.Equal(15, updatedStats.GamesWon);
        }

        [Fact]
        public async Task DeleteGameStats_Should_RemoveGameStatsFromDatabase()
        {
            // Arrange
            var gameStats = new GameStats { GamesPlayed = 10, GamesWon = 5, GamesLost = 5 };
            _context.GameStats.Add(gameStats);
            await _context.SaveChangesAsync();

            // Act
            await _gameStatsRepository.DeleteAsync(gameStats.Id);
            await _context.SaveChangesAsync();

            // Assert
            var deletedStats = await _gameStatsRepository.GetByIdAsync(gameStats.Id);
            Assert.Null(deletedStats);  // Ensure the stats have been deleted
        }

        [Fact]
        public async Task GetGameStatsById_Should_ReturnCorrectStats()
        {
            // Arrange
            var gameStats = new GameStats { GamesPlayed = 10, GamesWon = 5, GamesLost = 5 };
            _context.GameStats.Add(gameStats);
            await _context.SaveChangesAsync();

            // Act
            var result = await _gameStatsRepository.GetByIdAsync(gameStats.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(gameStats.Id, result.Id);
        }
    }
}
