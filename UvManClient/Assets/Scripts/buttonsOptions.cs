using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Se encarga de manejar las acciones de la pantalla de opciones
/// </summary>
public class buttonsOptions : MonoBehaviour
{
    public InputField IFDireccionIp;
    string direccionIP;
    public GameObject Panel;
    int counter = 0;

    /// <summary>
    /// Recupera la dirección ip del servidor
    /// </summary>
    private void RecuperarDireccionIP()
    {
        direccionIP = SessionCliente.clienteDeSesion.direccionIpDelServidor;
    }

    /// <summary>
    /// Guarda la dirección Ip en un archivo
    /// </summary>
    public void Guardar()
    {
        direccionIP = IFDireccionIp.text;
        SessionCliente.clienteDeSesion.direccionIpDelServidor = direccionIP;
        SessionCliente.clienteDeSesion.Guardar();
    }

    /// <summary>
    /// Regresa a la pantalla de login
    /// </summary>
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
