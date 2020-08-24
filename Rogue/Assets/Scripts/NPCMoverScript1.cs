﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Pathfinding;

public class NPCMoverScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyData enemyData;
    public WeaponData weaponData;
    
    
    public Keyframe[] keyframesX;
    public Keyframe[] keyframesY;
    float timeElapsed;
    bool isMoving;
    public GameObject player;
    Vector2 distanceToPlayer;
    public float distanceToPlayerfloat;

    Rigidbody2D rb;
    public Transform firePoint; //For firing from
    public float maximumShootingDistance, projectileSpeed = 1f;
    public GameObject projectilePrefab, 
        tHealth; //text health
    public AudioSource audioSource;

    public GameObject ProjectilesGO;

    float offsetRateOfFire; //To stop enemies shooting at the same speed
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    private void Start()
    {
        firePoint = transform.GetChild(0);
        tHealth = transform.GetChild(1).GetChild(0).gameObject;
        StartCoroutine(TargetEnemyOfNPC());
        this.gameObject.GetComponent<EntityHealth>().health = enemyData.healthMaximum;
        this.gameObject.GetComponent<EntityHealth>().healthMaximum = enemyData.healthMaximum;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = enemyData.sprite;
        transform.localScale = new Vector3(enemyData.scale, enemyData.scale, enemyData.scale);
        this.GetComponent<AIPath>().maxSpeed = enemyData.speedMovement;
        float low = (weaponData.rateoffire / 100) * 5f * -1f;
        float high = (weaponData.rateoffire / 100) * 5f;



        offsetRateOfFire = Random.RandomRange(low,high);
    }

    void Update()
    {
        //Move(); //Does stuff in A*
        Rotate();
        tHealth.GetComponent<TextMeshProUGUI>().text = GetComponent<EntityHealth>().health.ToString();
    }

    IEnumerator TargetEnemyOfNPC()
    {
        while (true)
        {
            if (player != null)
            {
                GetComponent<AIDestinationSetter>().target = player.transform;
                if (isCloseEnoughToShoot() && isCurrentlyLookingAtTarget())
                {
                    ShootEnemyOfNPC();
                }
            }
            else
            {
                player = GameObject.Find("Player");
            }
            yield return new WaitForSeconds(weaponData.rateoffire+ offsetRateOfFire);
        }
    }
    void ShootEnemyOfNPC()
    {
        //Debug.Log("Firing at player");
        audioSource.PlayOneShot(audioSource.clip);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.transform.position, WeaponFiringAngle());
        //No longer needed as projeectile handles its own thrusting heheh \o/
        //Rigidbody2D rbproj = projectile.GetComponent<Rigidbody2D>();
        projectile.GetComponent<Projectile>().firepoint = firePoint.transform;

        //Temp layer specifying - this needs to be changed later should we want to use this script to make friendly NPCs
        projectile.layer = 11; //The Enemy Projectile layer

        projectile.GetComponent<Projectile>().wd = weaponData;
        projectile.transform.parent = ProjectilesGO.transform;
    }
    void GetDistanceToPlayer()
    {
        if (player != null)
        {
            distanceToPlayer = player.transform.position - transform.position;
            distanceToPlayerfloat = Vector2.Distance(player.transform.position, transform.position);
            //Debug.Log(distanceToPlayerfloat);
        }
    }
    void Move()
    {
        
    }
    void Rotate()
    {
        GetDistanceToPlayer();
        if (player != null)
        {
            ////Old
            float angle = Mathf.Atan2(distanceToPlayer.y, distanceToPlayer.x) * Mathf.Rad2Deg - 90f;
            //rb.rotation = angle;

            //New
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 angle2 = transform.rotation.eulerAngles;

            //Debug.Log("Angle =" + angle.ToString() + ",Angle2 = "+ angle2.ToString());
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * enemyData.speedRotation);
        }
        else
        {
            //Can just do nothing I guess
            //rb.rotation = 0;
        }
    }
    bool isCloseEnoughToShoot() //So entity knows it can hit the target
    {
        if (distanceToPlayerfloat < weaponData.range)
        { 
            return true;
        }
        return false;

    }
    bool isCurrentlyLookingAtTarget() //So the entity is not just shooting aimlessly
    {
        return true;
    }
    Quaternion WeaponFiringAngle() //Adds weapon spread to rotation
    {
        Quaternion quat = firePoint.transform.rotation;
        //Somehow add the spread - weaponData.spread
        
        //Debug.Log(quat.ToString());
        return quat;
    }
}
