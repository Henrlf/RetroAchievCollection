using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Services;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<GameCardViewModel> _games = new();

    public GameViewModel(MainWindowViewModel mainVm, int consoleId) : base(mainVm)
    {
        LoadGames(consoleId);
    }

    public void LoadConsolesView()
    {
        _mainVm.ShowConsolesView();
    }

    private void LoadGames(int consoleId)
    {
        Games.Clear();
        GameService gameService = new();

        foreach (var gameModel in gameService.GetGames(consoleId))
        {
            Games.Add(new GameCardViewModel(_mainVm)
            {
                Id = gameModel.Id,
                Name = gameModel.Name,
                ImagePath = Path.Combine(BaseService.MainDirectory, gameModel.ImagePath)
            });
        }
    }
}