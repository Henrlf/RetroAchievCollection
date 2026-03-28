using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Commands.Console;
using RetroAchievCollection.Services.Console;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class ConsoleViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<ConsoleCardViewModel> _consoles = new();

    public ConsoleViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadConsoles();
    }

    public async Task SynchronizeConsoles()
    {
        try
        {
            _mainVm.TextLoading = "Synchronizing...";
            _mainVm.IsLoading = true;

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
            _mainVm.IsLoading = false;
        }
    }

    private void LoadConsoles()
    {
        Consoles.Clear();
        ConsoleService consoleService = new();

        foreach (var consoleModel in consoleService.GetConsoles())
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
    }
}