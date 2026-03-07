using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RetroAchievCollection.Services;

public abstract class BaseService
{
    public static readonly string Directory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "RetroAchievIntegration"
    );

    protected void SaveModelToJson(string fileName, object content)
    {
        var filePath = Path.Combine(Directory, fileName);
        var directory = Path.GetDirectoryName(filePath);

        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(content, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(filePath, json);
    }

    protected async Task SaveJsonAsync(string fileName, object content)
    {
        var filePath = Path.Combine(Directory, fileName);
        var directory = Path.GetDirectoryName(filePath);

        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(content, new JsonSerializerOptions {WriteIndented = true});
        await File.WriteAllTextAsync(filePath, json);
    }

    protected async Task SaveImageAsync(string url, string fileName)
    {
        var filePath = Path.Combine(Directory, "images", fileName);
        var directory = Path.GetDirectoryName(filePath);

        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

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

    public static async Task SaveError(string errorMessage)
    {
        await SaveLog("error.log", errorMessage);
    }

    public static async Task SaveLog(string fileName, string log)
    {
        if (string.IsNullOrWhiteSpace(log))
        {
            log = "Unknown error";
        }

        var filePath = Path.Combine(Directory, "log", fileName);
        var directory = Path.GetDirectoryName(filePath);

        if (!System.IO.Directory.Exists(directory))
        {
            System.IO.Directory.CreateDirectory(directory);
        }

        await File.AppendAllTextAsync(filePath, log + "\n");
    }
}