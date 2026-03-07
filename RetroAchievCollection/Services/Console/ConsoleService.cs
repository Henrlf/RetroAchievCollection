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
        string json = LoadJson("console.json");
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
            // console.Company = consoleDto.Company;

            if (string.IsNullOrWhiteSpace(console.ImagePath) || !File.Exists(console.ImagePath))
            {
                var extension = Path.GetExtension(new Uri(consoleDto.ImageUrl).AbsolutePath);
                var imagePath = Path.Combine("images", "console", console.Id + extension);

                await SaveImageAsync(consoleDto.ImageUrl, imagePath);

                console.ImagePath = imagePath;
            }

            consoleCollection[consoleDto.ConsoleId] = console;
        }

        await SaveFileAsync("consoles.json", consoleCollection.Values.OrderBy(c => c.Name));
    }
}