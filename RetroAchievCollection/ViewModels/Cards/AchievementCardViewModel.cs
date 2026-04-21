using System.IO;
using Avalonia.Media.Imaging;
using RetroAchievCollection.Enum;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class AchievementCardViewModel : BaseViewModel
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public string Description {get; set;} = "";
    public string ImagePath {get; set;} = "";
    public AchievementStatus Status {get; set;} = AchievementStatus.NotCompleted;
    
    public Bitmap? Image => File.Exists(ImagePath) ? new Bitmap(ImagePath) : null;
    
    public AchievementCardViewModel(MainWindowViewModel mainVm) : base(mainVm) {}
}