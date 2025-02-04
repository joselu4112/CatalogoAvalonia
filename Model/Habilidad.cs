using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CatalogoVista.Model
{
    public class HabilidadPasiva
    {
        //Habilidad pasiva del campeon
        //nombre, limitado a 50 caracteres
        private string nombre;
        public string Nombre {
            get {  return nombre; }
            set { if (value.Length > 20) nombre = value.Substring(0, 20);
                else nombre = value;
            }
        }
        //Descripcion, limitada a 100 caracteres
        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set
            {
                if (value.Length > 50) descripcion = value.Substring(0, 50);
                else descripcion = value;
            }
        }

        //enfriamiento/cooldown, si es que tiene, 0 si está activa todo el tiempo
        private double enfriamiento;
        public double Enfriamiento
        {
            get { return enfriamiento; }
            set
            {
                if (value < 0) enfriamiento = 0.1;
                else enfriamiento = value;
            }
        }

        public HabilidadPasiva(string nombre, string descripcion, double enfriamiento=0)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Enfriamiento= enfriamiento;
        }
        //constructor vacio
        public HabilidadPasiva() { }

        public override string ToString()
        {
            return $"Nombre: {Nombre}, Descripción: {Descripcion}, Enfriamiento: {Enfriamiento}";
        }

    }
}
