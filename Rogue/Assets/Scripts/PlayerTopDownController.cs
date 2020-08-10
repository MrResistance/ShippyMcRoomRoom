using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float movementspeed;
    public float movementspeedbonus;

    public PlayerManager pm;

    //Controller stuff?
    public float axisV, axisH;

    //Temp buff variables - so they can be wiped when round is over
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }
    void Start()
    {
        movementspeedbonus = 100; //Default start
        pm = transform.parent.GetComponent<PlayerManager>(); //For now, unused
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Move();
    }
    private float speed()
    {
        return movementspeed * (movementspeedbonus / 100);
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
}
