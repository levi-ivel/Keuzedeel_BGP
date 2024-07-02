using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
public GameObject SelectUI;
public GameObject MainMenu;


public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

public void Selectgame()
    {
        SelectUI.SetActive(false);
        MainMenu.SetActive(true);
    }

public void BackMenu()
    {
        SelectUI.SetActive(true);
        MainMenu.SetActive(false);
    }

public void SelectTut()
    {
        SceneManager.LoadScene(1);
    }

public void Select1()
    {
        SceneManager.LoadScene(2);
    }

public void Select2()
    {
        SceneManager.LoadScene(3);
    }

public void Select3()
    {
        SceneManager.LoadScene(4);
    }


}