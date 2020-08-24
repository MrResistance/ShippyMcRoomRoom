using System.Collections;
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
    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        this.gameObject.GetComponent<EntityHealth>().health = wd.healthProjectile;
        Rigidbody2D rbproj = this.gameObject.GetComponent<Rigidbody2D>();
        rbproj.AddForce(firepoint.right * wd.speed, ForceMode2D.Impulse);
        Destroy(this.gameObject, destroyTimer);
        

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
        return number;
    }

}
