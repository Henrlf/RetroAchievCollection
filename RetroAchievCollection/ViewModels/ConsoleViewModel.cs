using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RetroAchievCollection.ViewModels;

public partial class ConsoleViewModel : BaseViewModel
{
    private readonly MainWindowViewModel _mainVm;

    [ObservableProperty]
    private ObservableCollection<ConsoleCardViewModel> _consoles = new();

    public ConsoleViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
        LoadConsoles();
    }

    private void LoadConsoles()
    {
        Consoles.Clear();
        
        Consoles.Add(new ConsoleCardViewModel(_mainVm)
        {
            Id = 1,
            Name = "Nintendo Wii",
            Company = "Nintendo",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg"
        });

        Consoles.Add(new ConsoleCardViewModel(_mainVm)
        {
            Id = 2,
            Name = "Super Nintendo",
            Company = "Nintendo",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg"
        });
    }
}