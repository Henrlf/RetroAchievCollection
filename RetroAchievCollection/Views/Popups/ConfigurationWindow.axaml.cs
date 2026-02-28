using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using RetroAchievCollection.ViewModels.Popups;

namespace RetroAchievCollection.Views.Popups;

public partial class ConfigurationWindow : Window
{
    public ConfigurationWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
    
        if (DataContext is ConfigurationWindowModel vm)
        {
            vm.RequestClose += Close;
        }
    }
    
    private void CloseWindow(object sender, RoutedEventArgs e)
    {
        Close();
    }
}