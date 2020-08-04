using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage, destroyTimer = 10f;
    private SpriteRenderer sr;
    private void Start()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        Destroy(this.gameObject, destroyTimer);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Contains("Boundary"))
        {
            Destroy(this.gameObject);
        }
        if (collision.gameObject.tag.Contains("Player") || collision.gameObject.tag.Contains("Enemy"))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);
        }
        sr.enabled = false;
        Destroy(this.gameObject, 3);
    }
}
