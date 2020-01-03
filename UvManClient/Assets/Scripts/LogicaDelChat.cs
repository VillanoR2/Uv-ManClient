using GameChatService.Dominio;
using LogicaDelNegocio.Modelo;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de manejar el chat del juego
/// </summary>
public class LogicaDelChat : MonoBehaviour
{
    public Text MessageText;
    public InputField SendingMessage;
    private CuentaModel Account;
    private ChatCliente ChatClient;

    /// <summary>
    /// Metodo de UNITY que se ejecuta en el primer cuadro de la escena
    /// </summary>
    private void Start()
    {
        RecuperarCuentaLogeada();
        RecuperarServicioDeChat();
    }

    /// <summary>
    /// Metodo de UNITY que se ejecuta una vez por cuadro
    /// </summary>
    private void Update()
    {
        MessageText.text = ChatClient.mensajes;
    }

    /// <summary>
    /// Recupera la información de la cuenta que se encuentra logeaada
    /// </summary>
    private void RecuperarCuentaLogeada()
    {
        Account = Cuenta.CuentaLogeada.CuentaM;
    }

    /// <summary>
    /// Inicializa al cliente del chat
    /// </summary>
    private void RecuperarServicioDeChat()
    {
        ChatClient = ChatCliente.clienteDeChat;
    }

    /// <summary>
    /// Cambia a la escena de MainScreen
    /// </summary>
    public void ButtonReturn()
    {
        ChatClient.servicioDeChat.Desconectar(Account);
        SceneManager.LoadScene("MainScreen");
    }

    /// <summary>
    /// Envia el mensaje ccon el texto que se encuentra en el input al servidor
    /// </summary>
    public void EnviarMensaje()
    {
        if(SendingMessage.text != String.Empty)
        {
            Message MensajeEnviar = RecuperarMesajeParaEnviar();
            ChatClient.EnviarMensaje(MensajeEnviar);
            SendingMessage.text = String.Empty;
        }
    }

    /// <summary>
    /// Recupera la informacion para enviar el mensaje
    /// </summary>
    /// <returns>el Message que se enviara</returns>
    private Message RecuperarMesajeParaEnviar()
    {
        Message Mensaje = new Message();
        Mensaje.HoraEnvio = DateTime.Now;
        Mensaje.Remitente = Account;
        Mensaje.Mensaje = SendingMessage.text;
        return Mensaje;
    }
}
