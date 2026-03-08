using System;
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
        List<ConsoleModel> consoleList = new();

        if (string.IsNullOrWhiteSpace(json))
        {
            return consoleList;
        }

        List<ConsoleModel>? consoles = JsonSerializer.Deserialize<List<ConsoleModel>>(json);

        return consoles ?? consoleList;
    }

    public ConsoleModel? GetConsole(int id)
    {
        return GetConsoles().FirstOrDefault(console => console.Id == id);
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
                    var imagePath = Path.Combine("images", "console", console.Id + extension);

                    await SaveImageAsync(consoleDto.ImageUrl, imagePath);
                    console.ImagePath = imagePath;
                }
                catch (Exception e)
                {
                    await SaveError(e.ToString());
                }
            }

            consoleList[consoleDto.ConsoleId] = console;
        }

        await SaveJsonAsync("consoles.json", consoleList.Values.OrderBy(c => c.Name));
    }
}