using System;
using System.ServiceModel;
using System.Collections.Generic;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Channels;
using UnityEngine.Serialization;

public class JuegoCliente : MonoBehaviour, IGameServiceCallback
{
    public static JuegoCliente ClienteDelJuego;
    private string DireccionIpDelServidor;
    public GameServiceClient ServicioDeJuego;
    public CuentaModel CuentaEnSesion;
    public readonly List<CuentaModel> CuentasEnSesion = new List<CuentaModel>();
    public String IdDeMiSala;
    public Boolean MiSalaEsPublica;
    public delegate void ModificacionRoles();

    public event ModificacionRoles SeActualizaronRoles;
    public void SolcitarDetallesSala()
    {
        MiSalaEsPublica = ServicioDeJuego.MiSalaEsPublica(CuentaEnSesion);
        IdDeMiSala =ServicioDeJuego.RecuperarIdDeMiSala(CuentaEnSesion);
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
        SessionCliente.clienteDeSesion.ModificacionDeLaDireccion += ActualizarIpDelServidor;
        RecuperarIpDelServidor();
        InicializarServicioDeCuenta();
    }

    private void InicializarServicioDeCuenta()
    {
        ServicioDeJuego = new GameServiceClient(new InstanceContext(this), 
            new NetTcpBinding(SecurityMode.None), new EndpointAddress("net.tcp://" + DireccionIpDelServidor + ":8292/GameService"));
    }
    
    private void ActualizarIpDelServidor()
    {
        RecuperarIpDelServidor();
        InicializarServicioDeCuenta();
    }

    public void TerminarPartida()
    {
        throw new NotImplementedException();
    }

    public void SalaLlena()
    {
        throw new NotImplementedException();
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
        RecuperarIpDelServidor();
        InicializarServicioDeCuenta();
        RecuperarCuentaEnSession();
    }
}
