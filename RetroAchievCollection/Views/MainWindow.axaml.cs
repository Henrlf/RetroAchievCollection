using System.Collections.ObjectModel;
using Avalonia.Controls;
using RetroAchievCollection.Models;

namespace RetroAchievCollection.Views;

// WINDOW LOGIC, EVENTS
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new
        {
            Consoles = getConsoles()
        };
    }

    private ObservableCollection<ConsoleModel> getConsoles()
    {
        return new ObservableCollection<ConsoleModel>
        {
            new ConsoleModel {ConsoleImage = "https://img.cdndsgni.com/preview/11908070.jpg", ConsoleName = "Nintendo Wii", Company = "Nintendo"},
            new ConsoleModel {ConsoleImage = "https://img.cdndsgni.com/preview/11908070.jpg", ConsoleName = "Super Nintendo", Company = "Nintendo"},
        };
    }
}