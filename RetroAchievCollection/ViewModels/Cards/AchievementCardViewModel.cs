using System.IO;
using Avalonia.Media.Imaging;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class AchievementCardViewModel : BaseViewModel
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Description {get; set;} = "";
    public string ImagePath {get; set;} = "";
    public bool IsCompleted {get; set;}
    public bool IsCompletedHardcore {get; set;}

    public Bitmap? Image => File.Exists(ImagePath) ? new Bitmap(ImagePath) : null;
    
    public AchievementCardViewModel(MainWindowViewModel mainVm) : base(mainVm) {}
}