using CommunityToolkit.Mvvm.ComponentModel;

namespace RetroAchievCollection.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty]
    private object? _currentView;

    public MainWindowViewModel()
    {
        ShowConsoles();
    }
    
    public void ShowConsoles()
    {
        CurrentView = new ConsoleViewModel(this);
    }
    
    // public void ShowAchievements(int consoleId, string consoleName)
    // {
    //     CurrentView = new GamesViewModel(consoleId, consoleName);
    // }
}