using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.RetroAchievements.Dtos;

namespace RetroAchievCollection.RetroAchievements.Services;

public class RetroAchievementsService : BaseService
{
    public RetroAchievementsService(ConfigurationModel configurationModel) : base(configurationModel) {}

    /// <param name="isGameSystem"> Value 0 return all game systems and 1 return only the consoles.</param>
    public async Task<List<ConsoleDto>> getConsolesAsync(int isGameSystem)
    {
        var parameters = new Dictionary<string, string>
        {
            {"y", ApiKey},
            {"g", isGameSystem.ToString()},
            {"a", "1"}
        };

        var content = new FormUrlEncodedContent(parameters);
        string queryString = await content.ReadAsStringAsync();
        var json = await GetAsync($"API_GetConsoleIDs.php?{queryString}");

        var result = JsonSerializer.Deserialize<List<ConsoleDto>>(json);

        return result ?? new List<ConsoleDto>();
    }

    public async Task<List<GameDto>> getConsoleGamesAsync(int consoleId)
    {
        var parameters = new Dictionary<string, string>
        {
            {"y", ApiKey},
            {"i", consoleId.ToString()},
            {"f", "1"}
        };

        var content = new FormUrlEncodedContent(parameters);
        string queryString = await content.ReadAsStringAsync();
        var json = await GetAsync($"API_GetGameList.php?{queryString}");

        var result = JsonSerializer.Deserialize<List<GameDto>>(json);

        return result ?? new List<GameDto>();
    }

    public async Task<GameDto> getGameAndAchievementsAsync(int gameId)
    {
        var parameters = new Dictionary<string, string>
        {
            {"y", ApiKey},
            {"u", ApiUsername},
            {"g", gameId.ToString()}
        };

        var content = new FormUrlEncodedContent(parameters);
        string queryString = await content.ReadAsStringAsync();
        var json = await GetAsync($"API_GetGameInfoAndUserProgress.php?{queryString}");

        var result = JsonSerializer.Deserialize<GameDto>(json);

        return result ?? new GameDto();
    }
}