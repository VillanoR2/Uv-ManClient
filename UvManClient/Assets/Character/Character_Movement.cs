using LogicaDelNegocio.Modelo.Enum;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public bool EstaActivoElScript = false;
    private const int TOTAL_UV_COINS = 329;
    private bool VolverAPosicionInicial = false;
    public bool EsElJuegadorActual;
    public int VidasDisponibles;
    public int PuntacionTotal;
    public int CantidadDeUvCoinsRecolectadas;
    public bool EstaActivoTiempoDeMatar;


    public delegate void PosicionJugador(float x, float y, float movimientoX, float movimientoY);
    public delegate void MuerteDeJugador(EnumTipoDeJugador RolDelJugador, int NumeroVidas);
    public delegate void NotificacionPartidaTerminada();
    public event NotificacionPartidaTerminada RecolectoTodasLasMonedas;
    public event MuerteDeJugador MateAJugador;
    public event PosicionJugador MeMovi;

    public EnumTipoDeJugador RolDelJugador;
    public Vector2 PosicionInicial;
    public float Speed = 4f;
    Vector2 mov;
    Animator Anim;
    Rigidbody2D rb2D;
    Vector2 movimiento;

    void Start()
    {
        Anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        InicializarAnimacionHaciaAbajo();
        CantidadDeUvCoinsRecolectadas = 0;
        EstaActivoTiempoDeMatar = true;
    }

    private void InicializarAnimacionHaciaAbajo()
    {
        Anim.SetFloat("MovY", -0.1f);             
    }

    void Update()
    {
        movimiento = new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical"));

        if(movimiento != Vector2.zero)
        {
            mov = movimiento;
        }

        if (mov != Vector2.zero)
        {
            Anim.SetFloat("MovX",mov.x);
            Anim.SetFloat("MovY",mov.y);
            Anim.SetBool("Walking", true);
        }
        else{
            Anim.SetBool("Walking",false);
        }
        if (VolverAPosicionInicial)
        {
            rb2D.position = PosicionInicial;
            mov = Vector2.zero;
            VolverAPosicionInicial = false;
        }
        VerificarMovimiento();
    }

    void FixedUpdate()
    {
        if (EsElJuegadorActual)
        {
            rb2D.MovePosition(rb2D.position + mov * Speed * Time.deltaTime);
        }
    }

    private void VerificarMovimiento()
    {
        if (EsElJuegadorActual)
        {
            if (movimiento != Vector2.zero)
            {
                MeMovi?.Invoke(rb2D.position.x, rb2D.position.y, mov.x, mov.y);
            }
        }
    }

    public void DescontarVida(int CantidadVidas)
    {
        if (EstaActivoElScript)
        {
            VidasDisponibles = CantidadVidas;
            ColocarseEnLaPosicionInicial();
        }
    }

    public void ColocarseEnLaPosicionInicial()
    {
        if (EstaActivoElScript && PosicionInicial != null)
        {
            VolverAPosicionInicial = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (EstaActivoElScript && RolDelJugador > 0 && collision.gameObject.tag == "Corredor" && !EstaActivoTiempoDeMatar)
        {
            EnumTipoDeJugador RolDelJugadorMatado = EnumTipoDeJugador.Corredor;
            int numeroVidas = collision.gameObject.GetComponent<Character_Movement>().VidasDisponibles;
            collision.gameObject.GetComponent<Character_Movement>().DescontarVida(numeroVidas - 1);
            MateAJugador?.Invoke(RolDelJugadorMatado, numeroVidas-1);
        }
        else if(EstaActivoElScript && RolDelJugador == 0 && collision.gameObject.tag == "Perseguidor" && EstaActivoTiempoDeMatar)
        {
            EnumTipoDeJugador RolDelJugadorMatado = collision.gameObject.GetComponent<CharacterMovementOnline>().RolDelJugador;
            int numeroVidas = collision.gameObject.GetComponent<CharacterMovementOnline>().VidasDisponibles;
            collision.gameObject.GetComponent<CharacterMovementOnline>().DescontarVida(numeroVidas-1);
            MateAJugador?.Invoke(RolDelJugadorMatado, numeroVidas-1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(EstaActivoElScript && RolDelJugador == 0 && collision.gameObject.tag == "UvCoin")
        {
            Destroy(collision.gameObject);
            CantidadDeUvCoinsRecolectadas += 1;
            if(EsElJuegadorActual && CantidadDeUvCoinsRecolectadas == TOTAL_UV_COINS)
            {
                RecolectoTodasLasMonedas?.Invoke();
            }
        }   
    }
}
