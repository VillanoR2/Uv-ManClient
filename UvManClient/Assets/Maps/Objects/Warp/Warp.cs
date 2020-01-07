using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public GameObject target;

    /// <summary>
    /// El metodo Awake es un metodo predefinido por UNITY que su funcion es realizar todo lo que se encuentre dentro 
    /// de el al iniciar un objeto en este caso la camara. Este metodo no recibe ningun paramatro y es de tipo void.
    /// </summary>
    void Awake()
    {   
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }
    /// <summary>
    /// El metodo OnTriggerEnter2D es un metodo predefinido por UNITY que su funcion es manejar la accion disparador cuando dos coliciones se encuentran.Es un metodo de tipo void
    /// <paramref name="Collider">Es un parametro de tipo Collider2D <paramref/>
    /// </summary>
    void OnTriggerEnter2D(Collider2D other){
        if((other.tag == "Player") || (other.tag == "Corredor") || (other.tag == "Perseguidor"))
        {
            other.transform.position = target.transform.GetChild(0).transform.position;
        }
    }
}
