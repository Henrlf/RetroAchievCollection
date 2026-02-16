using Avalonia.Controls;
using RetroAchievCollection.ViewModels;

namespace RetroAchievCollection.Views;

public partial class ConsoleView : UserControl
{
    public ConsoleView()
    {
        InitializeComponent();

        if (DataContext == null)
        {
            DataContext = new ConsoleViewModel(new MainWindowViewModel());
        }
    }
}