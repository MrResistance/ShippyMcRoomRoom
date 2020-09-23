using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatTrak : MonoBehaviour
{
    public float playerShotsFired, playerShotsHit, playerShotsMissed, timesPlayerWasHit, damageGiven, damageTaken;
    public int playerFavouriteWeapon;
    public string timeTakenToCompleteWave;
    public TextMeshProUGUI[] statTextObjects;
    public string[] statTextStrings;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(LogStats());
    }
    public IEnumerator LogStats()
    {
        while (true)
        {
            statTextStrings[0] = ("Shots Fired " + playerShotsFired.ToString());
            statTextStrings[1] = ("Shots hit " + playerShotsHit.ToString());
            statTextStrings[2] = ("Shots missed " + playerShotsMissed.ToString());
            statTextStrings[3] = ("You were hit " + timesPlayerWasHit.ToString() + " times!");
            statTextStrings[4] = ("You did " + damageGiven.ToString() + " damage!");
            statTextStrings[5] = ("You took " + Mathf.Round(damageTaken).ToString() + " damage!");
            statTextStrings[6] = ("Your favourite weapon was " + playerFavouriteWeapon.ToString());
            statTextStrings[7] = ("You took " + timeTakenToCompleteWave + " to complete the wave!");
            yield return new WaitForEndOfFrame();
        }
    }
    public void StartShowing()
    {
        anim.SetBool("Going", true);
    }
    public void StopShowing()
    {
        anim.SetBool("Going", false);
    }
    public void UpdateTextObjects()
    {
        for (int i = 0; i < statTextObjects.Length; i++)
        {
            statTextObjects[i].text = statTextStrings[i];
        }
    }
}
