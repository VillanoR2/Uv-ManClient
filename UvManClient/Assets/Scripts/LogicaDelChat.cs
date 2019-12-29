using GameChatService.Dominio;
using LogicaDelNegocio.Modelo;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicaDelChat : MonoBehaviour
{
    public Text MessageText;
    public InputField SendingMessage;
    private CuentaModel Account;
    private ChatCliente ChatClient;

    private void Start()
    {
        RecuperarCuentaLogeada();
        RecuperarServicioDeChat();
    }

    private void Update()
    {
        MessageText.text = ChatClient.mensajes;
    }

    private void RecuperarCuentaLogeada()
    {
        Account = Cuenta.cuentaLogeada.cuenta;
    }

    private void RecuperarServicioDeChat()
    {
        ChatClient = ChatCliente.clienteDeChat;
    }

    public void ButtonReturn()
    {
        ChatClient.servicioDeChat.Desconectar(Account);
        SceneManager.LoadScene("MainScreen");
    }

    public void EnviarMensaje()
    {
        if(SendingMessage.text != "")
        {
            Message MensajeEnviar = RecuperarMesajeParaEnviar();
            ChatClient.EnviarMensaje(MensajeEnviar);
            SendingMessage.text = "";
        }
    }

    private Message RecuperarMesajeParaEnviar()
    {
        Message Mensaje = new Message();
        Mensaje.HoraEnvio = DateTime.Now;
        Mensaje.Remitente = Account;
        Mensaje.Mensaje = SendingMessage.text;
        return Mensaje;
    }
}
