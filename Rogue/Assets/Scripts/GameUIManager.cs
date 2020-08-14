using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI t_wavenumber;
    public GameObject UI;
    public int ItemNumberChosen; //1, 2, or 3
    public string[] upgrades;
    public int[] upgradeamount;


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
        }
        else
            t_wavenumber.text = "Boss Level";

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
    public void SwapPanel()
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
