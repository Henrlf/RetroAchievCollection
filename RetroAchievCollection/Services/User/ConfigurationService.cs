using System;
using System.Linq;
using RetroAchievCollection.Data;
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

        using var db = new AppDbContext();
        ConfigurationModel configurationModel = db.Configuration.First();
        configurationModel.UserName = UserName;
        configurationModel.ApiKey = ApiKey;

        db.Update(configurationModel);
        db.SaveChanges();
    }

    public ConfigurationModel getConfigurationModel()
    {
        using var db = new AppDbContext();
        return db.Configuration.First();
    }
}