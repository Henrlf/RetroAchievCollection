using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Services;
using RetroAchievCollection.Services.Console;
using RetroAchievCollection.Services.User;
using RetroAchievCollection.ViewModels.Popups;
using RetroAchievCollection.Views.Popups;

namespace RetroAchievCollection.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private object? _currentView;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _textLoading = "Loading...";

    public readonly ConfigurationService configurationService = new();

    public INotificationService NotificationService {get;}
    public IRelayCommand LoadConsoleView {get;}
    public IAsyncRelayCommand<Guid> LoadGameView {get;}
    public ConsoleViewModel? ConsoleViewCache {get; private set;}

    public MainWindowViewModel(INotificationService notificationService)
    {
        LoadConsoleView = new RelayCommand(ShowConsolesView);
        LoadGameView = new AsyncRelayCommand<Guid>(ShowGameView);
        NotificationService = notificationService;

        ShowConsolesView();
    }

    [RelayCommand]
    public void ShowConsolesView()
    {
        if (ConsoleViewCache == null)
        {
            ConsoleViewCache = new ConsoleViewModel(this);
        }

        CurrentView = ConsoleViewCache;
    }

    [RelayCommand]
    public async Task ShowGameView(Guid consoleId)
    {
        ConsoleService consoleService = new();
        var consoleModel = await consoleService.GetConsole(consoleId);

        CurrentView = new GameViewModel(this, consoleId)
        {
            ConsoleName = consoleModel != null ? consoleModel.Name : "",
            ConsoleCodeIntegration = consoleModel?.CodeIntegration ?? 0
        };
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
}