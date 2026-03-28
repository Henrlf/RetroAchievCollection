using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Models;
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
    public IRelayCommand<int> LoadGameView {get;}

    public MainWindowViewModel(INotificationService notificationService)
    {
        LoadConsoleView = new RelayCommand(ShowConsolesView);
        LoadGameView = new RelayCommand<int>(ShowGameView);
        NotificationService = notificationService;

        ShowConsolesView();
    }

    public void ShowConsolesView()
    {
        CurrentView = new ConsoleViewModel(this);
    }

    public void ShowGameView(int consoleId)
    {
        ConsoleService consoleService = new();
        ConsoleModel? consoleModel = consoleService.GetConsole(consoleId);

        CurrentView = new GameViewModel(this, consoleId)
        {
            ConsoleName = consoleModel != null ? consoleModel.Name : ""
        };
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