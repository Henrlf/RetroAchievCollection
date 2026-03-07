using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<GameCardViewModel> _games = new();

    public GameViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadGames();
    }

    public void LoadConsolesView()
    {
        _mainVm.ShowConsolesView();
    }

    private void LoadGames()
    {
        Games.Clear();

        Games.Add(new GameCardViewModel(_mainVm)
        {
            Id = 1,
            Name = "Super Mario",
            ReleaseDate = new DateOnly(2026, 10, 1),
        });
    }
}