using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class GameCardViewModel : BaseViewModel
{
    public int Id {get; set;}
    public string Name {get; set;} = "";
    public DateOnly? ReleaseDate {get; set;}
    public int AchievProgressPercentage {get; set;}
    public int AchievementsCompleted {get; set;}
    public int AchievementsCount {get; set;}
    public string ImagePath {get; set;} = "";
    public Collection<AchievementModel> Achievements {get; set;} = new();

    public Bitmap? GameImage => File.Exists(ImagePath) ? new Bitmap(ImagePath) : null;
    
    public GameCardViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadAchievements();
    }

    private void LoadAchievements()
    {
        Achievements.Clear();

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 1",
            Description = "Descrição Achievement 1",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            IsCompleted = false,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 2",
            Description = "Descrição Achievement 2",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            IsCompleted = true,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 3",
            Description = "Descrição Achievement 5",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            IsCompleted = false,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 4",
            Description = "Descrição Achievement 5",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            IsCompleted = true,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 5",
            Description = "Descrição Achievement 2",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            IsCompleted = true,
        });

        AchievementsCount = Achievements.Count;
        AchievementsCompleted = Achievements.Count(achiev => achiev.IsCompleted);
        
        double result = (double)AchievementsCompleted / AchievementsCount * 150;
        AchievProgressPercentage = (int)result;
    }
}