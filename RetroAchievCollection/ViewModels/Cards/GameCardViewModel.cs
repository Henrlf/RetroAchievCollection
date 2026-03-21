using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Commands.Game;
using RetroAchievCollection.Models;
using RetroAchievCollection.ViewModels.Popups;
using RetroAchievCollection.Views.Popups;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class GameCardViewModel : BaseViewModel
{
    public int Id {get; set;}
    public int ConsoleId {get; set;}
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

    [RelayCommand]
    public async Task SynchronizeAchievements()
    {
        try
        {
            _mainVm.TextLoading = "Synchronizing...";
            _mainVm.IsLoading = true;

            SynchronizeGameCommand command = new(_mainVm.configurationService)
            {
                GameId = Id
            };

            await command.execute();

            GameModel? gameModel = command.GameModel;

            _notificationService?.ShowSuccess("Game achievements synchronized.");
            // _mainVm.ShowGameView();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
        finally
        {
            _mainVm.IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task ShowGameConfigurations()
    {
        try
        {
            var dialog = new GameConfigurationWindow()
            {
                Width = 550,
                Height = 200,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                DataContext = new GameConfigurationWindowModel(_mainVm, Id, ConsoleId)
            };

            Window? owner = null;

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                owner = desktop.MainWindow;
            }

            if (owner != null)
            {
                await dialog.ShowDialog(owner);
                return;
            }

            dialog.Show();
        }
        catch (Exception e)
        {
            _notificationService?.ShowError(e.Message);
        }
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