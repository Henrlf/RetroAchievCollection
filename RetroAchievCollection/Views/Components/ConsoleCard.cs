using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace RetroAchievCollection.Views.Components;

public partial class ConsoleCard : UserControl
{
    public ConsoleCard()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static readonly StyledProperty<string> ConsoleImageProperty = AvaloniaProperty.Register<ConsoleCard, string>(nameof(ConsoleImage));
    public static readonly StyledProperty<string> ConsoleNameProperty = AvaloniaProperty.Register<ConsoleCard, string>(nameof(ConsoleName));
    public static readonly StyledProperty<string> CompanyProperty = AvaloniaProperty.Register<ConsoleCard, string>(nameof(Company));

    public string ConsoleImage
    {
        get => GetValue(ConsoleImageProperty);
        set => SetValue(ConsoleImageProperty, value);
    }

    public string ConsoleName
    {
        get => GetValue(ConsoleNameProperty);
        set => SetValue(ConsoleNameProperty, value);
    }

    public string Company
    {
        get => GetValue(CompanyProperty);
        set => SetValue(CompanyProperty, value);
    }

    private void SeeAchievementsCommand(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not int consoleId || consoleId == 0)
        {
            return;
        }
    }
}