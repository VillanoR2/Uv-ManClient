using System;
using System.ServiceModel;
using System.Collections.Generic;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using System.ServiceModel.Channels;

public class JuegoCliente : MonoBehaviour, IGameServiceCallback
{
    public static JuegoCliente ClienteDelJuego;
    private string DireccionIpDelServidor;
    public GameServiceClient ServicioDeJuego;
    public CuentaModel CuentaEnSesion;
    private List<CuentaModel> CuentasEnSesion = new List<CuentaModel>();

    private void RecuperarIpDelServidor()
    {
        DireccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
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
        Debug.Log(cuenta.NombreUsuario);
        CuentasEnSesion.Add(cuenta);
    }
}
