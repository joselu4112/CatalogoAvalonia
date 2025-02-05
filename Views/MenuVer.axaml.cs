using System;
using Avalonia.Controls;
using CatalogoVista.Model;

namespace CatalogoAvalonia.Views
{
    public partial class MenuVer : Window
    {
        private MenuPrincipal principal;
        private int campeonActual;
        private int totalCampeones;

        public MenuVer(MenuPrincipal menu)
        {
            principal = menu;
            InitializeComponent();
        }
        
    }
}
