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
}