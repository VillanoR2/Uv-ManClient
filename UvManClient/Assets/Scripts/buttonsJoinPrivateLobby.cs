using System;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de manejar los elementos que se encuentran en la escena JoinPrivateLobby
/// </summary>
public class buttonsJoinPrivateLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego;
    private CuentaModel CuentaLogeada;
    public InputField txtBoxIdSala;
    
    /// <summary>
    /// Inicializa el cliente del juego
    /// </summary>
    private void InicializarServicio()
    {
        ClienteDelJuego = JuegoCliente.ClienteDelJuego;
        RecuperarCuentaLogeada();
    }

    /// <summary>
    /// Le solicita al cliente del juego unirse a una sala privada
    /// </summary>
    public void UnirseASalaPrivada()
    {
        InicializarServicio();
        if (VerificarIdValido())
        {
            String IdDeLaSala = txtBoxIdSala.text;
            Boolean SeUniServicioChat = UnirseAlServicioDeChat(); 
            EnumEstadoDeUnirseASala EstadoUnirseASala = UnirseAlServicioDeJuego(IdDeLaSala);
            switch (EstadoUnirseASala) 
            {
                case EnumEstadoDeUnirseASala.UnidoCorrectamente:
                    if (!SeUniServicioChat)
                    {
                        //Mostrar mensaje de error
                    }
                    ClienteDelJuego.SolcitarDetallesSala();
                    SceneManager.LoadScene("Lobby");
                    break;
                case EnumEstadoDeUnirseASala.SalaInexistente:
                    //Mostrar mensaje
                    break;
                case EnumEstadoDeUnirseASala.SalaLlena:
                    //Mostrar mensaje
                    break;
                case EnumEstadoDeUnirseASala.NoSeEncuentraEnSesion:
                    //Mostrar mensaje
                    break;
            }
        }
    }

    /// <summary>
    /// Cambia a la escena JoinLobby
    /// </summary>
    public void RegresarAUnirseASala()
    {
        SceneManager.LoadScene("JoinLobby");
    }
    
    /// <summary>
    /// Solicita al servicio de chat unirse al chat del juego
    /// </summary>
    /// <returns>True si se unio correctamente o false si no</returns>
    private Boolean UnirseAlServicioDeChat()
    {
        Boolean SeUnioCorrectamenteAlChat;
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        ChatCliente.clienteDeChat.ReiniciarServicio();
        SeUnioCorrectamenteAlChat = clienteDeChat.Conectar(CuentaLogeada); 
        return SeUnioCorrectamenteAlChat;
    }
    
    /// <summary>
    /// Solicita al cliente del juego unirse a una sala privada
    /// </summary>
    /// <param name="id">String</param>
    /// <returns>EnumEstadoUnirseASala</returns>
    private EnumEstadoDeUnirseASala UnirseAlServicioDeJuego(String id)
    {
        ClienteDelJuego.ReinciarClienteDeJuego();
        return ClienteDelJuego.ServicioDeJuego.UnirseASalaPrivada(id, CuentaLogeada);
    }
    
    /// <summary>
    /// Recupera la informacion de la cuenta que se encuentra logeada
    /// </summary>
    private void RecuperarCuentaLogeada()
    {
        CuentaLogeada = Cuenta.CuentaLogeada.CuentaM;
    }
    
    /// <summary>
    /// Verifica que el id de la sala sea valido (no contenga espacios y no tenga mas de 10 caracteres)
    /// </summary>
    /// <returns>True si es valido o false si no</returns>
    private Boolean VerificarIdValido()
    {
        return txtBoxIdSala.text != "" && !txtBoxIdSala.text.Contains(" ") && txtBoxIdSala.text.Length < 10;
    }
}
