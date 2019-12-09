using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntuacion : MonoBehaviour
{
    private int PuntuacionActual = 0;
    public Text Marcador;
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this,"IncrementarPuntos");
        ActualizarMarcador();      
    }

    void IncrementarPuntos(Notification Notification){
        int PuntosAIncrementar = (int)Notification.data;
        PuntuacionActual += PuntosAIncrementar;
        ActualizarMarcador();
    }

    void ActualizarMarcador(){
        Marcador.text = "Puntaje: " + PuntuacionActual.ToString();
    }

    void Update(){

    }
}
