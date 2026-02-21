using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Services;
using RetroAchievCollection.ViewModels.Popups;
using RetroAchievCollection.Views.Popups;

namespace RetroAchievCollection.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private object? _currentView;

    public INotificationService NotificationService { get; }
    
    public IRelayCommand LoadConsoleView {get;}
    public IRelayCommand LoadGameView {get;}

    public MainWindowViewModel(INotificationService notificationService)
    {
        LoadConsoleView = new RelayCommand(ShowConsolesView);
        LoadGameView = new RelayCommand(ShowGameView);
        NotificationService = notificationService;
        
        ShowConsolesView();
    }

    public void ShowConsolesView()
    {
        CurrentView = new ConsoleViewModel(this);
    }

    public void ShowGameView()
    {
        CurrentView = new GameViewModel(this);
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