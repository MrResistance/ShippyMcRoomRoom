using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerGameObject;
    Transform spawn; //For initialising new player
    public int remainingLives;
    void Start()
    {
        PlayerGameObject = transform.GetChild(0).gameObject;

        spawn = transform;
    }

    // Update is called once per frame
    public void RespawnPlayer()
    {
        Debug.Log("Respawning player.");
        GameObject PLAYER = Instantiate(PlayerPrefab, spawn.position, spawn.rotation);
        PLAYER.name = "Player";
        PLAYER.transform.parent = transform;
        PlayerGameObject = PLAYER;
    }
    public void PlayerHasDied()
    {
        if (remainingLives > 0)
        {
            remainingLives--;
            RespawnPlayer();
        }
    }

}
