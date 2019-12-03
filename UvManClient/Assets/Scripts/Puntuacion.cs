using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puntuacion : MonoBehaviour
{
    private int puntuacion = 0;
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this,"IncrementarPuntos");
    }

    void IncrementarPuntos(Notification notification){
        int puntosAIncrementar = (int)notification.data;
        puntuacion+=puntosAIncrementar;
        Debug.Log("Incrementado "+puntosAIncrementar+" puntos. Total ganados: "+puntuacion);
    }
}
