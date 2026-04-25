using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;

namespace RetroAchievCollection.Data;

public class AppDbContext : DbContext
{
    public DbSet<ConfigurationModel> Configuration {get; set;}
    public DbSet<ConsoleModel> Consoles {get; set;}
    public DbSet<GameModel> Games {get; set;}
    public DbSet<AchievementModel> Achievements {get; set;}

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var dbPath = Path.Combine(BaseService.MainDirectory, "retroacheivintegration.db");
        options.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ConfigurationModel>().HasData(new ConfigurationModel
        {
            Id = Guid.Parse("166117e9-2488-4e1a-98fc-f3ec4ca298aa"),
            UserName = "",
            ApiKey = ""
        });
    }
}