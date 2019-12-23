using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// La clase Puntuacion se encarga de manejar el incremento de puntos que el jugador acumule en su partida.
/// </summary>
public class Puntuacion : MonoBehaviour
{
    int PuntuacionActual = 0;
    public Text Marcador;

    /// <summary>
    /// El metodo Start es un metodo predefinido por UNITY que su funcion es realizar todo lo que se encuentre dentro 
    /// de el al iniciar la ejecucion por primera vez del proyecto. Este metodo no recibe ningun parametro y es de tipo void.
    /// </summary>    
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this, "IncrementarPuntos");
        ActualizarMarcador();
        DontDestroyOnLoad(GameObject.Find("AlmacenPuntaje"));
    }
    /// <summary>
    /// El metodo IncremetarPuntos calcula en incremento de los puntos ganados y los actualiza en el marcador del HUD del jugador.
    /// <paramref name="Notification">Tipo de parametro Notification<paramref/>
    /// </summary>
    void IncrementarPuntos(Notification Notification)
    {
        int PuntosAIncrementar = (int)Notification.data;
        PuntuacionActual += PuntosAIncrementar;
        ActualizarMarcador();
    }

    /// <summary>
    /// El metodo ActualizarMarcador solo cambia el valor del texto del objeto Marcador por la puncuacion actual del jugador.
    /// </summary>
    void ActualizarMarcador()
    {
        Marcador.text = "Puntaje: " + PuntuacionActual.ToString();
        GameObject.Find("AlmacenPuntaje").GetComponent<Text>().text = PuntuacionActual.ToString();

    }

}
