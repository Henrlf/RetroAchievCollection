using System.IO;
using Avalonia.Media.Imaging;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class AchievementCardViewModel : BaseViewModel
{
    public AchievementModel AchievementModel {get; set;}
    public Bitmap? Image => File.Exists(AchievementModel.ImagePath) ? new Bitmap(AchievementModel.ImagePath) : null;

    public AchievementCardViewModel(MainWindowViewModel mainVm, AchievementModel achievementModel) : base(mainVm)
    {
        AchievementModel = achievementModel;
    }
}