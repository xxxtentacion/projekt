using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameStatsController : ControllerBase
    {
        private readonly IGameStatsRepository _gameStatsRepository;

        public GameStatsController(IGameStatsRepository gameStatsRepository)
        {
            _gameStatsRepository = gameStatsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGameStats()
        {
            var gameStats = await _gameStatsRepository.GetAllAsync();
            return Ok(gameStats);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGameStatById(int id)
        {
            var gameStat = await _gameStatsRepository.GetByIdAsync(id);
            if (gameStat == null) return NotFound();
            return Ok(gameStat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGameStat(GameStats gameStat)
        {
            await _gameStatsRepository.AddAsync(gameStat);
            return CreatedAtAction(nameof(GetGameStatById), new { id = gameStat.Id }, gameStat);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGameStat(int id, GameStats gameStat)
        {
            if (id != gameStat.Id) return BadRequest();
            await _gameStatsRepository.UpdateAsync(gameStat);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameStat(int id)
        {
            await _gameStatsRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
