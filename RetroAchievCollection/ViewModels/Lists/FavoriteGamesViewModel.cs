using System;
using System.Collections.ObjectModel;
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

        var gameModelCount = await gameRepository.GetFavoriteGamesCount(searchText);
        var gameModels = await gameRepository.GetFavoriteGamesPaged(CurrentPage, PageSize, searchText, true);

        TotalPages = Math.Max(1, (int)Math.Ceiling(gameModelCount / (double)PageSize));

        var viewModels = await Task.Run(() => gameModels
            .Select(g => new GameCardViewModel(_mainVm, g))
            .ToList());

        Games = new ObservableCollection<GameCardViewModel>(viewModels);
    }
}