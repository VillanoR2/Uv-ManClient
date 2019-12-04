using System;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsCreateLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego;
    private CuentaModel CuentaLogeada;
    public InputField txtBoxIdSala;
    public Toggle toggleEsSalaPrivada;
    
    
    private void InicializarServicio()
    {
        ClienteDelJuego = JuegoCliente.ClienteDelJuego;
        RecuperarCuentaLogeada();
    }

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
        ChatCliente.clienteDeChat.ReiniciarServicio();
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        SeUnioCorrectamenteAlChat = clienteDeChat.Conectar(CuentaLogeada); 
        return SeUnioCorrectamenteAlChat;
    }
    
    private EnumEstadoCrearSalaConId UnirseAlServicioDeJuego(String id, Boolean esPrivada)
    {
        ClienteDelJuego.ReinciarClienteDeJuego();
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
