using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonPressStart : MonoBehaviour
{
    public void PlayScene()
    {
        SceneManager.LoadScene("LoginScreen");
    }
}
