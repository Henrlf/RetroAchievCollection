using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RetroAchievCollection.ViewModels.Lists;

public abstract partial class PagedViewModel : BaseViewModel
{
    [ObservableProperty] private int _currentPage = 1;
    [ObservableProperty] private int _totalPages = 1;

    public bool HasPreviousPage {get; set;}
    public bool HasNextPage {get; set;}

    protected const int PageSize = 15;

    protected PagedViewModel(MainWindowViewModel mainVm) : base(mainVm)
    {
        HasPreviousPage = CurrentPage > 1;
        HasNextPage = CurrentPage < TotalPages;
    }

    protected int GetActualSkip()
    {
        return Math.Max(CurrentPage - 1, 0) * PageSize;
    }

    [RelayCommand]
    public async Task PreviousPage()
    {
        HasPreviousPage = CurrentPage > 1;

        if (!HasPreviousPage)
        {
            return;
        }

        CurrentPage--;
        await LoadViewData();
    }

    [RelayCommand]
    public async Task NextPage()
    {
        HasNextPage = CurrentPage < TotalPages;

        if (!HasNextPage)
        {
            return;
        }

        CurrentPage++;
        await LoadViewData();
    }

    // ----- ABSTRACT FUNCTIONS -----

    protected abstract Task LoadViewData();

}