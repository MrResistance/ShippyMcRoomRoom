﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public WeaponData wd;
    public float damageBonus;
    public Transform firepoint;
    public float destroyTimer = 10f;
    public SpriteRenderer sr;
    private BoxCollider2D bc;
    public GameObject target;
    //For if weapon has 'even' spread
    public float spreadNumber;
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        //For spread
        transform.rotation = CalculateRotationForProjectile();

        sr.sprite = wd.sprite;
        sr.color = wd.colourSprite;
        transform.localScale = new Vector3(wd.scale,wd.scale,wd.scale);
        this.gameObject.GetComponent<EntityHealth>().health = wd.healthProjectile;
        Rigidbody2D rbproj = this.gameObject.GetComponent<Rigidbody2D>();
        rbproj.AddForce(this.gameObject.transform.right * wd.speed, ForceMode2D.Impulse);
        if (wd.lifetimeProjectile > 0)
        { 
            Destroy(this.gameObject, wd.lifetimeProjectile);
        }
        if (wd.isTrackingProjectile == true && target != null)
        {
            StartCoroutine(TrackTarget());
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Boundary"))
        {
            Destroy(this.gameObject);
        }

        //Check if hitting another projectile
        if (collision.gameObject.tag.Contains("Projectile"))
        {
            //this.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(CalculateDamage());
            //Debug.Log("Giving " + CalculateDamange().ToString() + " to " + collision.gameObject.name);
            //Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }
        //Check if enemy
        if (collision.gameObject.tag == ("Player") || collision.gameObject.tag == ("Enemy"))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(CalculateDamage());
            sr.enabled = false;
            bc.enabled = false;
            Destroy(this.gameObject, 3);
        }
        //sr.enabled = false;
        //Destroy(this.gameObject, 3);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
    public float CalculateDamage()
    {
        float number;
        number = wd.damage + damageBonus;
        number /= wd.projectileCount;
        return number;
    }
    IEnumerator TrackTarget()
    {
        //Rotate towards
        while(true)
        { 
            Vector2 DistanceToTarget = GetDistanceToTarget();
            float angle = Mathf.Atan2(DistanceToTarget.y, DistanceToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * wd.trackingRotationSpeed);
            //thust torwards
            yield return new WaitForSeconds(0.1f);
        }
    }
    Vector2 GetDistanceToTarget()
    {
        Vector2 dtt;
        dtt = target.transform.position - transform.position;
        return dtt;
    }
    Quaternion CalculateRotationForProjectile()
    {
        Quaternion quat = transform.rotation;
        //Somehow add the spread - weaponData.spread
        if (wd.spreadType != "none" )
        { 
            if (wd.spreadType == "random")
            { 
                float spread = Random.Range(wd.spread * -1, wd.spread); //Get the random number
                quat *= Quaternion.Euler(new Vector3(0f, 0f, spread));
            }
            if (wd.spreadType == "even")
            {
                if (wd.projectileCount == 1)
                {
                    //doesn't need to be spread because there is only one so
                    //it goes in the middle
                }
                else if (wd.projectileCount % 2 == 0) //dis bitch even and not 1
                {
                    //needs to be spread evenly
                    //e.g if spread = 5, then it would be 3.33 each

                }
                else //dis bitch odd and not 1
                {

                    //means you can have a projectile on each end
                    //e.g if spread = 5, one would be on -5, on 0, and on 5

                    //take total spread distance
                    float spreadDistance = wd.spread * 2;
                    //e.g if 5, that would be 10 degrees, and divide by amount of projectiles (-1) to 
                    //determine where they would be.
                    float angleSpread = spreadDistance / (wd.projectileCount - 1);
                    //New float that has the start spread angle
                    float totalAngle = wd.spread *-1;
                    //Times angle spread by the projectile number to get where it would be
                    //e.g - 0 = -5, 1 = 0, 2 = 5
                    totalAngle += angleSpread * spreadNumber;
                    quat *= Quaternion.Euler(new Vector3(0f, 0f, totalAngle));
                    

                }
                //Do some annoying ass maths
            }
        }
        return quat;

    }
}
