using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;

namespace RetroAchievCollection.ViewModels.Popups;

public partial class ConfigurationWindowModel : BaseViewModel
{
    private readonly ConfigurationService configurationService = new();

    [ObservableProperty] private string _userName = "";
    [ObservableProperty] private string _apiKey = "";

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
        ClearErrors();

        try
        {
            configurationService.SaveConfigurations(UserName, ApiKey);
            _notificationService?.ShowError("Configurations saved.");

        }
        catch (Exception ex) when (ex.Message.Contains("Username"))
        {
            _notificationService?.ShowError(ex.Message);
        }
        catch (Exception ex) when (ex.Message.Contains("API Key"))
        {
            _notificationService?.ShowError(ex.Message);
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
    }
}