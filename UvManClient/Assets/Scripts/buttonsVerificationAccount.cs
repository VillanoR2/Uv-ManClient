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
            EnumEstadoVerificarCuenta SeVerificoCorrectamente =
                CuentaCliente.clienteDeCuenta.servicioDeCuenta.VerificarCuenta(codigoRecuperado, CuentaAVerificar);
            //Mostrar enum de se verifico correctamente
        }
        else
        {
            Debug.LogWarning("Formato incorrecto");
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = "Formato Incorrecto.\nDebe contener solo números.";
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
        CuentaCliente.clienteDeCuenta.servicioDeCuenta.ReEnviarCorreoVerificacion(CuentaAVerificar);
    }
}
