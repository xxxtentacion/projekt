using Authentication.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IGameStatsService
    {
        Task<GameStats> GetGameStatsByIdAsync(int id);
        Task<IEnumerable<GameStats>> GetAllGameStatsAsync();
        Task AddGameStatsAsync(GameStats gameStats);
        Task UpdateGameStatsAsync(GameStats gameStats);
        Task DeleteGameStatsAsync(int id);
    }
}
