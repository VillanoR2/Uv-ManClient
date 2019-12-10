using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UvCoinLogic : MonoBehaviour
{
    public int puntosGanados = 10;
    public AudioClip coinSound;
    public float volumeSound = 1f;

    void Update(){

    }
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
