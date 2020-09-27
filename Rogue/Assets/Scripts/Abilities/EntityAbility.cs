using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAbility : MonoBehaviour
{
    //Variables
    public string side;

    public string owner; //"npc","player"

    protected virtual void Start()
    {
        //Get Side
        Debug.Log("Yes!");
        GetSide();
    }

    void GetSide()
    {
        if (transform.parent.gameObject.GetComponent<PlayerTopDownController>() != null) //Player
        {
            side = "allied";
            owner = "player";
        }
        if (transform.parent.gameObject.GetComponent<NPCMoverScript1>() != null) //NPC
        {
            side = transform.parent.gameObject.GetComponent<NPCMoverScript1>().npcSide;
            owner = "npc";
        }
    }
    void GetOwner()
    {

    }
}
