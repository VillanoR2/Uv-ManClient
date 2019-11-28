using System;
using System.Collections;
using System.Collections.Generic;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicaDelLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private String IdDeLaSala;
    private Boolean EsSalaPublica;
    private List<RawImage> ListaDeImagenesDeJugadores = new List<RawImage>();
    private List<Text> ListaDetextoDeJugadores = new List<Text>();

    public Sprite MiImagen;
    
    public Text TextValorIdDeLaSala;
    public Text TexValorTipoDeSala;
    public RawImage ImageJugador1;
    public Text TextJugador1;
    public RawImage ImageJugador2;
    public Text TextJugador2;
    public RawImage ImageJugador3;
    public Text TextJugador3;
    public RawImage ImageJugador4;
    public Text TextJugador4;
    public RawImage ImageJugador5;
    public Text TextJugador5;

    private void InicializarListaDeImaganes()
    {
        ListaDeImagenesDeJugadores.Add(ImageJugador1);
        ListaDeImagenesDeJugadores.Add(ImageJugador2);
        ListaDeImagenesDeJugadores.Add(ImageJugador3);
        ListaDeImagenesDeJugadores.Add(ImageJugador4);
        ListaDeImagenesDeJugadores.Add(ImageJugador5);
    }

    private void InicializarListaDeTextos()
    {
        ListaDetextoDeJugadores.Add(TextJugador1);
        ListaDetextoDeJugadores.Add(TextJugador2);
        ListaDetextoDeJugadores.Add(TextJugador3);
        ListaDetextoDeJugadores.Add(TextJugador4);
        ListaDetextoDeJugadores.Add(TextJugador5);
    }
    
    private void RecuperarInformacionDeSala()
    {
        IdDeLaSala = ClienteDelJuego.IdDeMiSala;
        EsSalaPublica = ClienteDelJuego.MiSalaEsPublica;
    }
    
    private void InicializarTextosDeInformacionSala()
    {
        String TipoDeSala;
        if (EsSalaPublica)
        {
            TipoDeSala = "Publica";
        }else
        {
            TipoDeSala = "Privada";
        }
        TexValorTipoDeSala.text = TipoDeSala;
        TextValorIdDeLaSala.text = IdDeLaSala;
    }
    private void EsconderJugador(RawImage ImagenADesactivar, Text TextoADesactivar)
    {
        ImagenADesactivar.gameObject.SetActive(false);
        TextoADesactivar.gameObject.SetActive(false);
    }
    
    private void EsconderJugadoresEnSesion()
    {
        EsconderJugador(ImageJugador1, TextJugador1);
        EsconderJugador(ImageJugador2, TextJugador2);
        EsconderJugador(ImageJugador3, TextJugador3);
        EsconderJugador(ImageJugador4, TextJugador4);
        EsconderJugador(ImageJugador5, TextJugador5);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        EsconderJugadoresEnSesion();
        RecuperarInformacionDeSala();
        InicializarTextosDeInformacionSala();
        InicializarListaDeImaganes();
        InicializarListaDeTextos();
        ActualizarInformacionDeLosJugeadoresEnSala();
        ClienteDelJuego.SeActualizaronRoles += ActualizarInformacionDeLosJugeadoresEnSala;
    }

    private void ActualizarInformacionDelJugador(RawImage ImagenDelJugador, Text TextDelJudador, CuentaModel Cuenta)
    {
        ImagenDelJugador.gameObject.SetActive(true);
        Texture TexturaDelJugador = RecuperarTexturaDelJugador(null);
        if (TextDelJudador != null)
        {
            ImagenDelJugador.texture = MiImagen.texture;
        }
        TextDelJudador.gameObject.SetActive(true);
        TextDelJudador.text = Cuenta.NombreUsuario;
    }

    private Texture BuscarTextura(String nombre)
    {
        return null;
    }
    
    private Texture RecuperarTexturaDelJugador(JugadorModel Jugador)
    {
        Texture textura = BuscarTextura("blue-button00");
        return textura;
    }
    
    private void ActualizarInformacionDeLosJugeadoresEnSala()
    {
        for (int i = 0; i < ClienteDelJuego.CuentasEnSesion.Count; i++)
        {
            ActualizarInformacionDelJugador(ListaDeImagenesDeJugadores[i], ListaDetextoDeJugadores[i],
                ClienteDelJuego.CuentasEnSesion[i]);
        }
    }
}
