using UnityEngine;
using System.ServiceModel;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class SessionCliente : MonoBehaviour, ISessionServiceCallback
{
    public static SessionCliente clienteDeSesion;
    public SessionServiceClient servicioDeSesion;
    public string direccionIpDelServidor = "localhost";
    private String rutaDelArchivoDeConfiguracion;
    public delegate void CambioDireccionIP();
    public event CambioDireccionIP ModificacionDeLaDireccion;

    private void InicializarServicioDeSesion()
    {
        servicioDeSesion = new SessionServiceClient(new InstanceContext(this), new NetTcpBinding(SecurityMode.None), 
            new EndpointAddress("net.tcp://" + direccionIpDelServidor + ":7972/SessionService"));
    }

    private void RecuperarIpDelServidor()
    {
        direccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
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

    public bool EstaVivo()
    {
        return true;
    }

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

[Serializable]
class ConfiguracionAGuardar
{
    public string DireccionIP;

    public ConfiguracionAGuardar(string DireccionIP)
    {
        this.DireccionIP = DireccionIP;
    }
}
