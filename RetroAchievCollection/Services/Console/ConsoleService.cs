using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.Services.Console;

public class ConsoleService : BaseService
{
    public Collection<ConsoleModel> GetConsoles()
    {
        string json = LoadJson("consoles.json");
        Collection<ConsoleModel> consoleCollection = new();

        if (string.IsNullOrWhiteSpace(json))
        {
            return consoleCollection;
        }

        Collection<ConsoleModel>? consoles = JsonSerializer.Deserialize<Collection<ConsoleModel>>(json);

        return consoles ?? consoleCollection;
    }

    public ConsoleModel? GetConsole(int id)
    {
        Collection<ConsoleModel> consoleCollection = GetConsoles();

        return consoleCollection.FirstOrDefault(console => console.Id == id);
    }

    public async Task SaveConsoles(Collection<ConsoleDto> consolesDto)
    {
        Dictionary<int, ConsoleModel> consoleCollection = GetConsoles().ToDictionary(c => c.Id);

        foreach (var consoleDto in consolesDto)
        {
            ConsoleModel console = consoleCollection.GetValueOrDefault(consoleDto.ConsoleId) ?? new ConsoleModel();
            console.Id = consoleDto.ConsoleId;
            console.Name = consoleDto.Name;

            if ((string.IsNullOrWhiteSpace(console.ImagePath) || !File.Exists(console.ImagePath))
                && !string.IsNullOrWhiteSpace(consoleDto.ImageUrl))
            {
                var extension = Path.GetExtension(new Uri(consoleDto.ImageUrl).AbsolutePath);
                var imagePath = Path.Combine("console", console.Id + extension);

                try
                {
                    await SaveImageAsync(consoleDto.ImageUrl, imagePath);
                    console.ImagePath = imagePath;
                }
                catch (Exception e)
                {
                    await SaveError(e.ToString());
                }
            }

            consoleCollection[consoleDto.ConsoleId] = console;
        }

        await SaveJsonAsync("consoles.json", consoleCollection.Values.OrderBy(c => c.Name));
    }
}