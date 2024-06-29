using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStage : MonoBehaviour
{

private void OnTriggerEnter2D(Collider2D other)
{
    int currentIndex = SceneManager.GetActiveScene().buildIndex;
    int nextSceneIndex = currentIndex + 1;

    if (other.tag == "Player")
    {
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more scenes available.");
        }
    }
}
}