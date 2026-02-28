using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RetroAchievCollection.Services;

public abstract class BaseService
{
    private readonly string Directory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "RetroAchievIntegration"
    );

    public void SaveToJson(string fileName, object model)
    {
        var filePath = Path.Combine(Directory, fileName);

        if (!System.IO.Directory.Exists(Directory))
        {
            System.IO.Directory.CreateDirectory(Directory);
        }

        var json = JsonSerializer.Serialize(model, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(filePath, json);
    }

    public async Task SaveToJsonAsync(string fileName, object model)
    {
        var filePath = Path.Combine(Directory, fileName);

        if (!System.IO.Directory.Exists(Directory))
        {
            System.IO.Directory.CreateDirectory(Directory);
        }

        var json = JsonSerializer.Serialize(model, new JsonSerializerOptions {WriteIndented = true});
        await File.WriteAllTextAsync(filePath, json);
    }

    public string LoadJson(string fileName)
    {
        var filePath = Path.Combine(Directory, fileName);

        if (!File.Exists(filePath))
        {
            return "";
        }

        return File.ReadAllText(filePath);
    }
}