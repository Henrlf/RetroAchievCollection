using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.Services;
using RetroAchievCollection.ViewModels.Lists;
using RetroAchievCollection.ViewModels.Popups;
using RetroAchievCollection.Views.Popups;

namespace RetroAchievCollection.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private object? _consolesTab;
    [ObservableProperty] private object? _favoriteGamesTab;
    // [ObservableProperty] private object? _allGamesView;
    
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _textLoading = "Loading...";

    public INotificationService NotificationService {get;}
    public ConsoleViewModel? ConsoleViewCache {get; private set;}

    public MainWindowViewModel(INotificationService notificationService)
    {
        NotificationService = notificationService;

        ShowConsolesView();
    }

    [RelayCommand]
    public void ShowConsolesView()
    {
        ConsoleViewCache ??= new ConsoleViewModel(this);
        ConsolesTab = ConsoleViewCache;
    }

    [RelayCommand]
    public async Task ShowGameView(Guid consoleId)
    {
        ConsoleRepository consoleRepository = new();
        var consoleModel = await consoleRepository.GetConsole(consoleId);

        ConsolesTab = new GameViewModel(this, consoleId)
        {
            ConsoleName = consoleModel != null ? consoleModel.Name : "",
            ConsoleCodeIntegration = consoleModel?.CodeIntegration ?? 0
        };
    }

    [RelayCommand]
    public void ShowFavoriteGamesView()
    {
        FavoriteGamesTab = new FavoriteGamesViewModel(this);
    }

    [RelayCommand]
    public async Task ShowConfigurations()
    {
        var dialog = new ConfigurationWindow()
        {
            Width = 450,
            Height = 250,
            CanResize = false,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            DataContext = new ConfigurationWindowModel(this)
        };

        Window? owner = null;

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            owner = desktop.MainWindow;
        }

        if (owner != null)
        {
            await dialog.ShowDialog(owner);
        }
        else
        {
            dialog.Show();
        }
    }
    
    public void ShowLoadingScreen(string text)
    {
        TextLoading = text;
        IsLoading = true;
    }

    public void HideLoadingScreen()
    {
        IsLoading = false;
    }
}