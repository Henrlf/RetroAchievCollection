using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Commands.Game;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.ViewModels.Popups;
using RetroAchievCollection.Views.Popups;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class GameCardViewModel : BaseViewModel
{
    public int Id {get; set;}
    public int ConsoleId {get; set;}

    [ObservableProperty] public string _name = "";
    [ObservableProperty] public string _publisher = "";
    [ObservableProperty] public string _developer = "";
    [ObservableProperty] public string _genre = "";
    [ObservableProperty] public string _releaseDate = "-";
    [ObservableProperty] public string _playCommand = "-";
    [ObservableProperty] public bool _isFavorite = false;
    
    [ObservableProperty] public string _trophyIconPath = "/Assets/trophy.svg";
    [ObservableProperty] public bool _hasPlayCommand = false;

    [ObservableProperty] public int _achievementsCount;
    [ObservableProperty] public int _achievementsCompleted;
    [ObservableProperty] public int _achievProgressPercentage;
    [ObservableProperty] public ObservableCollection<AchievementCardViewModel> _achievements = new();

    public string ImagePath {get; set;} = "";
    public Bitmap? GameImage => File.Exists(ImagePath) ? new Bitmap(ImagePath) : null;

    public GameCardViewModel(MainWindowViewModel mainVm, int gameId, int consoleId) : base(mainVm)
    {
        Id = gameId;
        ConsoleId = consoleId;

        LoadAchievements();
    }

    [RelayCommand]
    public async Task SynchronizeAchievements()
    {
        try
        {
            _mainVm.ShowLoadingScreen("Synchronizing...");

            SynchronizeGameCommand command = new(_mainVm.configurationService)
            {
                GameId = Id
            };

            await command.execute();

            GameModel? gameModel = command.GameModel;

            if (gameModel == null)
            {
                return;
            }

            Name = gameModel.Name;
            Publisher = !string.IsNullOrWhiteSpace(gameModel.Publisher) ? $" / {gameModel.Publisher}" : "";
            Developer = gameModel.Developer;
            Genre = gameModel.Genre;

            if (!string.IsNullOrWhiteSpace(gameModel.Released))
            {
                DateOnly releaseDate = DateOnly.Parse(gameModel.Released);
                ReleaseDate = releaseDate.ToString("dd/MM/yyyy");
            }

            LoadAchievements();

            _notificationService?.ShowSuccess("Game achievements synchronized.");
        }
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
        finally
        {
            _mainVm.HideLoadingScreen();
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
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
    }

    [RelayCommand]
    public async Task PlayGame()
    {
        try
        {
            GameService gameService = new();
            GameModel? gameModel = gameService.GetGame(Id, ConsoleId);

            if (gameModel == null)
            {
                throw new NullReferenceException("Game was not found!");
            }

            if (string.IsNullOrWhiteSpace(gameModel.PlayCommand))
            {
                throw new NullReferenceException("Play command was not defined!");
            }

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {gameModel.PlayCommand}",
                UseShellExecute = false,
                CreateNoWindow = true
            });

            if (process == null)
            {
                throw new Exception("Failed to start the game!");
            }

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                throw new Exception("Failed to execute the command!");
            }
        }
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
    }

    [RelayCommand]
    public void FavoriteGame()
    {
        try
        {
            IsFavorite = !IsFavorite;
            
            GameService gameService = new();
            GameModel? gameModel = gameService.GetGame(Id, ConsoleId);

            if (gameModel == null)
            {
                throw new NullReferenceException("Game was not found!");
            }

            gameModel.IsFavorite = IsFavorite;
            gameService.SaveGameModel(gameModel);
        }
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
    }
    
    private void LoadAchievements()
    {
        Achievements.Clear();
        GameService gameService = new();
        GameModel? gameModel = gameService.GetGame(Id, ConsoleId);

        if (gameModel == null)
        {
            return;
        }

        foreach (var achievementModel in gameModel.Achievements)
        {
            Achievements.Add(new AchievementCardViewModel(_mainVm)
            {
                Id = achievementModel.Id,
                Name = achievementModel.Name,
                Description = achievementModel.Description,
                ImagePath = achievementModel.ImagePath,
                IsCompleted = achievementModel.IsCompleted,
                IsCompletedHardcore = achievementModel.IsCompletedHardcore,
            });
        }

        HasPlayCommand = !string.IsNullOrWhiteSpace(gameModel.PlayCommand);
        AchievementsCount = gameModel.TotalAchievements;
        AchievementsCompleted = gameModel.TotalAchievementsCompleted;
        
        double result = (double)AchievementsCompleted / AchievementsCount * 150;
        AchievProgressPercentage = (int)result;
        
        if (AchievementsCount == AchievementsCompleted)
        {
            TrophyIconPath = "/Assets/trophy_filled.svg";
        }
    }
}