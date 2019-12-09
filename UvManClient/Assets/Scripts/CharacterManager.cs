using System;
using System.Collections.Generic;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using LogicaDelNegocio.Modelo.Enum;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager ManejadorDePersonajes;
    private Dictionary<string, Sprite> Corredores = new Dictionary<string, Sprite>();
    private Dictionary<string, Sprite> Perseguidores = new Dictionary<string, Sprite>();
    private List<CorredorAdquiridoModel> CorredoresDisponibles = new List<CorredorAdquiridoModel>();
    private Dictionary<string, GameObject> PrefabsDeCorredores = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> PrefabsDePerseguidores = new Dictionary<string, GameObject>();

    public Sprite sCMegamam;
    public Sprite sCBomberman;
    public Sprite sCLink;
    public GameObject PrefabCMegaman;
    public GameObject PrefabCBomberman;
    public GameObject PrefabCLink;


    public Sprite sPCreeper;
    public GameObject PrefabPCreeper;
    
    private void Awake()
    {
        if ( ManejadorDePersonajes == null)
        {
            ManejadorDePersonajes = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ManejadorDePersonajes != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CrearDiccionarioDeCorredores();
        CrearDiccionarioDePerseguidores();
    }

    private void CrearDiccionarioDeCorredores()
    {
        Corredores.Add("Megaman", sCMegamam);
        Corredores.Add("Bomberman", sCBomberman);
        Corredores.Add("Link", sCLink);
        PrefabsDeCorredores.Add("Megaman", PrefabCMegaman);
        PrefabsDeCorredores.Add("Bomberman", PrefabCBomberman);
        PrefabsDeCorredores.Add("Link", PrefabCLink);
    }

    private void CrearDiccionarioDePerseguidores()
    {
        Perseguidores.Add("Creeper", sPCreeper);
        PrefabsDePerseguidores.Add("Creeper", PrefabPCreeper);
    }

    public Sprite ObtenerSpriteDePersonaje(JugadorModel Jugador)
    {
        return sPCreeper;
        if (Jugador.RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            String nombreDelPersonaje = Jugador.CorredorSeleccionado.Nombre;
            return Corredores[nombreDelPersonaje];
        }
        else
        {
            String nombreDelPersonaje = Jugador.PerseguidorSeleccionado.Nombre;
            return Perseguidores[nombreDelPersonaje];
        }
    }

    public GameObject ObtenerPrefabDePersonaje(JugadorModel Jugador)
    {
        return PrefabCMegaman;
        if (Jugador.RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            String nombreDelPersonaje = Jugador.CorredorSeleccionado.Nombre;
            return PrefabsDeCorredores[nombreDelPersonaje];
        }
        else
        {
            String nombreDelPersonaje = Jugador.PerseguidorSeleccionado.Nombre;
            return PrefabsDePerseguidores[nombreDelPersonaje];
        }
    }

}
