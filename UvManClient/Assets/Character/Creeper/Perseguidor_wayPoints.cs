using System.Collections;
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
    Vector3 target;
    GameObject player;
    Animator anim;
    Rigidbody2D rb2D;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        actualPosition = transform.position;
        anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        //PointPatrol();
            
        TargetFollow();
    }

    void PointPatrol()
    {
        Vector2 mov = waypoints[cur].position - transform.position;
        anim.SetFloat("MovX", mov.x);
        anim.SetFloat("MovY", mov.y);
        anim.SetBool("Walking", true);

        if (transform.position != waypoints[cur].position)
        {
            Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position,speed);
            rb2D.MovePosition(p);
        }
        else
        {
            cur = (cur + 1) % waypoints.Length;
        }

    }
    void TargetFollow()
    {
            target = actualPosition;

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
                    Debug.Log("Te veo...");
                    target = player.transform.position;
                }
            }

            float distance = Vector3.Distance(target, transform.position);
            Vector3 dir = (target - transform.position).normalized;

            if (target != actualPosition && distance < attackRadius)
            {
                Debug.Log("Te ataco");
                anim.SetFloat("MovX", dir.x);
                anim.SetFloat("MovY", dir.y);
                anim.Play("Creeper_Walk", -1, 0);
            }
            else
            {
                Debug.Log("Te sigo...");
                rb2D.MovePosition(transform.position + dir * speed);
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
    

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
