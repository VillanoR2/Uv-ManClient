using LogicaDelNegocio.Modelo;
using MessageService.Dominio.Enum;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonsOptions : MonoBehaviour
{
    public InputField IFDireccionIp;
    string direccionIP;
    public GameObject Panel;
    int counter = 0;

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

    public void ShowPanel()
    { 
        counter++;
        if(counter % 2 == 1 ){
            Panel.gameObject.SetActive(true);
        }
        else{
            Panel.gameObject.SetActive(false);
        }

    }
    
}
