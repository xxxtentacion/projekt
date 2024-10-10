using Authentication.API.Controllers;
using Authentication.Core.Database;
using Authentication.Core.Entities;
using Authentication.Core.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace Authentication.Test.Controllers
{
    public class GameStatsControllerTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly GameStatsController _gameStatsController;

        public GameStatsControllerTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "GameStatsControllerTests")
                .Options;

            _context = new(_options);

            var gameStatsRepository = new GameStatsRepository(_context); // Using the actual repository
            _gameStatsController = new GameStatsController(gameStatsRepository); // Inject the repository
        }

        [Fact]
        public async Task GetGameStatsById_Should_ReturnOkWithGameStats()
        {
            // Arrange
            var gameStats = new GameStats { GamesPlayed = 10, GamesWon = 5, GamesLost = 5 };
            _context.GameStats.Add(gameStats);
            await _context.SaveChangesAsync();

            // Act
            var result = await _gameStatsController.GetGameStatById(gameStats.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);  // Check if result is OK
            var returnedStats = Assert.IsType<GameStats>(okResult.Value);  // Verify the type
            Assert.Equal(gameStats.Id, returnedStats.Id);  // Check if the ID matches
            Assert.Equal(10, returnedStats.GamesPlayed);  // Check property
            Assert.Equal(5, returnedStats.GamesWon);
        }

        [Fact]
        public async Task AddGameStats_Should_ReturnCreatedAtAction()
        {
            // Arrange
            var newGameStats = new GameStats { GamesPlayed = 15, GamesWon = 10, GamesLost = 5 };

            // Act
            var result = await _gameStatsController.CreateGameStat(newGameStats);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdStats = Assert.IsType<GameStats>(createdAtActionResult.Value);
            Assert.Equal(newGameStats.GamesPlayed, createdStats.GamesPlayed);
            Assert.Equal(newGameStats.GamesWon, createdStats.GamesWon);
            Assert.Equal(newGameStats.GamesLost, createdStats.GamesLost);
        }

        [Fact]
        public async Task UpdateGameStats_Should_ReturnNoContent()
        {
            // Arrange
            var gameStats = new GameStats { GamesPlayed = 5, GamesWon = 3, GamesLost = 2 };
            _context.GameStats.Add(gameStats);
            await _context.SaveChangesAsync();

            // Fetch the existing entity from the database and update it
            var updatedGameStats = await _context.GameStats.FindAsync(gameStats.Id);
            updatedGameStats.GamesPlayed = 20;
            updatedGameStats.GamesWon = 15;
            updatedGameStats.GamesLost = 5;

            // Act
            var result = await _gameStatsController.UpdateGameStat(gameStats.Id, updatedGameStats);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var updatedInDb = await _context.GameStats.FindAsync(gameStats.Id);
            Assert.Equal(20, updatedInDb.GamesPlayed);
            Assert.Equal(15, updatedInDb.GamesWon);
        }

        [Fact]
        public async Task DeleteGameStats_Should_ReturnNoContent()
        {
            // Arrange
            var gameStats = new GameStats { GamesPlayed = 10, GamesWon = 5, GamesLost = 5 };
            _context.GameStats.Add(gameStats);
            await _context.SaveChangesAsync();

            // Act
            var result = await _gameStatsController.DeleteGameStat(gameStats.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var deletedStats = await _context.GameStats.FindAsync(gameStats.Id);
            Assert.Null(deletedStats);
        }
    }
}
