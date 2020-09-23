using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatTrak : MonoBehaviour
{
    public GameObject statTextPrefab;
    public TextMeshProUGUI prefabText;
    public float playerShotsFired, playerShotsHit, playerShotsMissed, timesPlayerWasHit, damageGiven, damageTaken;
    public int playerFavouriteWeapon;
    public float amountOfStats;
    public string[] statTextStrings;
    public double timeTakenToCompleteWave;
    public bool finishedShowingStats = false;
    // Start is called before the first frame update
    void Start()
    {
        amountOfStats = 8;
    }

    // Update is called once per frame
    void Update()
    {
        statTextStrings[0] = ("Shots Fired: " + playerShotsFired.ToString());
        statTextStrings[1] = ("Shots hit: " + playerShotsHit.ToString());
        statTextStrings[2] = ("Shots missed: " + playerShotsMissed.ToString());
        statTextStrings[3] = ("You were hit " + timesPlayerWasHit.ToString() + " times!");
        statTextStrings[4] = ("You did " + damageGiven.ToString() + " damage!");
        statTextStrings[5] = ("You took " + damageTaken.ToString() + " damage!");
        statTextStrings[6] = ("Your favourite weapon was: " + playerFavouriteWeapon.ToString());
        statTextStrings[7] = ("You took " + timeTakenToCompleteWave.ToString() + " to complete the wave!");
    }
    public void BeginShowingStats()
    {
        for (int i = 0; i < amountOfStats; i++)
        {
            SpawnTextLine(statTextStrings[i], i);
        }
    }

    public void EndShowingStats()
    {
        finishedShowingStats = true;
    }
    public void SpawnTextLine(string text, float yIncrementAmount)
    {
        GameObject textLine = Instantiate(statTextPrefab, gameObject.transform.position, gameObject.transform.rotation);
        prefabText = textLine.GetComponent<TextMeshProUGUI>();
        prefabText.text = text;
        textLine.transform.SetParent(GameObject.Find("Text").GetComponent<Transform>().transform);
        textLine.transform.localScale = new Vector3(1,1,1);
        textLine.transform.position = new Vector3 (textLine.transform.position.x, textLine.transform.position.y - (yIncrementAmount * 5), textLine.transform.position.z);
    }
}
