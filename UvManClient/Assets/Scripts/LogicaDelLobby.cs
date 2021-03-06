﻿using System;
using System.Collections.Generic;
using System.Timers;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de colocar la informacion de los personajes en el lobby y de cambiar de pantalla cuando la
/// partida comience
/// </summary>
public class LogicaDelLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private String IdDeLaSala;
    private Boolean EsSalaPublica;
    private List<Image> ListaDeImagenesDeJugadores = new List<Image>();
    private List<Text> ListaDetextoDeJugadores = new List<Text>();
    
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

    /// <summary>
    /// Inicicializa la lista de imagenes de los jugadores en la sala
    /// </summary>
    private void InicializarListaDeImaganes()
    {
        ListaDeImagenesDeJugadores.Add(imageJugador1);
        ListaDeImagenesDeJugadores.Add(imageJugador2);
        ListaDeImagenesDeJugadores.Add(imageJugador3);
        ListaDeImagenesDeJugadores.Add(imageJugador4);
        ListaDeImagenesDeJugadores.Add(imageJugador5);
    }
    
    /// <summary>
    /// Inicializa la list de los textos de los jugadores
    /// </summary>
    private void InicializarListaDeTextos()
    {
        ListaDetextoDeJugadores.Add(txtJugador1);
        ListaDetextoDeJugadores.Add(txtJugador2);
        ListaDetextoDeJugadores.Add(txtJugador3);
        ListaDetextoDeJugadores.Add(txtJugador4);
        ListaDetextoDeJugadores.Add(txtJugador5);
    }
    
    /// <summary>
    /// Recupera la información de la sala
    /// </summary>
    private void RecuperarInformacionDeSala()
    {
        IdDeLaSala = ClienteDelJuego.IdDeMiSala;
        EsSalaPublica = ClienteDelJuego.MiSalaEsPublica;
    }
    
    /// <summary>
    /// Coloca la información de la sala en los textbox
    /// </summary>
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
    
    /// <summary>
    /// Esconde el texto y la imagen de un jugador
    /// </summary>
    /// <param name="ImagenADesactivar">Image</param>
    /// <param name="TextoADesactivar">Text</param>
    private void EsconderJugador(Image ImagenADesactivar, Text TextoADesactivar)
    {
        ImagenADesactivar.gameObject.SetActive(false);
        TextoADesactivar.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Esconde la información de todos los jugadores en la sala
    /// </summary>
    private void EsconderJugadoresEnSesion()
    {
        EsconderJugador(imageJugador1, txtJugador1);
        EsconderJugador(imageJugador2, txtJugador2);
        EsconderJugador(imageJugador3, txtJugador3);
        EsconderJugador(imageJugador4, txtJugador4);
        EsconderJugador(imageJugador5, txtJugador5);
    }
    
    /// <summary>
    /// Metodo de UNITY que se ejecuta en el primer cuadro de la escena
    /// </summary>
    void Start()
    {
        EsconderJugadoresEnSesion();
        RecuperarInformacionDeSala();
        InicializarTextosDeInformacionSala();
        InicializarListaDeImaganes();
        InicializarListaDeTextos();
        ActualizarInformacionDeLosJugeadoresEnSala();
        ActualizarTextoCantidadDeJugadores();
        PreguntarSalaLlena();
        SuscribirseAEventosClienteJuego();
    }

    /// <summary>
    /// Se suscribe a los eventos del cliente de juego
    /// </summary>
    private void SuscribirseAEventosClienteJuego()
    {
        ClienteDelJuego.SeActualizaronRoles += ActualizarInformacionDeLosJugeadoresEnSala;
        ClienteDelJuego.SeActualizaronRoles += ActualizarTextoCantidadDeJugadores;
        ClienteDelJuego.SeLlenoLaSala += SeLlenoLaSala;
    }

    /// <summary>
    /// Le pregunta al servicio del juego si la sala ya se encuentra llena
    /// </summary>
    private void PreguntarSalaLlena()
    {
        ClienteDelJuego.EstaLlenaLaSala();
    }

    private void ActualizarInformacionDelJugador(Image ImagenDelJugador, Text TextDelJudador, CuentaModel Cuenta)
    {
        if (ImagenDelJugador.gameObject != null && TextDelJudador.gameObject != null)
        {
            ImagenDelJugador.gameObject.SetActive(true);
            if (TextDelJudador != null)
            {
                ImagenDelJugador.sprite = CharacterManager.ManejadorDePersonajes.ObtenerSpriteDePersonaje(Cuenta.Jugador);
            }

            TextDelJudador.gameObject.SetActive(true);
            TextDelJudador.text = Cuenta.NombreUsuario;   
        }
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
        System.Timers.Timer temporizador = new System.Timers.Timer(5000);
        temporizador.Elapsed += CambiarAMultijugador;
        temporizador.AutoReset = false;
        temporizador.Enabled = true;
        temporizador.Start();
    }

    /// <summary>
    /// Cambia a la escena de multiplaerScreen
    /// </summary>
    /// <param name="source">object</param>
    /// <param name="e">ElapsedEventArgs</param>
    private void CambiarAMultijugador(object source, ElapsedEventArgs e)
    {
        CambiarAPantallaMultijugador();
    }

    public void SoloPruebas()
    {
        CambiarAPantallaMultijugador();
    }

    /// <summary>
    /// Cambia a la escena MultiplayerScreen
    /// </summary>
    private void CambiarAPantallaMultijugador()
    {
        DesuscribirseALosServiciosDelJuego();
        SceneManager.LoadScene("MultiplayerScreen");
    }

    /// <summary>
    /// Se desuscribe a los servivios del cliente del juego
    /// </summary>
    private void DesuscribirseALosServiciosDelJuego()
    {
        ClienteDelJuego.SeActualizaronRoles -= ActualizarInformacionDeLosJugeadoresEnSala;
        ClienteDelJuego.SeActualizaronRoles -= ActualizarTextoCantidadDeJugadores;
        ClienteDelJuego.SeLlenoLaSala -= SeLlenoLaSala;
    }

    /// <summary>
    /// Saca a la cuenta de la sala y cambia a la escena MainScreen
    /// </summary>
    public void Regresar()
    {
        ClienteDelJuego.SalirDeLaSala();
        DesuscribirseALosServiciosDelJuego();
        SceneManager.LoadScene("MainScreen");
    }
}
