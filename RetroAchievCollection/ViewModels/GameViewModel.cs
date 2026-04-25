using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Commands.Game;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<GameCardViewModel> _games = new();
    [ObservableProperty] private string _searchTextGames = "";

    public Guid ConsoleId {get; set;}
    public int ConsoleCodeIntegration {get; set;}
    public string ConsoleName {get; set;} = "";

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

            SynchronizeConsoleGamesCommand command = new(_mainVm.configurationService)
            {
                ConsoleCodeIntegration = ConsoleCodeIntegration
            };

            await command.Execute();

            await LoadGames();

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
            // TODO: SEE ABOUT THE LOADING SCREEN
            _mainVm.ShowLoadingScreen("Loading...");

            await LoadGames(SearchTextGames);
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

    private async Task LoadGames(string searchText = "")
    {
        Games.Clear();
        GameService gameService = new();

        var games = await gameService.GetGames(ConsoleId);
        List<GameModel> gameModels = games
            .Where(n => n.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(a => a.IsFavorite)
            .ThenBy(a => a.Name)
            .ToList();

        foreach (var gameModel in gameModels)
        {
            Games.Add(new GameCardViewModel(_mainVm, gameModel)
            {
                Publisher = !string.IsNullOrWhiteSpace(gameModel.Publisher) ? $" / {gameModel.Publisher}" : "",
                ReleaseDate = gameModel.ReleaseDate?.ToString("dd/MM/yyyy") ?? "-"
            });
        }
    }
}