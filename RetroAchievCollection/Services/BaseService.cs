using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace RetroAchievCollection.Services;

public abstract class BaseService
{
    public static readonly string MainDirectory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "RetroAchievIntegration"
    );

    protected void SaveJson(string fileName, object content)
    {
        var filePath = Path.Combine(MainDirectory, fileName);
        var directory = Path.GetDirectoryName(filePath);

        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(content, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(filePath, json);
    }

    protected async Task SaveJsonAsync(string fileName, object content)
    {
        var filePath = Path.Combine(MainDirectory, fileName);
        var directory = Path.GetDirectoryName(filePath);

        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(content, new JsonSerializerOptions {WriteIndented = true});
        await File.WriteAllTextAsync(filePath, json);
    }

    protected async Task SaveImageAsync(string url, string filePath)
    {
        var directory = Path.GetDirectoryName(filePath);

        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var http = new HttpClient();
        var bytes = await http.GetByteArrayAsync(url);

        await File.WriteAllBytesAsync(filePath, bytes);
    }

    protected string LoadJson(string fileName)
    {
        var filePath = Path.Combine(MainDirectory, fileName);

        if (!File.Exists(filePath))
        {
            return "";
        }

        return File.ReadAllText(filePath);
    }

    public static void SaveError(string errorMessage)
    {
        try
        {
            SaveLog("error.log", errorMessage);
        }
        catch
        {
            // ignored
        }
    }

    public static void SaveLog(string fileName, string log)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(log))
            {
                log = "Unknown error";
            }

            var filePath = Path.Combine(MainDirectory, "log", fileName);
            var directory = Path.GetDirectoryName(filePath);

            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.AppendAllTextAsync(filePath, log + "\n");
        }
        catch
        {
            // ignored
        }
    }
}