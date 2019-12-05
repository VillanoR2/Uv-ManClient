using LogicaDelNegocio.Modelo;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonsMainMenu : MonoBehaviour
{
    private CuentaModel cuenta;

    void Start()
    {
        RecuperarCuentaLogeada();    
    }

    private void RecuperarCuentaLogeada()
    {
        cuenta = Cuenta.cuentaLogeada.cuenta;
    }
    
    public void NewGame(){
        SceneManager.LoadScene("SingleScreen");
    }
    public void Multiplayer()
    {
        ChatServiceClient clienteDeChat = ChatCliente.clienteDeChat.servicioDeChat;
        if(cuenta != null)
        {
            Boolean estadodelaConexion = clienteDeChat.Conectar(cuenta);
            Debug.Log(estadodelaConexion);
            if (estadodelaConexion){
                SceneManager.LoadScene("Lobby");
            }
        }
        else
        {
            //Aqui poner mensaje de no se puede acceder al servicio de chat
        }
    }

    public void Option(){
         SceneManager.LoadScene("OptionScreen");

    }

    public void Quit()
    {
        SessionCliente.clienteDeSesion.servicioDeSesion.CerrarSesion(cuenta);
        #if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
    }
}
