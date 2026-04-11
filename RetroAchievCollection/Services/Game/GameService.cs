using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Game;

public class GameService : BaseService
{
    public GameModel? GetGame(int gameId, int consoleId)
    {
        if (consoleId == 0 || gameId == 0)
        {
            return null;
        }

        var filePath = Path.Combine("games", $"console_{consoleId}", $"{gameId}.json");
        string json = LoadJson(filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return JsonSerializer.Deserialize<GameModel>(json);
    }

    public void SaveGameModel(GameModel? gameModel)
    {
        if (gameModel == null)
        {
            throw new NullReferenceException("Game was not found to be updated!");
        }

        if (gameModel.Id == 0)
        {
            throw new ArgumentException("Game ID is invalid!");
        }

        if (gameModel.ConsoleId == 0)
        {
            throw new ArgumentException("Console ID is invalid!");
        }

        var jsonPath = Path.Combine("games", $"console_{gameModel.ConsoleId}", $"{gameModel.Id}.json");
        SaveJson(jsonPath, gameModel);
    }

    public async Task SaveGame(GameDto gameDto)
    {
        GameModel gameModel = GetGame(gameDto.Id, gameDto.ConsoleId) ?? new GameModel();

        if (gameModel.Id >= 1)
        {
            return;
        }

        gameModel.Id = gameDto.Id;
        gameModel.ConsoleId = gameDto.ConsoleId;
        gameModel.Name = gameDto.Name;

        if (!string.IsNullOrWhiteSpace(gameDto.ImageUrl)
            && (string.IsNullOrWhiteSpace(gameModel.ImagePath) || !File.Exists(gameModel.ImagePath)))
        {
            try
            {
                var imageUrl = "https://media.retroachievements.org" + gameDto.ImageUrl;
                var extension = Path.GetExtension(new Uri(imageUrl).AbsolutePath);
                var imagePath = Path.Combine(MainDirectory, "images", "games", "console_" + gameDto.ConsoleId, gameDto.Id + extension);

                await SaveImageAsync(imageUrl, imagePath);
                gameModel.ImagePath = imagePath;
            }
            catch (Exception e)
            {
                SaveError(e.ToString());
            }
        }

        var jsonPath = Path.Combine("games", $"console_{gameDto.ConsoleId}", $"{gameDto.Id}.json");
        await SaveJsonAsync(jsonPath, gameModel);
    }

    public async Task<GameModel> SaveGameAndAchievements(GameDto gameDto)
    {
        GameModel gameModel = GetGame(gameDto.Id, gameDto.ConsoleId) ?? new GameModel();
        gameModel.Id = gameDto.Id;
        gameModel.ConsoleId = gameDto.ConsoleId;
        gameModel.Name = Regex.Replace(gameDto.Name, @"~[^~]*~\s*", "");
        gameModel.Publisher = gameDto.Publisher;
        gameModel.Developer = gameDto.Developer;
        gameModel.Genre = gameDto.Genre;
        gameModel.Released = gameDto.Released;
        gameModel.TotalAchievements = gameDto.NumAchievements;
        gameModel.TotalAchievementsCompleted = gameDto.NumAchievementsCompleted;
        gameModel.totalAchievementsCompletedHardcore = gameDto.NumAchievementsCompletedHardcore;

        if (!string.IsNullOrWhiteSpace(gameDto.ImageUrl) && (string.IsNullOrWhiteSpace(gameModel.ImagePath) || !File.Exists(gameModel.ImagePath)))
        {
            try
            {
                var imageUrl = "https://media.retroachievements.org" + gameDto.ImageUrl;
                var extension = Path.GetExtension(new Uri(imageUrl).AbsolutePath);
                var imagePath = Path.Combine(MainDirectory, "images", "games", $"console_{gameDto.ConsoleId}", gameDto.Id + extension);

                await SaveImageAsync(imageUrl, imagePath);
                gameModel.ImagePath = imagePath;
            }
            catch (Exception e)
            {
                SaveError(e.ToString());
            }
        }

        var achievementsDto = gameDto.Achievements;
        gameModel.Achievements = await GetAchievementModels(achievementsDto.Values.ToList(), gameDto.Id);

        var jsonPath = Path.Combine("games", $"console_{gameDto.ConsoleId}", $"{gameDto.Id}.json");
        await SaveJsonAsync(jsonPath, gameModel);

        return gameModel;
    }

    protected async Task<List<AchievementModel>> GetAchievementModels(List<AchievementDto> achievementsDto, int gameId)
    {
        List<AchievementModel> achievementModels = new();
        var semaphore = new SemaphoreSlim(25);

        await Task.WhenAll(achievementsDto.Select(async achievementDto =>
        {
            await semaphore.WaitAsync();

            try
            {
                AchievementModel achievementModel = new AchievementModel
                {
                    Id = achievementDto.Id,
                    Name = achievementDto.Name,
                    Description = achievementDto.Description,
                    IsCompleted = !string.IsNullOrWhiteSpace(achievementDto.DateEarned),
                    IsCompletedHardcore = !string.IsNullOrWhiteSpace(achievementDto.DateEarnedHardcore)
                };

                try
                {
                    string isLock = "";

                    if (!achievementModel.IsCompleted && !achievementModel.IsCompletedHardcore)
                    {
                        isLock = "_lock";
                    }

                    var imageUrl = $"https://media.retroachievements.org/Badge/{achievementDto.BadgeName}{isLock}.png";

                    if (string.IsNullOrWhiteSpace(achievementModel.ImagePath) || !File.Exists(achievementModel.ImagePath))
                    {
                        var imagePath = Path.Combine(MainDirectory, "images", "achievements", $"game_{gameId}", $"{achievementDto.Id}{isLock}.png");

                        await SaveImageAsync(imageUrl, imagePath);
                        achievementModel.ImagePath = imagePath;
                    }
                }
                catch (Exception e)
                {
                    SaveError(e.ToString());
                }

                achievementModels.Add(achievementModel);
            }
            finally
            {
                semaphore.Release();
            }
        }));

        return achievementModels.OrderBy(a => a.Name).ToList();
    }
}