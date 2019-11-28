using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using LogicaDelNegocio.Modelo;
using SessionService.Dominio.Enum;

public class buttonsLogin : MonoBehaviour
{

    public InputField usuario;
    public InputField contrasena;
    public CuentaModel cuentaLogeada;


    private CuentaModel RecuperarDatosDeLogin()
    {
        CuentaModel cuentaParaIniciarSesion = new CuentaModel();
        String nombre = usuario.text;
        String password = contrasena.text;
        cuentaParaIniciarSesion.NombreUsuario = nombre;
        cuentaParaIniciarSesion.Contrasena = password;
        return cuentaParaIniciarSesion; 
    }

    public void ButtonLogIn()
    {
        // try
        {
            SessionServiceClient clienteSesion = SessionCliente.clienteDeSesion.servicioDeSesion;
            if (CamposDeLogeoNoVacios())
            {
                CuentaModel cuentaIniciarSesion = RecuperarDatosDeLogin();
                SessionCliente.clienteDeSesion.AsegurarLaInformacion(cuentaIniciarSesion.NombreUsuario, cuentaIniciarSesion.Contrasena);
                
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
            
            //}
            //catch (Exception ex)
            //{
            //    Debug.Log(ex.GetType());
            //}
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

    private Boolean CamposDeLogeoNoVacios()
    {
        String nombre = usuario.text;
        String password = contrasena.text;
        if (nombre != "" && nombre != "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
