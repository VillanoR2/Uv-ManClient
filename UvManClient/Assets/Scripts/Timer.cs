using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text Tempo;
    public float Tiempo = 300.0f;
    public bool DebeAumentar = false;

    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "AunVidas");
    }
    void FixedUpdate()
    {

        DebeAumentar = (Tiempo <= 0.0f) ? true : false;

        if (DebeAumentar)
        {
            Tiempo += Time.deltaTime;
        }
        else
        {
            Tiempo -= Time.deltaTime;
        }

        Tempo.color = (Tiempo <= 40.0f) ? Color.red : Color.black;

        Tempo.text = "Tiempo:" + " " + Tiempo.ToString("f0");

        if (Tiempo <= 0.0f)
        {
            NotificationCenter.DefaultCenter().PostNotification(this, "PerderVidaTiempo", Tiempo);
        }

    }

    void AunVidas(Notification Notification)
    {
        int Vidas = (int)Notification.data;
        if (Vidas > 0)
        {
            Tiempo = 300.0f;
        }
        else
        {
            NotificationCenter.DefaultCenter().PostNotification(this, "PerderVida", Vidas);
        }
    }

}

