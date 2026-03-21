using System;
using System.Text.Json;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.Services.User;

public class ConfigurationService : BaseService
{
    public void SaveConfigurations(string UserName, string ApiKey)
    {
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new ArgumentException("Username and API Key is required!");
        }

        var configModel = new ConfigurationModel
        {
            UserName = UserName,
            ApiKey = ApiKey
        };

        SaveJson("config.json", configModel);
    }

    public ConfigurationModel getConfigurationModel()
    {
        string json = LoadJson("config.json");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new ConfigurationModel();
        }

        ConfigurationModel? configModel = JsonSerializer.Deserialize<ConfigurationModel>(json);

        return configModel ?? new ConfigurationModel();
    }
}