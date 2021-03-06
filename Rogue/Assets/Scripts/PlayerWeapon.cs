﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PlayerWeapon : MonoBehaviour
{
    //Basic for now, as later on we will want the player to be able to have many weapons
    public WeaponData wd;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public Animator weaponSelectAnim;
    public float permattackspeedbonus;
    public float damage, permdamagebonus;
    public float dpadX, dpadY;
    private float nextFire = 0.0f, nextChangeWeaponInput = 0.0f;
    public int previousWeaponIndex, currentWeaponIndex, nextWeaponIndex;
    public SFX_Player sFX_Player;
    public GameUIManager gm;
    public PlayerTopDownController pdtc;
    public List<WeaponData> weaponDataList;
    private Stopwatch stopwatch;
    public bool Up, Down;
    public StatTrak statTrak;
    private void Awake()
    {
        statTrak = GameObject.Find("StatTrak").GetComponent<StatTrak>();
        //sFX_Player = GameObject.Find("SFX_Player").GetComponent<SFX_Player>();
        gm = GameObject.Find("GameManager").GetComponent<GameUIManager>();
        weaponSelectAnim = GameObject.Find("Weapons").GetComponent<Animator>();
        pdtc = GetComponent<PlayerTopDownController>();
    }
    private void Start()
    {
        stopwatch = new Stopwatch();
        ChangeWeapon(0);
    }
    // Update is called once per frame
    void Update()
    {
        dpadX = Input.GetAxis("Dpad X");
        dpadY = Input.GetAxis("Dpad Y");
        if (!gm.ShowingRewards)
        {
            if (Input.GetAxis("ControllerTriggers") > 0 || Input.GetAxis("Fire1") > 0)
            {
                if (Time.time > nextFire)
                FireWeapon();
            }
            if (!gm.isGamePaused)
            {
                if ((dpadY < 0 || Input.mouseScrollDelta.y < 0)&& stopwatch.ElapsedMilliseconds > 500)
                {
                    //UP
                    Up = true;
                    GoToNextWeapon();
                    stopwatch.Reset();
                }
                else if ((dpadY > 0 || Input.mouseScrollDelta.y > 0) && stopwatch.ElapsedMilliseconds > 500)
                {
                    //DOWN
                    Down = true;
                    GoToPreviousWeapon();
                    stopwatch.Reset();
                }
                stopwatch.Start();
            }
        }
    }

    public void GoToNextWeapon()
    {
        if ((currentWeaponIndex) + 1 >= weaponDataList.Count)
        {
            ChangeWeapon(0);
        }
        else
        {
            ChangeWeapon(currentWeaponIndex + 1);
        }
    }
    public void GoToPreviousWeapon()
    {
        if ((currentWeaponIndex - 1) < 0)
        {
            ChangeWeapon(weaponDataList.Count - 1);
        }
        else
        {
            ChangeWeapon(currentWeaponIndex - 1);
        }
    }
    public void ChangeWeapon(int WeaponIndex)
    {
        currentWeaponIndex = WeaponIndex;
        if ((currentWeaponIndex - 1) < 0)
        {
            previousWeaponIndex = weaponDataList.Count - 1;
        }
        else
        {
            previousWeaponIndex = currentWeaponIndex - 1;
        }
        if ((currentWeaponIndex + 1) >= weaponDataList.Count)
        {
            nextWeaponIndex = 0;
        }
        else
        {
            nextWeaponIndex = currentWeaponIndex + 1;
        }
        wd = weaponDataList[currentWeaponIndex];
        gm.UpdatePreviousWeaponUI(weaponDataList[previousWeaponIndex]);
        gm.UpdateWeaponUI(weaponDataList[currentWeaponIndex]);
        gm.UpdateNextWeaponUI(weaponDataList[nextWeaponIndex]);
        if (Up)
        {
            weaponSelectAnim.SetTrigger("Up");
        }
        else if (Down)
        {
            weaponSelectAnim.SetTrigger("Down");
        }
        Up = false;
        Down = false;
    }
    void FireWeapon()
    {
        statTrak.playerShotsFired++;
        //sFX_Player.PlayRandomSound();
        nextFire = Time.time + CalculateAttackSpeed();
        //For amount of projectiles
        for (int p = 0; p < wd.projectileCount; p++)
            { 
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.layer = 10; //The Player Projectile layer
            projectile.tag = "Player Projectile"; 
            projectile.GetComponent<Projectile>().wd = wd;
            projectile.GetComponent<Projectile>().damageBonus = permdamagebonus;
            projectile.GetComponent<Projectile>().firepoint = firePoint;
            projectile.GetComponent<Projectile>().SetParent(this.gameObject);
            //Spread
            if (wd.spreadType == "even")
            {
                projectile.GetComponent<Projectile>().spreadNumber = p;
            }
            //Tracking
            if (wd.isTrackingProjectile == true)
            {
                projectile.GetComponent<Projectile>().target = GetClosestEnemy();
            }
        }

        //projectile.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

        //Projectile now handles its own damage
        //projectile.gameObject.GetComponent<Projectile>().damage = damage + permdamagebonus;
        //Projectile handles its own thrusting
        //Rigidbody2D rbproj = projectile.GetComponent<Rigidbody2D>();
        //rbproj.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);
    }
    public float CalculateAttackSpeed()
    {
        float number;
        number = wd.rateoffire - (permattackspeedbonus / 100);
        //Debug.Log("Attack speed = " + number.ToString());
        return number;
    }
    public float CalculateDamage()
    {
        float number;
        number = damage + permdamagebonus;
        if (number >= 100)
            number = 100f;
        return number;
    }
    GameObject GetClosestEnemy()
    {
        GameObject enemy = null;
        float minDist = Mathf.Infinity;
        Vector3 TargetLocation;
        if (pdtc.isControllerEnabled)
        {
            TargetLocation = transform.position;
        }
        else
        {
            TargetLocation = pdtc.mouseposition;
        }
        foreach (GameObject ge in gm.GetComponent<WaveManager>().listEnemies)
        {
            if (Vector3.Distance(ge.transform.position, TargetLocation) < minDist && ge.GetComponent<NPCMoverScript1>().isWithinArenaBoundary())
            {
                enemy = ge;
                minDist = Vector3.Distance(ge.transform.position, TargetLocation);
            }
        }
        return enemy;
    }
}
