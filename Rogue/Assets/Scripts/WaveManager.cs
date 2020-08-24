using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class WaveManager : MonoBehaviour
{
    public int WaveNumber;
    public int remainingEnemies;
    public bool isWaveChanging;

    public GameObject EnemiesGO;
    public GameObject EnemyPrefab;
    public WeaponData Repeater; //Base weapon
    public List<EnemyData> enemyDataList;
    public List<WeaponData> weaponDataList;
    public GameObject ProjectilesGO;

    public List<GameObject> listEnemies;

    [ShowInInspector, PropertyRange(-50,50)]
    public int ManualSpawnX { get; set;}
    [ShowInInspector, PropertyRange(-31, 31)]
    public int ManualSpawnY { get; set; }


    [Button("Spawn Enemy")]
    private void SpawnEnemyButton()
    {
        ManualSpawnEnemy();
    }
    void Start()
    {
        StartWave();
        StartCoroutine(WaveUpdate());
    }
    void StartWave()
    {
        //Spawns player
        this.gameObject.GetComponent<PlayerManager>().RespawnPlayer();
        NextWave();
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
        //Destroy all projectiles so they don't kill the player
        DestroyAllProjectiles();
        //Stuff here to give them upgrades and logic to be implemented so NextWave only happens when player has chosen said upgrades
        this.gameObject.GetComponent<PlayerManager>().RemoveTempBuffsFromPlayer();
        this.gameObject.GetComponent<GameUIManager>().CalculateRewardShowOnUI(WaveNumber); //Generates rewards
        //If not final wave (which rn we don't necessarily have implemented
        //Hide game HUD, Show Reward Menu
        this.gameObject.GetComponent<GameUIManager>().SwapPanel(1);
        
    }
    public void NextWave() //Starts the next wave
    {
        Debug.Log("Next wave!");
        this.gameObject.GetComponent<GameUIManager>().SwapPanel(2);
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
            SpawnEnemy(enemyDataList[0],weaponDataList[0], esp);
        }
        if (WaveNumber >= 5)
        {
            for (int e = 0; e < WaveNumber-4; e++)
            {
                Vector3 esp = new Vector3(-40f + (e * 10), -20f, 0f);
                SpawnEnemy(enemyDataList[1], weaponDataList[1], esp);
            }
        }
    }

    void ManualSpawnEnemy()
    {
        Transform EnemySpawn = EnemiesGO.transform;
        Vector3 esp = new Vector3(ManualSpawnX, ManualSpawnY, 0f);
        GameObject Enemy = Instantiate(EnemyPrefab, esp, EnemySpawn.rotation);
        //Enemy.GetComponent<NPCMoverScript1>().cooldownShooting -= (WaveNumber / 10);
        Enemy.transform.parent = EnemiesGO.transform;
        Enemy.GetComponent<NPCMoverScript1>().ProjectilesGO = ProjectilesGO;
        Enemy.GetComponent<NPCMoverScript1>().weaponData = Repeater;
        listEnemies.Add(Enemy);
    }

    void CountEnemies()
    {
        remainingEnemies = EnemiesGO.transform.childCount;
        //Debug.Log("Enemy count is: " + remainingEnemies.ToString());
    }
    public void Reset() //When player dies (ono), this will reset the game
    {
        WaveNumber = 0;
        this.gameObject.GetComponent<PlayerManager>().RemovePermBuffs();
    }
    void DestroyAllProjectiles()
    {
        foreach(Transform child in ProjectilesGO.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    public void SpawnEnemy(EnemyData ed, WeaponData wd, Vector3 location)
    {
        Transform EnemySpawn = EnemiesGO.transform;
        GameObject Enemy = Instantiate(EnemyPrefab, location, EnemySpawn.rotation);
        //Enemy.GetComponent<NPCMoverScript1>().cooldownShooting -= (WaveNumber / 10);
        Enemy.transform.parent = EnemiesGO.transform;
        Enemy.GetComponent<NPCMoverScript1>().ProjectilesGO = ProjectilesGO;
        Enemy.GetComponent<NPCMoverScript1>().enemyData = ed;
        Enemy.GetComponent<NPCMoverScript1>().weaponData = wd;
        listEnemies.Add(Enemy);
    }
}
