using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Commands.Game;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;
using RetroAchievCollection.Services.Console;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class GameViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<GameCardViewModel> _games = new();
    [ObservableProperty] private string _searchText = "";

    public int ConsoleId {get; set;}
    public string ConsoleName {get; set;} = "";

    public GameViewModel(MainWindowViewModel mainVm, int consoleId) : base(mainVm)
    {
        ConsoleId = consoleId;
        LoadGames();
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
            _mainVm.TextLoading = "Synchronizing...";
            _mainVm.IsLoading = true;

            SynchronizeConsoleGamesCommand command = new(_mainVm.configurationService);
            command.ConsoleId = ConsoleId;
            await command.execute();

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
            _mainVm.IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task SearchGames()
    {
        try
        {
            // TODO: SEE ABOUT THE LOADING SCREEN
            _mainVm.TextLoading = "Loading...";
            _mainVm.IsLoading = true;

            await LoadGames(SearchText);
        }
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
        finally
        {
            _mainVm.IsLoading = false;
        }
    }

    private Task LoadGames(string searchText = "")
    {
        Games.Clear();
        ConsoleService consoleService = new();

        List<GameModel> games = consoleService.GetGames(ConsoleId)
            .Where(n => n.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(a => a.IsFavorite)
            .ThenBy(a => a.Name)
            .ToList();

        foreach (var gameModel in games)
        {
            Games.Add(new GameCardViewModel(_mainVm, gameModel.Id, gameModel.ConsoleId)
            {
                Id = gameModel.Id,
                ConsoleId = gameModel.ConsoleId,
                Name = gameModel.Name,
                Publisher = !string.IsNullOrWhiteSpace(gameModel.Publisher) ? $" / {gameModel.Publisher}" : "",
                Developer = gameModel.Developer,
                Genre = gameModel.Genre,
                ReleaseDate = DateOnly.TryParse(gameModel.Released, out var d) ? d.ToString("dd/MM/yyyy") : "",
                PlayCommand = gameModel.PlayCommand,
                IsFavorite = gameModel.IsFavorite,
                ImagePath = gameModel.ImagePath
            });
        }

        return Task.CompletedTask;
    }
}