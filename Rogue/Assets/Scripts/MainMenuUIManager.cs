using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject defaultButton;
    public Animator playerShipAnim, buttonsAnim;
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
        StartCoroutine(NextSceneCo());
    }

    public IEnumerator NextSceneCo()
    {
        playerShipAnim.SetTrigger("FlyOff");
        buttonsAnim.SetTrigger("FlyOff");
        yield return new WaitForSeconds(3.5f);
        int currentScene;
        currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentScene + 1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
