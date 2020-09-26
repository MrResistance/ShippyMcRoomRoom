using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAbility : MonoBehaviour
{
    //Variables
    public string side;


    protected virtual void Start()
    {
        //Get Side
        Debug.Log("Yes!");
        GetSide();
    }

    
    void Update()
    {
        
    }

    void GetSide()
    {
        if (transform.parent.gameObject.GetComponent<PlayerTopDownController>() != null)
        {
            side = "allied";
        }
        if (transform.parent.gameObject.GetComponent<NPCMoverScript1>() != null)
        {
            side = transform.parent.gameObject.GetComponent<NPCMoverScript1>().npcSide;
        }
    }
}
