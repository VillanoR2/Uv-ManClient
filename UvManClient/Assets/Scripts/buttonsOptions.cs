using LogicaDelNegocio.Modelo;
using MessageService.Dominio.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsOptions : MonoBehaviour
{
    public InputField IFDireccionIp;
    string direccionIP;

    private void RecuperarDireccionIP()
    {
        direccionIP = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    public void Guardar()
    {
        direccionIP = IFDireccionIp.text;
        SessionCliente.clienteDeSesion.direccionIpDelServidor = direccionIP;
        SessionCliente.clienteDeSesion.Guardar();
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("LoginScreen");
    }

    private void Start()
    {
        RecuperarDireccionIP();
        IFDireccionIp.text = direccionIP;
    }
}
