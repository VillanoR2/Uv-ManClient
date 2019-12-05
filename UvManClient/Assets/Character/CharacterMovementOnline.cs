using GameService.Dominio.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementOnline : MonoBehaviour
{
    public int VidasDisponibles;
    public float Speed = 4f;
    Vector2 mov;
    Animator Anim;
    Rigidbody2D rb2D;
    Vector2 Posicion;
    public Vector2 PosicionInicial;
    public EnumTipoDeJugador RolDelJugador;


    void Start()
    {
        Anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        Posicion = rb2D.position;
        InicializarAnimacionHaciaAbajo();
    }

    private void InicializarAnimacionHaciaAbajo()
    {
        Anim.SetFloat("MovY", -0.1f);
    }

    public void RealizarMovimiento(float x, float y, float movimientoX, float movimientoY)
    {
        mov = new Vector2(movimientoX, movimientoY);
        Posicion = new Vector2(x, y);
    }

    private void FixedUpdate()
    {
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

        rb2D.position = Posicion;
        mov = Vector2.zero;
    }

    public void ColocarseEnLaPosicionInicial()
    {
        if(PosicionInicial != null)
        {
            mov = PosicionInicial;
        }
    }

}
