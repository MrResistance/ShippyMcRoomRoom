using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuUIManager : MonoBehaviour
{
    public void NextScene()
    {
        int currentScene;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
