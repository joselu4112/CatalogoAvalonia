
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoVista.Model
{
    internal class CampeonMagico:Campeon
    {
        private int danioAP;
        public int DanioAP
        {
            get { return danioAP; }
            set { if (value < 0) danioAP = -value;
                  else danioAP = value; }
        }
        public CampeonMagico(string nombre, int vida, int mana, TipoDeDanio tipoDanio, HabilidadPasiva pasiva,string foto,int danioAP)
        {
            this.Mana = mana;
            this.Nombre = nombre;
            this.Vida = vida;
            this.TipoDanio = TipoDeDanio.MAGICO;
            this.Foto = foto;
            this.Pasiva = pasiva;   
            this.danioAP = danioAP;
        }
        public CampeonMagico()//Constructor vacio
        {
            this.TipoDanio = TipoDeDanio.MAGICO;
        }

        public override string ToString()
        {
            return $"Es un campeon: {TipoDanio}, Nombre: {Nombre}, Vida: {Vida}, Mana: {Mana}, Daño AP: {DanioAP}, Habilidad Pasiva: {Pasiva.ToString()}, Foto: {Foto}";
        }
    }

    
}
