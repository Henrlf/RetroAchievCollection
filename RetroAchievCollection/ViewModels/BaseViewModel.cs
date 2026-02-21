using CommunityToolkit.Mvvm.ComponentModel;
using RetroAchievCollection.Services;

namespace RetroAchievCollection.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    protected readonly MainWindowViewModel? _mainVm;
    protected readonly INotificationService? _notificationService;
    
    protected BaseViewModel() {}
    
    protected BaseViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
        _notificationService = mainVm.NotificationService;
    }
}