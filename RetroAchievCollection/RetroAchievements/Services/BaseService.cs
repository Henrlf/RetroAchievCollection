using System.IO;

namespace RetroAchievCollection.RetroAchievements.Services;

public abstract class BaseService
{
    private readonly string url = "https://retroachievements.org/API";

    public string getUrl(string suffix)
    {
        return Path.Combine(url, suffix);
    }
}