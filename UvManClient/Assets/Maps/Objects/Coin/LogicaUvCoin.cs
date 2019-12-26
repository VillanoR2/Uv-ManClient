using UnityEngine;

namespace Maps.Objects.Coin
{
    /// <summary>
    /// La clase LogicaUvCoin maneja la logica de los objetos "monedas" que deberan ser recogidos por el jugador
    /// </summary>
    public class LogicaUvCoin : MonoBehaviour
    {
        public int puntosGanados = 10;
        public AudioClip coinSound;
        public float volumeSound = 1f;

        /// <summary>
        /// El metodo OnTriggerEnter2D es un metodo predefinido por UNITY que su funcion es manejar la accion disparador cuando dos coliciones se encuentran.Es un metodo de tipo void
        /// <paramref name="Collider">Es un parametro de tipo Collider2D <paramref/>
        /// </summary>
        void OnTriggerEnter2D(Collider2D Collider)
        {
            if (Collider.tag == "Player")
            {
                AudioSource.PlayClipAtPoint(coinSound, Camera.main.transform.position, volumeSound);
                NotificationCenter.DefaultCenter().PostNotification(this, "IncrementarPuntos", puntosGanados);
                Destroy(gameObject);
            }
        }

    }
}
