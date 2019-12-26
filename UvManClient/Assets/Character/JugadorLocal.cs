using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// La clase Character_Movement es una clase manager para el jugador que sirve para controlar sus accione y cambios dentro de la escena.
/// </summary>
public class JugadorLocal : MonoBehaviour
{
    private int PerderVida = 1;
    public float Speed = 4f;
    Vector2 Movement;
    Animator Animator;
    Rigidbody2D RigidBody2D;
    AudioSource AudioSource;
    public GameObject Spawn;
    int TotalCoins = 140;

    /// <summary>
    /// El metodo Start es un metodo predefinido por UNITY que su funcion es realizar todo lo que se encuentre dentro 
    /// de el al iniciar la ejecucion por primera vez del proyecto. Este metodo no recibe ningun parametro y es de tipo void.
    /// </summary>
    void Start()
    {
        Animator = GetComponent<Animator>();
        RigidBody2D = GetComponent<Rigidbody2D>();
        AudioSource = GetComponent<AudioSource>();
        NotificationCenter.DefaultCenter().AddObserver(this, "PerderVidaTiempo");
    }

    /// <summary>
    /// El metodo Update es un metodo predefinido por UNITY que su funcion es cargar todo lo que este dentro de el por cada
    /// frame que pase durante la ejecucion.false Es un metodo que no recibe valores y es de tipo void.
    /// </summary>
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
    /// <summary>
    /// El metodo FixedUpdate es un metodo predefinido por UNITY que su funcion es cargar todo lo que este dentro de el por cada
    /// segundo que pase durante la ejecucion sin esperar o. Es un metodo que no recibe valores y es de tipo void.
    /// </summary>
    void FixedUpdate()
    {
        RigidBody2D.MovePosition(RigidBody2D.position + Movement * Speed * Time.deltaTime);
    }
    /// <summary>
    /// El metodo OnTriggerEnter2D es un metodo predefinido por UNITY que su funcion es manejar la accion disparador cuando dos coliciones se encuentran.Es un metodo de tipo void
    /// <paramref name="Collider">Es un parametro de tipo Collider2D <paramref/>
    /// </summary>
    void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.gameObject.tag == "Enemy")
        {
            Animator.SetBool("Die", true);
            AudioSource.Play();
            gameObject.GetComponent<Collider2D>().enabled = false;
            NotificationCenter.DefaultCenter().PostNotification(this, "PerderVida", PerderVida);
            Speed = 0f;
            Invoke("Respawn", 4.0f);
        }
        else if (Collider.gameObject.tag == "Coin")
        {
            TotalCoins -= 1;
            Debug.Log("UVCoin: " + TotalCoins);
            if (TotalCoins == 0)
            {
                SceneManager.LoadScene("GameOverScene");
            }
        }
    }

    /// <summary>
    /// El metodo PerderVidaTiempo es un metodo que notifica a la clase Vida que se ha acabado el tiempo y realiza la animacion del jugador donde ha muerto.
    /// <paramref name="Notification">Tipo de parametro Notification<paramref/>
    /// </summary>
    public void PerderVidaTiempo(Notification Notification)
    {
        float Tiempo = (float)Notification.data;
        if (Tiempo <= 0)
        {
            Animator.SetBool("Die", true);
            AudioSource.Play();
            NotificationCenter.DefaultCenter().PostNotification(this, "PerderVida", PerderVida);
            Speed = 0f;

            Invoke("Respawn", 2.0f);
        }
        else
        {
            Debug.Log("El tiempo no llego a Cero.");
        }

    }
    /// <summary>
    /// El metodo Respawn es un metodo que reasigna al objeto a la posicin de el objeto llamado spawn generado en la escena,
    /// este metodo es de tipo void y no recibe parametros.
    /// </summary>
    void Respawn()
    {
        Animator.SetBool("Die", false);
        transform.position = Spawn.transform.position;
        gameObject.GetComponent<Collider2D>().enabled = true;
        Speed = 4f;
    }

}