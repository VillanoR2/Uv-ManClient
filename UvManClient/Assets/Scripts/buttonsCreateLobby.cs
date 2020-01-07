using System;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de controlar los elementos graficos que se encuentran en la escena de CreateLobby
/// </summary>
public class buttonsCreateLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego;
    private CuentaModel CuentaLogeada;
    public InputField txtBoxIdSala;
    public Toggle toggleEsSalaPrivada;
    public GameObject PanelException;

    /// <summary>
    /// Obtiene la cuenta logeada e inicializa el cliente del juego
    /// </summary>
    private void InicializarServicio()
    {
        ClienteDelJuego = JuegoCliente.ClienteDelJuego;
        RecuperarCuentaLogeada();
    }

    /// <summary>
    /// Le dice al cliente del juego que se desea crear una sala con la informacion de los campos
    /// </summary>
    public void CrearSala()
    {
        InicializarServicio();
        if (VerificarIdValido())
        {
            String IdDeLaSala = txtBoxIdSala.text;
            Boolean SeUniServicioChat = UnirseAlServicioDeChat();
            Boolean esSalaPrivada = toggleEsSalaPrivada.isOn;
            EnumEstadoCrearSalaConId estadoCrearSala = UnirseAlServicioDeJuego(IdDeLaSala, esSalaPrivada);
            switch (estadoCrearSala)
            {
                case EnumEstadoCrearSalaConId.CreadaCorrectamente:
                    if (!SeUniServicioChat)
                    {
                        Debug.LogWarning("Chat no disponible");
                        PanelException.SetActive(true);
                        PanelException.GetComponentInChildren<Text>().text = "El servicio de chat esta temporalmente inactivo.";
                    }
                    ClienteDelJuego.SolcitarDetallesSala();
                    SceneManager.LoadScene("Lobby");
                    break;
                case EnumEstadoCrearSalaConId.IdYaExistente:
                    Debug.LogWarning("ID Lobby existente");
                        PanelException.SetActive(true);
                        PanelException.GetComponentInChildren<Text>().text = "El Id ingresado y se encuentra en uso..";
                    break;
                case EnumEstadoCrearSalaConId.NoSeEncuentraEnSesion:
                    Debug.LogWarning("Cuenta no en sesión");
                        PanelException.SetActive(true);
                        PanelException.GetComponentInChildren<Text>().text = "La cuenta no se encuentra en sesion, no se puede crear el lobby.";
                    break;
            }
        }
    }

    /// <summary>
    /// Cambia a la escena de JoinLobby
    /// </summary>
    public void RegresarAUnirseASala()
    {
        SceneManager.LoadScene("JoinLobby");
    }

    /// <summary>
    /// Le solicita al cliente del chat unirse al chat
    /// </summary>
    /// <returns>True si se pudo unir al cliente del chat correctamente o false si no</returns>
    private Boolean UnirseAlServicioDeChat()
    {
        Boolean SeUnioCorrectamenteAlChat;
        ChatCliente.clienteDeChat.ReiniciarServicio();
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        SeUnioCorrectamenteAlChat = clienteDeChat.Conectar(CuentaLogeada);
        return SeUnioCorrectamenteAlChat;
    }

    /// <summary>
    /// Le solicita al cliente del juego crear una nueva sala
    /// </summary>
    /// <param name="id">String</param>
    /// <param name="esPrivada">Boolean</param>
    /// <returns>EnumEstadoCrearSala</returns>
    private EnumEstadoCrearSalaConId UnirseAlServicioDeJuego(String id, Boolean esPrivada)
    {
        ClienteDelJuego.ReinciarClienteDeJuego();
        return ClienteDelJuego.ServicioDeJuego.CrearSala(id, !esPrivada, CuentaLogeada);
    }

    /// <summary>
    /// Recupera la informacion de la cuenta logeada
    /// </summary>
    private void RecuperarCuentaLogeada()
    {
        CuentaLogeada = Cuenta.CuentaLogeada.CuentaM;
    }

    /// <summary>
    /// Verifica que el id de la sala introducido sea valido (no contenta espacios y no tenga mas de 10 caracteres)
    /// </summary>
    /// <returns>True si el id es valido o false si no</returns>
    private Boolean VerificarIdValido()
    {
        return txtBoxIdSala.text != String.Empty && !txtBoxIdSala.text.Contains(" ") && txtBoxIdSala.text.Length < 10;
    }
}
