using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage, destroyTimer = 10f;
    public SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, destroyTimer);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Boundary"))
        {
            Destroy(this.gameObject);
        }
        //Check if player
        if (collision.gameObject.tag.Contains("Player") && (this.gameObject.tag.Contains("Enemy Projectile")))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);
            Destroy(this.gameObject, 3);
        }
        if (collision.gameObject.tag.Contains("Enemy") && (this.gameObject.tag.Contains("Player Projectile")))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);
            Destroy(this.gameObject, 3);
        }

        //Check if enemy
        if (collision.gameObject.tag.Contains("Player") || collision.gameObject.tag.Contains("Enemy"))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);
        }
        //sr.enabled = false;
        //Destroy(this.gameObject, 3);
    }
}
