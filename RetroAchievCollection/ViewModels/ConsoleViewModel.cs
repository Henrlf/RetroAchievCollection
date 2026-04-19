using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Commands.Console;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;
using RetroAchievCollection.Services.Console;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class ConsoleViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<ConsoleCardViewModel> _consoles = new();
    [ObservableProperty] private string _searchTextConsoles = "";

    public ConsoleViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadConsoles();
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

    private Task LoadConsoles(string searchText = "")
    {
        Consoles.Clear();
        ConsoleService consoleService = new();

        List<ConsoleModel> consoles = consoleService.GetConsoles()
            .Where(n => n.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
            .OrderBy(a => a.Name)
            .ToList();

        foreach (var consoleModel in consoles)
        {
            Consoles.Add(new ConsoleCardViewModel(_mainVm)
            {
                Id = consoleModel.Id,
                Name = consoleModel.Name,
                Company = consoleModel.Company,
                Games = consoleService.GetGames(consoleModel.Id).Count,
                ImagePath = consoleModel.ImagePath
            });
        }
        
        return Task.CompletedTask;
    }
}