using CatalogoVista.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace CatalogoVista.Controller;


internal class ControladorCampeones : Estructurable
{
    private static ControladorCampeones ctr;

    public static int proxIDFoto;

    public List<Campeon>? Campeones=new List<Campeon>();

    //Constructor donde cargo los datos del fichero
    private ControladorCampeones()
    {
        try
        {
            // Obtener todas las imagenes del directorio
            string[] archivos = Directory.GetFiles("Imagenes");

            // Obtener solo los nombres de los archivos sin la extensión y convertirlos a números
            var numeros = archivos
                .Select(archivo => Path.GetFileNameWithoutExtension(archivo).Trim()) // Obtener solo el nombre del archivo sin extensión y quitar espacios extra
                .Where(nombre => int.TryParse(nombre, out _)) // Filtrar solo aquellos que son números válidos
                .Select(nombre => int.Parse(nombre)) // Convertir el nombre a número
                .ToList();

            // Verificar si encontramos números válidos
            if (numeros.Count > 0)
            {
                // Encontrar el número mayor
                ControladorCampeones.proxIDFoto = numeros.Max()+1;
            }
            else
            {
                // Si no hay imágenes válidas, asignamos 0 como valor predeterminado
                ControladorCampeones.proxIDFoto = 0;
            }


            if (!File.Exists(Estructurable.ruta)) //Si no existe lo crea
            {
                using FileStream fichero = File.Create(Estructurable.ruta);
                Campeones = new List<Model.Campeon>();
            }
            else //Si existe lo lee
            {
                using FileStream fichero = File.OpenRead(Estructurable.ruta);
                using BinaryReader br = new BinaryReader(fichero, Encoding.UTF8);

                //Leemos los elementos del fichero
                while (true)
                {
                    //Leo el byte de control que tengo guardado
                    int contr;
                    //Compruebo primero que el fichero no este vacio
                    if (fichero.Length < 1)
                    {
                        contr = -1;//Si no tiene ni un byte que leer sale directamente del programa.
                        Campeones = new List<Model.Campeon>();//Me aseguro tambien de que en este caso mi lista se inicialice vacia
                    }
                    else contr = fichero.ReadByte();//Leo el primer bite que es mi elemento de control

                    if (contr == -1)//Si es -1 se acabo el fichero
                    {
                        break;
                    }
                    switch (contr)
                    {
                        case Estructurable.BCMagico:
                            Campeones.Add(leerCampeonMagico(br));//Si mi byte de control es 1 es un campeon magico
                            break;
                        case Estructurable.BCFisico:
                            Campeones.Add(leerCampeonFisico(br));//Si mi byte de control es un 2 es un campeon fisico
                            break;
                        default:
                            {//Si mi byte de control no es ninguno de los anteriores ha habido un error
                                Campeones.Clear();//Como ha habido un error reinicio los datos
                                throw new FalloDeLecturaException("Ha ocurrido algun error con el fichero, datos no cargados");
                            }

                    }
                }
                //Cerramos el fichero para poder moverlo
                br.Close();
                fichero.Close();
                

            }

        }
        catch (FalloDeLecturaException e)
        {
            Console.WriteLine(e.ToString());

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());

        }

    }

    //estructura datos del fichero(237 bytes por campeon guardado, guardados en este orden), los valores y la estructura concreta
    //estan en la interfaz Estructurable que implementa esta clase para facilitar el uso o modificacion
    public static ControladorCampeones getControlador()
    {
        if (ctr == null)
        {
            ctr = new ControladorCampeones();
        }

        return ctr;
    }

    //Metodo que devuelve un campeon fisico leido del fichero
    private CampeonFisico leerCampeonFisico(BinaryReader br)
    {   //objetos auxiliares para ir añadiendo y devolver del metodo
        CampeonFisico campeon = new CampeonFisico();
        HabilidadPasiva habilidad = new HabilidadPasiva();

        //asigno el valor del tipo de daño que ya se de antemano
        campeon.TipoDanio = TipoDeDanio.FISICO;

        //Leo el nombre
        campeon.Nombre = br.ReadString().Trim();

        //leer la vida
        campeon.Vida = br.ReadInt32();
        //leer el mana
        campeon.Mana = br.ReadInt32();

        //leer la habilidad que tenemos guardada por composicion:
        //leer el nombre
        
        habilidad.Nombre = br.ReadString().Trim();
        //leer la descripcion
        habilidad.Descripcion = br.ReadString().Trim();
        //leer el enfriamiento
        habilidad.Enfriamiento = br.ReadDouble();
        //añadimos la habilidad al campeon
        campeon.Pasiva = habilidad;
        //leer la foto
        campeon.Foto = br.ReadString().Trim();

        //leer el daño AD al ser campeon fisico
        campeon.DanioAD = br.ReadInt32();

        return campeon;
    }
    //Metodo que devuelve un campeon magico leido del fichero, igual que el anterior pero cambiando danioAD por danioAP, por si te lo quieres saltar de revisar
    private CampeonMagico leerCampeonMagico(BinaryReader br)
    {
        CampeonMagico campeon = new CampeonMagico();
        HabilidadPasiva habilidad = new HabilidadPasiva();

        //asigno el valor del tipo de daño que ya se de antemano
        campeon.TipoDanio = TipoDeDanio.MAGICO;

        //Leo el nombre
        campeon.Nombre = br.ReadString().Trim();

        //leer la vida
        campeon.Vida = br.ReadInt32();
        //leer el mana
        campeon.Mana = br.ReadInt32();

        //leer la habilidad que tenemos guardada por composicion:
        //leer el nombre
        habilidad.Nombre = br.ReadString().Trim();
        //leer la descripcion
        habilidad.Descripcion = br.ReadString().Trim();
        //leer el enfriamiento
        habilidad.Enfriamiento = br.ReadDouble();
        //añadimos la habilidad al campeon
        campeon.Pasiva = habilidad;
        //leer la foto

        campeon.Foto = br.ReadString().Trim();

        //leer el daño AP al ser campeon magico
        campeon.DanioAP = br.ReadInt32();

        return campeon;
    }
    //Metodo ajustarString para asegurarme que todos los string tiene el espacio indicado:
    private string ajustarString(string text, int tamanodeseado)
    {
        string textoFinal=text;

        if (text.Length > tamanodeseado)
        {
            return text.Substring(0, tamanodeseado); // Recorta el string si es demasiado largo
        }
        else
        {
            return text.PadRight(tamanodeseado); // Rellena con espacios si es más corto
        }
    }

    //Guardar los datos
    public void guardardatos()
    {
        //Cambio el nombre del archivo aqui para asegurarme que se guarda correctamente
        File.Move(Estructurable.ruta, "campeones" + DateTime.Now.ToString("H_m_s,d_M_yyyy") + ".dat");

        //Abrimos el fichero en modo lectura
        using FileStream fichero = File.OpenWrite(Estructurable.ruta);
        using BinaryWriter br = new BinaryWriter(fichero, Encoding.UTF8);


        foreach (Campeon champ in this.Campeones)
        {
            //Guardar datos si el campeon es fisico, como tengo el atributo
            //del tipo de daño que se instancia automaticamente no necesito comprobar la clase sino solo este atributo.
            if (champ.TipoDanio == TipoDeDanio.FISICO)
            {
                CampeonFisico c = (CampeonFisico)champ;
                br.Write(Estructurable.BCFisico);//Escribir el byte de control
                br.Write(ajustarString(c.Nombre, Estructurable.tamanoNombre));//escribir nombre
                br.Write(c.Vida);//escribir vida
                br.Write(c.Mana);//escribir mana
                                 //escribir pasiva
                br.Write(ajustarString(c.Pasiva.Nombre, Estructurable.tamanoNombre));//escribir nombreP
                br.Write(ajustarString(c.Pasiva.Descripcion, Estructurable.tamanoDescripcion));//escribir descripcionP
                br.Write(c.Pasiva.Enfriamiento);//escribir enfriamientoP
                br.Write(ajustarString(c.Foto, Estructurable.tamanoFoto));//escribir foto
                br.Write(c.DanioAD);//escribir daño AD al ser fisico

            }
            //Guardar los datos si el campeon es Magico
            if (champ.TipoDanio == TipoDeDanio.MAGICO)
            {
                CampeonMagico c = (CampeonMagico)champ;
                br.Write(Estructurable.BCMagico);//Escribir el byte de control
                br.Write(ajustarString(c.Nombre, Estructurable.tamanoNombre));//escribir nombre
                br.Write(c.Vida);//escribir vida
                br.Write(c.Mana);//escribir mana
                                 //escribir pasiva
                br.Write(ajustarString(c.Pasiva.Nombre, Estructurable.tamanoNombre));//escribir nombreP
                br.Write(ajustarString(c.Pasiva.Descripcion, Estructurable.tamanoDescripcion));//escribir descripcionP
                br.Write(c.Pasiva.Enfriamiento);//escribir enfriamientoP
                br.Write(ajustarString(c.Foto, Estructurable.tamanoFoto));//escribir foto
                br.Write(c.DanioAP);//escribir daño AP al ser magico
            }
        }

    }
     
    //Borrar campeon(Cuando solo tiene uno seleccionado)
    public void borrarCampeon(Campeon c)
    {
        Campeones.Remove(c);
    }


    //Modificar un Campeon
    public void modificarCampeon(Campeon campeon)
    {
        int pos = Campeones.IndexOf(campeon);
        bool correcto;
        do
        {
            correcto = true;

            Console.WriteLine("Dime que campo quieres cambiar. ");
            Console.WriteLine("1- Nombre del campeon ");
            Console.WriteLine("2- Vida del campeon ");
            Console.WriteLine("3- Mana del campeon ");
            Console.WriteLine("4- Nombre de su habilidad pasiva ");
            Console.WriteLine("5- Descripcion de su pasiva ");
            Console.WriteLine("6- Enfriamiento de su pasiva ");
            Console.WriteLine("7- Daño del campeon ");
            Console.WriteLine("0- Salir ");

            int eleccion = Int32.Parse(Console.ReadLine());

            switch (eleccion)
            {
                case 1:
                    {
                        Console.WriteLine("Introduce el nuevo Nombre: ");
                        Campeones[pos].Nombre = Console.ReadLine();
                        break;
                    }
                case 2:
                    {
                        Console.WriteLine("Introduce el nuevo valor de Vida: ");
                        Campeones[pos].Vida = Int32.Parse(Console.ReadLine());
                        break;
                    }
                case 3:
                    {
                        Console.WriteLine("Introduce el nuevo valor del Mana: ");
                        Campeones[pos].Vida = Int32.Parse(Console.ReadLine());
                        break;
                    }
                case 4:
                    {
                        Console.WriteLine("Introduce el nuevo Nombre para su Pasiva: ");
                        Campeones[pos].Pasiva.Nombre = Console.ReadLine();
                        break;
                    }
                case 5:
                    {
                        Console.WriteLine("Introduce la nueva Descripcion para su Pasiva: ");
                        Campeones[pos].Pasiva.Descripcion = Console.ReadLine();
                        break;
                    }
                case 6:
                    {
                        Console.WriteLine("Introduce el nuevo Enfriamiento de la Pasiva: ");
                        Campeones[pos].Vida = Int32.Parse(Console.ReadLine());
                        break;
                    }
                case 7:
                    {
                        Console.WriteLine("Introduce el nuevo Daño del campeon (Numerico): ");
                        Campeones[pos].Vida = Int32.Parse(Console.ReadLine());
                        break;
                    }
                case 0:
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Elige una opcion valida por favor.");
                        correcto = false;
                        break;
                    }
            }


        } while (!correcto);
    }

    //ordenar por nombre, mana o vida
    public void ordenar(string str)
    {
        List<Model.Campeon>? campeonesOrdenados = this.Campeones;

        switch (str)
        {
            case "Nombre":
                {
                    campeonesOrdenados=campeonesOrdenados.OrderBy(champ => champ.Nombre).ToList();
                    break;
                }
            case "Vida":
                {
                    campeonesOrdenados=campeonesOrdenados.OrderByDescending(champ => champ.Vida).ToList();
                    break;
                }
            case "Mana":
                {
                    campeonesOrdenados=campeonesOrdenados.OrderByDescending(champ => champ.Mana).ToList();
                    break;
                }
                default : { break; }
        }
           
        this.Campeones = campeonesOrdenados;

    }

    //ordenar por tipo de daño (Para mostrarlos divididos segun el daño)
    public void mostrarDanio()
    {
        //Los separo en 2 listas
        List<Model.CampeonFisico> listaFisicos = new List<CampeonFisico>();
        List<Model.CampeonMagico> listaMagicos = new List<CampeonMagico>();
        for (int i = 0; i < Campeones.Count(); i++)
        {
            if (Campeones[i].TipoDanio.Equals(TipoDeDanio.MAGICO)) listaMagicos.Add((CampeonMagico) Campeones[i]);

            if (Campeones[i].TipoDanio.Equals(TipoDeDanio.FISICO))  listaFisicos.Add((CampeonFisico) Campeones[i]);
            
        }
        //Las muestro por separado:
        Console.WriteLine("Campeones fisicos: ");
        foreach (CampeonFisico c in listaFisicos) Console.WriteLine(c.ToString());

        Console.WriteLine("Campeones magicos: ");
        foreach (CampeonMagico c in listaMagicos) Console.WriteLine(c.ToString());


    }

    //buscar por valor seleccionado
    public List<Campeon> buscarPorValor(List<Campeon> lista,Dictionary<string,string> dic)
    {
        List<Campeon> campeonesBuscados = lista;
        List<string> parametrosBusqueda=dic.Keys.ToList();

        for(int i = 0; i < dic.Count(); i++)
        {
            if (parametrosBusqueda[i].Equals("NOMBRE"))
            {
                campeonesBuscados = campeonesBuscados
                .Where(campeon => campeon.Nombre.StartsWith(dic["NOMBRE"], StringComparison.OrdinalIgnoreCase))
                .ToList();
            }
            if (parametrosBusqueda[i].Equals("MANA"))
            {
                campeonesBuscados = campeonesBuscados
                .Where(campeon => campeon.Mana.Equals(int.Parse(dic["MANA"])))
                .ToList();
            }
            if (parametrosBusqueda[i].Equals("VIDA"))
            {
                campeonesBuscados = campeonesBuscados
                .Where(campeon => campeon.Vida.Equals(int.Parse(dic["VIDA"])))
                .ToList();
            }

        }
        
       
        return campeonesBuscados;
    }

    static public void sumarFoto()
    {
        ControladorCampeones.proxIDFoto++;
    }
}

