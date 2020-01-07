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
    public GameObject PanelException;

    /// <summary>
    /// Metodo que valida que el segundo valor coincida con el primer valor
    /// </summary>
    /// <param name="CadenaValida">String</param>
    /// <param name="CadenaIngresada">String</param>
    /// <returns>True si las cadenas coinciden, false si no</returns>
    public bool ValidarRegex(String CadenaValida, String CadenaIngresada)
    {
        bool Resultado;
        Regex Regex = new Regex(CadenaValida);
        if (Regex.IsMatch(CadenaIngresada))
        {
            Resultado = true;
        }
        else
        {
            Resultado = false;
        }
        return Resultado;
    }


    /// <summary>
    /// Valida que el texto no este vacio, que no contenga espacios y que contenga menos de 20 caracteres
    /// </summary>
    /// <param name="TextoAValidar">String</param>
    /// <returns>True si el texto es valido o false si no</returns>
    private Boolean ValidarCampoNombreDeUsuario(string TextoAValidar)
    {
        bool Resultado= false;

        if (TextoAValidar.Length <= 30)
        {
            Debug.Log("Nombre supera el limite.");
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = "El nombre de usuario debe tener como maximo 30 Carácteres";
        }
        else
        {

            String FormatoTextoValido = @"^[A-Za-z0-9]+(?:[ _-][A-Za-z0-9]+)*$";
            Resultado = ValidarRegex(FormatoTextoValido, TextoAValidar);

        }
        return Resultado;

    }

    /// <summary>
    /// Valida que la contraseña sea valida (no este vacia, y contenga menos de 18 cararcteres)
    /// </summary>
    /// <param name="Contraseña">String</param>
    /// <returns>True si la contraseña es valida o false si no</returns>
    private Boolean ValidarContrasena(String Contraseña)
    {
        bool Resultado;
        String FormatoTextoValido = @"^(?=\w*\d)(?=\w*[A-Z])(?=\w*[a-z])\S{8,16}$";
        Resultado = ValidarRegex(FormatoTextoValido, Contraseña);
        return Resultado;
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
        if (ConfirmacionContraseña == Contraseña)
        {
            CoincidenContraseñas = true;
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
        bool Resultado;
        String FormatoTextoValido = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        Resultado = ValidarRegex(FormatoTextoValido, CorreoElectronico);
        return Resultado;
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
        string CadenaErrores = "";
        if (!ValidarCampoNombreDeUsuario(NombreDeUsuario))
        {
            Debug.LogWarning("Usuario invalido");
            todoValido = false;
            CadenaErrores += "Usuario Invalido\n";
        }
        if (!ValidarContrasena(Contrasena))
        {
            Debug.LogWarning("Contraseña invalida");
            todoValido = false;
            CadenaErrores += "Contraseña Invalido: la contraseña debe contener almenos una mayúscula, una minuscula, un numero, un simbolo entre(! ? % $ &) y tener de 8 a 16 caracteres\n";
        }
        if (!ValidarContrasenasConinciden(Contrasena, ConfirmacionContraseña))
        {
            Debug.LogWarning("Contraseñas no coinciden");
            todoValido = false;
            CadenaErrores += "Contraseñas no coinciden: ambas contraseñas debe coincidir \n";
        }
        if (!ValidarCorreoElectronico(CorreoElectronico))
        {
            Debug.LogWarning("Correo invalido");
            todoValido = false;
            CadenaErrores += "Correo Invalido\n";
        }
        if (todoValido != true)
        {
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = CadenaErrores;
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
                    PanelException.SetActive(true);
                    PanelException.GetComponentInChildren<Text>().text = "Usuario ya existe.";
                    break;
                case EnumEstadoRegistro.ErrorEnBaseDatos:
                    PanelException.SetActive(true);
                    PanelException.GetComponentInChildren<Text>().text = "Imposible conectar a la base de datos, intente más tarde..";
                    break;
                default:
                    PanelException.SetActive(true);
                    PanelException.GetComponentInChildren<Text>().text = "Imposible conectar al servidor, intente más tarde..";
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
        Debug.Log("Registro Exito.");
        PanelException.SetActive(true);
        PanelException.GetComponentInChildren<Text>().text = "Registro Exitoso";
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
