using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.Repositories;

public class AchievementRepository
{
    public async Task<AchievementModel?> GetByCodeIntegration(int codeIntegration)
    {
        using var db = new AppDbContext();
        return await db.Achievements.SingleOrDefaultAsync(g => g.CodeIntegration == codeIntegration);
    }
    
    public async Task<List<AchievementModel>> GetAchievements(Guid gameId)
    {
        using var db = new AppDbContext();
        return await db.Achievements
            .Where(g => g.GameId == gameId)
            .ToListAsync();
    }
    
    public async Task InsertByRange(List<AchievementModel> achievements)
    {
        using var db = new AppDbContext();
        db.Achievements.AddRange(achievements);
        await db.SaveChangesAsync();
    }
    
    public async Task UpdateByRange(List<AchievementModel> achievements)
    {
        using var db = new AppDbContext();
        db.Achievements.UpdateRange(achievements);
        await db.SaveChangesAsync();
    }
}