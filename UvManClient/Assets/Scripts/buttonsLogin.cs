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
    
    /// <summary>
    /// Recupera los datos de los campos
    /// </summary>
    /// <returns>Una CuentaModel con la información de los campos</returns>
    private CuentaModel RecuperarDatosDeLogin()
    {
        CuentaModel cuentaParaIniciarSesion = new CuentaModel();
        String nombre = Usuario.text;
        String password = Contrasena.text;
        cuentaParaIniciarSesion.NombreUsuario = nombre;
        cuentaParaIniciarSesion.Contrasena = password;
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
        Boolean CamposValidos = false;
        String Nombre = Usuario.text;
        String Contraseña = Contrasena.text;
        if (Nombre != "" && Contraseña != "")
        {
            if (!Nombre.Contains(" ") && Nombre.Length < 50 && Contraseña.Length < 50)
            {
                CamposValidos = true;
            }
        }
        return CamposValidos;
    }
}
