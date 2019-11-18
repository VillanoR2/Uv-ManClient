using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorJson{

    private static ContenedorTexto contenedorTextos;
    private const SystemLanguage idiomaPorDefecto = SystemLanguage.English;

 public static string ObtenerTexto(TextoId textoId, string textoActual)
    {
        string textoRetornar = string.Empty;

        if (idiomaPorDefecto == Application.systemLanguage)
        {
            return textoActual;
        }
    
        else if (contenedorTextos == null)
        {
            ObtenerIdioma();
        }

        if(contenedorTextos != null)
        {
            textoRetornar = contenedorTextos.ObtenerTexto(textoId.ToString());
        }

        if (string.IsNullOrEmpty(textoRetornar))
        {
            textoRetornar = textoActual;
        }

        return textoRetornar;
                
    }
    
    private static void ObtenerIdioma()
    {
        Debug.Log("Idioma detectado: " + Application.systemLanguage.ToString());
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Spanish:
                LoadJsonFile(SystemLanguage.Spanish.ToString());
                break;
            default:
                LoadJsonFile(SystemLanguage.English.ToString());
                break;
        }
    }

    private static void LoadJsonFile(string name)
    {
        TextAsset asset = Resources.Load<TextAsset>(SystemLanguage.Spanish.ToString());
        if (asset != null)
        {
            contenedorTextos = JsonUtility.FromJson<ContenedorTexto>(asset.text);
        }
        else
        {
            Debug.LogWarning("No se ha encontrado el fichero " + name);
        }
        
    }
}