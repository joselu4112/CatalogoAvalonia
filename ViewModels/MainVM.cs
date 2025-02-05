using Avalonia.Controls;
using ReactiveUI;
using System.Reactive;
using System.Collections.ObjectModel;
using CatalogoVista.Model;
using System.Text.Json;
using System.IO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CatalogoAvalonia.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CatalogoAvalonia.Views;
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
    
    
    //Constructor
    public MainVM(MenuPrincipal ventanaPrincipal)
    {
        _mainWindow = ventanaPrincipal;
        CargarCampeones(fichero);
        CampeonActual = 0;
        CampeonMostrar = CampeonActual + 1;

        Nombre = Campeones[CampeonActual].Nombre;
        Vida = Campeones[CampeonActual].Vida;
        Mana = Campeones[CampeonActual].Mana;
        NombrePasiva = Campeones[CampeonActual].Pasiva.Nombre;
        DescripcionPasiva = Campeones[CampeonActual].Pasiva.Descripcion;
        EnfriamientoPasiva = Campeones[CampeonActual].Pasiva.Enfriamiento;
        TipoDanio = Campeones[CampeonActual].TipoDanio;
    }

    // Agregar un nuevo campeón
    [RelayCommand]
    public void anadirCampeon()
    {

    }

    // Ir a ventana ver
    [RelayCommand]
    public void abrirVentanaVer()
    {
        // Abre la ventana de "Ver Campeones"
        MenuVer menuVer = new MenuVer();
        menuVer.Show();
        _mainWindow.Hide();
        this._menuVer = menuVer;
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
    private void eliminarCampeon()
    {
        Campeones.RemoveAt(CampeonActual);
        CampeonActual = Math.Max(0, CampeonActual - 1); // Ajustar si se elimina un campeón
    }

    // Navegar al campeón anterior
    [RelayCommand]
    private void campeonAnterior()
    {
        if (CampeonActual > 0)
        {
            CampeonActual--;
        }
    }

    // Navegar al siguiente campeón
    [RelayCommand]
    private void campeonSiguiente()
    {
        if (CampeonActual < Campeones.Count - 1)
        {
            CampeonActual++;
        }
    }
    //Ir al primero
    [RelayCommand]
    private void campeonPrimero()
    {
        CampeonActual = 0;
    }
  
    //Ir al ultimo campeon
    [RelayCommand]
    private void campeonUltimo()
    {
        CampeonActual = Campeones.Count - 1;
    }
    
    [RelayCommand]
    public void guardarCampeones(string filePath)
    {
        var json = JsonSerializer.Serialize(Campeones);
        File.WriteAllText(filePath, json);
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

