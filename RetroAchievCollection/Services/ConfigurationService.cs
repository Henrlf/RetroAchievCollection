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