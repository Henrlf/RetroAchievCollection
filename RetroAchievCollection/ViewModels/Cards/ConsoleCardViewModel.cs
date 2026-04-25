using System;
using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class ConsoleCardViewModel : BaseViewModel
{
    public ConsoleModel ConsoleModel {get; set;} = new();
    public int GamesCount {get; set;}
    
    public Bitmap? Image => File.Exists(ConsoleModel.ImagePath) ? new Bitmap(ConsoleModel.ImagePath) : null;
    public IRelayCommand<Guid>? LoadGameViewCommand {get; set;}

    public ConsoleCardViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        LoadGameViewCommand = mainVm.LoadGameView;
    }
}