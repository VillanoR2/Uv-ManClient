using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickAxLogic : MonoBehaviour
{
	public int puntosGanados = 500;
    public AudioClip Sound;
    public float volumeSound = 1f;
    
    /// <summary>
    /// El metodo OnTriggerEnter2D es un metodo predefinido por UNITY que su funcion es manejar la accion disparador cuando dos coliciones se encuentran.Es un metodo de tipo void
    /// <paramref name="Collider">Es un parametro de tipo Collider2D <paramref/>
    /// </summary>
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(Sound, Camera.main.transform.position, volumeSound);
            NotificationCenter.DefaultCenter().PostNotification(this, "IncrementarPuntos", puntosGanados);
            Destroy(gameObject);
        }
    }
}
