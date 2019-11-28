using GameChatService.Dominio;
using LogicaDelNegocio.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsLobby : MonoBehaviour
{
    public Text TXTMensajes;
    public InputField IFMensajeAEnviar;
    private CuentaModel cuenta;
    private ChatCliente clienteDeChat;

    private void Start()
    {
        RecuperarCuentaLogeada();
        RecuperarServicioDeChat();
    }

    private void Update()
    {
        TXTMensajes.text = clienteDeChat.mensajes;
    }

    private void RecuperarCuentaLogeada()
    {
        cuenta = Cuenta.cuentaLogeada.cuenta;
    }

    private void RecuperarServicioDeChat()
    {
        clienteDeChat = ChatCliente.clienteDeChat;
    }

    public void ButtonReturn()
    {
        clienteDeChat.servicioDeChat.Desconectar(cuenta);
        
        SceneManager.LoadScene("MainScreen");
    }

    public void ButtonStart()
    {
        //Inicia Partida
    }

    public void EnviarMensaje()
    {
        if(IFMensajeAEnviar.text != "")
        {
            Message mensajeEnviar = RecuperarMesajeParaEnviar();
            clienteDeChat.EnviarMensaje(mensajeEnviar);
            IFMensajeAEnviar.text = "";
        }
    }

    private Message RecuperarMesajeParaEnviar()
    {
        Message mensaje = new Message();
        mensaje.HoraEnvio = DateTime.Now;
        mensaje.Remitente = cuenta;
        mensaje.Mensaje = IFMensajeAEnviar.text;
        return mensaje;
    }
}
