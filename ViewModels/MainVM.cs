using Avalonia.Controls;
using ReactiveUI;
using System.Reactive;
using System.Collections.ObjectModel;
using CatalogoVista.Model;
using System.Text.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CatalogoAvalonia.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CatalogoAvalonia.ViewModels;
public partial class MainVM : ObservableObject
{
    //Lista y movimiento sobre ella
    private List<Campeon> _campeones;
    private int _campeonActual;
    public List<Campeon> Campeones
    {
        get => _campeones;
        set
        {
            _campeones = value;
        }
    }
 
    public int CampeonActual
    {
        get => _campeonActual;
        set
        {
            // Cambiar el valor del campeón actual
            _campeonActual = value;
        }
    }

  
    //Fichero
    private String fichero = "campeones0.dat";

    // Ventanas para controlar cuales vamos mostrando
    private readonly MenuPrincipal _mainWindow;
    private MenuVer _menuVer;
    private AnadirCampeon _menuAnadir;
    
    
    //Atributos de campeon para mostrar
    [ObservableProperty] public int _campeonMostrar;
    [ObservableProperty] public int _totalCampeones;
    [ObservableProperty] private string _nombre;
    [ObservableProperty] private int _vida;
    [ObservableProperty] private int _mana;
    [ObservableProperty] private string _nombrePasiva;
    [ObservableProperty] private string _descripcionPasiva;
    [ObservableProperty] private double _enfriamientoPasiva;
    [ObservableProperty] private TipoDeDanio _tipoDanio;
    [ObservableProperty] private String rutaFoto;
    
    //Atributos de campeon para añadir
    [ObservableProperty] private string _nombreAnadir;
    [ObservableProperty] private int _vidaAnadir;
    [ObservableProperty] private int _manaAnadir;
    [ObservableProperty] private string _nombrePasivaAnadir;
    [ObservableProperty] private string _descripcionPasivaAnadir;
    [ObservableProperty] private double _enfriamientoPasivaAnadir;
    [ObservableProperty] private TipoDeDanio _tipoDanioAnadir;
    [ObservableProperty] private String _tipoDanioAnadirAux="MAGICO";
    [ObservableProperty] private String rutaFotoAnadir="incognita.jpg";

    private int proxIDFoto;
    
    //Constructor
    public MainVM(MenuPrincipal ventanaPrincipal)
    {
        _mainWindow = ventanaPrincipal;
        CargarCampeones(fichero);
        CampeonActual = 0;
        CampeonMostrar = CampeonActual;
        
        string[] archivos = Directory.GetFiles("..\\..\\..\\Imagenes");
        var numeros = archivos
            .Select(archivo => Path.GetFileNameWithoutExtension(archivo).Trim()) // Obtener solo el nombre del archivo sin extensión y quitar espacios extra
            .Where(nombre => int.TryParse(nombre, out _)) // Filtrar solo aquellos que son números válidos
            .Select(nombre => int.Parse(nombre)) // Convertir el nombre a número
            .ToList();
        if (numeros.Count > 0)
        {
            // Encontrar el número mayor
            proxIDFoto = numeros.Max()+1;
        }
        else
        {
            proxIDFoto = 0;
        }
    }
    //Metodo para actualizar la pantalla Ver
    public void actualizarCampos()
    {
        //Si tenemos algun campo actualiza
        if (Campeones.Count > 0)
        {
            //Actualizar campos a mostrar si tenemos algun elemento en la lista
            CampeonMostrar = CampeonActual + 1;
            TotalCampeones = Campeones.Count;
            Nombre = Campeones[CampeonActual].Nombre;
            Vida = Campeones[CampeonActual].Vida;
            Mana = Campeones[CampeonActual].Mana;
            NombrePasiva = Campeones[CampeonActual].Pasiva.Nombre;
            DescripcionPasiva = Campeones[CampeonActual].Pasiva.Descripcion;
            EnfriamientoPasiva = Campeones[CampeonActual].Pasiva.Enfriamiento;
            TipoDanio = Campeones[CampeonActual].TipoDanio;
            rutaFoto = Campeones[_campeonActual].Foto;
            

            // Ruta de la imagen basada en la propiedad rutaFoto
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Imagenes\", this.rutaFoto);

            // Verificar que el archivo de la imagen existe antes de cargarla
            if (File.Exists(imagePath))
            {
                _menuVer.MyImage.Source = new Bitmap(imagePath);
            }
            else
            {
                imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Imagenes\incognita.jpg");
                _menuVer.MyImage.Source = new Bitmap(imagePath);
            }
        }
        else
        {
            String imagePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\Imagenes\incognita.jpg");
            _menuVer.MyImage.Source = new Bitmap(imagePath);
        }
    }
    // Agregar un nuevo campeón

    public void limpiarCamposAgregar()
    {
        NombreAnadir = "";
        VidaAnadir = 0;
        ManaAnadir = 0;
        NombrePasivaAnadir = "";
        DescripcionPasivaAnadir = "";
        EnfriamientoPasivaAnadir = 0;
        TipoDanioAnadirAux = "MAGICO";
        rutaFotoAnadir="incognita.jpg";
        this._menuAnadir.MyImage.Source = new Bitmap($"..\\..\\..\\Imagenes/{rutaFotoAnadir}");
    }
    
    [RelayCommand]
    public void anadirCampeon()
    {
        // Verificar que los campos no estén vacíos antes de agregar
        if (string.IsNullOrEmpty(NombreAnadir) || string.IsNullOrEmpty(NombrePasivaAnadir)
            || string.IsNullOrEmpty(DescripcionPasivaAnadir))
        {
            var errorDialog = new ErrorDialog("Por favor, rellene todos los campos.");
            errorDialog.ShowDialog(_menuAnadir);
            return;
        }
        if (VidaAnadir < 0 || ManaAnadir < 0 || EnfriamientoPasivaAnadir< 0)
        {
            var errorDialog = new ErrorDialog("Los campos numericos no pueden ser negativos.");
            errorDialog.ShowDialog(_menuAnadir);
            return;
        }
        
        //Foto para añadir al campeon
        if (string.IsNullOrEmpty(RutaFotoAnadir))
        {
            RutaFotoAnadir = "incognita.jpg";
        }
        
        // Crear un nuevo campeón con los valores de las propiedades

        if (_tipoDanioAnadirAux.Equals("MAGICO"))
        {
            TipoDanioAnadir = TipoDeDanio.MAGICO;
        }
        else
        {
            TipoDanioAnadir = TipoDeDanio.FISICO;
        }
        var nuevoCampeon = new Campeon
        {
            Nombre = NombreAnadir,
            Vida = VidaAnadir,
            Mana = ManaAnadir,
            TipoDanio = TipoDanioAnadir,
            Pasiva=new HabilidadPasiva(_nombrePasivaAnadir,_descripcionPasivaAnadir,_enfriamientoPasivaAnadir),
            Foto = RutaFotoAnadir
        };

        // Agregar el nuevo campeón a la lista
        Campeones.Add(nuevoCampeon);
        limpiarCamposAgregar();
        guardarCampeones();
    }
    
    //Añadir foto:
    [RelayCommand]
    private async void SubirFoto(Window ventanaPadre)
    {
        var dlg = new OpenFileDialog();
        dlg.Filters.Add(new FileDialogFilter() { Name = "Imágenes JPEG", Extensions = { "jpg" } });
        dlg.Filters.Add(new FileDialogFilter() { Name = "Imágenes PNG", Extensions = { "png" } });
        dlg.Filters.Add(new FileDialogFilter() { Name = "Todos los archivos", Extensions = { "*" } });
        dlg.AllowMultiple = false;

        var result = await dlg.ShowAsync(ventanaPadre);
        if (result != null)
        {
            rutaFotoAnadir = result[0];
            //Cargar la foto
            if (!Directory.Exists("..\\..\\..\\Imagenes"))
            {
                Directory.CreateDirectory("..\\..\\..\\Imagenes");
            }
            if (!rutaFotoAnadir.Equals("incognita.jpg"))
            {
                File.Copy(rutaFotoAnadir, $"..\\..\\..\\Imagenes/{proxIDFoto}.jpg");
                //Aumentamos el valor del ID de la proxima foto
                this._menuAnadir.MyImage.Source = new Bitmap($"..\\..\\..\\Imagenes/{proxIDFoto}.jpg");
                this.proxIDFoto++;
            }
            
        }
        else
        {
            rutaFoto = "";
        }
    }
    
    // Ir a ventana ver
    [RelayCommand]
    public void abrirVentanaVer()
    {
        // Abre la ventana de "Ver Campeones"
        MenuVer ventanaVer = new MenuVer();
        ventanaVer.Show();
        _mainWindow.Hide();
        this._menuVer = ventanaVer;
        ventanaVer.DataContext = this;
        actualizarCampos();
    }

    // Ir a ventana anadir
    [RelayCommand]
    public void abrirVentanaAnadir()
    {
        // Abre la ventana de "Añadir Campeón"
        AnadirCampeon ventanaAñadir = new AnadirCampeon();
        ventanaAñadir.Show();
        _mainWindow.Hide();
        this._menuAnadir = ventanaAñadir;
        ventanaAñadir.DataContext = this;
    }
    //Para volver de las ventanas extra
    [RelayCommand]
    public void volverVentanaVer()
    {
        this._menuVer.Close();
        this._menuVer = null;
        this._mainWindow.Show();
    }
    [RelayCommand]
    public void volverVentanaAnadir()
    {
        this._menuAnadir.Close();
        this._menuAnadir = null;
        this._mainWindow.Show();
    }
    
    // Eliminar el campeón actual con confirmación
    [RelayCommand]
    private async void eliminarCampeon()
    {
        if (Campeones.Count > 0)
        {
            // Mostrar el diálogo de confirmación y esperar su resultado
            var confirmacionDialog = new ConfirmacionDialog();
            await confirmacionDialog.ShowDialog(_menuVer); // relacionado con la ventana menuver

            // Si el usuario hace clic en 'Sí' (Resultado == true), eliminamos el campeón
            if (confirmacionDialog.Resultado)
            {
                Campeones.RemoveAt(CampeonActual);
                CampeonActual = Math.Max(0, CampeonActual - 1); // Ajustar si se elimina un campeón
                actualizarCampos();
            }
            // Si el usuario hace clic en 'No', no se hace nada
        }
       
    }


    // Navegar al campeón anterior
    [RelayCommand]
    private void campeonAnterior()
    {
        if (CampeonActual > 0)
        {
            CampeonActual--;
            actualizarCampos();
        }
    }

    // Navegar al siguiente campeón
    [RelayCommand]
    private void campeonSiguiente()
    {
        if (CampeonActual < Campeones.Count - 1)
        {
            CampeonActual++;
            actualizarCampos();
        }
    }
    //Ir al primero
    [RelayCommand]
    private void campeonPrimero()
    {
        CampeonActual = 0;
        actualizarCampos();
    }
  
    //Ir al ultimo campeon
    [RelayCommand]
    private void campeonUltimo()
    {
        CampeonActual = Campeones.Count - 1;
        actualizarCampos();
    }
    
    [RelayCommand]
    public void guardarCampeones()
    {
        if (Campeones.Count > 0)
        {
            var json = JsonSerializer.Serialize(Campeones);
            File.WriteAllText(fichero, json);
        }
        
    }

    public void CargarCampeones(string filePath)
    {
        if (File.Exists(filePath))
        {
            var json = File.ReadAllText(filePath);
            Campeones = JsonSerializer.Deserialize<List<Campeon>>(json);
        }
        else
        {
            Campeones = new List<Campeon>();
        }
    }

}

