using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Commands.Console;
using RetroAchievCollection.Services;
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
            SynchronizeConsolesCommand command = new(_mainVm.configurationService);
            await command.execute();
            
            _notificationService?.ShowSuccess("Consoles synchronized.");
            _mainVm.ShowConsolesView();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
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
                ImagePath = Path.Combine(BaseService.Directory, "images", consoleModel.ImagePath)
            });
        }
    }
}