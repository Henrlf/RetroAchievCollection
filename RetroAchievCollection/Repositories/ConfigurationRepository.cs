using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.Repositories;

public class ConfigurationRepository
{
    public async Task<ConfigurationModel> GetConfiguration()
    {
        using var db = new AppDbContext();
        return await db.Configuration.FirstAsync();
    }
    
    public async Task UpdateConfiguration(ConfigurationModel configuration)
    {
        using var db = new AppDbContext();
        db.Configuration.Update(configuration);
        await db.SaveChangesAsync();
    }
}