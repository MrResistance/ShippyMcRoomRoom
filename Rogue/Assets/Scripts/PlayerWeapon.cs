using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 0.5f, projectileSpeed = 1f;
    private float nextFire = 0.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("ControllerTriggers") > 0 && Time.time > nextFire)
        {
            FireWeapon();
        }
    }
    void FireWeapon()
    {
        nextFire = Time.time + fireRate;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbproj = projectile.GetComponent<Rigidbody2D>();
        rbproj.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);
    }
}
