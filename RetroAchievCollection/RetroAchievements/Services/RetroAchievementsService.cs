using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public async Task<Collection<ConsoleDto>> getConsolesAsync(int isGameSystem)
    {
        var parameters = new Dictionary<string, string>
        {
            {"y", ApiKey},
            {"g", isGameSystem.ToString()}
        };

        var content = new FormUrlEncodedContent(parameters);
        string queryString = await content.ReadAsStringAsync();
        var json = await GetAsync($"API_GetConsoleIDs.php?{queryString}");
        
        var result = JsonSerializer.Deserialize<Collection<ConsoleDto>>(json);
        
        return result ?? new Collection<ConsoleDto>();
    }
}