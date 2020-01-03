using LogicaDelNegocio.Modelo;
using UnityEngine;

/// <summary>
/// Se encarga de almacenar la cuenta logeada
/// </summary>
public class Cuenta : MonoBehaviour
{
    public static Cuenta CuentaLogeada;
    public CuentaModel CuentaM;

    /// <summary>
    /// Metodo de UNITY que se llama al momento de cargar una escane
    /// </summary>
    private void Awake()
    {
        if (CuentaLogeada == null)
        {
            CuentaLogeada = this;
            DontDestroyOnLoad(gameObject);
        }else if(CuentaLogeada != this)
        {
            Destroy(gameObject);
        }
    }
}
