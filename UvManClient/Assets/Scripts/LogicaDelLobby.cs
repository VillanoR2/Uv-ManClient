using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using GameService.Dominio;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicaDelLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private String IdDeLaSala;
    private Boolean EsSalaPublica;
    private List<Image> ListaDeImagenesDeJugadores = new List<Image>();
    private List<Text> ListaDetextoDeJugadores = new List<Text>();

    public Sprite MiImagen;
    
    public Text txtIdSala;
    public Text txtTipoSala;
    public Text txtCantidadJugadores;
    public Image imageJugador1;
    public Text txtJugador1;
    public Image imageJugador2;
    public Text txtJugador2;
    public Image imageJugador3;
    public Text txtJugador3;
    public Image imageJugador4;
    public Text txtJugador4;
    public Image imageJugador5;
    public Text txtJugador5;

    private void InicializarListaDeImaganes()
    {
        ListaDeImagenesDeJugadores.Add(imageJugador1);
        ListaDeImagenesDeJugadores.Add(imageJugador2);
        ListaDeImagenesDeJugadores.Add(imageJugador3);
        ListaDeImagenesDeJugadores.Add(imageJugador4);
        ListaDeImagenesDeJugadores.Add(imageJugador5);
    }

    private void InicializarListaDeTextos()
    {
        ListaDetextoDeJugadores.Add(txtJugador1);
        ListaDetextoDeJugadores.Add(txtJugador2);
        ListaDetextoDeJugadores.Add(txtJugador3);
        ListaDetextoDeJugadores.Add(txtJugador4);
        ListaDetextoDeJugadores.Add(txtJugador5);
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
        txtTipoSala.text = TipoDeSala;
        txtIdSala.text = IdDeLaSala;
    }
    private void EsconderJugador(Image ImagenADesactivar, Text TextoADesactivar)
    {
        ImagenADesactivar.gameObject.SetActive(false);
        TextoADesactivar.gameObject.SetActive(false);
    }
    
    private void EsconderJugadoresEnSesion()
    {
        EsconderJugador(imageJugador1, txtJugador1);
        EsconderJugador(imageJugador2, txtJugador2);
        EsconderJugador(imageJugador3, txtJugador3);
        EsconderJugador(imageJugador4, txtJugador4);
        EsconderJugador(imageJugador5, txtJugador5);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SuscribirseAEventosClienteJuego();
        EsconderJugadoresEnSesion();
        RecuperarInformacionDeSala();
        InicializarTextosDeInformacionSala();
        InicializarListaDeImaganes();
        InicializarListaDeTextos();
        ActualizarInformacionDeLosJugeadoresEnSala();
        ActualizarTextoCantidadDeJugadores();
        PreguntarSalaLlena();
    }

    private void SuscribirseAEventosClienteJuego()
    {
        ClienteDelJuego.SeActualizaronRoles += ActualizarInformacionDeLosJugeadoresEnSala;
        ClienteDelJuego.SeActualizaronRoles += ActualizarTextoCantidadDeJugadores;
        ClienteDelJuego.SeLlenoLaSala += SeLlenoLaSala;
        //ClienteDelJuego.IniciaLaPartida += IniciaLaPartida;
    }

    private void PreguntarSalaLlena()
    {
        ClienteDelJuego.EstaLlenaLaSala();
    }

    private void ActualizarInformacionDelJugador(Image ImagenDelJugador, Text TextDelJudador, CuentaModel Cuenta)
    {
        ImagenDelJugador.gameObject.SetActive(true);
        if (TextDelJudador != null)
        {
            ImagenDelJugador.sprite = CharacterManager.ManejadorDePersonajes.ObtenerSpriteDePersonaje(Cuenta.Jugador);
        }

        TextDelJudador.gameObject.SetActive(true);
        TextDelJudador.text = Cuenta.NombreUsuario;
    }

    private void ActualizarInformacionDeLosJugeadoresEnSala()
    {
        for (int i = 0; i < ClienteDelJuego.CuentasEnLaSala.Count; i++)
        {
            ActualizarInformacionDelJugador(ListaDeImagenesDeJugadores[i], ListaDetextoDeJugadores[i],
                ClienteDelJuego.CuentasEnLaSala[i]);
        }
    }

    private void ActualizarTextoCantidadDeJugadores()
    {
        String textoAMostrar = String.Format("Esperando jugadores ({0}/5)...", ClienteDelJuego.CuentasEnLaSala.Count);
        txtCantidadJugadores.text = textoAMostrar;
    }

    private void SeLlenoLaSala()
    {
        IniciarTemporizador();
        CambiarTextoAEsperandoAIniciar();
    }

    private void CambiarTextoAEsperandoAIniciar()
    {
        txtCantidadJugadores.text = "La partida empezara en breve";
    }
    
    private void IniciarTemporizador()
    {
        Timer temporizador = new Timer(5000);
        temporizador.Elapsed += CambiarAMultijugador;
        temporizador.AutoReset = false;
        temporizador.Enabled = true;
        temporizador.Start();
    }
    
    private void CambiarAMultijugador(object source, ElapsedEventArgs e)
    {
        CambiarAPantallaMultijugador();
    }

    //private void IniciaLaPartida(InicioPartida datosDeInicioDePartida)
    //{
    //    if (datosDeInicioDePartida.CambiarPantallaMultijugador)
    //    {
    //        CambiarAPantallaMultijugador();
    //    }
    //}

    public void SoloPruebas()
    {
        CambiarAPantallaMultijugador();
    }

    private void CambiarAPantallaMultijugador()
    {
        SceneManager.LoadScene("MultiplayerScreen");
    }


}
