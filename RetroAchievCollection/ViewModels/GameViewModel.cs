using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

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
        if (_mainVm == null)
        {
            return;
        }

        _mainVm.ShowConsolesView();
    }

    private void LoadGames()
    {
        Games.Clear();

        if (_mainVm == null)
        {
            return;
        }

        Games.Add(new GameCardViewModel
        {
            Id = 1,
            Name = "Super Mario",
            ReleaseDate = new DateOnly(2026, 10, 1),
            AchievProgressPercentage = 20,
            TotalAchievements = 20,
            CompletedAchievements = 100,
        });

        Games.Add(new GameCardViewModel
        {
            Id = 2,
            Name = "Super Mario",
            ReleaseDate = new DateOnly(2026, 10, 1),
            AchievProgressPercentage = 20,
            TotalAchievements = 20,
            CompletedAchievements = 100,
        });

        Games.Add(new GameCardViewModel
        {
            Id = 3,
            Name = "Super Mario",
            ReleaseDate = new DateOnly(2026, 10, 1),
            AchievProgressPercentage = 20,
            TotalAchievements = 20,
            CompletedAchievements = 100,
        });
    }


}