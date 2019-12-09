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
        SceneManager.LoadScene("JoinLobby");
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
