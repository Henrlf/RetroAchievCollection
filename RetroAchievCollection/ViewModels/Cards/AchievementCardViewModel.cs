using System.IO;
using Avalonia.Media.Imaging;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class AchievementCardViewModel : BaseViewModel
{
    public AchievementModel AchievementModel {get; set;}
    public Bitmap? Image {get; set;}

    public AchievementCardViewModel(MainWindowViewModel mainVm, AchievementModel achievementModel) : base(mainVm)
    {
        AchievementModel = achievementModel;
        
        var imagePath = Path.Combine(BaseService.MainDirectory, achievementModel.ImagePath);

        if (File.Exists(imagePath))
        {
            Image = new Bitmap(imagePath);
        }
    }
}