using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoVista.Model
{
    internal class CampeonFisico:Campeon
    {
        private int danioAD;
        public int DanioAD
        {
            get { return danioAD; }
            set
            {
                if (value < 0) danioAD = -value;
                else danioAD = value;
            }
        }
        public CampeonFisico(string nombre, int vida, int mana, TipoDeDanio tipoDanio, HabilidadPasiva pasiva, string foto, int danioAD)
        {
            this.Mana = mana;
            this.Nombre = nombre;
            this.Vida = vida;
            this.TipoDanio = TipoDeDanio.FISICO;
            this.Foto = foto;
            this.Pasiva = pasiva;
            this.danioAD = danioAD;
        }

        public CampeonFisico()//Constructor vacio
        {
            this.TipoDanio = TipoDeDanio.FISICO;
        }

        public override string ToString()
        {
            return $"Es un campeon: {TipoDanio}, Nombre: {Nombre}, Vida: {Vida}, Mana: {Mana}, Daño AP: {DanioAD}, Habilidad Pasiva: {Pasiva.ToString()}, Foto: {Foto}";
        }
    }
}
