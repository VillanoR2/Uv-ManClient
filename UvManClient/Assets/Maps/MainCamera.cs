using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Esta Clase funciona como un manager para la camara de la escena que siempre seguira al jugador.
/// </summary>
public class MainCamera : MonoBehaviour
{
    Transform Target;

    /// <summary>
    /// El metodo Awake es un metodo predefinido por UNITY que su funcion es realizar todo lo que se encuentre dentro 
    /// de el al iniciar un objeto en este caso la camara. Este metodo no recibe ningun paramatro y es de tipo void.
    /// </summary>
    void Awake()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /// <summary>
    /// El metodo Update es un metodo predefinido por UNITY que su funcion es cargar todo lo que este dentro de el por cada
    /// frame que pase durante la ejecucion. Es un metodo que no recibe valores y es de tipo void.
    /// </summary>
    void Update()
    {
        transform.position = new Vector3(
            Target.position.x,
            Target.position.y,
            transform.position.z
            );

        if (Target == null)
        {
            Debug.Log("Player ha muerto");
        }
    }

}
