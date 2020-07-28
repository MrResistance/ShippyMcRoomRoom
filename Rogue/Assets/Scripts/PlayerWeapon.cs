using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject projectilePrefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            FireWeapon();
        }
    }
    void FireWeapon()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rbproj = projectile.GetComponent<Rigidbody2D>();
        rbproj.AddForce(firePoint.right * 20f, ForceMode2D.Impulse);
    }
}
