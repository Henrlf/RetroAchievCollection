using System;
using System.Linq;
using System.Threading.Tasks;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels.Lists;

public partial class FavoriteGamesViewModel : GameViewModel
{
    public FavoriteGamesViewModel(MainWindowViewModel mainVm) : base(mainVm, Guid.Empty) {}

    protected override async Task LoadGames(string searchText = "")
    {
        Games.Clear();
        GameRepository gameRepository = new();

        var gameModels = (await gameRepository.GetFavoriteGames(true))
            .Where(n => n.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(g => g.Name)
            .ToList();

        foreach (var gameModel in gameModels)
        {
            Games.Add(new GameCardViewModel(_mainVm, gameModel));
        }
    }
}