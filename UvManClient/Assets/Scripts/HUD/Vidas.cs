using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Vidas : MonoBehaviour
{
    private int VidaActual = 3;
    public Text MarcadorVidas;
    public Image VidaImg;

    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "PerderVida");
        ActualizarMarcador();
        VidaImg.GetComponent<Animator>().SetBool("Breath", false);

    }

    void PerderVida(Notification Notification)
    {
        int DecrementarVida = (int)Notification.data;
        VidaActual = VidaActual - DecrementarVida;
        if (VidaActual == 0)
        {
            Invoke("GameOver", 1.9f);
        }
        else
        {
            ActualizarMarcador();
            NotificationCenter.DefaultCenter().PostNotification(this, "AunVidas", VidaActual);
            
        }


    }

    void ActualizarMarcador()
    {
        MarcadorVidas.text = VidaActual.ToString();
        VidaImg.GetComponent<Animator>().SetBool("Breath", true);
    }

    void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
