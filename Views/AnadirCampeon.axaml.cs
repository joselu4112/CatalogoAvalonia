using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace CatalogoAvalonia.Views;

public partial class AnadirCampeon : Window
{
    public AnadirCampeon()
    {
        InitializeComponent();
        // Ruta relativa a la imagen
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Imagenes\incognita.jpg");

        // Verificar que la imagen existe antes de cargarla
        if (File.Exists(imagePath))
        {
            MyImage.Source = new Bitmap(imagePath);
        }
        
    }
    private void TextBox_KeyDown(object sender, Avalonia.Input.KeyEventArgs e)
    {
        // Permitir las teclas numéricas
        if ((e.Key >= Avalonia.Input.Key.D0 && e.Key <= Avalonia.Input.Key.D9) || 
            (e.Key >= Avalonia.Input.Key.NumPad0 && e.Key <= Avalonia.Input.Key.NumPad9))
        {
            return; // Permitir los números
        }

        // Si no es una tecla permitida, manejar el evento y no permitir la tecla
        e.Handled = true;

    }

}