using Authentication.Core.Database;
using Authentication.Core.Entities;
using Authentication.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Core.Repository
{
    public class GameStatsRepository : IGameStatsRepository
    {
        private readonly DatabaseContext _context;

        public GameStatsRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameStats>> GetAllAsync()
        {
            return await _context.GameStats.ToListAsync();
        }

        public async Task<GameStats> GetByIdAsync(int id)
        {
            return await _context.GameStats.FindAsync(id);
        }

        public async Task AddAsync(GameStats entity)
        {
            await _context.GameStats.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GameStats entity)
        {
            _context.GameStats.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var gameStat = await _context.GameStats.FindAsync(id);
            if (gameStat != null)
            {
                _context.GameStats.Remove(gameStat);
                await _context.SaveChangesAsync();
            }
        }
    }
}