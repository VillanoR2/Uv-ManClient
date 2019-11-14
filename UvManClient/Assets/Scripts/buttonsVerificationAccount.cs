using LogicaDelNegocio.Modelo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsVerificationAccount : MonoBehaviour
{
    public InputField IFCodigoVerificacion;
    CuentaModel cuentaAVerificar;

    // Start is called before the first frame update

    private void RecuperarCuentaAVerificar()
    {
        cuentaAVerificar = CuentaCliente.clienteDeCuenta.cuentaAVerificar;
    }

    void Start()
    {
        RecuperarCuentaAVerificar();
        VerificarCuentaAVerificarValida();
    }

    public void VerificarCuenta()
    {
        string codigoRecuperado = IFCodigoVerificacion.text;
        if (codigoRecuperado.Length == 10)
        {
            Debug.Log(CuentaCliente.clienteDeCuenta.servicioDeCuenta.VerifyAccount(codigoRecuperado, cuentaAVerificar));
        }
        else
        {
            Debug.LogWarning("Formato incorrecto");
            //Mostrar mensaje de como debe de estar el formato
        }
    }

    private void VerificarCuentaAVerificarValida()
    {
        if (cuentaAVerificar == null)
        {
            //Mostrar mensaje de error al realizar el registro
            SceneManager.LoadScene("LoginScreen");
        }
    }

    public void RegresarLoginScreen()
    {
        SceneManager.LoadScene("LoginScreen");
    }

    //private void ReenviarCodigo()
    //{
    //    CuentaCliente.clienteDeCuenta.servicioDeCuenta.
    //}
}
