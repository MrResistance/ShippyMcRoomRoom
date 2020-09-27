using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Security.AccessControl;
using System.Diagnostics;
using Unity.Mathematics;

public class GameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI t_wavenumber, t_waveNumberPauseMenu, t_SpeedrunnerClock;
    public GameObject UI, defaultButton, defaultPauseButton,livesPrefab, pointsText, pauseMenu, rewardsPanel, player;
    public GameObject  previousWeaponBackup, previousWeapon, currentWeapon, nextWeapon, nextWeaponBackup;
    public GameObject[] projectileObjects;
    public GameObject[] lives;
    public GameObject[] scoreTextObjects;
    public Sprite[] weaponSelectSprites;
    public EventSystem eventSystem;
    public Slider healthBar, shieldBar;
    public Sprite buttonDefaultSprite;
    public Transform livesStartPoint;
    public Animator waveNumberAnim;
    public float livesIconSeparationDistance = 5f, playerCurrentScore = 0f;
    public int ItemNumberChosen; //1, 2, or 3
    public string[] upgrades;
    public float[] upgradeamount;
    public bool ShowingRewards = false, isGamePaused = false;
    public Button item1;
    public PlayerManager pm;
    public PlayerWeapon pw;
    public WaveManager wm;
    public StatTrak statTrak;
    public TextMeshProUGUI hiScoreTxt, item1txt, item2txt, item3txt, upgradeAmountDebug1, upgradeAmountDebug2, upgradeAmountDebug3;
    public Stopwatch speedrunnerStopwatch;
    private void Awake()
    {
        wm = GetComponent<WaveManager>();
        statTrak = GameObject.Find("StatTrak").GetComponent<StatTrak>();
        pm = GetComponent<PlayerManager>();
        healthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
        shieldBar = GameObject.Find("PlayerShieldBar").GetComponent<Slider>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        defaultButton = GameObject.FindGameObjectWithTag("Default Button");
    }
    void Start()
    {
        speedrunnerStopwatch = new Stopwatch();
        speedrunnerStopwatch.Start();
        player = GameObject.Find("Player");
        pw = player.GetComponent<PlayerWeapon>();
        upgrades = new string[] { "movement", "attackspeed", "damage" };
        upgradeamount = new float[] { 5f * (1f + (0.1f)), 5f * (1f + (0.1f)), 5f * (1f + (0.1f)) };
        updateLives(pm.remainingLives);
        UpdateHiScore(0f);
        //StartCoroutine(CheckForControllers());
    }
    private void Update()
    {
        if (!isGamePaused && !rewardsPanel.activeInHierarchy)
        {
            if (!speedrunnerStopwatch.IsRunning)
            {
                speedrunnerStopwatch.Start();
            }
            t_SpeedrunnerClock.text = speedrunnerStopwatch.Elapsed.ToString("mm' : 'ss' . 'ff");
            
        }
        else
        {
            if (speedrunnerStopwatch.IsRunning)
            {
                speedrunnerStopwatch.Stop();
            }
        }
        if (!wm.waveComplete)
        {
            statTrak.timeTakenToCompleteWave = speedrunnerStopwatch.Elapsed.ToString("mm' : 'ss' . 'ff");
        }
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
    public IEnumerator CheckForControllers()
    {
        while (true)
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (Input.GetJoystickNames().Length == 0)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }
    public void ShowHideSpeedrunnerClock()
    {
        if (t_SpeedrunnerClock.GetComponent<TextMeshProUGUI>().fontSize != 36f)
        {
            t_SpeedrunnerClock.GetComponent<TextMeshProUGUI>().fontSize = 36f;
        }
        else if (t_SpeedrunnerClock.GetComponent<TextMeshProUGUI>().fontSize == 36f)
        {
            t_SpeedrunnerClock.GetComponent<TextMeshProUGUI>().fontSize = 0f;
        }
    }
    public void UpdateWeaponUI(WeaponData wd)
    {
        currentWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wd.sprite;
        currentWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().color = wd.colourSprite;
    }
    public void UpdatePreviousWeaponUI(WeaponData wd)
    {
        previousWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wd.sprite;
        previousWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().color = wd.colourSprite;
    }
    public void UpdateNextWeaponUI(WeaponData wd)
    {
        nextWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = wd.sprite;
        nextWeapon.transform.GetChild(0).GetComponent<SpriteRenderer>().color = wd.colourSprite;
    }
    public void UpdateHiScore(float score)
    {
        playerCurrentScore += score;
        hiScoreTxt.text = ("Hi score: " + playerCurrentScore);
    }
    public void ShowWaveNumberUI()
    {
        waveNumberAnim.SetTrigger("Go");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (isGamePaused)
        {
            UnpauseGame();
        }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        if (isGamePaused)
        {
            UnpauseGame();
        }
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        SwapPanel(3);
        isGamePaused = true;
        Time.timeScale = 0.0f;
    }

    public void UnpauseGame()
    {
        SwapPanel(2);
        isGamePaused = false;
        Time.timeScale = 1.0f;
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
    public void HighlightButton(GameObject button)
    {
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(button);
    }
    public void ChangeWaveNumber(int number) //Called when NextWave is called
    {
        if (number != 0) //Just for now, until we decide how to implement other stuff
        {
            float ua = 5f * (1f + (number / 10f));
            t_wavenumber.text = "Wave " + number.ToString();
            t_waveNumberPauseMenu.text = "Wave " + number.ToString();
            upgradeamount = new float[] { ua , ua , ua };
            upgradeAmountDebug1.text = ("Upgrade Amount[0]: " + upgradeamount[0]);
            upgradeAmountDebug2.text = ("Upgrade Amount[1]: " + upgradeamount[1]);
            upgradeAmountDebug3.text = ("Upgrade Amount[2]: " + upgradeamount[2]);
            
        }
        else
            t_wavenumber.text = "Boss Level";
            upgradeamount = new float[] { 5 * number, 5 * number, 5 * number };

    }
    public void ClearField()
    {
        scoreTextObjects = GameObject.FindGameObjectsWithTag("Score");
        foreach (GameObject score in scoreTextObjects)
        {
            GameObject.Destroy(score);
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
            gameObject.GetComponent<WaveManager>().NextWave();
        }
    }
    public void SetPanels(int panelNo)
    {
        foreach (Transform child in UI.transform)
        {
            child.gameObject.SetActive(false);
        }
        UI.transform.GetChild(panelNo).gameObject.SetActive(true);
    }
    public void SwapPanel(int panel) //Superior because you can specify
    {
        SetPanels(panel);
        switch (panel)
        {
            case 1: //Reward panel
                ShowingRewards = true;
                HighlightButton(defaultButton);
                player.GetComponent<EntityHealth>().RestoreHealthToMaximum();
                player.GetComponent<EntityHealth>().RestoreShieldToMaximum();
                //Debug UI stays
                UI.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                //Player can see health/shields/lives when paused
                UI.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                break;
            case 2: //Game UI - health, wave stuff
                ShowingRewards = false;

                //Debug UI stays
                UI.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 3: //Pause Menu
                ShowingRewards = false;
                HighlightButton(defaultPauseButton);

                //Debug UI stays
                UI.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                //Player can see health/shields/lives when paused
                UI.gameObject.transform.GetChild(2).gameObject.SetActive(true);
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
        //Debug.Log(ca);
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
