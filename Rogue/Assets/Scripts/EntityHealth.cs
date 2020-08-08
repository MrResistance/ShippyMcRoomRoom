using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    //What is this?
    //This script is for entity health. This also includes the player.
    //Variables
    public float health; //VERY BASIC FOR NOW - Later versions will have different stuff
    public bool isInvunerable = true;

    void Start()
    {
        Invoke("DisableInvunerability", 1.5f);
    }
    public void TakeDamage(float damage)
    {
        if (isInvunerable != true)
            health -= damage;
        CheckIfDead();
    }
    public void CheckIfDead()
    {
        if (health <= 0)
        {
            //If player
            if (gameObject.tag.Contains("Player"))
            { 
                transform.parent.gameObject.GetComponent<PlayerManager>().PlayerHasDied();
            }
            //Finally
            Destroy(this.gameObject);
            //Die
            
                
            
            //Else
                //Guess we respawn them
                //And deduct a life
        }
    }
    void DisableInvunerability()
    {
        isInvunerable = false;
    }
    //Have its own collision detection
    //If is player
    //If has been hit by non-player projectile
    //Did it damage me lol
    //Take from health
    //Did it damage me enough for me to kill me
}
