using Assets.Scripts.Util;
using LogicaDelNegocio.Modelo.Enum;
using UnityEngine;

/// <summary>
/// Se encarga de controlar al personaje de un jugador en linea
/// </summary>
public class JugadorEnLinea : MonoBehaviour
{
    public bool EstaActivoElScript;
    public int VidasDisponibles;
    public bool EstaActivoTiempoDeMatar = true;

    public EnumTipoDeJugador RolDelJugador;
    public Vector2 PosicionInicial;
    private Vector2 Posicion;
    public float Speed = 4f;
    private Vector2 mov;
    private Animator Anim;
    private Rigidbody2D rb2D;
    private Color ColorDelPersonaje;

    void Start()
    {
        Anim = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        Posicion = rb2D.position;
        InicializarAnimacionHaciaAbajo();
        InicializarColorDelPersonaje();
    }

    /// <summary>
    /// Coloca el color del personaje en color blanco
    /// </summary>
    private void InicializarColorDelPersonaje()
    {
        ColorDelPersonaje = new Color(255, 255, 255, 255);
    }

    /// <summary>
    /// Actualiza el color del personaje al color que se encuentra en el atributo Color
    /// </summary>
    private void ActualizarColorDelPersonaje()
    {
        GetComponent<SpriteRenderer>().color = ColorDelPersonaje;
    }

    /// <summary>
    /// Coloca la animacion del personaje hacia abajo
    /// </summary>
    private void InicializarAnimacionHaciaAbajo()
    {
        Anim.SetFloat("MovY", -0.1f);
    }

    /// <summary>
    /// Coloca la poscion del personaje en las coordenadas x,y y se empieza a mover hacia la direccion movimientoX,
    /// movimientoY
    /// </summary>
    /// <param name="x">float</param>
    /// <param name="y">float</param>
    /// <param name="movimientoX">float</param>
    /// <param name="movimientoY">float</param>
    public void RealizarMovimiento(float x, float y, float movimientoX, float movimientoY)
    {
        mov = new Vector2(movimientoX, movimientoY);
        Posicion = new Vector2(x, y);
    }

    /// <summary>
    /// Desactiva al personaje
    /// </summary>
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

        ActualizarColorDelPersonaje();
    }

    /// <summary>
    /// Descuenta una vida del personaje
    /// </summary>
    /// <param name="CantidadVidas">int</param>
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

    /// <summary>
    /// Coloca al personaje en su posicion inicial
    /// </summary>
    public void ColocarseEnLaPosicionInicial()
    {
        if (EstaActivoElScript && PosicionInicial != null)
        {
            mov = Vector2.zero;
            Posicion = PosicionInicial;
        }
    }

    /// <summary>
    /// Detecta cuando el personaje entra en contacto con una UvCoin
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (EstaActivoElScript && RolDelJugador == 0 && collision.gameObject.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
        }
    }
    
    /// <summary>
    /// Inicia un cronometro con el tiempo de matar
    /// </summary>
    public void ActivaTiempoDeMatar()
    {
        EstaActivoTiempoDeMatar = false;
        Cronometro CronometroTiempoActivoDeMatar = new Cronometro(500, 10000);
        CronometroTiempoActivoDeMatar.TranscurrioUnIntervalo += CambiarColorPersonaje;
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
}
