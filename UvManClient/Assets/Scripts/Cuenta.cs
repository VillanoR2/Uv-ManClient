using LogicaDelNegocio.Modelo;
using UnityEngine;

public class Cuenta : MonoBehaviour
{
    public static Cuenta cuentaLogeada;
    public CuentaModel cuenta;

    //Se llama antes que estar
    private void Awake()
    {
        if (cuentaLogeada == null)
        {
            cuentaLogeada = this;
            DontDestroyOnLoad(gameObject);
        }else if(cuentaLogeada != this)
        {
            Destroy(gameObject);
        }
    }
}
