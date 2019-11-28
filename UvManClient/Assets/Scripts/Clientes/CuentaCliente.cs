using LogicaDelNegocio.Modelo;
using System.ServiceModel;
using UnityEngine;

public class CuentaCliente : MonoBehaviour
{
    public static CuentaCliente clienteDeCuenta;
    private string direccionIpDelServidor;
    public CuentaServiceClient servicioDeCuenta;
    public CuentaModel cuentaAVerificar;

    private void InicializarServicioDeCuenta()
    {
        servicioDeCuenta = new CuentaServiceClient(new NetTcpBinding(SecurityMode.None),
            new EndpointAddress("net.tcp://" + direccionIpDelServidor + ":8092/CuentaService"));
    }

    private void RecuperarIpDelServidor()
    {
        direccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    private void Awake()
    {
        if (clienteDeCuenta == null)
        {
            clienteDeCuenta = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (clienteDeCuenta != this)
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

    private void ActualizarIpDelServidor()
    {
        RecuperarIpDelServidor();
        InicializarServicioDeCuenta();
    }

}
