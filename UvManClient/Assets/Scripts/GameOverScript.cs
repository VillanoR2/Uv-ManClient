using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    public void PlayScene(){
        SceneManager.LoadScene("SingleScreen");
    }

    public void ExitScreen(){
        SceneManager.LoadScene("MainScreen");
    }
}
