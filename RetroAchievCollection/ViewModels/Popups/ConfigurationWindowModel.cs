using System;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.ViewModels.Popups;

public partial class ConfigurationWindowModel : BaseViewModel
{
    public event Action? RequestClose;

    [ObservableProperty] private string _userName = "";
    [ObservableProperty] private string _apiKey = "";

    [ObservableProperty] private bool _userNameHasError;
    [ObservableProperty] private bool _apiKeyHasError;

    public ConfigurationWindowModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadValues();
    }

    public void LoadValues()
    {
        ConfigurationModel configModel = _mainVm.configurationService.getConfigurationModel();
        UserName = configModel.UserName;
        ApiKey = configModel.ApiKey;
    }

    public void SaveConfigurations()
    {
        UserNameHasError = string.IsNullOrWhiteSpace(UserName);
        ApiKeyHasError = string.IsNullOrWhiteSpace(ApiKey);

        try
        {
            _mainVm.configurationService.SaveConfigurations(UserName, ApiKey);
            _notificationService?.ShowSuccess("Configurations saved.");
            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
    }
}