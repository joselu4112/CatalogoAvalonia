using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CatalogoAvalonia.ViewModels;
using CatalogoAvalonia.Views;

namespace CatalogoAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            MenuPrincipal menup=new MenuPrincipal();
            desktop.MainWindow = menup;
            desktop.MainWindow.DataContext = new MainVM(menup);
        }

        base.OnFrameworkInitializationCompleted();
    }
}