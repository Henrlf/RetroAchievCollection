using Avalonia.Controls;
using RetroAchievCollection.ViewModels;

namespace RetroAchievCollection.Views;

// WINDOW LOGIC, EVENTS
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        DataContext = new MainWindowViewModel();
    }
}