using System;
using Avalonia.Controls;
using RetroAchievCollection.Services;
using RetroAchievCollection.ViewModels;

namespace RetroAchievCollection.Views;

// WINDOW LOGIC, EVENTS
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // DataContext = new MainWindowViewModel();
    }
    
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        var notificationService = new NotificationService(this);
        DataContext = new MainWindowViewModel(notificationService);
    }
}