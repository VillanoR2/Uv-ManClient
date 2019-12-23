using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOverScript : MonoBehaviour
{
    public Text MarcadorFinal;
    void Start(){
        PuntosGanados();
    }
    public void PlayScene(){
        SceneManager.LoadScene("SingleScreen");
    }

    public void ExitScreen(){
        SceneManager.LoadScene("MainScreen");
    }

    void PuntosGanados(){
        MarcadorFinal.text = "Puntuacion: " + GameObject.Find("AlmacenPuntaje").GetComponent<Text>().text;
        Destroy(GameObject.Find("AlmacenPuntaje"));
    }


}
