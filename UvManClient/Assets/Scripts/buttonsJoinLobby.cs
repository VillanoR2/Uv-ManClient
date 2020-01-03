using System;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Se encarga de controlar los elementos de la escena JoinLobby
/// </summary>
public class buttonsJoinLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego;
    private CuentaModel CuentaLogeada;
    
    /// <summary>
    /// Recupera la información de la cuenta que se encuentra logeada
    /// </summary>
    private void RecuperarCuentaLogeada()
    {
        CuentaLogeada = Cuenta.CuentaLogeada.CuentaM;
    }
    
    /// <summary>
    /// Le solicita al cliente del juego unirse a una sala aleatoria y al chat
    /// </summary>
    public void UnirseASala()
    {
        ObtenerRecursosParaUso();
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

    /// <summary>
    /// Solicita al servicio de chat unirse al chat del juego
    /// </summary>
    /// <returns>True si se pudo unir al servicio de chat o false si no</returns>
    private Boolean UnirseAlServicioDeChat()
    {
        Boolean SeUnioCorrectamenteAlChat;
        ChatCliente.clienteDeChat.ReiniciarServicio();
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        SeUnioCorrectamenteAlChat = clienteDeChat.Conectar(CuentaLogeada); 
        return SeUnioCorrectamenteAlChat;
    }

    /// <summary>
    /// Solicita al cliente del juego unirse a una sala disponible
    /// </summary>
    /// <returns>Verdadero si se logro unir a la sala o false si no</returns>
    private Boolean UnirseAlServicioDeJuego()
    {
        ClienteDelJuego.ReinciarClienteDeJuego();
        return ClienteDelJuego.ServicioDeJuego.UnirseASala(CuentaLogeada);
    }
    
    /// <summary>
    /// Cambia a la escena de MainScreen
    /// </summary>
    public void RegresarMenuPrincipal()
    {
        SceneManager.LoadScene("MainScreen");
    }
    
    /// <summary>
    /// Inicializa el cliente del juego
    /// </summary>
    private void ObtenerRecursosParaUso()
    {
        RecuperarCuentaLogeada();
        ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    }

    /// <summary>
    /// Cambia a la escena de JoinPrivateLobby
    /// </summary>
    public void UnirseASalaConId()
    {
        SceneManager.LoadScene("JoinPrivateLobby");
    }

    /// <summary>
    /// Cambia a la escena de CreateLobby
    /// </summary>
    public void CrearSala()
    {
        SceneManager.LoadScene("CreateLobby");
    }
}
