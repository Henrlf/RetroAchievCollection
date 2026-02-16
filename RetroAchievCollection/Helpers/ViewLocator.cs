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
            Console.WriteLine("Sem DATA");
            return new TextBlock {Text = "Null view model"};
        }

        var name = data.GetType().FullName!
            .Replace("ViewModel", "View");
        
        var type = Type.GetType(name);

        if (type == null)
        {
            Console.WriteLine($"View NÃO encontrada: {name}");
            return new TextBlock {Text = "Not Found: " + name};
        }

        Console.WriteLine($"View encontrada: {name}");
        
        return (Control)Activator.CreateInstance(type)!;
    }

    public bool Match(object? data)
    {
        var matches = data is BaseViewModel;
        
        Console.WriteLine($"ViewLocator.Match chamado para {data?.GetType().Name ?? "null"} -> retorna {matches}");
        
        return matches;
    }
}