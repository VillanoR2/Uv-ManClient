using LogicaDelNegocio.Modelo;
using MessageService.Dominio.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de manejar los elementos de la escena de VerifyAccountScreen
/// </summary>
public class buttonsVerificationAccount : MonoBehaviour
{
    public InputField IFCodigoVerificacion;
    private CuentaModel CuentaAVerificar;
    public GameObject PanelException;

    /// <summary>
    /// Recupera la información de la cuenta a verificar
    /// </summary>
    private void RecuperarCuentaAVerificar()
    {
        CuentaAVerificar = CuentaCliente.clienteDeCuenta.cuentaAVerificar;
    }

    /// <summary>
    /// Metodo propio de UNITY que ejecuta lo que se encuentra dentro de el
    /// </summary>
    void Start()
    {
        RecuperarCuentaAVerificar();
        VerificarCuentaAVerificarValida();
    }

    /// <summary>
    /// Solicita al servicio de sesion que verfique la cuenta con el codigo introducido
    /// </summary>
    public void VerificarCuenta()
    {
        string codigoRecuperado = IFCodigoVerificacion.text;
        if (codigoRecuperado.Length == 10)
        {
            CuentaCliente.clienteDeCuenta.ReiniciarServicio();
            EnumEstadoVerificarCuenta SeVerificoCorrectamente = CuentaCliente.clienteDeCuenta.servicioDeCuenta.VerificarCuenta(codigoRecuperado, CuentaAVerificar);
            if (SeVerificoCorrectamente == EnumEstadoVerificarCuenta.VerificadaCorrectamente)
            {
                Debug.LogWarning("Cuenta Verificada Correctamente");
                PanelException.SetActive(true);
                PanelException.GetComponentInChildren<Text>().text = "Se ha verificado correctamente la cuenta.";
            }
            else if (SeVerificoCorrectamente == EnumEstadoVerificarCuenta.NoCoincideElCodigo)
            {
                Debug.LogWarning("El código no coincide");
                PanelException.SetActive(true);
                PanelException.GetComponentInChildren<Text>().text = "Lo sentimos, el código ingresado no coincide con el código de verificación.";
            }
            else
            {
                Debug.LogWarning("Cuenta No Verificada Correctamente");
                PanelException.SetActive(true);
                PanelException.GetComponentInChildren<Text>().text = "Se encontro un error en nuestra base de datos, porfavor intente más tarde y si el problema persiste reinicie el juego.";
            }
        }
        else
        {
            Debug.LogWarning("Formato incorrecto");
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = "Formato Incorrecto. Debe contener solo 10 números.";
        }
    }

    /// <summary>
    /// Regresa a la pantalla de login si la cuenta a verificar es invalida
    /// </summary>
    private void VerificarCuentaAVerificarValida()
    {
        if (CuentaAVerificar == null)
        {
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = "Regresando a la pantalla de login, Su cuenta es Invalida.";
            SceneManager.LoadScene("LoginScreen");
        }
    }

    /// <summary>
    /// Regresa a la escena de login
    /// </summary>
    public void RegresarLoginScreen()
    {
        SceneManager.LoadScene("LoginScreen");
    }

    /// <summary>
    /// Reenvia el codigo de verficacion a la cuenta a verificar
    /// </summary>
    public void ReenviarCodigo()
    {
        Debug.LogWarning("Reenviando código...");
        PanelException.SetActive(true);
        PanelException.GetComponentInChildren<Text>().text = "Se ha reenviado el código de verificación, favor de revisar su correo.";
        CuentaCliente.clienteDeCuenta.servicioDeCuenta.ReEnviarCorreoVerificacion(CuentaAVerificar);
    }
}
