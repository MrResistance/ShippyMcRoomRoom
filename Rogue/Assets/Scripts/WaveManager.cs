using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class WaveManager : MonoBehaviour
{
    public int WaveNumber;
    public int remainingEnemies;
    public bool isWaveChanging;


    //Parent GameObject for all allies to the player
    public GameObject AlliesGO;
    //Parent GameObject for all projectiles
    public GameObject ProjectilesGO;
    //Parent GameObject for all Enemies
    public GameObject EnemiesGO;

    //Prefab for spawning any enemy
    public GameObject EnemyPrefab;
    //Types of enemies
    public List<EnemyData> enemyDataList;
    //Types of weapons
    public List<WeaponData> weaponDataList;

    
    public List<GameObject> listEnemies;
    public List<GameObject> listAllies;


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
        listEnemies.Clear();
        
    }
    public void NextWave() //Starts the next wave
    {
        //Resets health of player back to maximum
        this.gameObject.GetComponent<PlayerManager>().RestoreHealthToMaximum();
        //Hides Reward screen and shows game UI;
        this.gameObject.GetComponent<GameUIManager>().SwapPanel(2);
        //Increases wave number
        WaveNumber++;
        //Clears list of anything left
        listEnemies.Clear();
        //Changes the wave number displayed
        this.gameObject.GetComponent<GameUIManager>().ChangeWaveNumber(WaveNumber);
        //Spawns enemies for the wave
        SpawnNPCs();
        isWaveChanging = false; 
    }
    void SpawnNPCs()
    {
        //ENEMY STUFF
        Transform EnemySpawn = EnemiesGO.transform;
        for (int e = 0; e < WaveNumber;e++)
        {
            Vector3 esp = new Vector3(-40f+(e*10), 25f,0f);
            SpawnNPC(enemyDataList[0],weaponDataList[0], esp,"enemy");
        }
        if (WaveNumber >= 5) //Cannon spawner
        {
            for (int e = 0; e < WaveNumber-4; e++)
            {
                Vector3 esp = new Vector3(-40f + (e * 10), -20f, 0f);
                SpawnNPC(enemyDataList[1], weaponDataList[1], esp,"enemy");
            }
        }
        //ALLIED STUFF - mainly for testing to see if allies vs enemies works

        for (int e = 0; e < WaveNumber-3; e++)
        {
            Vector3 esp = new Vector3(-40f + (e * 10), -25f, 0f);
            SpawnNPC(enemyDataList[0], weaponDataList[0], esp,"allied");
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
        Enemy.GetComponent<NPCMoverScript1>().wd = weaponDataList[0];
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
    public void SpawnNPC(EnemyData ed, WeaponData wd, Vector3 location,string side)
    {
        Transform NPCSpawn = EnemiesGO.transform;
        GameObject NPC = Instantiate(EnemyPrefab, location, NPCSpawn.rotation);
        NPC.GetComponent<EntityHealth>().thisObjectPoints = ed.points;
        //Enemy.GetComponent<NPCMoverScript1>().cooldownShooting -= (WaveNumber / 10);
        if (side == "enemy")
        {
            NPC.name = "EnemyNPC-" + WaveNumber.ToString();
            NPC.transform.parent = EnemiesGO.transform;
            listEnemies.Add(NPC);
            NPC.tag = "Enemy";
            NPC.layer = 11;
        }
        if (side == "allied")
        {
            NPC.name = "AlliedNPC-"+WaveNumber.ToString();
            NPC.transform.parent = AlliesGO.transform;
            listAllies.Add(NPC);
            NPC.tag = "Ally";
            NPC.layer = 10;
            NPC.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        NPC.GetComponent<NPCMoverScript1>().npcSide = side;
        NPC.GetComponent<NPCMoverScript1>().ProjectilesGO = ProjectilesGO; //Specifying where projectiles go
        NPC.GetComponent<NPCMoverScript1>().enemyData = ed;
        NPC.GetComponent<NPCMoverScript1>().wd = wd;
        NPC.GetComponent<NPCMoverScript1>().wm = this.gameObject.GetComponent<WaveManager>();
        NPC.GetComponent<NPCMoverScript1>().wavenumberSpawnedIn = WaveNumber;

    }
}
