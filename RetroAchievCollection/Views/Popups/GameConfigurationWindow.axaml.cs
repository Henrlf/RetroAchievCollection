using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using RetroAchievCollection.ViewModels.Popups;

namespace RetroAchievCollection.Views.Popups;

public partial class GameConfigurationWindow : Window
{
    public GameConfigurationWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is GameConfigurationWindowModel vm)
        {
            vm.RequestClose += Close;
        }
    }

    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}