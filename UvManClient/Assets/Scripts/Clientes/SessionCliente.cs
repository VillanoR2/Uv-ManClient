using UnityEngine;
using System.ServiceModel;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using System.Net;
using LogicaDelNegocio.Modelo;
using System.Security.Principal;

/// <summary>
/// Se encarga de manejar el servicio de sesion
/// </summary>
public class SessionCliente : MonoBehaviour, ISessionServiceCallback
{
    public static SessionCliente clienteDeSesion;
    public SessionServiceClient servicioDeSesion;
    public string direccionIpDelServidor = "127.0.0.1";
    private String rutaDelArchivoDeConfiguracion;
    private CuentaModel cuentaLogeada;
    public delegate void CambioDireccionIP();
    public event CambioDireccionIP ModificacionDeLaDireccion;

    /// <summary>
    /// Iniciliza el servicio de sesion
    /// </summary>
    private void InicializarServicioDeSesion()
    {
        servicioDeSesion = new SessionServiceClient(new InstanceContext(this), new NetTcpBinding(SecurityMode.Transport), 
            new EndpointAddress("net.tcp://" + direccionIpDelServidor + ":7972/SessionService"));
        
    }

    private void Awake()
    {
        rutaDelArchivoDeConfiguracion = Application.persistentDataPath + "/configuracion.dat";
        if(clienteDeSesion == null)
        {
            clienteDeSesion = this;
            DontDestroyOnLoad(gameObject);
        }else if (clienteDeSesion != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CargarArchivoDeConfiguracion();
        InicializarServicioDeSesion();
    }

    /// <summary>
    /// Responde al servidor que el cliente esta vivo
    /// </summary>
    /// <returns>Verdadero</returns>
    public bool EstaVivo()
    {
        return true;
    }

    /// <summary>
    /// Reinicia el servicio del cliente
    /// </summary>
    public void ReiniciarServicio()
    {
        InicializarServicioDeSesion();
    }

    /// <summary>
    /// Guarda la direccion Ip del cliente
    /// </summary>
    /// <returns></returns>
    public Boolean Guardar()
    {
        BinaryFormatter formatoBinario = new BinaryFormatter();
        try
        {
            FileStream archivo = File.Create(rutaDelArchivoDeConfiguracion);

            ConfiguracionAGuardar configuracion = new ConfiguracionAGuardar(direccionIpDelServidor);

            formatoBinario.Serialize(archivo, configuracion);

            archivo.Close();

            ModificacionDeLaDireccion?.Invoke();
            return true;
        }catch(IOException)
        {
            return false;
        }
    }

    /// <summary>
    /// Recupera el archivo de la configuracion Ip del servidor
    /// </summary>
    private void CargarArchivoDeConfiguracion()
    {
        if (File.Exists(rutaDelArchivoDeConfiguracion))
        {
            BinaryFormatter formatoBinario = new BinaryFormatter();
            try
            {
                FileStream archivo = File.Open(rutaDelArchivoDeConfiguracion, FileMode.Open);
                ConfiguracionAGuardar configuracion = (ConfiguracionAGuardar)formatoBinario.Deserialize(archivo);
                direccionIpDelServidor = configuracion.DireccionIP;
                archivo.Close();
            }
            catch (IOException)
            {
                direccionIpDelServidor = "localhost";
            }
        }
        else
        {
            direccionIpDelServidor = "localhost";
        }
        
    }

}

/// <summary>
/// Se encarga de guardar la direccion ip del servidor
/// </summary>
[Serializable]
class ConfiguracionAGuardar
{
    public string DireccionIP;

    public ConfiguracionAGuardar(string DireccionIP)
    {
        this.DireccionIP = DireccionIP;
    }
}
