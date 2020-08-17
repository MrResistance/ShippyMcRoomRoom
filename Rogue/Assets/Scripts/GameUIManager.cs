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
    public GameObject UI;
    public int ItemNumberChosen; //1, 2, or 3
    public string[] upgrades;
    public int[] upgradeamount;
    public bool ShowingRewards = false;
    public Button item1;
    public TextMeshProUGUI item1txt, item2txt, item3txt;


    void Start()
    {
        upgrades = new string[] { "movement", "attackspeed", "damage" };
        upgradeamount = new int[] { 5, 5, 5 };
        //InvokeRepeating("Pond", 2f, 3f);
    }
    void Pond()
    {
        //Debug.Log("Honlo, time is: " + Time.time);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeWaveNumber(int number)
    {
        if (number != 0)
        {
            t_wavenumber.text = "Wave " + number.ToString();
            upgradeamount = new int[] { 5 * number, 5 * number, 5 * number };
            item1txt.text = ("Movement Speed +" + upgradeamount[0]);
            item2txt.text = ("Attack Speed +" + upgradeamount[1]);
            item3txt.text = ("Attack Damage +" + upgradeamount[2]);
        }
        else
            t_wavenumber.text = "Boss Level";
            upgradeamount = new int[] { 5 * number, 5 * number, 5 * number };

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
    public void SwapPanel() //Not used now 'cause it just switched between two
    {
        if (UI.transform.GetChild(0).gameObject.activeInHierarchy) //If Game HUD is enabled, swap for reward
        {
            UI.transform.GetChild(0).gameObject.SetActive(false);
            UI.transform.GetChild(1).gameObject.SetActive(true);
            Debug.Log("Swapping to Rewards Panel");
        }
        else
        {
            UI.transform.GetChild(0).gameObject.SetActive(true);
            UI.transform.GetChild(1).gameObject.SetActive(false);
            Debug.Log("Swapping to Game Panel");
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
    int GetChosenItemAmount()
    {
        int ca;
        ca = upgradeamount[ItemNumberChosen-1]; //minus 1 because buttons start at 1 and array starts at 0
        return ca;
            
    }

}
