using System;
using System.Collections;
using System.Collections.Generic;
using GameService.Dominio.Enum;
using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsJoinPrivateLobby : MonoBehaviour
{
    private JuegoCliente ClienteDelJuego;
    private CuentaModel CuentaLogeada;
    public InputField txtBoxIdSala;
    
    
    private void InicializarServicio()
    {
        ClienteDelJuego = JuegoCliente.ClienteDelJuego;
        RecuperarCuentaLogeada();
    }

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

    public void RegresarAUnirseASala()
    {
        SceneManager.LoadScene("JoinLobby");
    }
    
    private Boolean UnirseAlServicioDeChat()
    {
        Boolean SeUnioCorrectamenteAlChat;
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        ChatCliente.clienteDeChat.ReiniciarServicio();
        SeUnioCorrectamenteAlChat = clienteDeChat.Conectar(CuentaLogeada); 
        return SeUnioCorrectamenteAlChat;
    }
    
    private EnumEstadoDeUnirseASala UnirseAlServicioDeJuego(String id)
    {
        ClienteDelJuego.ReinciarClienteDeJuego();
        return ClienteDelJuego.ServicioDeJuego.UnirseASalaPrivada(id, CuentaLogeada);
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
