using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Pathfinding;

public class NPCMoverScript1 : MonoBehaviour
{
    public string npcSide; //"enemy","allied"
    public WaveManager wm; //For getting the most updated lists of enemies to npc

    public GameObject hsCanvas;

    public EnemyData enemyData;
    public WeaponData wd;

    public int boundaryXmin = -40;
    public int boundaryXmax = 40;
    public int boundaryYmin = -25;
    public int boundaryYmax = 25;


    float timeElapsed;
    bool isMoving;

    //New targetting
    public GameObject target;
    Vector2 distanceToTarget;
    public float distanceToTargetFloat;

    Rigidbody2D rb;
    public Transform firePoint; //For firing from

    //To be outdated via getting the max distance one can shoot from the weapon
    //Max shooting distance should be projectile wd.speed x wd.projectileLifetime
    public float maximumShootingDistance;
    public GameObject projectilePrefab, 
        tHealth; //text health
    public AudioSource audioSource;
    //The GO parent where all projectiles are stored
    public GameObject ProjectilesGO;

    float offsetRateOfFire; //To stop enemies shooting at the same speed

   public int wavenumberSpawnedIn;
    /// <summary>
    /// /////////////////////////////////////////////// METHODS /////////////////////////////////////////////
    /// </summary>
    /// 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame

    private void Start()
    {
        SetupNPC();
    }
    void SetupNPC()
    {
        firePoint = transform.GetChild(0);
        tHealth = transform.GetChild(1).GetChild(0).gameObject;
        distanceToTargetFloat = Mathf.Infinity;
        this.gameObject.GetComponent<EntityHealth>().health = enemyData.healthMaximum;
        this.gameObject.GetComponent<EntityHealth>().healthMaximum = enemyData.healthMaximum;
        this.gameObject.GetComponent<EntityHealth>().shield = enemyData.shieldMaximum;
        this.gameObject.GetComponent<EntityHealth>().shieldMaximum = enemyData.shieldMaximum;

        this.gameObject.GetComponent<SpriteRenderer>().sprite = enemyData.sprite;
        transform.localScale = new Vector3(enemyData.scale, enemyData.scale, enemyData.scale);
        this.GetComponent<AIPath>().maxSpeed = enemyData.speedMovement;
        //Offsets rate of fire for NPC - so it does not shoot at the same time as everything else
        float low = (wd.rateoffire / 100) * 5f * -1f;
        float high = (wd.rateoffire / 100) * 5f;
        offsetRateOfFire = Random.RandomRange(low, high);
        //For getting a target
        StartCoroutine(TargetEnemyOfNPC());
        StartCoroutine(ShootEnemyOfNPC());
        
    }

    void Update()
    {
        //Move(); //Does stuff in A*
        if (wm.isWaveChanging == false)
        { 
            Rotate();
            tHealth.GetComponent<TextMeshProUGUI>().text = GetComponent<EntityHealth>().health.ToString("F1");
        }
    }
    
    void ShootTarget()
    {
        //Debug.Log("Firing at player");
        audioSource.PlayOneShot(audioSource.clip);

        for (int p = 0; p < wd.projectileCount; p++)
        { 
            GameObject projectile = Instantiate(projectilePrefab, firePoint.transform.position, WeaponFiringAngle());
            projectile.GetComponent<Projectile>().firepoint = firePoint.transform;
            projectile.GetComponent<Projectile>().wd = wd;
            projectile.transform.parent = ProjectilesGO.transform;
            //NPC Side stuff - only for NPCs
            if (npcSide == "enemy")
            {
                projectile.tag = "Enemy Projectile";
                projectile.layer = 11;
            }
            if (npcSide == "allied")
            {
                projectile.tag = "Player Projectile";
                projectile.layer = 10;
            }
            //Spread
            if (wd.spreadType == "even")
            {
                projectile.GetComponent<Projectile>().spreadNumber = p;
            }
            //Targetting
            if (wd.isTrackingProjectile == true)
            {
                projectile.GetComponent<Projectile>().target = target;
            }
        }
    }
    
    void Rotate()
    {
        if (target != null)
        {
            GetDistanceToTarget();
            float angle = Mathf.Atan2(distanceToTarget.y, distanceToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 angle2 = transform.rotation.eulerAngles;
            
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * enemyData.speedRotation);

            hsCanvas.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        }
        else
        {
            //Can just do nothing I guess
            //rb.rotation = 0;
        }
    }
    bool isCloseEnoughToShoot() //So entity knows it can hit the target
    {
        if (wd.range == 0) //The weapon has no limit therefore it can shoot as far as it wants
        {
            return true;
        }
        if (distanceToTargetFloat < wd.range)
        { 
            return true;
        }
        return false;

    }
    bool isCurrentlyLookingAtTarget() //So the entity is not just shooting aimlessly
    {
        return true;
    }
    bool isWithinArenaBoundary()
    {
        if (transform.position.x >= boundaryXmin && transform.position.x <= boundaryXmax)
        {
            if (transform.position.y >= boundaryYmin && transform.position.y <= boundaryYmax)
            {
                return true;
            }
        }
        return false;
    }
    Quaternion WeaponFiringAngle() //Adds weapon spread to rotation - moved to projectile
    {
        Quaternion quat = firePoint.transform.rotation;
        //Somehow add the spread - weaponData.spread
        //quat *= Quaternion.Euler(new Vector3(0f, 0f, 5f));
        //Debug.Log(quat.ToString());
        return quat;
    }
    bool npcHasEnemies() //Used to determine whether there is something for the NPC to target
    {
        if (npcSide == "enemy")
        {
            if (wm.listAllies.Count != 0)
            {
                return true;
            }
        }
        else if (npcSide == "allied")
        {
            if (wm.listEnemies.Count != 0 && wm.EnemiesGO.transform.childCount != 0)
            {
                return true;
            }
        }
        //No target available
        distanceToTargetFloat = Mathf.Infinity; //So getting shorter distance will be easier
        return false;
    }
    //For getting a target for the NPC
    IEnumerator TargetEnemyOfNPC()
    {
        while (true)
        {
            if (npcHasEnemies() && wm.isWaveChanging == false)
            {
                distanceToTargetFloat = Mathf.Infinity;
                if (npcSide == "enemy")
                {
                    //Get closest
                    foreach (GameObject go in wm.listAllies)
                    {
                        if (go != null)
                        { 
                            Vector2 distanceToGO = go.transform.position - transform.position;
                            float distanceToGOFloat = Vector2.Distance(go.transform.position, transform.position);
                            if (distanceToGOFloat < distanceToTargetFloat)
                            {
                                target = go.gameObject;
                                GetComponent<AIDestinationSetter>().target = target.transform; //Moves NPC to target
                            }
                        }
                    }
                }
                else if (npcSide == "allied")
                {
                    //Get closest
                    //Make that the target
                    
                    for (int e = 0; e < wm.listEnemies.Count;e++)
                    {
                        GameObject go = wm.listEnemies[e].gameObject;
                        if (go != null)
                        {
                            Vector2 distanceToGO = go.transform.position - transform.position;
                            float distanceToGOFloat = Vector2.Distance(go.transform.position, transform.position);
                            if (distanceToGOFloat < distanceToTargetFloat)
                            {
                                target = go.gameObject;
                                GetComponent<AIDestinationSetter>().target = target.transform; //Moves NPC to target
                            }
                        }
                    }
                }
                if (target != null)
                GetComponent<AIDestinationSetter>().target = target.transform; //Moves NPC to target
                yield return new WaitForSeconds(wd.rateoffire * 3); //Ensures that the target is shot at least 3 times before switching
            }
            else //No enemies for NPC available
            {
                //Guess no target.
                yield return new WaitForSeconds(1f); //Waits one second before trying again
            }
            
        }
    }
    //For shooting the target
    IEnumerator ShootEnemyOfNPC()
    {
        while (true)
        {
            if (CanShoot())
            {
                ShootTarget();
            }
            yield return new WaitForSeconds(wd.rateoffire + offsetRateOfFire);
        }
    }
    bool CanShoot() //Checks to make sure the NPC passes all checks before they can shoot;
    {
        if (wm.isWaveChanging == false)
        {
            if (target != null)
            {
                if (isCloseEnoughToShoot())
                {
                    if (isCurrentlyLookingAtTarget())
                    {
                        if (isWithinArenaBoundary())
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    void GetDistanceToTarget()
    {
        distanceToTarget = target.transform.position - transform.position;
        distanceToTargetFloat = Vector2.Distance(target.transform.position, transform.position);
    }

}
