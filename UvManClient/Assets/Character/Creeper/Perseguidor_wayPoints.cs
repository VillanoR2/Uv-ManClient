using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perseguidor_wayPoints : MonoBehaviour
{
    public Transform[] waypoints;
    int cur = 0;
    public float speed = 0.3f;
    void FixedUpdate()
    {
        if (transform.position != waypoints[cur].position){
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position,speed);
            GetComponent<Rigidbody2D>().MovePosition(p);
        }
        else
        { 
            cur = (cur + 1) % waypoints.Length;
        }

        Vector2 mov = waypoints[cur].position - transform.position;
        GetComponent<Animator>().SetFloat("MovX", mov.x);
        GetComponent<Animator>().SetFloat("MovY", mov.y);
        GetComponent<Animator>().SetBool("Walking", true);
    }

    void OnTriggerEnter2D(Collider2D co){
        if(co.tag == "Player")
        { 
            Destroy(co.gameObject);
        }
    }
}
