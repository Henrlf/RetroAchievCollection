using Avalonia.Controls;
using RetroAchievCollection.ViewModels;

namespace RetroAchievCollection.Views;

public partial class GameView : UserControl
{
    public GameView()
    {
        InitializeComponent();
        
        if (DataContext == null)
        {
            DataContext = new GameViewModel(new MainWindowViewModel());
        }
    }
}