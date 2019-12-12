using Assets.Scripts.Util;
using LogicaDelNegocio.Modelo.Enum;
using UnityEngine;

public class Character_Movement : MonoBehaviour
{
    public bool EstaActivoElScript = false;
    private const int TOTAL_UVCOINS = 329;
    private const int UVCOINS_NECESARIAS_PARA_ACTIVARMATAR = 28;
    private bool VolverAPosicionInicial = false;
    public bool EsElJuegadorActual;
    public int VidasDisponibles;
    public int PuntacionTotal;
    public int CantidadDeUvCoinsRecolectadas;
    public bool EstaActivoTiempoDeMatar;
    public EnumTipoDeJugador RolDelJugador;
    public Vector2 PosicionInicial;

    public delegate void PosicionJugador(float x, float y, float movimientoX, float movimientoY);
    public delegate void MuerteDeJugador(EnumTipoDeJugador RolDelJugador, int NumeroVidas);
    public delegate void NotificacionPartida();
    public event NotificacionPartida RecolectoTodasLasMonedas;
    public event NotificacionPartida PuedoComerPerseguidores;
    public event MuerteDeJugador MateAJugador;
    public event PosicionJugador MeMovi;

    
    public float Speed = 4f;
    private Vector2 mov;
    private Animator Anim;
    private Rigidbody2D rb2D;
    private Vector2 movimiento;
    private bool LaCamaraEstaActiva;
    private SpriteRenderer RenderDelPersonaje;

    void Start()
    {
        Anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        InicializarAnimacionHaciaAbajo();
        CantidadDeUvCoinsRecolectadas = 0;
        EstaActivoTiempoDeMatar = false;
        LaCamaraEstaActiva = true;
        RenderDelPersonaje = GetComponent<SpriteRenderer>();
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
        ActualizarCamara();
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
        if (EstaActivoElScript)
        {
            VolverAPosicionInicial = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (EstaActivoElScript && RolDelJugador > 0 && collision.gameObject.CompareTag("Corredor") && !EstaActivoTiempoDeMatar)
        {
            PuntacionTotal += 1000;
            EnumTipoDeJugador RolDelJugadorMatado = EnumTipoDeJugador.Corredor;
            int numeroVidas = collision.gameObject.GetComponent<Character_Movement>().VidasDisponibles;
            collision.gameObject.GetComponent<Character_Movement>().DescontarVida(numeroVidas - 1);
            MateAJugador?.Invoke(RolDelJugadorMatado, numeroVidas-1);
        }
        else if(EstaActivoElScript && RolDelJugador == 0 && collision.gameObject.CompareTag("Perseguidor") && EstaActivoTiempoDeMatar)
        {
            PuntacionTotal += 100;
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
            PuntacionTotal += 3;
            Destroy(collision.gameObject);
            CantidadDeUvCoinsRecolectadas += 1;
            if(CantidadDeUvCoinsRecolectadas % UVCOINS_NECESARIAS_PARA_ACTIVARMATAR == 0)
            {
                ActivaTiempoDeMatar();
                PuedoComerPerseguidores?.Invoke();
            }

            if(EsElJuegadorActual && CantidadDeUvCoinsRecolectadas == TOTAL_UVCOINS)
            {
                CantidadDeUvCoinsRecolectadas = 0;
                RecolectoTodasLasMonedas?.Invoke();
            }
        }   
    }

    /// <summary>
    /// Desactiva la camara que sigue al jugador
    /// </summary>
    public void DesactivarCamara()
    {
        LaCamaraEstaActiva = false;
    }

    /// <summary>
    /// Activa la camara que sigue al jugador
    /// </summary>
    public void ActivarCamara()
    {
        LaCamaraEstaActiva = true;
    }
   
    /// <summary>
    /// Inicia un cronometro con el tiempo de matar
    /// </summary>
    public void ActivaTiempoDeMatar()
    {
        EstaActivoTiempoDeMatar = true;
        Cronometro CronometroTiempoActivoDeMatar = new Cronometro(500, 10000);
        if(RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            CronometroTiempoActivoDeMatar.TranscurrioUnIntervalo += CambiarColorPersonaje;
        }
        CronometroTiempoActivoDeMatar.FinalizoElTimepo += DesactivarTiempoMatar;
        CronometroTiempoActivoDeMatar.Iniciar();
    }

    /// <summary>
    /// Cambia el color del render del personaje para indicar que puede comer personajes
    /// </summary>
    private void CambiarColorPersonaje()
    {
        Color ColorOriginal = new Color(255, 255, 255, 255);
        Color ColorComer1 = new Color(234, 143, 47, 255);
        Color ColorComer2 = new Color(255, 0, 0, 255);
        if(RenderDelPersonaje.color == ColorOriginal)
        {
            RenderDelPersonaje.color = ColorComer1;
        }else if(RenderDelPersonaje.color == ColorComer1)
        {
            RenderDelPersonaje.color = ColorComer2;
        }
        else
        {
            RenderDelPersonaje.color = ColorComer1;
        }
    }

    /// <summary>
    /// Desactiva el tiempo de matar
    /// </summary>
    private void DesactivarTiempoMatar()
    {
        if(RolDelJugador == EnumTipoDeJugador.Corredor)
        {
            RenderDelPersonaje.color = new Color(255, 255, 255, 255);
        }
        EstaActivoTiempoDeMatar = false;
    }

    /// <summary>
    /// Actualiza el estado de la camara
    /// </summary>
    private void ActualizarCamara()
    {
        GetComponentInChildren<Camera>().enabled = LaCamaraEstaActiva;
    }
}
