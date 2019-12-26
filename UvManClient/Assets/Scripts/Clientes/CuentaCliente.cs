using LogicaDelNegocio.Modelo;
using System.ServiceModel;
using UnityEngine;

public class CuentaCliente : MonoBehaviour
{
    public static CuentaCliente clienteDeCuenta;
    private string direccionIpDelServidor;
    public CuentaServiceClient servicioDeCuenta;
    public CuentaModel cuentaAVerificar;

    /// <summary>
    /// Inicializa el servicio de cuentas
    /// </summary>
    private void InicializarServicioDeCuenta()
    {
        servicioDeCuenta = new CuentaServiceClient(new NetTcpBinding(SecurityMode.None),
            new EndpointAddress("net.tcp://" + direccionIpDelServidor + ":8092/CuentaService"));
    }

    /// <summary>
    /// Recupera la dirección Ip del servidor
    /// </summary>
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

    /// <summary>
    /// Actualiza la direccion ip del servidor y reincia el servicio
    /// </summary>
    private void ActualizarIpDelServidor()
    {
        RecuperarIpDelServidor();
        InicializarServicioDeCuenta();
    }

    /// <summary>
    /// Reinicia el cliente del servicio de cuenta
    /// </summary>
    public void ReiniciarServicio()
    {
        InicializarServicioDeCuenta();
    }

}
