using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    //What is this?
    //This script is for entity health. This also includes the player.
    //Variables
    public float health; //The main variable of an entity
    public float healthMaximum; //The starting and maximum health of the entity - not including healthbonus
    public float healthBonus; //Adds onto the healthMaximum - mainly for the player but we can use later
    public GameUIManager gameUIManager;
    public bool isInvunerable = true;
    private void Awake()
    {
        if (gameObject.tag == "Player")
        {
            gameUIManager = GameObject.Find("GameManager").GetComponent<GameUIManager>();
        }
    }
    void Start()
    {
        Invoke("DisableInvunerability", 1.5f);
    }
    public void TakeDamage(float damage)
    {
        if (isInvunerable != true)
            health -= damage;
        if (gameObject.tag == "Player")
        {
            gameUIManager.updateHealth(health);
        }
        CheckIfDead();
    }
    public void CheckIfDead()
    {
        if (health <= 0)
        {
            //If player
            if (gameObject.tag == ("Player"))
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
    public void RestoreHealthToMaximum()
    {
        health = healthMaximum + healthBonus;
    }
    //Have its own collision detection
    //If is player
    //If has been hit by non-player projectile
    //Did it damage me lol
    //Take from health
    //Did it damage me enough for me to kill me
}
