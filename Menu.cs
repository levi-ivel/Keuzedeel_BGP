using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
public GameObject SelectUI;
public GameObject MainMenu;

//START --Main Menu--
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

//END --Main Menu--

//START --Select Menu--
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

public void BackMenu()
    {
        SelectUI.SetActive(true);
        MainMenu.SetActive(false);
    }
//END --Select Menu--

}