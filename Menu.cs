using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{

public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

}