using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Enum;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Game;

public class GameService : BaseService
{
    public async Task<List<GameModel>> GetGames(Guid consoleId)
    {
        using var db = new AppDbContext();

        return await db.Games
            .Include(g => g.Achievements)
            .Where(c => c.ConsoleId == consoleId)
            .ToListAsync();
    }

    public void SaveGameModel(GameModel gameModel)
    {
        if (gameModel.CodeIntegration == 0)
        {
            throw new ArgumentException("Game ID is invalid!");
        }

        if (gameModel.ConsoleId == Guid.Empty)
        {
            throw new NoNullAllowedException("Console ID is invalid!");
        }

        using var db = new AppDbContext();
        db.Games.Update(gameModel);
        db.SaveChanges();
    }

    public async Task<GameModel> SaveGameDto(GameDto gameDto, int consoleCodeintegration)
    {
        using var db = new AppDbContext();
        ConsoleModel? consoleModel = await db.Consoles.SingleOrDefaultAsync(c => c.CodeIntegration == consoleCodeintegration);

        if (consoleModel == null)
        {
            throw new NullReferenceException("Console does not exist!");
        }

        GameModel? gameModel = await db.Games.SingleOrDefaultAsync(g => g.CodeIntegration == gameDto.CodeIntegration);

        if (gameModel == null)
        {
            gameModel = new GameModel();
        }

        gameModel.ConsoleId = consoleModel.Id;
        gameModel.CodeIntegration = gameDto.CodeIntegration;
        gameModel.Name = Regex.Replace(gameDto.Name, @"~[^~]*~\s*", "");
        gameModel.Publisher = gameDto.Publisher;
        gameModel.Developer = gameDto.Developer;
        gameModel.Genre = gameDto.Genre;
        gameModel.ReleaseDate = DateOnly.TryParse(gameDto.Released, out var date) ? date : null;

        if ((string.IsNullOrWhiteSpace(gameModel.ImagePath) || !File.Exists(gameModel.ImagePath))
            && !string.IsNullOrWhiteSpace(gameDto.ImageUrl))
        {
            try
            {
                var imageUrl = "https://media.retroachievements.org" + gameDto.ImageUrl;
                var extension = Path.GetExtension(new Uri(imageUrl).AbsolutePath);
                var imagePath = Path.Combine("images", "games", $"console_{consoleModel.CodeIntegration}", gameDto.CodeIntegration + extension);

                await SaveImageAsync(imageUrl, Path.Combine(MainDirectory, imagePath));
                gameModel.ImagePath = imagePath;
            }
            catch (Exception e)
            {
                SaveError(e.ToString());
            }
        }

        if (gameModel.Id == Guid.Empty)
        {
            db.Games.Add(gameModel);
        }
        else
        {
            db.Games.Update(gameModel);
        }

        await db.SaveChangesAsync();

        return gameModel;
    }

    public async Task<AchievementModel> SaveAchievementDto(AchievementDto achievementDto, GameModel gameModel)
    {
        using var db = new AppDbContext();

        AchievementModel? achievementModel = await db.Achievements.SingleOrDefaultAsync(a => a.CodeIntegration == achievementDto.CodeIntegration);

        if (achievementModel == null)
        {
            achievementModel = new AchievementModel();
        }

        achievementModel.GameId = gameModel.Id;
        achievementModel.CodeIntegration = achievementDto.CodeIntegration;
        achievementModel.Name = achievementDto.Name;
        achievementModel.Description = achievementDto.Description;

        if (!string.IsNullOrWhiteSpace(achievementDto.DateEarnedHardcore))
        {
            achievementModel.Status = AchievementStatus.CompletedHardcore;
        }
        else if (!string.IsNullOrWhiteSpace(achievementDto.DateEarned))
        {
            achievementModel.Status = AchievementStatus.Completed;
        }

        try
        {
            string isLock = "";

            if (achievementModel.Status == AchievementStatus.NotCompleted)
            {
                isLock = "_lock";
            }

            var imageUrl = $"https://media.retroachievements.org/Badge/{achievementDto.BadgeName}{isLock}.png";

            if (string.IsNullOrWhiteSpace(achievementModel.ImagePath) || !File.Exists(achievementModel.ImagePath))
            {
                var imagePath = Path.Combine("images", "achievements", $"game_{gameModel.CodeIntegration}", $"{achievementDto.CodeIntegration}{isLock}.png");

                await SaveImageAsync(imageUrl, Path.Combine(MainDirectory, imagePath));
                achievementModel.ImagePath = imagePath;
            }
        }
        catch (Exception e)
        {
            SaveError(e.ToString());
        }

        if (achievementModel.Id == Guid.Empty)
        {
            db.Achievements.Add(achievementModel);
        }
        else
        {
            db.Achievements.Update(achievementModel);
        }

        await db.SaveChangesAsync();

        return achievementModel;
    }
}