using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManageranag : MonoBehaviour
{
    private int Life = 3;
    void Start()
    {
        NotificationCenter.DefaultCenter().AddObserver(this,"PerderVida");
    }

    void PerderVida(Notification notification)
    {

    }
    void Update()
    {
        
    }
}
