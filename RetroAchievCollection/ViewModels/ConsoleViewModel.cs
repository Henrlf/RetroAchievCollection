using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Commands.Console;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;
using RetroAchievCollection.Services.Console;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class ConsoleViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<ConsoleCardViewModel> _consoles = new();
    [ObservableProperty] private string _searchTextConsoles = "";

    public ConsoleViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        Dispatcher.UIThread.InvokeAsync(async () => {await LoadConsoles();});
    }

    [RelayCommand]
    public async Task SynchronizeConsoles()
    {
        try
        {
            _mainVm.ShowLoadingScreen("Synchronizing...");

            SynchronizeConsolesCommand command = new(_mainVm.configurationService);
            await command.execute();

            _notificationService?.ShowSuccess("Consoles synchronized.");
            _mainVm.ShowConsolesView();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
        finally
        {
            _mainVm.HideLoadingScreen();
        }
    }

    [RelayCommand]
    public async Task SearchConsoles()
    {
        try
        {
            await LoadConsoles(SearchTextConsoles);
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

    private async Task LoadConsoles(string searchText = "")
    {
        Consoles.Clear();
        ConsoleService consoleService = new();
        GameService gameService = new();

        var consoles = await consoleService.GetConsoles();
        List<ConsoleModel> consoleModels = consoles
            .Where(n => n.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a => a.Name)
            .ToList();

        foreach (var consoleModel in consoleModels)
        {
            var games = await gameService.GetGames(consoleModel.Id);

            Consoles.Add(new ConsoleCardViewModel(_mainVm)
            {
                ConsoleModel = consoleModel,
                GamesCount = games.Count,
            });
        }
    }
}