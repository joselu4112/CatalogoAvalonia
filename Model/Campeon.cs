using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoVista.Model
{
    [Serializable]
    public class Campeon
    {

        //clase abstracta para definir atributos y metodos de todos los campeones
        //nombre del campeon
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (value.Length > 20) nombre = value.Substring(0, 20);
                else nombre = value;
            }
        }
        //Vida
        private int vida;
        public int Vida
        {
            get { return vida;}
            set { if (value < 0) vida = 0; 
                  else vida = value;   }
        }
        //Mana
        private int mana;
        public int Mana
        {
            get { return mana; }
            set
            {
                if (value < 0) mana = 0;
                else mana = value;
            }
        }
        //Habilidad unica pasiva del campeon
        public HabilidadPasiva Pasiva { get; set; }

        
        //Tipo de daño
        public TipoDeDanio TipoDanio { get;set; }
        
        //Ruta foto
        private string foto;


        public string Foto
        {
            get { return foto; }
            set
            {
                if (value.Length > 100) foto = value.Substring(0, 100);
                else foto = value;
            }
        }
         
        public Campeon(string nombre, int vida, int mana, TipoDeDanio tipoDanio, HabilidadPasiva pasiva, string foto)
        {
            this.Mana = mana;
            this.Nombre = nombre;
            this.Vida = vida;
            this.TipoDanio = tipoDanio;
            this.Foto = foto;
            this.Pasiva = pasiva;
        }

        public Campeon()//Constructor vacio
        {
        }
        public override string ToString()
        {
            return $"Es un campeon: {TipoDanio}, Nombre: {Nombre}, Vida: {Vida}" +
                   $", Mana: {Mana}, Habilidad Pasiva: {Pasiva.ToString()}, Foto: {Foto}";
        }


    }
}
