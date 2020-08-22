using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject defaultButton;
    private void Awake()
    {
        defaultButton = GameObject.FindGameObjectWithTag("Default Button");
    }
    public void HighlightButton()
    {
        defaultButton.GetComponent<Button>().Select();
        defaultButton.GetComponent<Button>().OnSelect(null);
    }
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
