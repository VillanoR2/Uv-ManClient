using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntuacion : MonoBehaviour
{
    private int puntuacion = 0;
    public Text marcador;
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this,"IncrementarPuntos");
        ActualizarMarcador();      
    }

    void IncrementarPuntos(Notification notification){
        int puntosAIncrementar = (int)notification.data;
        puntuacion += puntosAIncrementar;
        ActualizarMarcador();
    }

    void ActualizarMarcador(){
        marcador.text = "Puntaje: " + puntuacion.ToString();
    }

    void Update(){

    }
}
