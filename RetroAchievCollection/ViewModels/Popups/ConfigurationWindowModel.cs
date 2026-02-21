using System;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;

namespace RetroAchievCollection.ViewModels.Popups;

public class ConfigurationWindowModel : BaseViewModel
{
    private readonly ConfigurationService configurationService = new();

    public string UserName {get; set;} = "";
    public string ApiKey {get; set;} = "";

    public ConfigurationWindowModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadValues();
    }

    public void LoadValues()
    {
        ConfigurationModel configModel = configurationService.LoadConfigurationModel();

        UserName = configModel.UserName;
        ApiKey = configModel.ApiKey;
    }

    public void SaveConfigurations()
    {
        try
        {
            configurationService.SaveConfigurations(UserName, ApiKey);
            _notificationService?.ShowError("Configurations saved.");
        }
        catch (Exception ex)
        {
            // Console.WriteLine(ex);
            
            _notificationService?.ShowError(ex.Message);
        }
    }
}