using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class Player : MonoBehaviour
{
    private GameObject playerObject;
    private Rigidbody2D rb;
    public float h, v, speed;
    public UnityEngine.Experimental.Rendering.Universal.Light2D engineThrusterLight, brakeL, brakeR;
    public Animator vertThrusterAnim, diagThrusterL, diagThrusterR;

    void Start()
    {
        playerObject = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);
        if (h < 0)
        {
            diagThrusterR.SetBool("MovingLeft", true);
        }
        else
        {
            diagThrusterR.SetBool("MovingLeft", false);
        }
        if (h > 0)
        {
            diagThrusterL.SetBool("MovingRight", true);
        }
        else
        {
            diagThrusterL.SetBool("MovingRight", false);
        }
        if (v > 0)
        {
            vertThrusterAnim.SetBool("Moving", true);
        }
        else
        {
            vertThrusterAnim.SetBool("Moving", false);
        }
        if (v < 0)
        {
            brakeL.intensity = 5f;
            brakeR.intensity = 5f;
        }
        else
        {
            brakeL.intensity = 0f;
            brakeR.intensity = 0f;
        }
        if (h != 0 || v > 0)
        {
            engineThrusterLight.intensity = 5f;
        }
        else
        {
            engineThrusterLight.intensity = 0f;
        }
    }
}



    
