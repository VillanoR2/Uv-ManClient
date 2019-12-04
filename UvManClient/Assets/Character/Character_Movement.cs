using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public delegate void PosicionJugador(float x, float y, float movimientoX, float movimientoY);
    public event PosicionJugador MeMovi;

    public float Speed = 4f;
    Vector2 mov;
    Animator Anim;
    Rigidbody2D rb2D;
    

    void Start()
    {
        Anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        InicializarAnimacionHaciaAbajo();
    }

    private void InicializarAnimacionHaciaAbajo()
    {
        Anim.SetFloat("MovY", -0.1f);             
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
        VerificarMovimiento();
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + mov * Speed * Time.deltaTime);
    }

    private void VerificarMovimiento()
    {
        if (mov != Vector2.zero)
        {
            MeMovi?.Invoke(rb2D.position.x, rb2D.position.y, mov.x, mov.y);
        }
    }
}
