using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    Transform Target;
    void Awake()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3(
            Target.position.x,
            Target.position.y,
            transform.position.z
            );

        if (Target == null)
        {
            Debug.Log("Player ha muerto");
        }
    }

}
