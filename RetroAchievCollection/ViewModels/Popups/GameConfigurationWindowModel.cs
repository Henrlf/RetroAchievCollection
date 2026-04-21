using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services.Game;
using RetroAchievCollection.ViewModels.Cards;

namespace RetroAchievCollection.ViewModels.Popups;

public partial class GameConfigurationWindowModel : BaseViewModel
{
    public event Action? RequestClose;

    private readonly GameService GameService = new();
    private GameCardViewModel GameCardViewModel {get; set;}
    private GameModel GameModel {get; set;}

    [ObservableProperty] private string _playCommand = "";

    public GameConfigurationWindowModel(MainWindowViewModel mainVm, GameCardViewModel gameCardViewModel) : base(mainVm)
    {
        var model = GameService.GetGame(gameCardViewModel.Id, gameCardViewModel.ConsoleId);

        if (model == null)
        {
            throw new NullReferenceException("Game was not found!");
        }

        GameCardViewModel = gameCardViewModel;

        GameModel = model;
        PlayCommand = GameModel.PlayCommand;
    }

    [RelayCommand]
    public void SaveConfigurations()
    {
        try
        {
            GameModel.PlayCommand = PlayCommand;
            GameService.SaveGameModel(GameModel);

            GameCardViewModel.HasPlayCommand = !string.IsNullOrWhiteSpace(PlayCommand);

            _notificationService?.ShowSuccess("Configurations saved.");
            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            _notificationService?.ShowError(ex.Message);
        }
    }
}