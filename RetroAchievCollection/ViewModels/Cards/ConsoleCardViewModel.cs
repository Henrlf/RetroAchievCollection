using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using RetroAchievCollection.Models;
using RetroAchievCollection.Services;

namespace RetroAchievCollection.ViewModels.Cards;

public partial class ConsoleCardViewModel : BaseViewModel
{
    public Guid Id {get; set;}
    public string Name {get; set;} = "";
    public string Company {get; set;} = "";
    public int GamesCount {get; set;}
    public Bitmap? Image {get; set;}

    public ConsoleCardViewModel(MainWindowViewModel mainVm, ConsoleModel consoleModel) : base(mainVm)
    {
        LoadValues(consoleModel);
    }

    [RelayCommand]
    public async Task LoadGamesView()
    {
        await _mainVm.ShowGameView(Id);
    }

    private void LoadValues(ConsoleModel consoleModel)
    {
        Id = consoleModel.Id;
        Name = consoleModel.Name;
        Company = consoleModel.Company ?? "";

        var imagePath = Path.Combine(BaseService.MainDirectory, consoleModel.ImagePath);

        if (File.Exists(imagePath))
        {
            Image = new Bitmap(imagePath);
        }
    }
}