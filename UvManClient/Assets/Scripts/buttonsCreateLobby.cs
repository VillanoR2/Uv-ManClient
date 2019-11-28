using System;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsCreateLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego = JuegoCliente.ClienteDelJuego;
    private CuentaModel CuentaLogeada;
    public InputField txtBoxIdSala;
    public Toggle toggleEsSalaPrivada;
    
    // Start is called before the first frame update
    void Start()
    {
        RecuperarCuentaLogeada();
    }

    public void CrearSala()
    {
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
                        //Mostrar mensaje de error
                    }
                    ClienteDelJuego.SolcitarDetallesSala();
                    SceneManager.LoadScene("Lobby");
                    break;
                case EnumEstadoCrearSalaConId.IdYaExistente:
                    //Mostrar mensaje
                    break;
                case EnumEstadoCrearSalaConId.NoSeEncuentraEnSesion:
                    //Mostrar mensaje
                    break;
            }
        }
    }

    public void RegresarAUnirseASala()
    {
        SceneManager.LoadScene("JoinLobby");
    }
    
    private Boolean UnirseAlServicioDeChat()
    {
        Boolean SeUnioCorrectamenteAlChat;
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        ClienteDelJuego.ReinciarClienteDeJuego();
        ChatCliente.clienteDeChat.ReiniciarServicio();
        SeUnioCorrectamenteAlChat = clienteDeChat.Conectar(CuentaLogeada); 
        return SeUnioCorrectamenteAlChat;
    }
    
    private EnumEstadoCrearSalaConId UnirseAlServicioDeJuego(String id, Boolean esPrivada)
    {
        return ClienteDelJuego.ServicioDeJuego.CrearSala(id, !esPrivada, CuentaLogeada);
    }
    
    private void RecuperarCuentaLogeada()
    {
        CuentaLogeada = Cuenta.cuentaLogeada.cuenta;
    }
    
    private Boolean VerificarIdValido()
    {
        return txtBoxIdSala.text != "";
    }
}
