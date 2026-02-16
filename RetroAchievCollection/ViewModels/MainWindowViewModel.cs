using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RetroAchievCollection.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] 
    private object? _currentView;

    public IRelayCommand LoadConsoleView {get;}
    public IRelayCommand LoadGameView {get;}

    public MainWindowViewModel()
    {
        LoadConsoleView = new RelayCommand(ShowConsolesView);
        LoadGameView = new RelayCommand(ShowGameView);

        ShowConsolesView();
    }

    public void ShowConsolesView()
    {
        Console.WriteLine(CurrentView?.GetType().Name);

        CurrentView = new ConsoleViewModel(this);

        Console.WriteLine(CurrentView?.GetType().Name);
    }

    public void ShowGameView()
    {
        Console.WriteLine(CurrentView?.GetType().Name);

        CurrentView = new GameViewModel(this);

        Console.WriteLine(CurrentView?.GetType().Name);
    }
}