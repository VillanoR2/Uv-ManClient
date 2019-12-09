using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickAxLogic : MonoBehaviour
{
	public int puntosGanados = 100;
    public AudioClip Sound;
    public float volumeSound = 1f;
    
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
