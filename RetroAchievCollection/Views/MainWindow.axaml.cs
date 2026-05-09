using System;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using RetroAchievCollection.Data;
using RetroAchievCollection.Services;
using RetroAchievCollection.ViewModels;

namespace RetroAchievCollection.Views;

// WINDOW LOGIC, EVENTS
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        var notificationService = new NotificationService(this);
        DataContext = new MainWindowViewModel(notificationService);

        using var db = new AppDbContext();
        db.Database.Migrate();
    }

    private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // IF TAB FAVORITES
        if (sender is TabControl tabControl
            && tabControl.SelectedIndex == 1
            && DataContext is MainWindowViewModel vm)
        {
            vm.ShowFavoriteGamesView();
        }
    }
}