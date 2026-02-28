using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Commands.Console;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels;

public partial class ConsoleViewModel : BaseViewModel
{
    [ObservableProperty] private ObservableCollection<ConsoleCardViewModel> _consoles = new();

    public ConsoleViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadConsoles();
    }

    private void LoadConsoles()
    {
        Consoles.Clear();

        if (_mainVm == null)
        {
            return;
        }

        Consoles.Add(new ConsoleCardViewModel(_mainVm.LoadGameView)
        {
            Id = 1,
            Name = "Nintendo Wii",
            Company = "Nintendo",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg"
        });

        Consoles.Add(new ConsoleCardViewModel(_mainVm.LoadGameView)
        {
            Id = 2,
            Name = "Super Nintendo",
            Company = "Nintendo",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg"
        });
    }

    public void SynchronizeConsoles()
    {
        try
        {
            SynchronizeConsolesCommand command = new();
            command.execute();
            
            _notificationService?.ShowSuccess("Configurations saved.");
            _mainVm.ShowConsolesView();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
    }
}