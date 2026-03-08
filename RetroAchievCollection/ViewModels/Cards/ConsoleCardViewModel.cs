using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class ConsoleCardViewModel : BaseViewModel
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Company {get; set;} = "";
    public int Games {get; set;} = 0;
    public string ImagePath {get; set;} = "";

    public Bitmap? Image => File.Exists(ImagePath) ? new Bitmap(ImagePath) : null;
    public IRelayCommand<int>? LoadGameViewCommand {get; set;}

    public ConsoleCardViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadGameViewCommand = mainVm.LoadGameView;
    }
}