using System.IO;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using CatalogoAvalonia.Views;

namespace CatalogoAvalonia.Views
{
    public partial class MenuPrincipal : Window
    {

        public MenuPrincipal()
        {
            InitializeComponent();

            // Ruta relativa a la imagen
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Imagenes\imagenlol.jpg");

            // Verificar que la imagen existe antes de cargarla
            if (File.Exists(imagePath))
            {
                MyImage.Source = new Bitmap(imagePath);
            }
        }


        
    }
}