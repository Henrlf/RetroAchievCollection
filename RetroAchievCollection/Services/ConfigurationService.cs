using System;
using System.IO;
using System.Text.Json;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.Services;

public class ConfigurationService : BaseService
{
    public void SaveConfigurations(string UserName, string ApiKey)
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            throw new ArgumentException("Username is required!");
        }

        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new ArgumentException("API Key is required!");
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
        if (!File.Exists("config.json"))
        {
            return new ConfigurationModel();
        }

        string json = File.ReadAllText("config.json");
        ConfigurationModel? configModel = JsonSerializer.Deserialize<ConfigurationModel>(json);

        if (configModel == null)
        {
            return new ConfigurationModel();
        }

        return configModel;
    }
}