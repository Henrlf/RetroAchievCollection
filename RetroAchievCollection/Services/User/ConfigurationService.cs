using System;
using System.Threading.Tasks;
using RetroAchievCollection.Models;
using RetroAchievCollection.Repositories;

namespace RetroAchievCollection.Services.User;

public class ConfigurationService : BaseService
{
    public async Task SaveConfigurations(string UserName, string ApiKey)
    {
        if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new ArgumentException("Username and API Key is required!");
        }

        ConfigurationRepository configurationRepository = new ConfigurationRepository();
        
        ConfigurationModel configurationModel = await configurationRepository.GetConfiguration();
        configurationModel.UserName = UserName;
        configurationModel.ApiKey = ApiKey;

        await configurationRepository.UpdateConfiguration(configurationModel);
    }
}