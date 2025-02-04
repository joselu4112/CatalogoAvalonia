using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoVista.Controller
{
    public interface Estructurable
    {
        /*estructura datos del fichero(237 bytes por campeon guardado, guardados en este orden):
            char(1)       para saber si es 1 (magico) o 2 (fisico)
            nombre(20)    string
            vida(4)       int
            mana(4)       int
            Habilidad(78)
                {
                nombre(20)          string
                descripcion(50)     string
                enfriamiento(8)     double
                }
            foto(100)           string
            dañoAP/dañoAD(4)    int
*/
        //Aqui guardo las constantes de tamaño en bytes de cada elemento para facilitar el uso
        const byte BCFisico = 1;//byte de control fisico
        const byte BCMagico = 0;//byte de control magico
        const int tamanoNombre = 20;
        const int tamanovida = 4;
        const int tamanoMana = 4;
        const int tamanoDescripcion = 50;
        const int tamanoEnfriamiento = 8;
        const int tamanoFoto = 100;
        const int tamanoDanio = 4;

        //Ruta del fichero:
        static public string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "catalogo.dat");
    }
}
