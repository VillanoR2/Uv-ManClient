using Assets.Scripts.Util;
using LogicaDelNegocio.Modelo.Enum;
using UnityEngine;

/// <summary>
/// Se encarga de controlar al personaje del jugador actual
/// </summary>
public class Character_Movement : MonoBehaviour
{
    public bool EstaActivoElScript;
    private const int TOTAL_UVCOINS = 329;
    private const int UVCOINS_NECESARIAS_PARA_ACTIVARMATAR = 28;
    private bool VolverAPosicionInicial;
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
    
    public float Velocidad = 4f;
    private Vector2 MovimientoContinuo;
    private Animator Animacion;
    private Rigidbody2D Rigidbody;
    private Vector2 MovimientoNuevo;
    private bool LaCamaraEstaActiva;
    private Color ColorDelPersonaje;

    void Start()
    {
        Animacion = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody2D>();
        InicializarAnimacionHaciaAbajo();
        CantidadDeUvCoinsRecolectadas = 0;
        EstaActivoTiempoDeMatar = false;
        LaCamaraEstaActiva = true;
        InicializarColorDelPersonaje();
    }

    /// <summary>
    /// Coloca la animacion del personaje hacia abajo
    /// </summary>
    private void InicializarAnimacionHaciaAbajo()
    {
        Animacion.SetFloat("MovY", -0.1f);             
    }

    /// <summary>
    /// Coloca el color del personaje en blanco
    /// </summary>
    private void InicializarColorDelPersonaje()
    {
        ColorDelPersonaje = new Color(255, 255, 255, 255);
    }
    
    void Update()
    {
        MovimientoNuevo = new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical"));

        if(MovimientoNuevo != Vector2.zero)
        {
            MovimientoContinuo = MovimientoNuevo;
        }

        if (MovimientoContinuo != Vector2.zero)
        {
            Animacion.SetFloat("MovX",MovimientoContinuo.x);
            Animacion.SetFloat("MovY",MovimientoContinuo.y);
            Animacion.SetBool("Walking", true);
        }
        else{
            Animacion.SetBool("Walking",false);
        }
        if (VolverAPosicionInicial)
        {
            Rigidbody.position = PosicionInicial;
            MovimientoContinuo = Vector2.zero;
            VolverAPosicionInicial = false;
        }
        ActualizarCamara();
        VerificarMovimiento();
        ActualizarColorDelPersonaje();
    }

    void FixedUpdate()
    {
        if (EsElJuegadorActual)
        {
            Rigidbody.MovePosition(Rigidbody.position + MovimientoContinuo * Velocidad * Time.deltaTime);
        }
    }

    /// <summary>
    /// Verifica si el jugador cambio de direccion su movimiento
    /// </summary>
    private void VerificarMovimiento()
    {
        if (EsElJuegadorActual)
        {
            if (MovimientoNuevo != Vector2.zero)
            {
                MeMovi?.Invoke(Rigidbody.position.x, Rigidbody.position.y, MovimientoContinuo.x, MovimientoContinuo.y);
            }
        }
    }

    /// <summary>
    /// Descuenta una vida al jugador
    /// </summary>
    /// <param name="CantidadVidas"></param>
    public void DescontarVida(int CantidadVidas)
    {
        if (EstaActivoElScript)
        {
            VidasDisponibles = CantidadVidas;
            ColocarseEnLaPosicionInicial();
        }
    }

    /// <summary>
    /// Coloca al personaje en su posicion inicial
    /// </summary>
    public void ColocarseEnLaPosicionInicial()
    {
        if (EstaActivoElScript)
        {
            VolverAPosicionInicial = true;
        }
    }

    /// <summary>
    /// Detecta las colisiones que tiene el personje con otros objetos
    /// </summary>
    /// <param name="collision">Collision2D</param>
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

    /// <summary>
    /// Detecta cuando el personaje entra en el disparador de una UvCoin
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(EstaActivoElScript && RolDelJugador == 0 && collision.gameObject.CompareTag("UvCoin"))
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
        if (ColorDelPersonaje == ColorOriginal)
        {
            ColorDelPersonaje = ColorComer1;
        }
        else if (ColorDelPersonaje == ColorComer1)
        {
            ColorDelPersonaje = ColorComer2;
        }
        else
        {
            ColorDelPersonaje = ColorComer1;
        }
    }

    /// <summary>
    /// Desactiva el tiempo de matar
    /// </summary>
    private void DesactivarTiempoMatar()
    {
        ColorDelPersonaje = new Color(255, 255, 255, 255);
        EstaActivoTiempoDeMatar = false;
    }

    /// <summary>
    /// Actualiza el estado de la camara
    /// </summary>
    private void ActualizarCamara()
    {
        GetComponentInChildren<Camera>().enabled = LaCamaraEstaActiva;
    }
    
    /// <summary>
    /// Actualiza el color del sprite del persoje colocando la que se encuentra en el atributo Color
    /// </summary>
    private void ActualizarColorDelPersonaje()
    {
        GetComponent<SpriteRenderer>().color = ColorDelPersonaje;
    }
}
