using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float thisObjectPoints;
    //Shields
    public Shield_Obj shieldObj;
    public float shield, shieldMaximum;
    //UI
    public Slider healthBar, shieldBar;
    private void Awake()
    {
        gameUIManager = GameObject.Find("GameManager").GetComponent<GameUIManager>();
        shieldObj = GetComponentInChildren<Shield_Obj>();
    }
    void Start()
    {
        Invoke("DisableInvunerability", 1.5f);
        SetMaxOnBars();
        UpdateBars();
    }
    public void TakeDamage(float damage)
    {
        float remainderDamageFromShields;
        if (!isInvunerable)
            { 
            if (shield > 0)
            {
                if (shield-damage < 0)
                {
                    //Get remainder that would have been from shields
                    remainderDamageFromShields = shield % damage;
                    //Do damage to shields
                    shield -= damage;
                    shield = 0;

                    //Do remaining damage to health
                    health -= remainderDamageFromShields;
                }
                else
                {
                    shield -= damage;
                }
            
            }
            else
            {
                if (shieldObj != null && shieldObj.isActiveAndEnabled)
                {
                    shieldObj.gameObject.SetActive(false);
                }
                health -= damage;
            }
            if (gameObject.tag == "Player")
            {
                //Update shields
                gameUIManager.updateHealth(health);
                gameUIManager.updateShield(shield);
            }
            UpdateBars();
        }
        CheckIfDead();
    }
    public void SetMaxOnBars()
    {
        if (healthBar != null)
        {
            SetMaxHealthBar();
        }
        if (shieldBar != null)
        {
            SetMaxShieldBar();
        }
    }
    public void SetMaxHealthBar()
    {
        healthBar.maxValue = healthMaximum;
    }
    public void SetMaxShieldBar()
    {
        shieldBar.maxValue = shieldMaximum;
    }
    public void UpdateBars()
    {
        if (healthBar != null)
        {
            UpdateLocalHealthbar();
        }
        if (shieldBar != null)
        {
            UpdateLocalShieldbar();
        }
    }
    public void UpdateLocalHealthbar()
    {
        healthBar.value = health;
    }
    public void UpdateLocalShieldbar()
    {
        shieldBar.value = shield;
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
            if (gameObject.tag == ("Enemy"))
            {
                gameUIManager.PointsForKillingEnemy(gameObject.transform, thisObjectPoints);
            }
            Destroy(gameObject);
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
