using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float attackspeed = 0.5f, projectileSpeed = 1f, permattackspeedbonus;
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
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        //projectile.gameObject.GetComponent<Projectile>().damage = damage + permdamagebonus;
        Rigidbody2D rbproj = projectile.GetComponent<Rigidbody2D>();
        rbproj.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);
    }
    float CalculateAttackSpeed()
    {
        float number;
        number = attackspeed - (permattackspeedbonus / 100);
        Debug.Log("Attack speed = " + number.ToString());
        return number;
    }
}
