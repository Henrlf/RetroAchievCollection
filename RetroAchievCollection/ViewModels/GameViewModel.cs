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

    protected virtual async Task LoadGames(string searchText = "")
    {
        Games.Clear();
        GameRepository gameRepository = new();

        var gameModels = (await gameRepository.GetConsoleGames(ConsoleId, true))
            .Where(n => n.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(a => a.IsFavorite)
            .ThenBy(a => a.Name)
            .ToList();

        foreach (var gameModel in gameModels)
        {
            Games.Add(new GameCardViewModel(_mainVm, gameModel));
        }
    }
}