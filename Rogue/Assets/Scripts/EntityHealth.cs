using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    //What is this?
    //This script is for entity health. This also includes the player.
    public bool isEnemy, isPlayer;
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

    public void TakeDamage(float damage)
    {
        health -= damage;
        CheckIfDead();
    }
    public void CheckIfDead()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
            //Die
            //If player
                //Check if has any lives left
            //If no lives
                //GG - end game
            //Else
                //Guess we respawn them
                //And deduct a life
        }
    }
    //Have its own collision detection
    //If is player
    //If has been hit by non-player projectile
    //Did it damage me lol
    //Take from health
    //Did it damage me enough for me to kill me
}
