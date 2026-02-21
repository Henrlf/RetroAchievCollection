using Avalonia.Controls;
using Avalonia.Controls.Notifications;

namespace RetroAchievCollection.Services;

public class NotificationService : INotificationService
{
    private readonly WindowNotificationManager _manager;

    public NotificationService(Window window)
    {
        _manager = new WindowNotificationManager(window)
        {
            Position = NotificationPosition.TopRight,
            MaxItems = 3
        };
    }

    public void ShowError(string message)
    {
        _manager.Show(new Notification("Error", message, NotificationType.Error));
    }

    public void ShowSuccess(string message)
    {
        _manager.Show(new Notification("Success", message, NotificationType.Success));
    }
}