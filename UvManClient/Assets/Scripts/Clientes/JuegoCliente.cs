using System;
using System.Collections.Generic;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using System.Threading;
using GameService.Dominio;
using System.ServiceModel;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo.Enum;
using ConexionRed.Udp;

/// <summary>
/// Se encarga de comunicarse con el servicio del juego y almacena informacion del juego
/// </summary>
public class JuegoCliente : MonoBehaviour, IGameServiceCallback
{
    private const int PUERTO_ENVIO_UDP1 = 8090;
    private const int PUERTO_ENVIO_UDP2 = 8091;
    private const int PUERTO_ESCUCHA_UDP = 8296;
    private const int PUERTO_ESCUCHA_UDP2 = 8297;
    public int Puntuacion = 0;
    private UdpReciverClient RecibidorDePaquetesUDP;
    private Thread HiloDeEscuchaPaquetesUDP;

    public delegate void DatosDelMovimientoDeJugador(MovimientoJugador movimientoJugador);
    public delegate void DatosDeInicioDeLaPartida(InicioPartida inicioPartida);
    public delegate void DatosDeLaMuerteDeJugador(MuerteJugador muerteJugador);
    public delegate void NotificacionSobrePartida();
    public event DatosDelMovimientoDeJugador SeMovioUnJugador;
    public event DatosDeInicioDeLaPartida IniciaLaPartida;
    public event NotificacionSobrePartida SeActivoElTiempoParaComer;
    public event DatosDeLaMuerteDeJugador SeMurioUnJugador;
    public event NotificacionSobrePartida TerminoLaPartida;
    public event NotificacionSobrePartida MostrarPantallaFinJuego;
    public static JuegoCliente ClienteDelJuego;
    private string DireccionIpDelServidor;
    public GameServiceClient ServicioDeJuego;
    public CuentaModel CuentaEnSesion;
    public readonly List<CuentaModel> CuentasEnLaSala = new List<CuentaModel>();
    public String IdDeMiSala;
    public Boolean MiSalaEsPublica;
    public delegate void NotificacionEvento();
    public event NotificacionEvento SeActualizaronRoles;
    public event NotificacionEvento SeLlenoLaSala;

    /// <summary>
    /// Solicita al servidor los detalles de la sala en donde se encuentra la cuenta en sesion
    /// </summary>
    public void SolcitarDetallesSala()
    {
        MiSalaEsPublica = ServicioDeJuego.MiSalaEsPublica(CuentaEnSesion);
        IdDeMiSala = ServicioDeJuego.RecuperarIdDeMiSala(CuentaEnSesion);
        RefrescarCuentasEnSala(ServicioDeJuego.ObtenerCuentasEnMiSala(CuentaEnSesion));
        PonerCuentaEnSesionCompleta();
    }

    /// <summary>
    /// Recupera la direccion ip del servidor 
    /// </summary>
    private void RecuperarIpDelServidor()
    {
        DireccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    /// <summary>
    /// Recupera la cuenta que se logeo en el cliente actual
    /// </summary>
    private void RecuperarCuentaEnSession()
    {
        CuentaEnSesion = Cuenta.CuentaLogeada.CuentaM;
    }

    /// <summary>
    /// Invoca el evento de EstaLaSalaLlena
    /// </summary>
    public void EstaLlenaLaSala()
    {
        ServicioDeJuego.EstaLaSalaLlena(CuentaEnSesion);
    }

    private void Awake()
    {
        if (ClienteDelJuego == null)
        {
            ClienteDelJuego = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (ClienteDelJuego != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RecuperarCuentaEnSession();
        RecuperarIpDelServidor();
        InicializarServicioDeJuego();
        CrearHiloDeEscuchaUDP();
    }

    /// <summary>
    /// Crea un hilo para la escucha de paquetes UDP en uno de los dos puertos disponibles
    /// </summary>
    private void CrearHiloDeEscuchaUDP()
    {
        try
        {
            RecibidorDePaquetesUDP = new UdpReciverClient(PUERTO_ESCUCHA_UDP, PUERTO_ESCUCHA_UDP2);
            RecibidorDePaquetesUDP.EventoRecibido += RecibirEventoEnElJuego;
            HiloDeEscuchaPaquetesUDP = new Thread(RecibidorDePaquetesUDP.RecibirDatos);
            HiloDeEscuchaPaquetesUDP.Start();
        }
        catch (Exception ex)
        {
            Debug.Log("Error en CrearHiloDeEscucha" + ex);
        }         
    }
    
    /// <summary>
    /// Crea la conexion con el servicio del juego
    /// </summary>
    private void InicializarServicioDeJuego()
    {
        ServicioDeJuego = new GameServiceClient(new InstanceContext(this), 
            new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://" + DireccionIpDelServidor + ":8292/GameService"));
    }

    /// <summary>
    /// Invoca al evento de SeLlenoLaSala
    /// </summary>
    public void SalaLlena()
    {
        SeLlenoLaSala?.Invoke();
    }

    /// <summary>
    /// Agrega a un nuevo usuario en la lista de cuentas en la sala e invoca al evento SeActualiazonRoles
    /// </summary>
    /// <param name="cuenta"></param>
    public void NuevoCuentaEnLaSala(CuentaModel cuenta)
    {
        CuentasEnLaSala.Add(cuenta);
        SeActualizaronRoles?.Invoke();
    }

    /// <summary>
    /// Remueve una cuenta de la lista de cuentas en la sala
    /// </summary>
    /// <param name="cuenta"></param>
    public void CuentaAbandoSala(CuentaModel cuenta)
    {
        CuentasEnLaSala.Remove(cuenta);
    }

    /// <summary>
    /// Actualiza las cuentas que se encuentran en la sala
    /// </summary>
    /// <param name="CuentasEnMiSala"></param>
    public void RefrescarCuentasEnSala(CuentaModel[] CuentasEnMiSala)
    {
        CuentasEnLaSala.Clear();
        foreach (CuentaModel Cuenta in CuentasEnMiSala)
        {
            CuentasEnLaSala.Add(Cuenta);
        }
        SeActualizaronRoles?.Invoke();
    }

    /// <summary>
    /// Reinicia el cliente que consume los servicios del juego
    /// </summary>
    public void ReinciarClienteDeJuego()
    {
        RecuperarIpDelServidor();
        InicializarServicioDeJuego();
        RecuperarCuentaEnSession();
    }
    
    /// <summary>
    /// Lanza notificaciones dependiendo del tipo de eventoEnJuego
    /// </summary>
    /// <param name="eventoEnJuego">EventoEnJuego</param>
    private void RecibirEventoEnElJuego(EventoEnJuego eventoEnJuego)
    {
        if (eventoEnJuego.IdSala == IdDeMiSala)
        {
            switch (eventoEnJuego.TipoDeEvento)
            {
                case EnumTipoDeEventoEnJuego.IniciarCuentaRegresivaInicioJuego:
                    IniciaLaPartida?.Invoke(eventoEnJuego.DatosInicioDePartida);
                    break;
                case EnumTipoDeEventoEnJuego.IniciarPartida:
                    IniciaLaPartida?.Invoke(eventoEnJuego.DatosInicioDePartida);
                    break;
                case EnumTipoDeEventoEnJuego.MuerteJugador:
                    if (eventoEnJuego.CuentaRemitente != CuentaEnSesion.NombreUsuario)
                    {
                        SeMurioUnJugador?.Invoke(eventoEnJuego.DatosMuerteDeUnJugador);
                    }
                    break;
                case EnumTipoDeEventoEnJuego.MovimientoJugador:
                    if (eventoEnJuego.CuentaRemitente != CuentaEnSesion.NombreUsuario)
                    {
                        SeMovioUnJugador?.Invoke(eventoEnJuego.DatosDelMovimiento);
                    }
                    break;
               case EnumTipoDeEventoEnJuego.IniciaTiempoMatar:
                    if(eventoEnJuego.CuentaRemitente != CuentaEnSesion.NombreUsuario)
                    {
                        SeActivoElTiempoParaComer?.Invoke();
                    }
                    break;
            }
        }
    }
    
    /// <summary>
    /// Envia a las demas cuentas el movimiento realizado por el jugador
    /// </summary>
    /// <param name="x">float</param>
    /// <param name="y">float</param>
    /// <param name="movimientoX">float</param>
    /// <param name="movimientoY">float</param>
    public void EnviarMovimiento(float x, float y, float movimientoX, float movimientoY)
    {
        EventoEnJuego eventoEnJuego = new EventoEnJuego();
        eventoEnJuego.EventoEnJuegoMovimientoJugador(CuentaEnSesion.NombreUsuario, IdDeMiSala ,CuentaEnSesion.NombreUsuario,
            x, y, movimientoX, movimientoY);
        UdpSender enviadorDePaquetesUDP = new UdpSender(DireccionIpDelServidor, PUERTO_ENVIO_UDP1, PUERTO_ENVIO_UDP2);
        enviadorDePaquetesUDP.EnviarPaquete(eventoEnJuego);
    }

    /// <summary>
    /// Coloca en la cuenta en sesion la cuenta completa
    /// </summary>
    private void PonerCuentaEnSesionCompleta()
    {
        CuentaModel MiCuentaCompleta = null;
        foreach(CuentaModel Cuenta in CuentasEnLaSala)
        {
            if(Cuenta.NombreUsuario == CuentaEnSesion.NombreUsuario)
            {
                MiCuentaCompleta = Cuenta;
            }
        }
        if(MiCuentaCompleta != null)
        {
            Cuenta.CuentaLogeada.CuentaM = MiCuentaCompleta;
            CuentaEnSesion = MiCuentaCompleta;
        }
    }

    /// <summary>
    /// Envia un mensaje de partida terminada con la mejor puntuacion del corredor
    /// </summary>
    /// <param name="PersonajeDelCorredor"></param>
    public void TerminarPartida(Jugador PersonajeDelCorredor)
    {
        if (CuentaEnSesion.Jugador.RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            if(PersonajeDelCorredor.PuntacionTotal > CuentaEnSesion.Jugador.MejorPuntacion)
            {
                CuentaEnSesion.Jugador.MejorPuntacion = PersonajeDelCorredor.PuntacionTotal;
            }
            ServicioDeJuego.TerminarPartida(CuentaEnSesion);
        }
        MostrarPantallaFinJuego?.Invoke();
    }
    
    /// <summary>
    /// Lanza una notificacion indicando que la partida a finalizado
    /// </summary>
    public void NotificarTerminaPartida()
    {
        TerminoLaPartida?.Invoke();
    }
    
    /// <summary>
    /// Notifica a los demas jugadores que el jugador mato a otro
    /// </summary>
    /// <param name="nombreUsuario">String</param>
    /// <param name="NumeroVidas">String</param>
    public void NotificarMuerteJugador(String nombreUsuario, int NumeroVidas)
    {
        EventoEnJuego MuerteDeJugador = new EventoEnJuego();
        MuerteDeJugador.EventoEnJuegoMuerteJugador(CuentaEnSesion.NombreUsuario, IdDeMiSala, nombreUsuario, NumeroVidas);
        UdpSender EnviadorDePaquetesUDP = new UdpSender(DireccionIpDelServidor, PUERTO_ENVIO_UDP1, PUERTO_ENVIO_UDP2);
        EnviadorDePaquetesUDP.EnviarPaquete(MuerteDeJugador);
    }

    /// <summary>
    /// Notica a las demas cuentas que el corredor puede comer perseguidores
    /// </summary>
    public void NotificarCorredorPuedeComerJugadores()
    {
        EventoEnJuego CorredorPuedeComerCorredores = new EventoEnJuego();
        CorredorPuedeComerCorredores.SeInicioTiempoDeMatar(CuentaEnSesion.NombreUsuario, IdDeMiSala);
        UdpSender EnviadorDePauetesUdp = new UdpSender(DireccionIpDelServidor, PUERTO_ENVIO_UDP1, PUERTO_ENVIO_UDP2);
        EnviadorDePauetesUdp.EnviarPaquete(CorredorPuedeComerCorredores);
    }

    /// <summary>
    /// Borra los datos exlusivos de la partida
    /// </summary>
    public void ReiniciarDatosJuego()
    {
        Puntuacion = 0;
        CuentasEnLaSala.Clear();
        IdDeMiSala = String.Empty;
        MiSalaEsPublica = false;
    }

    /// <summary>
    /// Abandona la sala actual
    /// </summary>
    public void SalirDeLaSala()
    {
        ServicioDeJuego.AbandonarSala(CuentaEnSesion);
        ReiniciarDatosJuego();
    }
}