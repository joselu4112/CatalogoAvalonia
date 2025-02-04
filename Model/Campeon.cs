using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoVista.Model
{
    abstract class Campeon
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

        

        public TipoDeDanio TipoDanio { get;set; }

        //Lo que nos dijiste de guardar una foto sin usarla, yo la limite a 100 caracteres
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

        public abstract override string ToString();


    }
}
