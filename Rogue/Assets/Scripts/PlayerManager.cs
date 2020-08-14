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

    private void Awake()
    {
        PlayerGameObject = transform.GetChild(0).gameObject;
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

        PlayerGameObject = PLAYER;
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
        PlayerGameObject.GetComponent<PlayerTopDownController>().RemoveTempBuffs();
    }

    public void RemovePermBuffs() //Removes the players collected buffs, usually when game is reset.
    {
        perm_movementspeed = 0;
        perm_attackspeed = 0;
    }
    public void AddPermBuffToPlayer(string bonusType,int bonusAmount) //To be called if either the player needs to be respawned - only adds to Player
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
                PlayerGameObject.GetComponent<PlayerWeapon>().projectilePrefab.GetComponent<Projectile>().permdamange += bonusAmount;
                perm_damage += bonusAmount;
                break;

        }
    }
    public void AddToPermBuffAndPlayer(string bonusType, int bonusAmount) //Used when player has chosen a reward - adds to PM and to Player
    {
        
        switch (bonusType)
        {
            //Movement speed
            case "movement":
                PlayerGameObject.GetComponent<PlayerTopDownController>().movementspeedbonus += bonusAmount;
                perm_movementspeed += bonusAmount;
                break;
            case "attackspeed":
                PlayerGameObject.GetComponent<PlayerWeapon>().permattackspeedbonus += bonusAmount;
                perm_attackspeed += bonusAmount;
                break;
            case "damage":
                PlayerGameObject.GetComponent<PlayerWeapon>().projectilePrefab.GetComponent<Projectile>().permdamange += bonusAmount;
                break;

        }
    }

}
