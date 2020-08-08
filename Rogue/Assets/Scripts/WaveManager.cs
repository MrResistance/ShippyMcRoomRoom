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
                NextWave();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void NextWave()
    {
        Debug.Log("Next wave!");
        WaveNumber++;
        this.gameObject.GetComponent<GameUIManager>().ChangeWaveNumber(WaveNumber);
        SpawnEnemies();
        isWaveChanging = false; 
    }
    void SpawnEnemies()
    {
        Transform EnemySpawn = EnemiesGO.transform;
        
        for (int e = 0; e < WaveNumber;e++)
        {
            EnemySpawn.position = new Vector3(-40f+(e*10), 25f,0f);
            GameObject Enemy = Instantiate(EnemyPrefab, EnemySpawn.position, EnemySpawn.rotation);
            Enemy.transform.parent = EnemiesGO.transform;
            listEnemies.Add(Enemy);
        }
    }
    void CountEnemies()
    {
        
        remainingEnemies = EnemiesGO.transform.childCount;
        Debug.Log("Enemy count is: " + remainingEnemies.ToString());
    }
}
