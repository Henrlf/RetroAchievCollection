using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Console;

public class ConsoleService : BaseService
{
    public List<ConsoleModel> GetConsoles()
    {
        string json = LoadJson("consoles.json");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<ConsoleModel>();
        }

        return JsonSerializer.Deserialize<List<ConsoleModel>>(json) ?? new List<ConsoleModel>();
    }

    public ConsoleModel? GetConsole(int id)
    {
        return GetConsoles().FirstOrDefault(console => console.Id == id);
    }

    public List<GameModel> GetGames(int consoleId)
    {
        var directoryPath = Path.Combine(MainDirectory, "games", $"console_{consoleId}");

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
                    string json = File.ReadAllText(arquivo);
                    GameModel? game = JsonSerializer.Deserialize<GameModel>(json);

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
    
    public async Task SaveConsoles(List<ConsoleDto> consolesDto)
    {
        Dictionary<int, ConsoleModel> consoleList = GetConsoles().ToDictionary(c => c.Id);

        foreach (var consoleDto in consolesDto)
        {
            ConsoleModel console = consoleList.GetValueOrDefault(consoleDto.ConsoleId) ?? new ConsoleModel();
            console.Id = consoleDto.ConsoleId;
            console.Name = consoleDto.Name;

            if ((string.IsNullOrWhiteSpace(console.ImagePath) || !File.Exists(console.ImagePath))
                && !string.IsNullOrWhiteSpace(consoleDto.ImageUrl))
            {
                try
                {
                    var extension = Path.GetExtension(new Uri(consoleDto.ImageUrl).AbsolutePath);
                    var imagePath = Path.Combine(MainDirectory, "images", "console", console.Id + extension);

                    await SaveImageAsync(consoleDto.ImageUrl, imagePath);
                    console.ImagePath = imagePath;
                }
                catch (Exception e)
                {
                    SaveError(e.ToString());
                }
            }

            consoleList[consoleDto.ConsoleId] = console;
        }

        await SaveJsonAsync("consoles.json", consoleList.Values.OrderBy(c => c.Name));
    }
}