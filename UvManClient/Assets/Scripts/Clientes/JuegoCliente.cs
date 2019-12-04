using System;
using System.Collections.Generic;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using System.Threading;
using GameService.Dominio;
using System.ServiceModel;
using GameService.Dominio.Enum;
using System.Net.Sockets;

public class JuegoCliente : MonoBehaviour, IGameServiceCallback
{
    private UdpReciver RecibidorDePaquetesUDP;
    private Thread HiloDeEscuchaPaquetesUDP;

    public delegate void DatosDelMovimientoDeJugador(MovimientoJugador movimientoJugador);
    public delegate void DatosDeInicioDeLaPartida(InicioPartida inicioPartida);
    public delegate void DatosDeLaMuerteDeJugador(MuerteJugador muerteJugador);
    public event DatosDelMovimientoDeJugador SeMovioUnJugador;
    public event DatosDeInicioDeLaPartida IniciaLaPartida;
    public event DatosDeLaMuerteDeJugador SeMurioUnJugador;

    public static JuegoCliente ClienteDelJuego;
    private string DireccionIpDelServidor;
    public GameServiceClient ServicioDeJuego;
    public CuentaModel CuentaEnSesion;
    public readonly List<CuentaModel> CuentasEnSesion = new List<CuentaModel>();
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
    }

    private void RecuperarIpDelServidor()
    {
        DireccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    private void RecuperarCuentaEnSession()
    {
        CuentaEnSesion = Cuenta.cuentaLogeada.cuenta;
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
            RecibidorDePaquetesUDP = new UdpReciver(DireccionIpDelServidor);
            RecibidorDePaquetesUDP.EventoRecibido += RecibirEventoEnElJuego;
            HiloDeEscuchaPaquetesUDP = new Thread(RecibidorDePaquetesUDP.RecibirDatos);
            HiloDeEscuchaPaquetesUDP.Start();
        }
        catch (SocketException)
        {
            Debug.Log("No se puede iniciar el servicio de escuha UDP");
            HiloDeEscuchaPaquetesUDP?.Abort();
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
        throw new NotImplementedException();
    }

    public void SalaLlena()
    {
        SeLlenoLaSala?.Invoke();
    }

    public void NuevoCuentaEnLaSala(CuentaModel cuenta)
    {
        CuentasEnSesion.Add(cuenta);
        SeActualizaronRoles?.Invoke();
    }

    //Puede que ya no lo necesite
    public void CuentaAbandoSala(CuentaModel cuenta)
    {
        CuentasEnSesion.Remove(cuenta);
    }

    public void RefrescarCuentasEnSala(CuentaModel[] CuentasEnMiSala)
    {
        CuentasEnSesion.Clear();
        foreach (CuentaModel Cuenta in CuentasEnMiSala)
        {
            CuentasEnSesion.Add(Cuenta);
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
                case EnumTipoDeEventoEnJuego.CambiarPantalla:
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
            }
        }
    }
    
    public void EnviarMovimiento(float x, float y, float movimientoX, float movimientoY)
    {
        EventoEnJuego eventoEnJuego = new EventoEnJuego();
        eventoEnJuego.EventoEnJuegoMovimientoJugador(CuentaEnSesion.NombreUsuario, IdDeMiSala ,CuentaEnSesion.NombreUsuario,
            x, y, movimientoX, movimientoY);
        UdpSender enviadorDePaquetesUDP = new UdpSender(DireccionIpDelServidor);
        enviadorDePaquetesUDP.EnviarPaquete(eventoEnJuego);
    }

}