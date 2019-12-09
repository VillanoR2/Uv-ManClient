using System;
using System.Collections.Generic;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using System.Threading;
using GameService.Dominio;
using System.ServiceModel;
using GameService.Dominio.Enum;
using System.Net.Sockets;
using LogicaDelNegocio.Modelo.Enum;
using ConexionRed.Udp;

public class JuegoCliente : MonoBehaviour, IGameServiceCallback
{
    private const int PUERTO_ENVIO_UDP1 = 8090;
    private const int PUERTO_ENVIO_UDP2 = 8091;
    private const int PUERTO_ESCUCHA_UDP = 8296;
    private const int PUERTO_ESCUCHA_UDP2 = 8297;

    private UdpReciverClient RecibidorDePaquetesUDP;
    private Thread HiloDeEscuchaPaquetesUDP;

    public delegate void DatosDelMovimientoDeJugador(MovimientoJugador movimientoJugador);
    public delegate void DatosDeInicioDeLaPartida(InicioPartida inicioPartida);
    public delegate void DatosDeLaMuerteDeJugador(MuerteJugador muerteJugador);
    public delegate void NotificacionSobrePartida();
    public event DatosDelMovimientoDeJugador SeMovioUnJugador;
    public event DatosDeInicioDeLaPartida IniciaLaPartida;
    //public event NotificacionSobrePartida IniciaNuevoNivel;
    public event DatosDeLaMuerteDeJugador SeMurioUnJugador;
    public event DatosDeInicioDeLaPartida NuevoNivel;
    public event NotificacionSobrePartida TerminoLaPartida;
    public event NotificacionSobrePartida PrepararNuevoNivel;

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

    public void SolcitarDetallesSala()
    {
        MiSalaEsPublica = ServicioDeJuego.MiSalaEsPublica(CuentaEnSesion);
        IdDeMiSala = ServicioDeJuego.RecuperarIdDeMiSala(CuentaEnSesion);
        RefrescarCuentasEnSala(ServicioDeJuego.ObtenerCuentasEnMiSala(CuentaEnSesion));
        PonerCuentaEnSesionCompleta();
    }

    private void RecuperarIpDelServidor()
    {
        DireccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    private void RecuperarCuentaEnSession()
    {
        CuentaEnSesion = Cuenta.cuentaLogeada.cuenta;
    }

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
            Debug.Log(ex.Message);
            Debug.Log("No se puede iniciar el servicio de escuha UDP");
            
        }        
    }

    private void TerminarHiloDeEscuchaUDP()
    {
        RecibidorDePaquetesUDP.LiberarRecursos();
        HiloDeEscuchaPaquetesUDP.Abort();
        HiloDeEscuchaPaquetesUDP = null;
    }


private void InicializarServicioDeJuego()
    {
        ServicioDeJuego = new GameServiceClient(new InstanceContext(this), 
            new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://" + DireccionIpDelServidor + ":8292/GameService"));
    }
   
    public void TerminarPartida()
    {
        TerminoLaPartida?.Invoke();
    }

    public void SalaLlena()
    {
        SeLlenoLaSala?.Invoke();
    }

    public void NuevoCuentaEnLaSala(CuentaModel cuenta)
    {
        CuentasEnLaSala.Add(cuenta);
        SeActualizaronRoles?.Invoke();
    }

    //Puede que ya no lo necesite
    public void CuentaAbandoSala(CuentaModel cuenta)
    {
        CuentasEnLaSala.Remove(cuenta);
    }

    public void RefrescarCuentasEnSala(CuentaModel[] CuentasEnMiSala)
    {
        CuentasEnLaSala.Clear();
        foreach (CuentaModel Cuenta in CuentasEnMiSala)
        {
            CuentasEnLaSala.Add(Cuenta);
        }
        SeActualizaronRoles?.Invoke();
    }

    public void ReinciarClienteDeJuego()
    {
        if(ServicioDeJuego.State == CommunicationState.Closed)
        {
            RecuperarIpDelServidor();
            InicializarServicioDeJuego();
            RecuperarCuentaEnSession();
        }
    }
    
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
                case EnumTipoDeEventoEnJuego.IniciarCuentaRegresivaInicioNivel:
                    NuevoNivel?.Invoke(eventoEnJuego.DatosInicioDePartida);
                    break;
                case EnumTipoDeEventoEnJuego.IniciarNivel:
                    NuevoNivel?.Invoke(eventoEnJuego.DatosInicioDePartida);
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
            }
        }
    }
    
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
            Cuenta.cuentaLogeada.cuenta = MiCuentaCompleta;
            CuentaEnSesion = MiCuentaCompleta;
        }
    }

    public void TerminarPartida(Character_Movement PersonajeDelCorredor)
    {
        if (CuentaEnSesion.Jugador.RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            if(PersonajeDelCorredor.PuntacionTotal > CuentaEnSesion.Jugador.MejorPuntacion)
            {
                CuentaEnSesion.Jugador.MejorPuntacion = PersonajeDelCorredor.PuntacionTotal;
            }
        }
        ServicioDeJuego.TerminarPartida(CuentaEnSesion);
    }

    /// <summary>
    /// Notifica que el conteo para el inicio del nuevo nivel va a comenzar o que el nuevo nivel comenzo
    /// </summary>
    public void NotificarInicioPartida()
    {
        ServicioDeJuego.NotificarIniciarNivel(CuentaEnSesion);
    }

    public void NotificarTerminaPartida()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Notifica que un nuevo nivel esta apunto de iniciar
    /// </summary>
    void IGameServiceCallback.NuevoNivel()
    {
        PrepararNuevoNivel?.Invoke();
    }

    public void NotificarMuerteJugador(String nombreUsuario, int NumeroVidas)
    {
        EventoEnJuego MuerteDeJugador = new EventoEnJuego();
        MuerteDeJugador.EventoEnJuegoMuerteJugador(CuentaEnSesion.NombreUsuario, IdDeMiSala, nombreUsuario, NumeroVidas, 0);
        UdpSender enviadorDePaquetesUDP = new UdpSender(DireccionIpDelServidor, PUERTO_ENVIO_UDP1, PUERTO_ENVIO_UDP2);
        enviadorDePaquetesUDP.EnviarPaquete(MuerteDeJugador);
    }
}