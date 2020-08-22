using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerGameObject;
    Transform spawn; //For initialising new player
    public int remainingLives;

    //Perm buffs
    public int perm_movementspeed;
    public int perm_attackspeed;
    public int perm_damage;

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
        PLAYER.name = "Player";
        PLAYER.transform.parent = transform;
        PLAYER.GetComponent<PlayerTopDownController>().movementspeedbonus += perm_movementspeed;
        //Apply all perm buffs that player has collected to the player
        AddPermBuffToPlayer("movement", perm_movementspeed);
        AddPermBuffToPlayer("attackspeed", perm_attackspeed);
        AddPermBuffToPlayer("damage", perm_damage);
        PLAYER.GetComponent<PlayerTopDownController>().debugCanvas = debugCanvas;
        //PlayerGameObject = PLAYER;
    }
    public void PlayerHasDied()
    {
        if (remainingLives > 0)
        {
            remainingLives--;
            Invoke("RespawnPlayer",3f);
        }
    }
    public void RemoveTempBuffsFromPlayer()
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
    }
    public void AddPermBuffToPlayer(string bonusType,int bonusAmount) //To be called if either the player needs to be respawned - only adds to Player
    {
        if (PlayerGameObject != null)
        {
            Debug.Log("bonustype:" + bonusType + ",bonus amount:" + bonusAmount.ToString());
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

            }
        }
    }
    public void AddToPermBuffAndPlayer(string bonusType, int bonusAmount) //Used when player has chosen a reward - adds to PM and to Player
    {
        if (PlayerGameObject != null)
        {
            Debug.Log("bonus type: " + bonusType + ", bonus amount: " + bonusAmount.ToString());
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
            }
        }
    }

}
