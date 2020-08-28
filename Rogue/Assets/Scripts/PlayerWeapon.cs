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
    private float nextFire = 0.0f;
    public AudioSource audioSource;
    public SFX_Player sFX_Player;
    public GameUIManager gm;
    private void Awake()
    {
        //sFX_Player = GameObject.Find("SFX_Player").GetComponent<SFX_Player>();
        gm = GameObject.Find("GameManager").GetComponent<GameUIManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!gm.ShowingRewards)
        {
            if (Input.GetAxis("ControllerTriggers") > 0 && Time.time > nextFire)
            {
                FireWeapon();
            }
        }
    }
    void FireWeapon()
    {
        //sFX_Player.PlayRandomSound();
        audioSource.PlayOneShot(audioSource.clip);
        nextFire = Time.time + CalculateAttackSpeed();
        //For amount of projectiles
        for (int p = 0; p < wd.projectileCount; p++)
            { 
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.layer = 10; //The Player Projectile layer
            projectile.GetComponent<Projectile>().wd = wd;
            projectile.GetComponent<Projectile>().damageBonus = permdamagebonus;
            projectile.GetComponent<Projectile>().firepoint = firePoint;
            if (wd.isTrackingProjectile == true)
            {
                projectile.GetComponent<Projectile>().target = GetClosestEnemy();
            }
            if (wd.spreadType == "even")
            {
                projectile.GetComponent<Projectile>().spreadNumber = p;
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
        if (number >= 100)
            number = 100f;
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
            if (Vector3.Distance(ge.transform.position,transform.position) < minDist)
            {
                enemy = ge;
            }
        }
        return enemy;
    }
}
