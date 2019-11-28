using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UvCoinLogic : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.name == "Link_Player")
        {
            Destroy(gameObject);
        }
    }
}
