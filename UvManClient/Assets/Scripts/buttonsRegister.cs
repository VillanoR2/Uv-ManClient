using LogicaDelNegocio.Modelo;
using MessageService.Dominio.Enum;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de manejar los elementos de la escena RegisterScreen
/// </summary>
public class buttonsRegister : MonoBehaviour
{
    public InputField TFNombreUsuario;
    public InputField TFContrasena;
    public InputField TFCorreo;
    public InputField TFConfirmacionContrasena;

    /// <summary>
    /// Valida que el texto no este vacio, que no contenga espacios y que contenga menos de 50 caracteres
    /// </summary>
    /// <param name="TextoAValidar">String</param>
    /// <returns>True si el texto es valido o false si no</returns>
    private Boolean ValidarCampoNombreDeUsuario(string TextoAValidar)
    {
        bool CamposValidos = TextoAValidar != String.Empty && !TextoAValidar.Contains(" ") && TextoAValidar.Length < 50;
        return CamposValidos;
    }

    /// <summary>
    /// Valida que la contraseña sea valida (no este vacia, y contenga menos de 50 cararcteres)
    /// </summary>
    /// <param name="Contraseña">String</param>
    /// <returns>True si la contraseña es valida o false si no</returns>
    private Boolean ValidarContrasena(String Contraseña)
    {
        Boolean ContraseñaValida = Contraseña != String.Empty && Contraseña.Length < 50;
        return ContraseñaValida;
    }

    /// <summary>
    /// Valida que las dos contraseñas coincidan
    /// </summary>
    /// <param name="Contraseña">String</param>
    /// <param name="ConfirmacionContraseña">String</param>
    /// <returns>True si las contraseñas coinciden, false si no</returns>
    private Boolean ValidarContrasenasConinciden(String Contraseña, String ConfirmacionContraseña)
    {
        Boolean CoincidenContraseñas = false;
        if(ConfirmacionContraseña != String.Empty)
        {
            CoincidenContraseñas = Contraseña == ConfirmacionContraseña;
        }
        return CoincidenContraseñas;
    }

    /// <summary>
    /// Valida que el formato del correo electronico sea valido
    /// </summary>
    /// <param name="CorreoElectronico">String</param>
    /// <returns>True si el formato del correo es valido, false si no</returns>
    private Boolean ValidarCorreoElectronico(String CorreoElectronico)
    {
        String Expresion;
        Expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (Regex.IsMatch(CorreoElectronico, Expresion))
        {
            if (Regex.Replace(CorreoElectronico, Expresion, String.Empty).Length == 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Valida que los campos del formulario del registro sean validos
    /// </summary>
    /// <param name="NombreDeUsuario">String</param>
    /// <param name="Contrasena">String</param>
    /// <param name="ConfirmacionContraseña">String</param>
    /// <param name="CorreoElectronico">String</param>
    /// <returns>True si todos los campos son validos, false si no</returns>
    private Boolean ValidarCamposCorrectos(String NombreDeUsuario, String Contrasena, String ConfirmacionContraseña,
        String CorreoElectronico)
    {
        Boolean todoValido = true;
        if (!ValidarCampoNombreDeUsuario(NombreDeUsuario))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Usuario invalido");
            todoValido = false;
        }
        if (!ValidarContrasena(Contrasena))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Contraseña invalida");
            todoValido = false;
        }
        if (!ValidarContrasenasConinciden(Contrasena, ConfirmacionContraseña))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Contraseñas no coinciden");
            todoValido = false;
        }
        if (!ValidarCorreoElectronico(CorreoElectronico))
        {
            //PonerCamposEnRojo
            Debug.LogWarning("Correo invalido");
            todoValido = false;
        }
        return todoValido;
    }

    /// <summary>
    /// Registra una nueva cuenta
    /// </summary>
    public void Registrarse()
    {
        String nombreDeusuario = TFNombreUsuario.text;
        String contrasena = TFContrasena.text;
        String confirmacion = TFConfirmacionContrasena.text;
        String correoElectronico = TFCorreo.text;
        if (ValidarCamposCorrectos(nombreDeusuario, contrasena, confirmacion, correoElectronico))
        {
            CuentaModel cuentaARegistrar = CrearCuentaARegistrar();
            CuentaCliente.clienteDeCuenta.ReiniciarServicio();
            EnumEstadoRegistro estadoDelRegistro = CuentaCliente.clienteDeCuenta.servicioDeCuenta.Registrarse(cuentaARegistrar);
            Debug.Log(estadoDelRegistro);
            switch (estadoDelRegistro)
            {
                case EnumEstadoRegistro.RegistroCorrecto:
                    RegistroExitoso(cuentaARegistrar);
                    break;
                case EnumEstadoRegistro.UsuarioExistente:
                    break;
                case EnumEstadoRegistro.ErrorEnBaseDatos:
                    break;
            }
        }
    }

    /// <summary>
    /// Asigna la cuenta a verificar y cambia a la escena VerificationScreen
    /// </summary>
    /// <param name="cuentaRegistrada"></param>
    private void RegistroExitoso(CuentaModel cuentaRegistrada)
    {
        CuentaCliente.clienteDeCuenta.cuentaAVerificar = cuentaRegistrada;
        SceneManager.LoadScene("VerificacionScreen");
    }

    /// <summary>
    /// Crea una CuentaModel a partir de la información de los campos del formulario de registro
    /// </summary>
    /// <returns></returns>
    private CuentaModel CrearCuentaARegistrar()
    {
        CuentaModel CuentaARegistrar = new CuentaModel();
        CuentaARegistrar.NombreUsuario = TFNombreUsuario.text;
        CuentaARegistrar.Contrasena = TFContrasena.text;
        CuentaARegistrar.CorreoElectronico = TFCorreo.text;
        return CuentaARegistrar;
    }

    /// <summary>
    /// Cambia a la escena de LoginScreen
    /// </summary>
    public void RegresarAlLogin()
    {
        SceneManager.LoadScene("LoginScreen");
    }
}
