using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    //Basic for now, as later on we will want the player to be able to have many weapons
    public WeaponData wd;
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float permattackspeedbonus;
    public float damage, permdamagebonus;
    public float dpadX, dpadY;
    private float nextFire = 0.0f, nextChangeWeaponInput = 0.0f;
    public int previousWeaponIndex, currentWeaponIndex, nextWeaponIndex;
    public SFX_Player sFX_Player;
    public GameUIManager gm;
    public List<WeaponData> weaponDataList;
    private void Awake()
    {
        //sFX_Player = GameObject.Find("SFX_Player").GetComponent<SFX_Player>();
        gm = GameObject.Find("GameManager").GetComponent<GameUIManager>();
    }
    private void Start()
    {
        ChangeWeapon(0);
    }
    // Update is called once per frame
    void Update()
    {
        dpadX = Input.GetAxis("Dpad X");
        dpadY = Input.GetAxis("Dpad Y");
        if (!gm.ShowingRewards)
        {
            if (Input.GetAxis("ControllerTriggers") > 0 && Time.time > nextFire)
            {
                FireWeapon();
            }
            if (!gm.isGamePaused)
            {
                if (dpadY > 0 && Time.time > nextChangeWeaponInput)
                {
                    GoToNextWeapon();
                }
                else if (dpadY < 0 && Time.time > nextChangeWeaponInput)
                {
                    GoToPreviousWeapon();
                }
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
        if ((currentWeaponIndex) -1 <= 0)
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
        gm.UpdatePreviousWeaponUI(previousWeaponIndex);
        gm.UpdateWeaponUI(currentWeaponIndex);
        gm.UpdateNextWeaponUI(nextWeaponIndex);
    }
    void FireWeapon()
    {
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
        foreach (GameObject ge in gm.GetComponent<WaveManager>().listEnemies)
        {
            if (Vector3.Distance(ge.transform.position,transform.position) < minDist && ge.GetComponent<NPCMoverScript1>().isWithinArenaBoundary())
            {
                enemy = ge;
                minDist = Vector3.Distance(ge.transform.position, transform.position);
            }
        }
        return enemy;
    }
}
