using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    Transform target;
    float tLX, tLY, bRX, bRY;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(target.position.x,tLX,bRX),
            Mathf.Clamp(target.position.y,bRY,tLY),
            transform.position.z
            );
    }

    public void setBound(GameObject map){
        float cameraSize = Camera.main.orthographicSize;
        
        tLX = map.transform.position.x + cameraSize;
        tLY = map.transform.position.y - cameraSize;
        bRX = map.transform.position.x + 500 - cameraSize;
        bRY = map.transform.position.y - 354 + cameraSize; 

    }
}
