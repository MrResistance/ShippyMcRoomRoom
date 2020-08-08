using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public GameObject PlayerGameObject;
    Transform spawn; //For initialising new player
    public int remainingLives;
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

}
