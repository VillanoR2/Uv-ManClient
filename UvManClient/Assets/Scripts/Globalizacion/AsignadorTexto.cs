using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AsignadorTexto : MonoBehaviour
{
    public TextoId textoId;

        private void Awake() 
        {
            Text texto = GetComponent<Text>();
            texto.text = ControladorJson.ObtenerTexto(textoId, texto.text);
        }

}
