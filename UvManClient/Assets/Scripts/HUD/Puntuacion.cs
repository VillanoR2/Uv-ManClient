using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntuacion : MonoBehaviour
{
    int PuntuacionActual = 0;
    public Text Marcador;
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this,"IncrementarPuntos");
        ActualizarMarcador();  
        DontDestroyOnLoad(GameObject.Find("AlmacenPuntaje"));    
    }

    void IncrementarPuntos(Notification Notification){
        int PuntosAIncrementar = (int)Notification.data;
        PuntuacionActual += PuntosAIncrementar;
        ActualizarMarcador();
    }

    void ActualizarMarcador(){
        Marcador.text = "Puntaje: " + PuntuacionActual.ToString();
        GameObject.Find("AlmacenPuntaje").GetComponent<Text>().text = PuntuacionActual.ToString();
        
    }

}
