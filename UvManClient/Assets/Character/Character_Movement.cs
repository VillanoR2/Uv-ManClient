using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    private int perderVida = 1;
    public float Speed = 4f;
    Vector2 mov;
    Animator Anim;
    Rigidbody2D rb2D;
    AudioSource audioS;

    void Start()
    {
        Anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        audioS = GetComponent<AudioSource>();
    }

    void Update()
    {
        mov = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        if (mov != Vector2.zero)
        {
            Anim.SetFloat("MovX", mov.x);
            Anim.SetFloat("MovY", mov.y);
            Anim.SetBool("Walking", true);
        }
        else
        {
            Anim.SetBool("Walking", false);
        }
    }
    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + mov * Speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            Anim.SetBool("Die", true);
            audioS.Play();
            Speed = 0;
            NotificationCenter.DefaultCenter().PostNotification(this, "PerderVida", perderVida);
        }
    }
}