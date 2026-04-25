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
    [ObservableProperty] public GameModel _gameModel = new();
    [ObservableProperty] public string _publisher = "";
    [ObservableProperty] public string _releaseDate = "-";

    [ObservableProperty] public string _trophyIconPath = "/Assets/trophy.svg";
    [ObservableProperty] public bool _hasPlayCommand = false;
    [ObservableProperty] public int _achievProgressPercentage;
    [ObservableProperty] public ObservableCollection<AchievementCardViewModel> _achievements = new();

    public Bitmap? GameImage => File.Exists(GameModel.ImagePath) ? new Bitmap(GameModel.ImagePath) : null;

    public GameCardViewModel(MainWindowViewModel mainVm, GameModel gameModel) : base(mainVm)
    {
        GameModel = gameModel;

        LoadValues();
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
                GameCodeIntegration = GameModel.CodeIntegration
            };

            await command.execute();

            GameModel? gameModel = command.GameModel;

            if (gameModel == null)
            {
                return;
            }

            GameModel = gameModel;
            Publisher = !string.IsNullOrWhiteSpace(gameModel.Publisher) ? $" / {gameModel.Publisher}" : "";
            ReleaseDate = gameModel.ReleaseDate?.ToString("dd/MM/yyyy") ?? "-";

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
                DataContext = new GameConfigurationWindowModel(_mainVm, this)
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
            if (GameModel == null)
            {
                throw new NullReferenceException("Game was not found!");
            }

            if (string.IsNullOrWhiteSpace(GameModel.PlayCommand))
            {
                throw new NullReferenceException("Play command was not defined!");
            }

            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {GameModel.PlayCommand}",
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
            if (GameModel == null)
            {
                throw new NullReferenceException("Game was not found!");
            }

            GameModel.IsFavorite = !GameModel.IsFavorite;

            GameService gameService = new();
            gameService.SaveGameModel(GameModel);
        }
        catch (Exception ex)
        {
            BaseService.SaveError(ex.ToString());
            _notificationService?.ShowError(ex.Message);
        }
    }

    private void LoadValues()
    {
        HasPlayCommand = !string.IsNullOrWhiteSpace(GameModel.PlayCommand);
        var achievementsCount = GameModel.AchievementsCount;
        var achievementsCompleted = GameModel.AchievementsCompleted;

        double result = (double)achievementsCompleted / achievementsCount * 150;
        AchievProgressPercentage = (int)result;

        if (achievementsCount == achievementsCompleted)
        {
            TrophyIconPath = "/Assets/trophy_filled.svg";
        }
    }

    private void LoadAchievements()
    {
        Achievements.Clear();
        GameModel gameModel = GameModel;

        foreach (var achievementModel in gameModel.Achievements)
        {
            Achievements.Add(new AchievementCardViewModel(_mainVm, achievementModel));
        }
    }
}