using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Transform target;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
            );

        if(target == null){
            Debug.Log("Player ha muerto");
        }
    }

}
