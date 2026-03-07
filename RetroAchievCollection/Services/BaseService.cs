using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RetroAchievCollection.Services;

public abstract class BaseService
{
    public readonly string Directory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "RetroAchievIntegration"
    );

    protected void SaveModelToJson(string fileName, object content)
    {
        var filePath = Path.Combine(Directory, fileName);

        if (!System.IO.Directory.Exists(Directory))
        {
            System.IO.Directory.CreateDirectory(Directory);
        }

        var json = JsonSerializer.Serialize(content, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(filePath, json);
    }

    protected async Task SaveFileAsync(string fileName, object content)
    {
        var filePath = Path.Combine(Directory, fileName);

        if (!System.IO.Directory.Exists(Directory))
        {
            System.IO.Directory.CreateDirectory(Directory);
        }

        var json = JsonSerializer.Serialize(content, new JsonSerializerOptions {WriteIndented = true});
        await File.WriteAllTextAsync(filePath, json);
    }

    protected async Task SaveImageAsync(string url, string fileName)
    {
        var filePath = Path.Combine(Directory, fileName);
        using var http = new HttpClient();
        var bytes = await http.GetByteArrayAsync(url);
        
        await File.WriteAllBytesAsync(filePath, bytes);
    }
    
    protected string LoadJson(string fileName)
    {
        var filePath = Path.Combine(Directory, fileName);

        if (!File.Exists(filePath))
        {
            return "";
        }

        return File.ReadAllText(filePath);
    }
}