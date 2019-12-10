using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.ServiceModel;
using System.Data;
using System;
using System.Runtime.Serialization;
using LogicaDelNegocio.Modelo;
using SessionService.Dominio.Enum;

public class buttonsLogin : MonoBehaviour
{

    public InputField usuario;
    public InputField contrasena;
    public CuentaModel cuentaLogeada;

    public GameObject ExceptionObject;


    private CuentaModel RecuperarDatosDeLogin()
    {
        CuentaModel cuentaParaIniciarSesion = new CuentaModel();
        String nombre = usuario.text;
        String password = contrasena.text;
        cuentaParaIniciarSesion.nombreUsuario = nombre;
        cuentaParaIniciarSesion.contrasena = password;
        return cuentaParaIniciarSesion; 
    }

    public void ButtonLogIn()
    {
        try
        {
            SessionServiceClient clienteSesion = SessionCliente.clienteDeSesion.servicioDeSesion;

            CuentaModel cuentaIniciarSesion = RecuperarDatosDeLogin();
            EnumEstadoInicioSesion estadoDeInicioDeSesion = clienteSesion.IniciarSesion(cuentaIniciarSesion);
            Debug.Log(estadoDeInicioDeSesion);
            switch (estadoDeInicioDeSesion)
            {
                case EnumEstadoInicioSesion.Correcto:
                    InicioDeSesionCorrecto(cuentaIniciarSesion);
                    break;
                case EnumEstadoInicioSesion.CredencialesInvalidas:
                    //Mostrar mensaje credenciales invalidas
                    break;
                case EnumEstadoInicioSesion.CuentaYaLogeada:
                    //Mostrar mensaje cuenta ya logeada
                    break;
                case EnumEstadoInicioSesion.ErrorBD:
                    break;
                case EnumEstadoInicioSesion.CuentaNoVerificada:
                    CuentaNoVerificada(cuentaIniciarSesion);
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.GetType());
        }
    }

    public void ButtonRegister()
    {
        SceneManager.LoadScene("RegisterScreen");
    }

    private void InicioDeSesionCorrecto(CuentaModel cuentaIniciarSesion)
    {
        Cuenta.cuentaLogeada.cuenta = cuentaIniciarSesion;
        SceneManager.LoadScene("MainScreen");
    }

    private void CuentaNoVerificada(CuentaModel cuentaARegistrar)
    {
        CuentaCliente.clienteDeCuenta.cuentaAVerificar = cuentaARegistrar;
        SceneManager.LoadScene("VerificacionScreen");
    }
}
