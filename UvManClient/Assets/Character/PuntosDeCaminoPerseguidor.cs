using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntosDeCaminoPerseguidor : MonoBehaviour
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
    public GameObject SpawnEnemy;

    /// <summary>
    /// El metodo Start es un metodo predefinido por UNITY que su funcion es realizar todo lo que se encuentre dentro 
    /// de el al iniciar la ejecucion por primera vez del proyecto. Este metodo no recibe ningun parametro y es de tipo void.
    /// </summary>
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ActualPosition = transform.position;
        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();
    }
    /// <summary>
    /// El metodo FixedUpdate es un metodo predefinido por UNITY que su funcion es cargar todo lo que este dentro de el por cada
    /// segundo que pase durante la ejecucion sin esperar o. Es un metodo que no recibe valores y es de tipo void.
    /// </summary>
    void FixedUpdate()
    {
        TargetFollow();
    }
    /// <summary>
    /// El metodo PointPatrol define un arreglo de puntos que funcionaran como puntos de patrullaje a la que el objeto se dirigira.
    /// </summary>
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
    /// <summary>
    /// El metodo TargetFollow designa dos areas una de vision y una de ataque y designa que si un objetivo se encuentra 
    /// en el area de visión transforma su posicion actual a la posicion del objetivo.
    /// </summary>
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

        }
        else if (Target != ActualPosition && (Distance > AttackRadius && Distance < VisionRadius))
        {
            Speed = 0.07f;
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
    /// <summary>
    /// El metodo Respawn es un metodo que reasigna al objeto a la posicin de el objeto llamado spawn generado en la escena,
    /// este metodo es de tipo void y no recibe parametros.
    /// </summary>
    void Respawn()
    {
        gameObject.SetActive(true);
        ActualPosition = SpawnEnemy.transform.position;

        Vector2 MovementPosition = Vector2.MoveTowards(transform.position, SpawnEnemy.transform.position, Speed);
        RigidBody2D.MovePosition(MovementPosition);

        if(transform.position == SpawnEnemy.transform.position){
            TargetFollow();
        }
    }
    /// <summary>
    /// El metodo OnTriggerEnter2D es un metodo predefinido por UNITY que su funcion es manejar la accion disparador cuando dos coliciones se encuentran.Es un metodo de tipo void
    /// <paramref name="Collider">Es un parametro de tipo Collider2D <paramref/>
    /// </summary>
        void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            Invoke("Respawn", 3.0f);

        }
    }
    /// <summary>
    /// El metodo OnDrawGizmosSelected es un metodo que dibuja una circunferencia alrededor del objeto asignado.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, VisionRadius);
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
