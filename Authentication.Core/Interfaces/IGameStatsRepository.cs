using Authentication.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Authentication.Core.Interfaces
{
    public interface IGameStatsRepository
    {
        Task<GameStats> GetByIdAsync(int id);
        Task<IEnumerable<GameStats>> GetAllAsync();
        Task AddAsync(GameStats gameStats);
        Task UpdateAsync(GameStats gameStats);
        Task DeleteAsync(int id);
    }
}
