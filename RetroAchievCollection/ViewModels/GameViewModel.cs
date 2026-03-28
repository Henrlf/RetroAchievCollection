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

    public int ConsoleId {get; set;}
    public string ConsoleName {get; set;} = "";

    public GameViewModel(MainWindowViewModel mainVm, int consoleId) : base(mainVm)
    {
        ConsoleId = consoleId;
        LoadGames();
    }

    public void LoadConsolesView()
    {
        _mainVm.ShowConsolesView();
    }

    private void LoadGames()
    {
        Games.Clear();
        ConsoleService consoleService = new();

        List<GameModel> games = consoleService.GetGames(ConsoleId)
            .OrderBy(a => a.IsFavorite)
            .ThenBy(a => a.Name)
            .ToList();

        foreach (var gameModel in games)
        {
            Games.Add(new GameCardViewModel(_mainVm, gameModel.Id, gameModel.ConsoleId)
            {
                Id = gameModel.Id,
                ConsoleId = gameModel.ConsoleId,
                Name = gameModel.Name,
                Publisher = !string.IsNullOrWhiteSpace(gameModel.Publisher) ? " / " + gameModel.Publisher : "",
                Developer = gameModel.Developer,
                Genre = gameModel.Genre,
                ReleaseDate = DateOnly.TryParse(gameModel.Released, out var d) ? d.ToString("dd/MM/yyyy") : "",
                PlayCommand = gameModel.PlayCommand,
                IsFavorite = gameModel.IsFavorite,
                ImagePath = gameModel.ImagePath
            });
        }
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

            LoadGames();

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
}