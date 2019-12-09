using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UvCoinLogic : MonoBehaviour
{
	public int puntosGanados = 5;
    public AudioClip coinSound;
    public float volumeSound = 1f;
    
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position, volumeSound);
            NotificationCenter.DefaultCenter().PostNotification(this, "IncrementarPuntos", puntosGanados);
            Destroy(gameObject);
        }
    }
}
