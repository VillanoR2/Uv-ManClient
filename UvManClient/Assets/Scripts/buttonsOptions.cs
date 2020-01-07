using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Se encarga de manejar las acciones de la pantalla de opciones
/// </summary>
public class buttonsOptions : MonoBehaviour
{
    public InputField IFDireccionIp;
    string direccionIP;
    public GameObject PanelException;

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
        if (ValidarIp(IFDireccionIp.text) == true)
        {
            Debug.Log("Dirección Actualizada.");
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = "Se ha actualizado la dirección IP correctamente.";
            direccionIP = IFDireccionIp.text;
            SessionCliente.clienteDeSesion.direccionIpDelServidor = direccionIP;
            SessionCliente.clienteDeSesion.Guardar();
        }
        else
        {
            Debug.Log("Dirección IP erronea.");
            PanelException.SetActive(true);
            PanelException.GetComponentInChildren<Text>().text = "Porfavor ingrese una dirección ip valida, el formato es 000.000.000.000";
        }
    }

    /// <summary>
    /// Regresa a la pantalla de login
    /// </summary>
    public void ReturnMenu()
    {
        SceneManager.LoadScene("LoginScreen");
    }
    /// <summary>
    /// Metodo para validar una direccion Ipp
    /// </summary>
    public bool ValidarIp(string DireccionIP)
    {
        string Expresion;
        Expresion = @"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b";
        if (Regex.IsMatch(DireccionIP, Expresion))
        {
            if (Regex.Replace(DireccionIP, Expresion, string.Empty).Length == 0)
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Metodo propio de UNITY que ejecuta lo que se encuentra dentro del metodo al iniciar la escena
    /// </summary>
    private void Start()
    {
        RecuperarDireccionIP();
        IFDireccionIp.text = direccionIP;
    }

    /// <summary>
    /// Muestra el panel de la configuración de idiomas
    /// </summary>
    public void ShowPanel()
    {
        counter++;
        if (counter % 2 == 1)
        {
            Panel.gameObject.SetActive(true);
        }
        else
        {
            Panel.gameObject.SetActive(false);
        }

    }

}
