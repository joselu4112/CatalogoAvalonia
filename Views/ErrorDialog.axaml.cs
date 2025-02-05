using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;



using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CatalogoAvalonia.Views;
public partial class ErrorDialog : Window
{
    public ErrorDialog(string message)
    {
        InitializeComponent();
        this.FindControl<TextBlock>("MessageText").Text = message;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    // Método que maneja el click en el botón "Aceptar"
    private void OnAcceptClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        // Cierra el diálogo cuando el usuario hace clic en el botón "Aceptar"
        this.Close();
    }
    

}