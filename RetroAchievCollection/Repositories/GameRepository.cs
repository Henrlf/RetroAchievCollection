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
    public async Task<GameModel?> GetGameByCodeIntegration(int codeIntegration, bool includeAchievements = false)
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

    public async Task<List<GameModel>> GetConsoleGamesPaged(Guid consoleId, int page = 1, int take = 20, string searchText = "", bool includeAchievements = false)
    {
        using var db = new AppDbContext();

        int skip = Math.Max(page - 1, 0) * take;

        return await BuildQuery(db, includeAchievements, searchText)
            .Where(g => g.ConsoleId == consoleId)
            .OrderByDescending(g => g.IsFavorite)
            .ThenBy(g => g.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetConsoleGamesCount(Guid consoleId, string searchText = "")
    {
        using var db = new AppDbContext();
        return await BuildQuery(db, false, searchText)
            .CountAsync(g => g.ConsoleId == consoleId);
    }

    public async Task<List<GameModel>> GetFavoriteGamesPaged(int page = 1, int take = 20, string searchText = "", bool includeAchievements = false)
    {
        using var db = new AppDbContext();

        int skip = Math.Max(page - 1, 0) * take;

        return await BuildQuery(db, includeAchievements, searchText)
            .Where(g => g.IsFavorite)
            .OrderByDescending(g => g.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetFavoriteGamesCount(string searchText = "")
    {
        using var db = new AppDbContext();
        return await BuildQuery(db, false, searchText)
            .CountAsync(g => g.IsFavorite);
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

    private IQueryable<GameModel> BuildQuery(AppDbContext db, bool includeAchievements, string searchText = "")
    {
        var query = db.Games.AsQueryable();

        if (includeAchievements)
        {
            query = query.Include(g => g.Achievements);
        }

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(g => EF.Functions.Like(g.Name, $"%{searchText}%"));
        }

        return query;
    }
}