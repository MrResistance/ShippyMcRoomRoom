﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTopDownController : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isDirectionalMovement; //If true, player moves based on where they look; else, upie downlie lefty rightie
    public bool isControllerEnabled; //If true, player uses controller

    public Rigidbody2D rb;
    public Camera camera;
    // Movement
    //Mouse and keyboard
    public Vector2 mouseposition;
    public Vector2 lookdir;
    public float angle,h,v;

    public float movementspeed, movementspeedbonus,movementspeedMaximum;

    public PlayerManager pm;
    public GameUIManager gm;

    //Controller stuff?
    public float axisV, axisH;

    public GameObject debugCanvas;

    //Temp buff variables - so they can be wiped when round is over
    private void Awake()
    {
        debugCanvas = GameObject.Find("Debug");
        gm = GameObject.Find("GameManager").GetComponent<GameUIManager>();
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
        
    }
    void Start()
    {
        movementspeedbonus = 100; //Default start
        pm = transform.parent.GetComponent<PlayerManager>(); //For now, unused
        pm.PlayerGameObject = gameObject;
        pm.RestoreHealthToMaximum();
        gameObject.GetComponent<EntityHealth>().shield = 100;
        gameObject.GetComponent<EntityHealth>().shieldMaximum = 100;
        StartCoroutine(CheckForInputs());
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.ShowingRewards)
        {
            Look();
            Move();
        }
        else
        {
            transform.position = new Vector2(0, 0);
        }
        if (debugCanvas != null && debugCanvas.activeSelf == true)
        {
            UpdateDebugText();
        }
    }
    private IEnumerator CheckForInputs()
    {
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                if (Input.GetKeyDown("joystick 1 button " + i))
                {
                    isControllerEnabled = true;
                    print("CONTROLLER ANYKEY");
                }
                if (Input.anyKey && !Input.GetKeyDown("joystick 1 button " + i))
                {
                    isControllerEnabled = false;
                    print("KEYBOARD/MOUSE ANYKEY");
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private float speed()
    {
        float s;
        s = movementspeed * (movementspeedbonus / 100);
        if (s > movementspeedMaximum)
        {
            return movementspeedMaximum;
        }
        return s;
    }

    void Look()
    {
        //Mouse
        
        if (!isControllerEnabled)
        {
            mouseposition = camera.ScreenToWorldPoint(Input.mousePosition);
            lookdir = mouseposition - rb.position;
            angle = Mathf.Atan2(lookdir.y, lookdir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle; //for mouse
        }
        else
        { 
            //Controller
            axisV = Input.GetAxis("Vertical2");
            axisH = Input.GetAxis("Horizontal2");
            float controllerangle = Mathf.Atan2(axisV, axisH) * Mathf.Rad2Deg - 90f;

            if ((axisV != 0)||(axisH != 0))
                rb.rotation = controllerangle; //for controller
        }
    }
    void Move()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        if (isDirectionalMovement)
        {
            rb.AddForce(transform.up * v * speed());
            rb.AddForce(transform.right * h * speed());
        }
        else
        {
            rb.AddForce(Vector2.up * v * speed());
            rb.AddForce(Vector2.right * h * speed());
        }

    }
    public void RemoveTempBuffs() //To be called by the WaveManager at the end of every wave
    {

        //Set all buffs to ZERO
    }
    void UpdateDebugText()
    {
        //Movement
        debugCanvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Movement: D=" + movementspeed.ToString()+"/B="+movementspeedbonus.ToString() + "/T="+speed().ToString();
        debugCanvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "Damage: D=" + gameObject.GetComponent<PlayerWeapon>().wd.damage + "/B=" + gameObject.GetComponent<PlayerWeapon>().permdamagebonus + "/T=" + gameObject.GetComponent<PlayerWeapon>().CalculateDamage();
        debugCanvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = "AttackSpeed: D=" + gameObject.GetComponent<PlayerWeapon>().wd.rateoffire + "/B=" + gameObject.GetComponent<PlayerWeapon>().permattackspeedbonus + "/T=" + gameObject.GetComponent<PlayerWeapon>().CalculateAttackSpeed();

    }
}
