using CommunityToolkit.Mvvm.ComponentModel;

namespace RetroAchievCollection.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    protected readonly MainWindowViewModel? _mainVm;

    protected BaseViewModel() {}
    
    protected BaseViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
    }
}