using CommunityToolkit.Mvvm.Input;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class ConsoleCardViewModel : BaseViewModel
{
    public int Id {get; set;} = 0;
    public string Name {get; set;} = "";
    public string Company {get; set;} = "";
    public int Games {get; set;} = 0;
    public string ImagePath {get; set;} = "";

    public IRelayCommand? LoadGameViewCommand {get; set;}

    public ConsoleCardViewModel(IRelayCommand loadGameViewCommand)
    {
        LoadGameViewCommand = loadGameViewCommand;
    }
}