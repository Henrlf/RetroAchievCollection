using System;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services.Game;

namespace RetroAchievCollection.ViewModels.Popups;

public partial class GameConfigurationWindowModel : BaseViewModel
{
    public event Action? RequestClose;

    private readonly GameService GameService = new();
    private GameModel GameModel {get; set;}

    [ObservableProperty] private string _playCommand = "";

    public GameConfigurationWindowModel(MainWindowViewModel mainVm, int gameId, int consoleId) : base(mainVm)
    {
        var model = GameService.GetGame(gameId, consoleId);

        if (model == null)
        {
            throw new NullReferenceException("Game was not found!");
        }
        
        GameModel = model;
        PlayCommand = GameModel.PlayCommand;
    }

    public void SaveConfigurations()
    {
        try
        {
            GameModel.PlayCommand = PlayCommand;
            GameService.SaveGameModel(GameModel);
            
            _notificationService?.ShowSuccess("Configurations saved.");
            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
    }
}