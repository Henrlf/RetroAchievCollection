using Avalonia.Controls;
using Avalonia.Interactivity;

namespace RetroAchievCollection.Views.Popups;

public partial class ConfigurationWindow : Window
{
    public ConfigurationWindow()
    {
        InitializeComponent();
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}