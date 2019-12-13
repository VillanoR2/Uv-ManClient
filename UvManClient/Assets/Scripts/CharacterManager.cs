using System;
using System.Collections.Generic;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using LogicaDelNegocio.Modelo.Enum;
using UnityEngine;

/// <summary>
/// Se encarga de proporcionar aprites y prefabs de personajes
/// </summary>
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager ManejadorDePersonajes;
    private Dictionary<string, Sprite> SpritesDePersonajes = new Dictionary<string, Sprite>();
    private Dictionary<string, GameObject> PrefabsDePersonajes = new Dictionary<string, GameObject>();
    
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
        CrearDiccionarioDeSprites();
    }

    /// <summary>
    /// Crea un diccionario con los sprites de los personajes
    /// </summary>
    private void CrearDiccionarioDeSprites()
    {
        SpritesDePersonajes.Add("Megaman", sCMegamam);
        SpritesDePersonajes.Add("Bomberman", sCBomberman);
        SpritesDePersonajes.Add("Link", sCLink);
        PrefabsDePersonajes.Add("Megaman", PrefabCMegaman);
        PrefabsDePersonajes.Add("Bomberman", PrefabCBomberman);
        PrefabsDePersonajes.Add("Link", PrefabCLink);
        SpritesDePersonajes.Add("Creeper", sPCreeper);
        PrefabsDePersonajes.Add("Creeper", PrefabPCreeper);
    }
    
    /// <summary>
    /// Regresa el sprite que le corresponde al jugador dependiendo de su rol
    /// </summary>
    /// <param name="Jugador">JugadorModel</param>
    /// <returns>El sprite que le pertenece al jugador</returns>
    public Sprite ObtenerSpriteDePersonaje(JugadorModel Jugador)
    {
        Sprite SpriteDelJugador = SpritesDePersonajes["Megaman"];
        switch (Jugador.RolDelJugador)
        {
            case EnumTipoDeJugador.Corredor:
                SpriteDelJugador = SpritesDePersonajes["Bomberman"];
                break;
            case EnumTipoDeJugador.Perseguidor1:
                SpriteDelJugador = SpritesDePersonajes["Megaman"];
                break;
            case EnumTipoDeJugador.Perseguidor2:
                SpriteDelJugador = SpritesDePersonajes["Link"];
                break;
            case EnumTipoDeJugador.Perseguidor3:
                SpriteDelJugador = SpritesDePersonajes["Creeper"];
                break;
            case EnumTipoDeJugador.Perseguidor4:
                SpriteDelJugador = SpritesDePersonajes["Megaman"];
                break;
        }

        return SpriteDelJugador;
    }

    /// <summary>
    /// Regresa el Prefab que le corresponde al jugador dependiendo de su rol
    /// </summary>
    /// <param name="Jugador">JugadorModel</param>
    /// <returns>Un GameObject que es un prefab del personaje que le pertenece al jugador</returns>
    public GameObject ObtenerPrefabDePersonaje(JugadorModel Jugador)
    {
        GameObject PrefabDelJugador = PrefabsDePersonajes["Megaman"];
        switch (Jugador.RolDelJugador)
        {
            case EnumTipoDeJugador.Corredor:
                PrefabDelJugador = PrefabsDePersonajes["Bomberman"];
                break;
            case EnumTipoDeJugador.Perseguidor1:
                PrefabDelJugador = PrefabsDePersonajes["Megaman"];
                break;
            case EnumTipoDeJugador.Perseguidor2:
                PrefabDelJugador = PrefabsDePersonajes["Link"];
                break;
            case EnumTipoDeJugador.Perseguidor3:
                PrefabDelJugador = PrefabsDePersonajes["Link"];
                break;
            case EnumTipoDeJugador.Perseguidor4:
                PrefabDelJugador = PrefabsDePersonajes["Megaman"];
                break;
        }

        return PrefabDelJugador;
    }

}
