using LogicaDelNegocio.Modelo;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Se encarga de los elementos de la escena de MainScreen
/// </summary>
public class buttonsMainMenu : MonoBehaviour
{
    private CuentaModel cuenta;

    /// <summary>
    /// Metodo predefinido de UNITY que se encarga de ejecutar todo cuando inicia una escena
    /// </summary>
    void Start()
    {
        RecuperarCuentaLogeada();    
    }

    /// <summary>
    /// Recupera la informacion de la cuenta logeada
    /// </summary>
    private void RecuperarCuentaLogeada()
    { 
        cuenta = Cuenta.CuentaLogeada.CuentaM;
    }
    
    /// <summary>
    /// Cambia a la escena SingleScreen
    /// </summary>
    public void NewGame(){
        SceneManager.LoadScene("SingleScreen");
    }
    
    /// <summary>
    /// Cambia a la escena JoinLobby
    /// </summary>
    public void Multiplayer()
    {
        SceneManager.LoadScene("JoinLobby");
    }
    
    /// <summary>
    /// Cierra el programa
    /// </summary>
    public void Quit()
    {
        SessionCliente.clienteDeSesion.servicioDeSesion.CerrarSesion(cuenta);
        #if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
    }
}
