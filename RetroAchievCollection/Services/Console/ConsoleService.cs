using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Console;

public class ConsoleService : BaseService
{
    public async Task<ConsoleModel?> GetConsole(Guid consoleId)
    {
        using var db = new AppDbContext();
        return await db.Consoles.FindAsync(consoleId);
    }

    public async Task<List<ConsoleModel>> GetConsoles()
    {
        using var db = new AppDbContext();

        return await db.Consoles
            .ToListAsync();
    }

    public async Task SaveConsoleDto(ConsoleDto consoleDto)
    {
        using var db = new AppDbContext();

        var consoleModel = db.Consoles.FirstOrDefault(c => c.CodeIntegration == consoleDto.CodeIntegration);

        if (consoleModel == null)
        {
            consoleModel = new ConsoleModel();
        }

        consoleModel.CodeIntegration = consoleDto.CodeIntegration;
        consoleModel.Name = consoleDto.Name;

        if ((string.IsNullOrWhiteSpace(consoleModel.ImagePath) || !File.Exists(consoleModel.ImagePath))
            && !string.IsNullOrWhiteSpace(consoleDto.ImageUrl))
        {
            try
            {
                var extension = Path.GetExtension(new Uri(consoleDto.ImageUrl).AbsolutePath);
                var imagePath = Path.Combine(MainDirectory, "images", "console", consoleModel.CodeIntegration + extension);

                await SaveImageAsync(consoleDto.ImageUrl, imagePath);
                consoleModel.ImagePath = imagePath;
            }
            catch (Exception e)
            {
                SaveError(e.ToString());
            }
        }

        if (consoleModel.Id == Guid.Empty)
        {
            db.Consoles.Add(consoleModel);
        }
        else
        {
            db.Consoles.Update(consoleModel);
        }

        await db.SaveChangesAsync();
    }

    // ----------------------------------------------------------------------------------------------------------------
    // TODO: REMOVE

    // public List<ConsoleModel> GetConsoles()
    // {
    //     string json = LoadJson("consoles.json");
    //
    //     if (string.IsNullOrWhiteSpace(json))
    //     {
    //         return new List<ConsoleModel>();
    //     }
    //
    //     return JsonSerializer.Deserialize<List<ConsoleModel>>(json) ?? new List<ConsoleModel>();
    // }

    // public ConsoleModel? GetConsole(int id)
    // {
    //     return GetConsoles().FirstOrDefault(console => console.Id == id);
    // }

    // public List<GameModel> GetGames(int consoleId)
    // {
    //     var directoryPath = Path.Combine(MainDirectory, "games", $"console_{consoleId}");
    //
    //     if (!Directory.Exists(directoryPath))
    //     {
    //         return new List<GameModel>();
    //     }
    //
    //     var gameCollection = new ConcurrentBag<GameModel>();
    //     var arquivos = Directory.GetFiles(directoryPath, "*.json");
    //
    //     Parallel.ForEach(arquivos, new ParallelOptions {MaxDegreeOfParallelism = 8},
    //         arquivo =>
    //         {
    //             try
    //             {
    //                 string json = File.ReadAllText(arquivo);
    //                 GameModel? game = JsonSerializer.Deserialize<GameModel>(json);
    //
    //                 if (game != null)
    //                 {
    //                     gameCollection.Add(game);
    //                 }
    //             }
    //             catch (Exception e)
    //             {
    //                 SaveError(e.ToString());
    //             }
    //         });
    //
    //     return gameCollection.ToList();
    // }

    // public async Task SaveConsoles(List<ConsoleDto> consolesDto)
    // {
    //     Dictionary<int, ConsoleModel> consoleList = GetConsoles().ToDictionary(c => c.Id);
    //
    //     foreach (var consoleDto in consolesDto)
    //     {
    //         ConsoleModel console = consoleList.GetValueOrDefault(consoleDto.CodeIntegration) ?? new ConsoleModel();
    //         console.Id = consoleDto.CodeIntegration;
    //         console.Name = consoleDto.Name;
    //
    //         if ((string.IsNullOrWhiteSpace(console.ImagePath) || !File.Exists(console.ImagePath))
    //             && !string.IsNullOrWhiteSpace(consoleDto.ImageUrl))
    //         {
    //             try
    //             {
    //                 var extension = Path.GetExtension(new Uri(consoleDto.ImageUrl).AbsolutePath);
    //                 var imagePath = Path.Combine(MainDirectory, "images", "console", console.Id + extension);
    //
    //                 await SaveImageAsync(consoleDto.ImageUrl, imagePath);
    //                 console.ImagePath = imagePath;
    //             }
    //             catch (Exception e)
    //             {
    //                 SaveError(e.ToString());
    //             }
    //         }
    //
    //         consoleList[consoleDto.CodeIntegration] = console;
    //     }
    //
    //     await SaveJsonAsync("consoles.json", consoleList.Values.OrderBy(c => c.Name));
    // }
}