using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Commands.Game;
using RetroAchievCollection.Repositories;
using RetroAchievCollection.Services;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels.Lists;

public partial class GameViewModel : PagedViewModel
{
    [ObservableProperty] private ObservableCollection<GameCardViewModel> _games = new();
    [ObservableProperty] private string _searchTextGames = "";

    public Guid ConsoleId {get; set;}
    public int ConsoleCodeIntegration {get; set;}
    public string ConsoleName {get; set;} = "";
    
    protected override async Task LoadViewData() => await LoadGames(SearchTextGames);

    public GameViewModel(MainWindowViewModel mainVm, Guid consoleId) : base(mainVm)
    {
        ConsoleId = consoleId;
        Dispatcher.UIThread.InvokeAsync(async () => {await LoadGames();});
    }

    [RelayCommand]
    public void LoadConsolesView()
    {
        _mainVm.ShowConsolesView();
    }

    [RelayCommand]
    public async Task SynchronizeConsoleGames()
    {
        try
        {
            _mainVm.ShowLoadingScreen("Synchronizing...");

            SynchronizeConsoleGamesCommand command = new()
            {
                ConsoleCodeIntegration = ConsoleCodeIntegration
            };

            await command.Execute();

            await LoadGames(SearchTextGames);

            _notificationService?.ShowSuccess("Console games synchronized.");
        }
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
        finally
        {
            _mainVm.HideLoadingScreen();
        }
    }

    [RelayCommand]
    public async Task SearchGames()
    {
        try
        {
            await LoadGames(SearchTextGames);
        }
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
    }

    protected virtual async Task LoadGames(string searchText = "")
    {
        Games.Clear();
        GameRepository gameRepository = new();

        var gameModelCount = await gameRepository.GetConsoleGamesCount(ConsoleId, searchText);
        var gameModels = await gameRepository.GetConsoleGamesPaged(ConsoleId, CurrentPage, PageSize, searchText, true);

        TotalPages = Math.Max(1, (int)Math.Ceiling(gameModelCount / (double)PageSize));

        var viewModels = await Task.Run(() => gameModels
            .Select(g => new GameCardViewModel(_mainVm, g))
            .ToList());

        Games = new ObservableCollection<GameCardViewModel>(viewModels);
    }
}