
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CatalogoAvalonia.Views;
    public partial class ConfirmacionDialog : Window
    {
        public bool Resultado { get; private set; } = false;

        public ConfirmacionDialog()
        {
            InitializeComponent();
        }

        // Acción cuando el usuario presiona 'Sí'
        private void OnYesClicked(object sender, RoutedEventArgs e)
        {
            Resultado = true;
            this.Close(); // Cerrar la ventana
        }

        // Acción cuando el usuario presiona 'No'
        private void OnNoClicked(object sender, RoutedEventArgs e)
        {
            Resultado = false;
            this.Close(); // Cerrar la ventana
        }
    }