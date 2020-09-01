﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System.Security.AccessControl;
using Unity.Mathematics;

public class GameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI t_wavenumber;
    public GameObject UI, defaultButton, livesPrefab, pointsText, pauseMenu, rewardsPanel;
    public GameObject[] lives;
    public GameObject[] scoreTextObjects;
    public EventSystem eventSystem;
    public Slider healthBar, shieldBar;
    public Sprite buttonDefaultSprite;
    public Transform livesStartPoint;
    public float livesIconSeparationDistance = 5f;
    public int ItemNumberChosen; //1, 2, or 3
    public string[] upgrades;
    public float[] upgradeamount;
    public bool ShowingRewards = false, isGamePaused = false;
    public Button item1;
    public PlayerManager pm;
    public TextMeshProUGUI item1txt, item2txt, item3txt, upgradeAmountDebug1, upgradeAmountDebug2, upgradeAmountDebug3;

    private void Awake()
    {
        pm = GetComponent<PlayerManager>();
        healthBar = GameObject.Find("Healthbar").GetComponent<Slider>();
        shieldBar = GameObject.Find("Shieldbar").GetComponent<Slider>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        defaultButton = GameObject.FindGameObjectWithTag("Default Button");
    }
    void Start()
    {
        upgrades = new string[] { "movement", "attackspeed", "damage" };
        upgradeamount = new float[] { 5f * (1f + (0.1f)), 5f * (1f + (0.1f)), 5f * (1f + (0.1f)) };
        updateLives(pm.remainingLives);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Start") && !rewardsPanel.activeInHierarchy)
        {
            if (isGamePaused)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        HidePanels();
        pauseMenu.gameObject.SetActive(true);
        isGamePaused = true;
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        ShowPanels();
        rewardsPanel.SetActive(false);
        pauseMenu.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1.0f;
    }
    public void HidePanels()
    {
        foreach (Transform child in GameObject.Find("UI").transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void ShowPanels()
    {
        foreach (Transform child in GameObject.Find("UI").transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    public void updateHealth(float health)
    {
        healthBar.value = health;
    }
    public void updateShield(float shield)
    {
        shieldBar.value = shield;
    }
    public void updateLives(float livesRemaining)
    {
        foreach (Transform child in livesStartPoint.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        for (int i = 0; i < pm.remainingLives; i++)
        {
            Instantiate(livesPrefab, new Vector3(livesStartPoint.position.x + (i * livesIconSeparationDistance), livesStartPoint.position.y, livesStartPoint.position.z), livesStartPoint.rotation, GameObject.Find("LivesStartPoint").transform);
        }
    }
    public void HighlightButton()
    {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(defaultButton);
    }
    public void ChangeWaveNumber(int number) //Called when NextWave is called
    {
        if (number != 0) //Just for now, until we decide how to implement other stuff
        {
            float ua = 5f * (1f + (number / 10f));
            t_wavenumber.text = "Wave " + number.ToString();
            upgradeamount = new float[] { ua , ua , ua };
            upgradeAmountDebug1.text = ("Upgrade Amount[0]: " + upgradeamount[0]);
            upgradeAmountDebug2.text = ("Upgrade Amount[1]: " + upgradeamount[1]);
            upgradeAmountDebug3.text = ("Upgrade Amount[2]: " + upgradeamount[2]);
            
        }
        else
            t_wavenumber.text = "Boss Level";
            upgradeamount = new float[] { 5 * number, 5 * number, 5 * number };

    }
    public void ClearPointsFromField()
    {
        scoreTextObjects = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject score in scoreTextObjects)
        {
            GameObject.Destroy(score.gameObject);
        }
    }
    public void ConfirmRewardNextWave()
    {
        if (ItemNumberChosen != 0)
        {
            //Give player reward
            this.gameObject.GetComponent<PlayerManager>().AddToPermBuffAndPlayer(GetChosenItemString(), GetChosenItemAmount());
            //Add reward to PM

            //Reset ItemNumberChosen to 0 for future use
            ItemNumberChosen = 0;

            //Start next round
            this.gameObject.GetComponent<WaveManager>().NextWave();
        }
    }
    public void SwapPanel(int panel) //Superior because you can specify
    {
        switch(panel)
        {
            case 1: //Reward panel
                UI.transform.GetChild(0).gameObject.SetActive(false);
                UI.transform.GetChild(1).gameObject.SetActive(true);
                ShowingRewards = true;
                HighlightButton();
                ClearPointsFromField();
                break;
            case 2: //Game UI - health, wave stuff
                UI.transform.GetChild(0).gameObject.SetActive(true);
                UI.transform.GetChild(1).gameObject.SetActive(false);
                ShowingRewards = false;
                break;
        }
    }
    public void ChangeItemNumberChosen(int number)
    {
        ItemNumberChosen = number;
    }
    string GetChosenItemString() //Gets the item string from the array depending on the number chosen
    {
        string ci;
        ci = upgrades[ItemNumberChosen-1]; //minus 1 because buttons start at 1 and array starts at 0
        return ci;
    }
    float GetChosenItemAmount()
    {
        float ca;
        ca = upgradeamount[ItemNumberChosen-1]; //minus 1 because buttons start at 1 and array starts at 0
        Debug.Log(ca);
        return ca;
            
    }
    public void CalculateRewardShowOnUI(int number) //Basic for now until we add other rewards later
    {
        float ua = 5f * (1f + (number / 10f));
        upgradeamount = new float[] { ua, ua, ua };
        UpdateRewardText();
    }
    void UpdateRewardText()
    {
        item1txt.text = ("Movement Speed +" + upgradeamount[0]);
        item2txt.text = ("Attack Speed +" + upgradeamount[1]);
        item3txt.text = ("Attack Damage +" + upgradeamount[2]);
    }
    public void PointsForKillingEnemy(Transform enemy, float points)
    {
        GameObject pointsClone = Instantiate(pointsText, new Vector3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z), new quaternion (0,0,0,0), GameObject.Find("Game HUD").transform);
        pointsClone.GetComponent<TextMeshProUGUI>().text = ("+" + points);
    }
}
