using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.Repositories;

public class GameRepository
{
    public async Task<GameModel?> GetByCodeIntegration(int codeIntegration, bool includeAchievements = false)
    {
        using var db = new AppDbContext();
        return await BuildQuery(db, includeAchievements)
            .SingleOrDefaultAsync(g => g.CodeIntegration == codeIntegration);
    }
        
    public async Task<List<GameModel>> GetConsoleGames(Guid consoleId, bool includeAchievements = false)
    {
        using var db = new AppDbContext();

        return await BuildQuery(db, includeAchievements)
            .Where(g => g.ConsoleId == consoleId)
            .ToListAsync();
    }
    
    public async Task<List<GameModel>> GetFavoriteGames(bool includeAchievements = false)
    {
        using var db = new AppDbContext();

        return await BuildQuery(db, includeAchievements)
            .Where(g => g.IsFavorite)
            .ToListAsync();
    }
    
    public async Task<int> GetConsoleGamesCount(Guid consoleId)
    {
        using var db = new AppDbContext();
        return await db.Games.CountAsync(g => g.ConsoleId == consoleId);
    }
    
    public async Task InsertGame(GameModel model)
    {
        using var db = new AppDbContext();
        db.Games.Add(model);
        await db.SaveChangesAsync();
    }
    
    public async Task UpdateGame(GameModel model)
    {
        using var db = new AppDbContext();
        db.Games.Update(model);
        await db.SaveChangesAsync();
    }
    
    private IQueryable<GameModel> BuildQuery(AppDbContext db, bool includeAchievements)
    {
        var query = db.Games.AsQueryable();

        if (includeAchievements)
        {
            query = query.Include(g => g.Achievements);
        }

        return query;
    }
}