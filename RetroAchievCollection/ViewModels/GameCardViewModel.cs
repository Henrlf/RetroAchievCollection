using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.ViewModels;

public partial class GameCardViewModel : BaseViewModel
{
    public int Id {get; set;} = 0;
    public string Name {get; set;} = "";
    public DateOnly? ReleaseDate {get; set;} = null;
    public int AchievProgressPercentage {get; set;} = 50;
    public int TotalAchievements {get; set;} = 10;
    public int CompletedAchievements {get; set;} = 10;
    public string GameImage {get; set;} = "";
    public Collection<AchievementModel> Achievements {get; set;} = new();
    
    public GameCardViewModel()
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
            Completed = false,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 2",
            Description = "Descrição Achievement 2",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            Completed = true,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 3",
            Description = "Descrição Achievement 5",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            Completed = false,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 4",
            Description = "Descrição Achievement 5",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            Completed = true,
        });

        Achievements.Add(new AchievementModel
        {
            Id = 1,
            Name = "Achievement 5",
            Description = "Descrição Achievement 2",
            ImagePath = "https://img.cdndsgni.com/preview/11908070.jpg",
            Completed = true,
        });
    }

    private static async Task<Bitmap?> LoadImageAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        try
        {
            using var http = new HttpClient();
            await using var stream = await http.GetStreamAsync(url);
            return new Bitmap(stream);
        }
        catch
        {
            return null;
        }
    }
}