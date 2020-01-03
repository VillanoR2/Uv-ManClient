using Assets.Scripts.Util;
using GameService.Dominio;
using System;
using System.Collections.Generic;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de mostrar un canvas cuando el juego ha terminado o cuando el juego esta por iniciar
/// </summary>
public class ManejoDeCanvas : MonoBehaviour
{
    private const int SEGUNDOS_INICIO_PARTIDA = 5;
    private Canvas cInicioPartida;
    private bool EstaElJuegoPausado;
    private bool ElJuegoFinalizo;
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private Cronometro CronometroInicioDePartida;
    public Text txtCentro;
    public Text txtPuntuacionActual;
    public Text txtMensajePartida;
    private int SegundosCuentaRegresiva;
    private string TextoEnCanvas;
    public Transform MejoresPuntuaciones;
    public Transform PlantillaPuntuacionAlta;
    private List<DatosMejorPuntuacion> InstanciasDePuntuacionesAltas;
    private GameObject InstanciaMejoresPuntuaciones;
    public GameObject buttonRegresarMenuPrincipal;
    
    /// <summary>
    /// Metodo de UNITY que se ejecuta al cargar la escena 
    /// </summary>
    private void Awake()
    {
        InstanciaMejoresPuntuaciones = MejoresPuntuaciones.gameObject;
        InstanciaMejoresPuntuaciones.SetActive(false);
        InicializarPlantillaPuntuacionesAltas();
        buttonRegresarMenuPrincipal.SetActive(false);
    }

    /// <summary>
    /// Coloca en la tabla de mejores posiciones 5 filas 
    /// </summary>
    private void InicializarPlantillaPuntuacionesAltas()
    {
        InstanciasDePuntuacionesAltas = new List<DatosMejorPuntuacion>();
        float AlturaDeMejoresPuntuaciones = 35f;
        for (int i = 0; i < 5; i++)
        {
            Transform PuntuacionAlta = Instantiate(PlantillaPuntuacionAlta, MejoresPuntuaciones);
            RectTransform Posicion = PuntuacionAlta.GetComponent<RectTransform>();
            Posicion.anchoredPosition = new Vector2(0, -AlturaDeMejoresPuntuaciones * (i+1));
            Text txtPosicion = PuntuacionAlta.Find("txtPosicion").GetComponent<Text>();
            Text txtMarcador = PuntuacionAlta.Find("txtMarcador").GetComponent<Text>();
            Text txtNombreUsuario = PuntuacionAlta.Find("txtNombreUsuario").GetComponent<Text>();
            DatosMejorPuntuacion Puntuacion = new DatosMejorPuntuacion(PuntuacionAlta.gameObject , txtPosicion, txtMarcador, txtNombreUsuario);
            InstanciasDePuntuacionesAltas.Add(Puntuacion);
        }
    }

    /// <summary>
    /// Coloca en la tabla de puntuaciones alta la informacion de la puntuacion alta y la activa
    /// </summary>
    private void ColocarAltasPuntuacionesEnLaTabla()
    {
        CuentaModel[] MejoresPuntuaciones = ClienteDelJuego.ServicioDeJuego.RecuperarMejoresPuntuaciones();
        for (int i = 0; i < MejoresPuntuaciones.Length; i++)
        {
            if(MejoresPuntuaciones[i] != null){
                InstanciasDePuntuacionesAltas[i].TextoPosicion.text = Convert.ToString((i+1));
                InstanciasDePuntuacionesAltas[i].TextoPuntuacion.text = Convert.ToString(MejoresPuntuaciones[i].Jugador.MejorPuntacion);
                InstanciasDePuntuacionesAltas[i].TextoUsuario.text = MejoresPuntuaciones[i].NombreUsuario;
                InstanciasDePuntuacionesAltas[i].MejorPuntuacion.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Coloca en los Text del canvas la puntuacion del jugador en la partida
    /// </summary>
    /// <param name="Puntacion">String</param>
    /// <param name="NombreUsuario">String</param>
    private void ColocarTextoDeFinPartida(String Puntacion, String NombreUsuario)
    {
        txtPuntuacionActual.text = "Tu puntuacion fue de " + Puntacion;
        txtMensajePartida.text = "Te esperamos pronto " + NombreUsuario;
    }
    
    /// <summary>
    /// Coloca en el cambas la informacion de la partida y muestra la tabla de puntuaciones
    /// </summary>
    private void TerminarPartida()
    {
        if(ClienteDelJuego.CuentaEnSesion.Jugador.RolDelJugador == 0)
        {
            String PuntuacionDelJugador = Convert.ToString(ClienteDelJuego.Puntuacion);
            String NombreUsuario = ClienteDelJuego.CuentaEnSesion.NombreUsuario;
            ColocarTextoDeFinPartida(PuntuacionDelJugador, NombreUsuario);
            ColocarAltasPuntuacionesEnLaTabla();
        }
        TextoEnCanvas = "Juego terminad";
        ActivarPausa();
        ElJuegoFinalizo = true;   
    }
    
    /// <summary>
    ///  Coloca el canvas de cuenta regresiva y detiene la partida
    /// </summary>
    void InicializarCanvasInicioPartida()
    {
        ElJuegoFinalizo = false;
        ActivarPausa();
        txtPuntuacionActual.gameObject.SetActive(false);
        txtMensajePartida.gameObject.SetActive(false);
        txtCentro.gameObject.SetActive(true);
        cInicioPartida = GetComponent<Canvas>();
        TextoEnCanvas = "La partida comienza en ";
        RealizarConteo(SEGUNDOS_INICIO_PARTIDA);
    }

    /// <summary>
    /// Se suscribe a los eventos del cliente del juego
    /// </summary>
    private void SuscribirseAEventosDelJuego()
    {
        ClienteDelJuego.IniciaLaPartida += RealizarAccionDeInicioDeJuego;
        ClienteDelJuego.MostrarPantallaFinJuego += TerminarPartida;
    }
    
    /// <summary>
    /// Verifica el tipo de mensaje recibido, dependiendo del mensaje quita la pausa o inicia el conteo
    /// </summary>
    /// <param name="iniciaPartida">InicioPartida</param>
    private void RealizarAccionDeInicioDeJuego(InicioPartida iniciaPartida)
    {
        if (iniciaPartida.IniciarPartida)
        {
            if (EstaElJuegoPausado)
            {
                DesactivarPausa();
            }
        }else if (iniciaPartida.IniciarCuentaRegresivaInicioPartida)
        {
            RealizarConteo(SEGUNDOS_INICIO_PARTIDA);
        }
    }

    /// <summary>
    /// Desactiva la pausa del juego
    /// </summary>
    private void DesactivarPausa()
    {
        EstaElJuegoPausado = false;
    }

    /// <summary>
    /// Activa la pausa del juego
    /// </summary>
    private void ActivarPausa()
    {
        EstaElJuegoPausado = true;
    }

    /// <summary>
    /// Inicia una cuenta regresiva
    /// </summary>
    /// <param name="SegundosAContar">int</param>
    private void RealizarConteo(int SegundosAContar)
    {
        CronometroInicioDePartida?.Stop();
        SegundosCuentaRegresiva = SegundosAContar;
        CronometroInicioDePartida = new Cronometro(1000, (SegundosAContar * 1000));
        CronometroInicioDePartida.TranscurrioUnIntervalo += ActualizarSegundosContador;
        CronometroInicioDePartida.FinalizoElTimepo += DesactivarPausa;
        CronometroInicioDePartida.Start();
    }

    /// <summary>
    /// Metodo de UNITY que se ejecuta en el primer cuadro de la escena
    /// </summary>
    void Start()
    {
        SuscribirseAEventosDelJuego();
        InicializarCanvasInicioPartida();
    }

    /// <summary>
    /// Verifica si el juego se encuentra pausado y si es asi detiene el tiempo si no es asi lo establece normal
    /// </summary>
    private void VerificarSiEstaEnPausaElJuego()
    {
        if (EstaElJuegoPausado)
        {
            cInicioPartida.enabled = true;
            Time.timeScale = 0;
        }
        else
        {
            cInicioPartida.enabled = false;
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// Actualiza el texto que se muestra en el canvas
    /// </summary>
    private void ActualizarTexto()
    {
        txtCentro.text = TextoEnCanvas + SegundosCuentaRegresiva;
    }
    
    /// <summary>
    /// Metodo de UNITY que se ejecuta una vez por cuadro
    /// </summary>
    void Update()
    {
        ActualizarTexto();
        VerificarSiEstaEnPausaElJuego();
        VerificarSiElJuegoFinalizo();
    }

    /// <summary>
    /// Verifica si el juego ha finalizado para activar o desactivar los textos de puntuacion
    /// </summary>
    private void VerificarSiElJuegoFinalizo()
    {
        if (ElJuegoFinalizo)
        {
            if(ClienteDelJuego.CuentaEnSesion.Jugador.RolDelJugador == 0)
            {
                txtMensajePartida.gameObject.SetActive(true);
                txtPuntuacionActual.gameObject.SetActive(true);
                InstanciaMejoresPuntuaciones.SetActive(true);
            }
            buttonRegresarMenuPrincipal.SetActive(true);
        }
        else
        {
            txtMensajePartida.gameObject.SetActive(false);
            txtMensajePartida.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Actualiza la cuenta regresiva
    /// </summary>
    private void ActualizarSegundosContador()
    {
        SegundosCuentaRegresiva -= 1;
    }

    /// <summary>
    /// Cambia de escena a la del menu principal
    /// </summary>
    public void IrAMenuPrincipal()
    {
        DesuscribirseAEventosDelJuego();
        ClienteDelJuego.ReiniciarDatosJuego();
        SceneManager.LoadScene("MainScreen");
    }
    
    /// <summary>
    /// Se desuscribe a los eventos del cliente del juego
    /// </summary>
    private void DesuscribirseAEventosDelJuego(){
        ClienteDelJuego.IniciaLaPartida -= RealizarAccionDeInicioDeJuego;
        ClienteDelJuego.MostrarPantallaFinJuego -= TerminarPartida;
    }
}

/// <summary>
/// Almacena los componentes de una PlantillaPuntuacionAlta
/// </summary>
class DatosMejorPuntuacion
{
    public GameObject MejorPuntuacion;
    public Text TextoPosicion;
    public Text TextoPuntuacion;
    public Text TextoUsuario;

    public DatosMejorPuntuacion(GameObject mejorPuntuacion, Text textoPosicion, Text textoPuntuacion, Text textoUsuario)
    {
        MejorPuntuacion = mejorPuntuacion;
        TextoPosicion = textoPosicion;
        TextoPuntuacion = textoPuntuacion;
        TextoUsuario = textoUsuario;
    }
}