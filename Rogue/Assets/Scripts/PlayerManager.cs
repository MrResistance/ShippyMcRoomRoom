﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerGameObject;
    public GameUIManager gameUIManager;
    public AudioSource audioSource;
    Transform spawn; //For initialising new player
    public int remainingLives;

    //Perm buffs
    public float perm_movementspeed;
    public float perm_attackspeed;
    public float perm_damage;
    public float perm_health;
    public float perm_shield;
    public float perm_shieldRechargeAmount;
    public float perm_shieldRechargeTime; //Can be deducted from delay - used when takes damage

    public GameObject debugCanvas;

    private void Awake()
    {
        //PlayerGameObject = transform.GetChild(0).gameObject;
    }
    void Start()
    {
        spawn = transform;
    }

    // Update is called once per frame
    public void RespawnPlayer()
    {
        Debug.Log("Respawning player.");
        GameObject PLAYER = Instantiate(PlayerPrefab, spawn.position, spawn.rotation);
        PlayerGameObject = PLAYER;
        PLAYER.name = "Player";
        PLAYER.transform.parent = transform;
        PLAYER.GetComponent<PlayerTopDownController>().movementspeedbonus += perm_movementspeed;
        //Apply all perm buffs that player has collected to the player
        AddPermBuffToPlayer("movement", perm_movementspeed);
        AddPermBuffToPlayer("attackspeed", perm_attackspeed);
        AddPermBuffToPlayer("damage", perm_damage);
        AddPermBuffToPlayer("health", perm_health);
        PLAYER.GetComponent<PlayerTopDownController>().debugCanvas = debugCanvas;

        //PlayerGameObject = PLAYER;
        gameUIManager.updateHealth(100);
        //Adds player to allies list - thing NPCs look at for potential enemies
        gameObject.GetComponent<WaveManager>().listAllies.Add(PLAYER);
    }
    public void PlayerHasDied() //For deducting lives
    {
        audioSource.PlayOneShot(audioSource.clip);
        if (remainingLives > 0)
        {
            remainingLives--;
            gameUIManager.updateLives(remainingLives);
            Invoke("RespawnPlayer",3f);
        }
    }
    public void RemoveTempBuffsFromPlayer() //Used when player dies. Don't want them to have temp buffs when they respawn. Can't be rewarding that
    {
        if (PlayerGameObject != null)
        {
            PlayerGameObject.GetComponent<PlayerTopDownController>().RemoveTempBuffs();
        }
    }

    public void RemovePermBuffs() //Removes the players collected buffs, usually when game is reset.
    {
        perm_movementspeed = 0;
        perm_attackspeed = 0;
        perm_damage = 0;
        perm_health = 0;
        perm_shield = 0;
        perm_shieldRechargeAmount = 0;
        perm_shieldRechargeTime = 0;
}
    public void AddPermBuffToPlayer(string bonusType,float bonusAmount) //To be called if either the player needs to be respawned - only adds to Player
    {
        if (PlayerGameObject != null)
        {
            switch (bonusType)
            {
                //Movement speed
                case "movement":
                    PlayerGameObject.GetComponent<PlayerTopDownController>().movementspeedbonus += bonusAmount;
                    break;
                case "attackspeed":
                    PlayerGameObject.GetComponent<PlayerWeapon>().permattackspeedbonus += bonusAmount;
                    break;
                case "damage":
                    PlayerGameObject.GetComponent<PlayerWeapon>().permdamagebonus += bonusAmount;
                    break;
                case "health":
                    PlayerGameObject.GetComponent<EntityHealth>().healthBonus += bonusAmount;
                    RestoreHealthToMaximum();
                    break;

            }
        }
    }
    public void AddToPermBuffAndPlayer(string bonusType, float bonusAmount) //Used when player has chosen a reward - adds to PM and to Player
    {
        if (PlayerGameObject != null)
        {
            //Debug.Log("bonus type: " + bonusType + ", bonus amount: " + bonusAmount.ToString());
            switch (bonusType)
            {
                //Movement speed
                case "movement":
                    PlayerGameObject.GetComponent<PlayerTopDownController>().movementspeedbonus += bonusAmount;
                    perm_movementspeed += bonusAmount; //Saves new movement speed bonus to PM
                    break;
                case "attackspeed":
                    PlayerGameObject.GetComponent<PlayerWeapon>().permattackspeedbonus += bonusAmount;
                    perm_attackspeed += bonusAmount; //Saves new attack speed to PM
                    break;
                case "damage":
                    PlayerGameObject.GetComponent<PlayerWeapon>().permdamagebonus += bonusAmount;
                    perm_damage += bonusAmount; //Saves new bonus damage to PM
                    break;
                case "health":
                    PlayerGameObject.GetComponent<EntityHealth>().healthBonus += bonusAmount;
                    perm_health += bonusAmount; //Saves new bonus damage to PM
                    RestoreHealthToMaximum();
                    break;

            }
        }
    }
    public void RestoreHealthToMaximum() //Commonly used when game is about to go into next round - could make it difficult dependent, so harder difficulties have it as a reward
    {
        PlayerGameObject.GetComponent<EntityHealth>().RestoreHealthToMaximum();
    }

}
