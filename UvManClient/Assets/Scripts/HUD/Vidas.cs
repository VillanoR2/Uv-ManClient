using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vidas : MonoBehaviour
{
    private int VidaActual = 3;
    public Text MarcadorVidas;
    public Image VidaImg;

    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "PerderVida");
        ActualizarMarcador();
        VidaImg.GetComponent<Animator>().SetBool("Breath",false);
        
    }

    void PerderVida(Notification Notification)
    {
        int DecrementarVida = (int)Notification.data;
        VidaActual = VidaActual - DecrementarVida;
        ActualizarMarcador();
    }

    void ActualizarMarcador()
    {
        MarcadorVidas.text = VidaActual.ToString();
        VidaImg.GetComponent<Animator>().SetBool("Breath",true);
    }
    void Update()
    {

    }
}
