using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Link_Movement : MonoBehaviour
{
   public float Speed = 4f;
   Vector2 mov;
   Animator Anim;
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {
        mov = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));

        if(mov != Vector2.zero)
        {
        Anim.SetFloat("MovX",mov.x);
        Anim.SetFloat("MovY",mov.y);
        Anim.SetBool("Walking", true);
        }
        else{
            Anim.SetBool("Walking",false);
        }
    }

}
