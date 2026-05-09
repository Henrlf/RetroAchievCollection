using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Models;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.Services.User;

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
        Dispatcher.UIThread.InvokeAsync(async () => {await LoadValues();});
    }

    public async Task LoadValues()
    {
        ConfigurationRepository configurationRepository = new();

        ConfigurationModel configModel = await configurationRepository.GetConfiguration();
        UserName = configModel.UserName;
        ApiKey = configModel.ApiKey;
    }

    [RelayCommand]
    public async Task SaveConfigurations()
    {
        UserNameHasError = string.IsNullOrWhiteSpace(UserName);
        ApiKeyHasError = string.IsNullOrWhiteSpace(ApiKey);

        try
        {
            ConfigurationService configurationService = new();
            await configurationService.SaveConfigurations(UserName, ApiKey);

            _notificationService?.ShowSuccess("Configurations saved.");
            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
    }
}