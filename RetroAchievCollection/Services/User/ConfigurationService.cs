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

        SaveToJson("config.json", configModel);
    }

    public ConfigurationModel LoadConfigurationModel()
    {
        string json = LoadJson("config.json");

        if (string.IsNullOrWhiteSpace(json))
        {
            return new ConfigurationModel();
        }

        ConfigurationModel? configModel = JsonSerializer.Deserialize<ConfigurationModel>(json);

        if (configModel == null)
        {
            return new ConfigurationModel();
        }

        return configModel;
    }
}