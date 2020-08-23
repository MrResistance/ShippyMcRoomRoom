using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI t_wavenumber;
    public GameObject UI, defaultButton;
    public EventSystem eventSystem;
    public Sprite buttonDefaultSprite;
    public int ItemNumberChosen; //1, 2, or 3
    public string[] upgrades;
    public float[] upgradeamount;
    public bool ShowingRewards = false;
    public Button item1;
    public TextMeshProUGUI item1txt, item2txt, item3txt, upgradeAmountDebug1, upgradeAmountDebug2, upgradeAmountDebug3;

    private void Awake()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        defaultButton = GameObject.FindGameObjectWithTag("Default Button");
    }
    void Start()
    {
        upgrades = new string[] { "movement", "attackspeed", "damage" };
        upgradeamount = new float[] { 5f * (1f + (0.1f)), 5f * (1f + (0.1f)), 5f * (1f + (0.1f)) };
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
            item1txt.text = ("Movement Speed +" + upgradeamount[0]);
            item2txt.text = ("Attack Speed +" + upgradeamount[1]);
            item3txt.text = ("Attack Damage +" + upgradeamount[2]);
        }
        else
            t_wavenumber.text = "Boss Level";
            upgradeamount = new float[] { 5 * number, 5 * number, 5 * number };

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
    public void CalculateRewardShowOnUI()
    {

    }
}
