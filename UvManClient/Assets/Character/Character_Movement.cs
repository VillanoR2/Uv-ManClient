using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    private int PerderVida = 1;
    public float Speed = 4f;
    Vector2 Movement;
    Animator Animator;
    Rigidbody2D RigidBody2D;
    AudioSource AudioSource;
    public GameObject Spawn;

    void Start()
    {
        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
        NotificationCenter.DefaultCenter().AddObserver(this, "PerderVidaTiempo");
    }

    void Update()
    {
        Movement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        if (Movement != Vector2.zero)
        {
            Animator.SetFloat("MovX", Movement.x);
            Animator.SetFloat("MovY", Movement.y);
            Animator.SetBool("Walking", true);
        }
        else
        {
            Animator.SetBool("Walking", false);
        }
    }
    void FixedUpdate()
    {
        RigidBody2D.MovePosition(RigidBody2D.position + Movement * Speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            Animator.SetBool("Die", true);
            AudioSource.Play();
            NotificationCenter.DefaultCenter().PostNotification(this, "PerderVida", PerderVida);
            Speed = 0f;
            Invoke("Respawn", 3.0f);

        }
    }

    public void PerderVidaTiempo(Notification Notification)
    {
        float Tiempo = (float)Notification.data;
        if(Tiempo <= 0){
        Animator.SetBool("Die", true);
        AudioSource.Play();
        NotificationCenter.DefaultCenter().PostNotification(this, "PerderVida", PerderVida);
        Speed = 0f;
        Invoke("Respawn", 3.0f);
        }else{
            Debug.Log("El tiempo no llego a Cero.");
        }

    }

    void Respawn()
    {
        gameObject.SetActive(false);
        transform.position = Spawn.transform.position;
        gameObject.SetActive(true);
        Speed = 4f;
    }


}