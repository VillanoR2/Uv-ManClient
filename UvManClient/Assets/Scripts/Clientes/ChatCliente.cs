using UnityEngine;
using LogicaDelNegocio.Modelo;
using System.ServiceModel;
using System;
using GameChatService.Dominio;

/// <summary>
/// Se encarga de manejar el servicio de chat
/// </summary>
public class ChatCliente : MonoBehaviour, IChatServiceCallback
{
    public static ChatCliente clienteDeChat;
    public ChatServiceClient servicioDeChat;
    private String direccionIpDelServidor;
    public String mensajes;

    /// <summary>
    /// Recupera la direccion Ip del servidor
    /// </summary>
    private void RecuperarIpDelServidor()
    {
        direccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    /// <summary>
    /// Reinicia el servicio del juego
    /// </summary>
    public void ReiniciarServicio()
    {
        RecuperarIpDelServidor();
        InicializarClienteDeChat();
    }

    /// <summary>
    /// Inicia el cliente del servicio de chat
    /// </summary>
    private void InicializarClienteDeChat()
    {
        mensajes = String.Empty;
        servicioDeChat = new ChatServiceClient(new InstanceContext(this) ,
            new NetTcpBinding(SecurityMode.None), 
            new EndpointAddress("net.tcp://" + direccionIpDelServidor + ":8192/ChatService"));
    }

    /// <summary>
    /// Muestra el mensaje de que un mensaje abandono el servicio de chat
    /// </summary>
    /// <param name="cuenta"></param>
    public void Abandonar(CuentaModel cuenta)
    {
        mensajes += "### El usuario " + cuenta.NombreUsuario + " ha abandonado la partida ###\n";
    }
    
    /// <summary>
    /// Agrega el mensaje recibido a la lista de mensajes
    /// </summary>
    /// <param name="mensaje">Message</param>
    public void RecibirMensaje(Message mensaje)
    {
        mensajes += (mensaje.Remitente.NombreUsuario + " >> " + mensaje.Mensaje + ": " + mensaje.HoraEnvio.ToShortTimeString() + "\n");
    }
    
    /// <summary>
    /// Muestra en los mensajes el nombre de usuario de la cuenta que se conecto
    /// </summary>
    /// <param name="cuenta">CuentaModel</param>
    public void Unirse(CuentaModel cuenta)
    {
        mensajes += "### El usuario " + cuenta.NombreUsuario + " se ha unido a la partida ###\n";
    }

    /// <summary>
    /// Metodo de UNITY que se ejecuta cuando se carga la escena
    /// </summary>
    private void Awake()
    {
        if (clienteDeChat == null)
        {
            clienteDeChat = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (clienteDeChat != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RecuperarIpDelServidor();
        InicializarClienteDeChat();
    }

    /// <summary>
    /// Envia mensaje a todos los usuarios de la sala
    /// </summary>
    /// <param name="mensajeAEnviar">Message</param>
    public void EnviarMensaje(Message mensajeAEnviar)
    {       
        servicioDeChat.EnviarMensaje(mensajeAEnviar);
    }

}
