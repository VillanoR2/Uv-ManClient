using System;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonsJoinLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private CuentaModel CuentaLogeada;
    
    private void RecuperarCuentaLogeada()
    {
        CuentaLogeada = Cuenta.cuentaLogeada.cuenta;
    }
    
    public void UnirseASala()
    {
        if(CuentaLogeada != null)
        {
            Boolean seuniAlServicioDeJuego = UnirseAlServicioDeJuego();
            Boolean seUnioAlServicioDeChat = UnirseAlServicioDeChat();
            if (seuniAlServicioDeJuego)
            {
                if (!seUnioAlServicioDeChat)
                {
                    //MostrarMensasjeErrorEnchat
                }
                ClienteDelJuego.SolcitarDetallesSala();
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                //MostrarMensajeErrorEnSala
            }
        }
        else
        {
            //Aqui poner mensaje de no se puede acceder al servicio de chat
        }
    }

    private Boolean UnirseAlServicioDeChat()
    {
        Boolean SeUnioCorrectamenteAlChat = false;
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        ChatCliente.clienteDeChat.ReiniciarServicio();
        ClienteDelJuego.ReinciarClienteDeJuego();
        SeUnioCorrectamenteAlChat = clienteDeChat.Conectar(CuentaLogeada); 
        return SeUnioCorrectamenteAlChat;
    }

    private Boolean UnirseAlServicioDeJuego()
    {
        return ClienteDelJuego.ServicioDeJuego.UnirseASala(CuentaLogeada);
    }
    
    public void RegresarMenuPrincipal()
    {
        SceneManager.LoadScene("MainScreen");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        RecuperarCuentaLogeada();
    }

    public void UnirseASalaConId()
    {
        SceneManager.LoadScene("JoinPrivateLobby");
    }

    public void CrearSala()
    {
        SceneManager.LoadScene("CreateLobby");
    }
}
