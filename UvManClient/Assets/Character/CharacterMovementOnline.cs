using LogicaDelNegocio.Modelo.Enum;
using UnityEngine;

public class CharacterMovementOnline : MonoBehaviour
{
    public bool EstaActivoElScript = false;
    private const int TOTAL_UV_COINS = 329;
    public int VidasDisponibles;
    public int PuntacionTotal;
    public int CantidadDeUvCoinsRecolectadas;
    public bool EstaActivoTiempoDeMatar = true;

    public EnumTipoDeJugador RolDelJugador;
    public Vector2 PosicionInicial;
    Vector2 Posicion;
    public float Speed = 4f;
    Vector2 mov;
    Animator Anim;
    Rigidbody2D rb2D;


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

    public void DesactivarObjeto()
    {
        gameObject.SetActive(false);
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
        if(Posicion != Vector2.zero)
        {
            rb2D.position = Posicion;
            Posicion = Vector2.zero;
        }
        rb2D.MovePosition(rb2D.position + mov * Speed * Time.deltaTime);
    }

    public void DescontarVida(int CantidadVidas)
    {
        if (EstaActivoElScript)
        {
            VidasDisponibles = CantidadVidas;
            if(VidasDisponibles == 0)
            {
                DesactivarObjeto();
            }
            ColocarseEnLaPosicionInicial();
        }
        
    }

    public void ColocarseEnLaPosicionInicial()
    {
        if (EstaActivoElScript && PosicionInicial != null)
        {
            mov = Vector2.zero;
            Posicion = PosicionInicial;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EstaActivoElScript && RolDelJugador == 0 && collision.gameObject.tag == "UvCoin")
        {
            Destroy(collision.gameObject);
            CantidadDeUvCoinsRecolectadas += 1;
        }
    }

}
