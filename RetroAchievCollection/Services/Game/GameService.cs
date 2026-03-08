using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Game;

public class GameService : BaseService
{
    public List<GameModel> GetGames(int consoleId)
    {
        var directoryPath = Path.Combine(MainDirectory, "games", "console_" + consoleId);

        if (!Directory.Exists(directoryPath))
        {
            return new List<GameModel>();
        }

        var gameCollection = new ConcurrentBag<GameModel>();
        var arquivos = Directory.GetFiles(directoryPath, "*.json");

        Parallel.ForEach(arquivos, new ParallelOptions {MaxDegreeOfParallelism = 8},
            arquivo =>
            {
                try
                {
                    string json = LoadJson(arquivo);
                    var game = JsonSerializer.Deserialize<GameModel>(json);

                    if (game != null)
                    {
                        gameCollection.Add(game);
                    }
                }
                catch (Exception e)
                {
                    SaveError(e.ToString());
                }
            });

        return gameCollection.ToList();
    }

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

    public async Task SaveGame(GameDto gameDto)
    {
        GameModel gameModel = GetGame(gameDto.Id, gameDto.ConsoleId) ?? new GameModel();
        gameModel.Id = gameDto.Id;
        gameModel.ConsoleId = gameDto.ConsoleId;
        gameModel.Name = gameDto.Name;

        if ((string.IsNullOrWhiteSpace(gameModel.ImagePath) || !File.Exists(gameModel.ImagePath))
            && !string.IsNullOrWhiteSpace(gameDto.ImageUrl))
        {
            try
            {
                var imageUrl = "https://media.retroachievements.org" + gameDto.ImageUrl;
                var extension = Path.GetExtension(new Uri(imageUrl).AbsolutePath);
                var imagePath = Path.Combine("images", "games", "console_" + gameDto.ConsoleId, gameDto.Id + extension);

                await SaveImageAsync(imageUrl, imagePath);
                gameModel.ImagePath = imagePath;
            }
            catch (Exception e)
            {
                await SaveError(e.ToString());
            }
        }

        var jsonPath = Path.Combine("games", $"console_{gameDto.ConsoleId}", $"{gameDto.Id}.json");
        await SaveJsonAsync(jsonPath, gameModel);
    }
}