using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int WaveNumber;
    public int remainingEnemies;
    public bool isWaveChanging;

    public GameObject EnemiesGO;
    public GameObject EnemyPrefab;

    public List<GameObject> listEnemies;
    void Start()
    {
        NextWave();
        StartCoroutine(WaveUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaveUpdate()
    {
        while (true)
        {
            CountEnemies();
            if (remainingEnemies == 0 && isWaveChanging == false)
            {
                isWaveChanging = true;
                EndWave();
                
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    void EndWave()
    {
        //Stuff here to give them upgrades and logic to be implemented so NextWave only happens when player has chosen said upgrades
        this.gameObject.GetComponent<PlayerManager>().RemoveTempBuffsFromPlayer();
        NextWave();
    }
    public void NextWave() //Starts the next wave
    {
        Debug.Log("Next wave!");
        WaveNumber++;
        listEnemies.Clear();
        this.gameObject.GetComponent<GameUIManager>().ChangeWaveNumber(WaveNumber);
        SpawnEnemies();
        isWaveChanging = false; 
    }
    void SpawnEnemies()
    {
        Transform EnemySpawn = EnemiesGO.transform;


        for (int e = 0; e < WaveNumber;e++)
        {
            Vector3 esp = new Vector3(-40f+(e*10), 25f,0f);
            GameObject Enemy = Instantiate(EnemyPrefab, esp, EnemySpawn.rotation);
            //Enemy.GetComponent<NPCMoverScript1>().cooldownShooting -= (WaveNumber / 10);
            Enemy.transform.parent = EnemiesGO.transform;
            listEnemies.Add(Enemy);
        }
        foreach (GameObject enemy in listEnemies)
        {
            float cds = (float)WaveNumber / 10;
            enemy.GetComponent<NPCMoverScript1>().cooldownShooting -= Random.Range(cds/10, cds);
            Debug.Log(enemy.GetComponent<NPCMoverScript1>().cooldownShooting.ToString());
        }
    }
    void CountEnemies()
    {
        
        remainingEnemies = EnemiesGO.transform.childCount;
        //Debug.Log("Enemy count is: " + remainingEnemies.ToString());
    }
}
