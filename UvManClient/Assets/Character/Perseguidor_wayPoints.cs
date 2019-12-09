using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perseguidor_wayPoints : MonoBehaviour
{

    public Transform[] Waypoints;
    int Point = 0;
    public float Speed = 0.8f;
    public float VisionRadius;
    public float AttackRadius;
    Vector3 ActualPosition;
    Vector3 Target;
    GameObject Player;
    Animator Animator;
    Rigidbody2D RigidBody2D;
    public GameObject Spawn;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ActualPosition = transform.position;
        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        TargetFollow();
    }

    void PointPatrol()
    {
        Vector2 Movement = Waypoints[Point].position - transform.position;
        Animator.SetFloat("MovX", Movement.x);
        Animator.SetFloat("MovY", Movement.y);
        Animator.SetBool("Walking", true);

        if (transform.position != Waypoints[Point].position)
        {

            Vector2 MovementPosition = Vector2.MoveTowards(transform.position, Waypoints[Point].position, Speed);
            RigidBody2D.MovePosition(MovementPosition);
        }
        else
        {
            Point = (Point + 1) % Waypoints.Length;
        }
    }
    void TargetFollow()
    {
        Target = ActualPosition;

        RaycastHit2D Hit = Physics2D.Raycast(transform.position,
        Player.transform.position - transform.transform.position,
        VisionRadius,
        1 << LayerMask.NameToLayer("Default")
        );

        Vector3 Forward = transform.TransformDirection(Player.transform.position - transform.position);
        Debug.DrawRay(transform.position, Forward, Color.red);

        if (Hit.collider != null)
        {
            if (Hit.collider.tag == "Player")
            {
                Target = Player.transform.position;
            }
        }

        float Distance = Vector3.Distance(Target, transform.position);
        Vector3 Direction = (Target - transform.position).normalized;

        if (Target != ActualPosition && Distance < AttackRadius)
        {
            Animator.SetFloat("MovX", Direction.x);
            Animator.SetFloat("MovY", Direction.y);
            Animator.SetBool("Walking", false);
            Invoke("Respawn", 0.4f);

        }
        else if (Target != ActualPosition && (Distance > AttackRadius && Distance < VisionRadius))
        {
            Speed = 0.08f;
            RigidBody2D.MovePosition(transform.position + Direction * Speed);
            Animator.speed = 1;
            Animator.SetFloat("MovX", Direction.x);
            Animator.SetFloat("MovY", Direction.y);
            Animator.SetBool("Walking", true);
        }
        else
        {
            PointPatrol();
        }

        Debug.DrawLine(transform.position, Target, Color.green);
    }

    void Respawn()
    {
        Vector2 MovementPosition = Vector2.MoveTowards(transform.position, Spawn.transform.position, Speed);
        RigidBody2D.MovePosition(MovementPosition);

        if(transform.position == Spawn.transform.position){
            TargetFollow();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, VisionRadius);
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
