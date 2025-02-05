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

    public int CampeonMostrar;
    //Fichero
    private String fichero = "campeones0.dat";

    // Ventanas para controlar cuales vamos mostrando
    private readonly MenuPrincipal _mainWindow;
    private MenuVer _menuVer;
    private AnadirCampeon _menuAnadir;
    
    
    //Atributos de campeon para mostrar
    [ObservableProperty] public string _nombre;
    [ObservableProperty] public int _vida;
    [ObservableProperty] public int _mana;
    [ObservableProperty] public string _nombrePasiva;
    [ObservableProperty] public string _descripcionPasiva;
    [ObservableProperty] public double _enfriamientoPasiva;
    [ObservableProperty] public TipoDeDanio _tipoDanio;
    [ObservableProperty] public String rutaFoto;
    
    //Atributos de campeon para añadir
    [ObservableProperty] public string _nombreAnadir;
    [ObservableProperty] public int _vidaAnadir;
    [ObservableProperty] public int _manaAnadir;
    [ObservableProperty] public string _nombrePasivaAnadir;
    [ObservableProperty] public string _descripcionPasivaAnadir;
    [ObservableProperty] public double _enfriamientoPasivaAnadir;
    [ObservableProperty] public TipoDeDanio _tipoDanioAnadir;
    [ObservableProperty] public String rutaFotoAnadir="incognita.jpg";
    
    //Constructor
    public MainVM(MenuPrincipal ventanaPrincipal)
    {
        _mainWindow = ventanaPrincipal;
        CargarCampeones(fichero);
        CampeonActual = 0;
        CampeonMostrar = CampeonActual + 1;
        
    }
//Metodo para actualizar la pantalla Ver
    public void actualizarCampos()
    {
        //Si tenemos algun campo actualiza
        if (Campeones.Count > 0)
        {
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
    [RelayCommand]
    public void anadirCampeon()
    {
        // Verificar que los campos no estén vacíos antes de agregar
        if (string.IsNullOrEmpty(NombreAnadir) || string.IsNullOrEmpty(NombrePasivaAnadir) || VidaAnadir <= 0 || ManaAnadir <= 0
            || string.IsNullOrEmpty(DescripcionPasivaAnadir) || EnfriamientoPasivaAnadir<= 0)
        {
            // Aquí puedes agregar alguna validación o mostrar un mensaje de error
            return;
        }
        
        //Foto para añadir al campeon
        if (string.IsNullOrEmpty(RutaFotoAnadir))
        {
            RutaFotoAnadir = "incognita.jpg";
        }
        
        // Crear un nuevo campeón con los valores de las propiedades
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

        // Limpiar los campos después de agregar
        Nombre = string.Empty;
        NombrePasiva = string.Empty;
        Vida = 0;
        Mana = 0;
        DescripcionPasiva = string.Empty;
        EnfriamientoPasiva = 0;
        TipoDanio = TipoDeDanio.FISICO;
        RutaFoto = string.Empty;  // Limpiar la ruta de la imagen si es necesario
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
            rutaFoto = result[0];
        }
        else
        {
            rutaFoto = "Imagenes/imagenlol.jpg";
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
        // Mostrar el diálogo de confirmación y esperar su resultado
        var confirmacionDialog = new ConfirmacionDialog();
        await confirmacionDialog.ShowDialog(_menuVer); // relacionado con la ventana menuver

        // Si el usuario hace clic en 'Sí' (Resultado == true), eliminamos el campeón
        if (confirmacionDialog.Resultado)
        {
            Campeones.RemoveAt(CampeonActual);
            CampeonActual = Math.Max(0, CampeonActual - 1); // Ajustar si se elimina un campeón
        }
        // Si el usuario hace clic en 'No', no se hace nada
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
    public void guardarCampeones(string filePath)
    {
        if (Campeones.Count > 0)
        {
            var json = JsonSerializer.Serialize(Campeones);
            File.WriteAllText(filePath, json);
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

