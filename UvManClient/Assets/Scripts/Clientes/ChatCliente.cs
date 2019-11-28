using UnityEngine;
using LogicaDelNegocio.Modelo;
using System.ServiceModel;
using System;
using GameChatService.Dominio;

public class ChatCliente : MonoBehaviour, IChatServiceCallback
{
    public static ChatCliente clienteDeChat;
    public ChatServiceClient servicioDeChat;
    private String direccionIpDelServidor;
    public String mensajes;

    private void RecuperarIpDelServidor()
    {
        direccionIpDelServidor = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    public void ReiniciarServicio()
    {
        RecuperarIpDelServidor();
        InicializarClienteDeChat();
    }

    private void InicializarClienteDeChat()
    {
        servicioDeChat = new ChatServiceClient(new InstanceContext(this) ,
            new NetTcpBinding(SecurityMode.None), 
            new EndpointAddress("net.tcp://" + direccionIpDelServidor + ":8192/ChatService"));

    }

    public void Abandonar(CuentaModel cuenta)
    {
        mensajes += "### El usuario " + cuenta.NombreUsuario + " ha abandonado la partida ###\n";
    }

    public void EstaEscribiendoCallback(string cuenta)
    {
        
    }

    public void RecibirMensaje(Message mensaje)
    {
        mensajes += (mensaje.Remitente.NombreUsuario + " >> " + mensaje.Mensaje + ": " + mensaje.HoraEnvio.ToShortTimeString() + "\n");
    }

    public void RefrescarCuentasConectadas(CuentaModel[] cuentasConectadas)
    {
        
    }

    public void Unirse(CuentaModel cuenta)
    {
        mensajes += "### El usuario " + cuenta.NombreUsuario + " se ha unido a la partida ###\n";
    }

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
        SessionCliente.clienteDeSesion.ModificacionDeLaDireccion += ActualizarIpDelServidor;
        RecuperarIpDelServidor();
        InicializarClienteDeChat();
    }

    public void EnviarMensaje(Message mensajeAEnviar)
    {       
        servicioDeChat.EnviarMensaje(mensajeAEnviar);
    }

    private void ActualizarIpDelServidor()
    {
        RecuperarIpDelServidor();
        InicializarClienteDeChat();
    }
}
