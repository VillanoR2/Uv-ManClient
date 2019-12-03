﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perseguidor_wayPoints : MonoBehaviour
{

    public Transform[] waypoints;
    int cur = 0;
    public float speed = 0.3f;
    public float visionRadius;
    public float attackRadius;
    Vector3 actualPosition;
    GameObject player;
    Animator anim;
    Rigidbody2D rb2D;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        actualPosition = waypoints[cur].position;
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 mov = waypoints[cur].position - transform.position;
        anim.SetFloat("MovX", mov.x);
        anim.SetFloat("MovY", mov.y);
        anim.SetBool("Walking", true);

        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed);
            rb2D.MovePosition(p);

            Vector3 target = actualPosition;

            RaycastHit2D hit = Physics2D.Raycast(transform.position,
            player.transform.position - transform.transform.position,
            visionRadius,
            1 << LayerMask.NameToLayer("Default")
            );

            Vector3 forward = transform.TransformDirection(player.transform.position - transform.position);
            Debug.DrawRay(transform.position, forward, Color.red);

            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    target = player.transform.position;
                }
            }

            float distance = Vector3.Distance(target, transform.position);
            Vector3 dir = (target - transform.position).normalized;

            if (target != actualPosition && distance < attackRadius)
            {
                anim.SetFloat("MovX", dir.x);
                anim.SetFloat("MovY", dir.y);
                anim.Play("Creeper_Walk", -1, 0);
            }
            else
            {
                rb2D.MovePosition(transform.position + dir * speed * Time.deltaTime);
                anim.speed = 1;
                anim.SetFloat("MovX", dir.x);
                anim.SetFloat("MovY", dir.y);
                anim.SetBool("Walking", true);
            }

            if (target == actualPosition && distance < 0.02f)
            {
                transform.position = actualPosition;
                anim.SetBool("Walking", false);
            }

            Debug.DrawLine(transform.position, target, Color.green);
        }
        else
        {
            cur = (cur + 1) % waypoints.Length;
        }

    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "Player")
        {

            Destroy(co.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
}
