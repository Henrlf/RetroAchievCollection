using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using RetroAchievCollection.ViewModels;

namespace RetroAchievCollection.Helpers;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
        {
            return new TextBlock {Text = "Null view model"};
        }

        var name = data.GetType().FullName!
            .Replace("ViewModel", "View");
        
        var type = Type.GetType(name);

        if (type == null)
        {
            return new TextBlock {Text = "Not Found: " + name};
        }

        return (Control)Activator.CreateInstance(type)!;
    }

    public bool Match(object? data)
    {
        return data is BaseViewModel;
    }
}