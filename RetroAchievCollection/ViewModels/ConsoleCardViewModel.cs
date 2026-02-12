namespace RetroAchievCollection.ViewModels;

public partial class ConsoleCardViewModel : BaseViewModel
{
    public int Id {get; set;} = 0;
    public string Name {get; set;} = "";
    public string ImagePath {get; set;} = "";
    public string Company {get; set;} = "";

    private readonly MainWindowViewModel _mainVm;

    public ConsoleCardViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
    }

    public ConsoleCardViewModel(int id, string name, string imagePath, MainWindowViewModel mainVm)
    {
        Id = id;
        Name = name;
        ImagePath = imagePath;
        _mainVm = mainVm;
    }

    // [RelayCommand]
    // private void ViewGames()
    // {
    //     _mainVm.ShowAchievements(Id, Name);
    // }
}