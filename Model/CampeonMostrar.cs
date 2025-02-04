using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoVista.Model
{
     class CampeonMostrar
    {
        public CampeonMostrar(string nombre, int vida, int mana)
        {
            Nombre = nombre;
            Vida = vida;
            Mana = mana;
        }

        //Clase solo para mostrar estos campos en el form de MenuBuscar
        public string Nombre { get; set; }
        public int Vida { get; set; }
        public int Mana { get; set; }

        
    }
}

