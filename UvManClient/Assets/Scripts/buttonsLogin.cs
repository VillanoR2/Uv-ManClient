using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using LogicaDelNegocio.Modelo;
using SessionService.Dominio.Enum;

/// <summary>
/// Se encarga de controlar los elementos que se encuentran en la escena Login
/// </summary>
public class buttonsLogin : MonoBehaviour
{

    public InputField Usuario;
    public InputField Contrasena;
    public GameObject PanelException;

    /// <summary>
    /// Recupera los datos de los campos
    /// </summary>
    /// <returns>Una CuentaModel con la información de los campos</returns>
    private CuentaModel RecuperarDatosDeLogin()
    {
        CuentaModel cuentaParaIniciarSesion = new CuentaModel();
        String Nombre = Usuario.text;
        String Password = Contrasena.text;
        cuentaParaIniciarSesion.NombreUsuario = Nombre;
        cuentaParaIniciarSesion.Contrasena = Password;
        return cuentaParaIniciarSesion;
    }

    /// <summary>
    /// Le solcita al servicio de sesion logearse con los datos de los campos
    /// </summary>
    public void ButtonLogIn()
    {
        try
        {
            SessionServiceClient clienteSesion = SessionCliente.clienteDeSesion.servicioDeSesion;
            if (CamposDeLogeoValidos())
            {
                SessionCliente.clienteDeSesion.ReiniciarServicio();
                CuentaModel cuentaIniciarSesion = RecuperarDatosDeLogin();
                EnumEstadoInicioSesion estadoDeInicioDeSesion = clienteSesion.IniciarSesion(cuentaIniciarSesion);
                switch (estadoDeInicioDeSesion)
                {
                    case EnumEstadoInicioSesion.Correcto:
                        Debug.Log("Credenciales Validas, ingreso exitoso.");
                        InicioDeSesionCorrecto(cuentaIniciarSesion);
                        break;
                    case EnumEstadoInicioSesion.CredencialesInvalidas:
                        Debug.Log("Credenciales Invalidas, ingrese una validas porfavor.");
                        PanelException.SetActive(true);
                        PanelException.GetComponentInChildren<Text>().text = "Credenciales Invalidas, ingrese una validas porfavor";
                        break;
                    case EnumEstadoInicioSesion.CuentaYaLogeada:
                        Debug.Log("La cuenta ya se encuentra logeada.");
                        PanelException.SetActive(true);
                        PanelException.GetComponentInChildren<Text>().text = "La cuenta ya se encuentra logeada.";
                        break;
                    case EnumEstadoInicioSesion.ErrorBD:
                        Debug.Log("Error en Base de Datos.");
                        PanelException.SetActive(true);
                        PanelException.GetComponentInChildren<Text>().text = "Surgio un Error, porfavor intente más tarde";
                        break;
                    case EnumEstadoInicioSesion.CuentaNoVerificada:
                        Debug.Log("Cuenta No Verificada.");
                        PanelException.SetActive(true);
                        PanelException.GetComponentInChildren<Text>().text = "Su cuenta no está verificada aun, porfavor verifiquela.";
                        CuentaNoVerificada(cuentaIniciarSesion);
                        break;
                }
            }
        }
        catch (Exception) //Verificar el tipo de excepcion (creo que es SocketException)
        {

        }
    }

    /// <summary>
    /// Cambia a la escena de RegisterScreen
    /// </summary>
    public void ButtonRegister()
    {
        SceneManager.LoadScene("RegisterScreen");
    }

    /// <summary>
    /// Cambia a la escena de OptionScreen
    /// </summary>
    public void ButtonOptions()
    {
        SceneManager.LoadScene("OptionScreen");
    }

    /// <summary>
    /// Guarda la infomacion de la cuenta logeada y cambia a la escena de MainScreen
    /// </summary>
    /// <param name="cuentaIniciarSesion">CuentaModel</param>
    private void InicioDeSesionCorrecto(CuentaModel cuentaIniciarSesion)
    {
        Cuenta.CuentaLogeada.CuentaM = cuentaIniciarSesion;
        SceneManager.LoadScene("MainScreen");
    }

    /// <summary>
    /// Cambia a la escena de VerificationScreen con la cuenta no verificada
    /// </summary>
    /// <param name="CuentaAVaidar">CuentaModel</param>
    private void CuentaNoVerificada(CuentaModel CuentaAVaidar)
    {
        CuentaCliente.clienteDeCuenta.cuentaAVerificar = CuentaAVaidar;
        SceneManager.LoadScene("VerificacionScreen");
    }

    /// <summary>
    /// Verifica que los campos de logeo sean validos (no contengan espacios (nombreUsuario) y no sean vacios)
    /// </summary>
    /// <returns>True si son validos o false si no</returns>
    private Boolean CamposDeLogeoValidos()
    {
        bool CamposDeLogeoNoVacios = false;
        String Nombre = Usuario.text;
        String Password = Contrasena.text;
        if (Nombre != String.Empty && Password != String.Empty)
        {
            CamposDeLogeoNoVacios = true;
        }
        else
        {
            Debug.Log("Error, campos vacios.");
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = "Los campos se encuentran vacios, favor de llenar los campos necesarios.";
        }
        return CamposDeLogeoNoVacios;
    }
}
