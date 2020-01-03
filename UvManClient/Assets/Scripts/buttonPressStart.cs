using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Se encarga de cambiar de escena al presionar el bototn de start
/// </summary>
public class buttonPressStart : MonoBehaviour
{
    /// <summary>
    /// Cambia a la escena de login
    /// </summary>
    public void PlayScene()
    {
        SceneManager.LoadScene("LoginScreen");
    }
}
