using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    //What is this
    //This script is for entity health. This also includes the player.
    private bool isEnemy, isPlayer;
    //Variables
    public float health; //VERY BASIC FOR NOW - Later versions will have different stuff
    void Start()
    {
        if (this.gameObject.tag.Contains("Player"))
        {
            isPlayer = true;
        }
        else if (this.gameObject.tag.Contains("Enemy"))
        {
            isEnemy = true;
        }
    }

    void TakeDamage(float damage)
    {
        health -= damage;
    }

    private void Update()
    {
        if (health <= 0)
        {
            //End game
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isEnemy && collision.otherCollider.tag.Contains("Player Projectile"))
        {
            //TakeDamage(collision.otherCollider.Projectile.damage);
        }
        else if (isPlayer && collision.otherCollider.tag.Contains("Enemy Projectile"))
        {
            //TakeDamage(collision.otherCollider.Projectile.damage);
        }
    }

    //Have its own collision detection
    //If is player
    //If has been hit by non-player projectile
    //Did it damage me lol
    //Take from health
    //Did it damage me enough for me to kill me
}
