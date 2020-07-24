using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public Light2D brakeL, brakeR, engineLight;
    public float h, v, brakeLightIntensity,speed = 5f;
    public Animator vertThrust, diagThrustL, diagThrustR;
    public SpriteRenderer thrustL, thrustR;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 tempVect = new Vector3(h, v, 0);
        tempVect = tempVect.normalized * speed * Time.deltaTime;
        rb.MovePosition(transform.position + tempVect);

        if (h > 0)
        {
            diagThrustL.SetBool("Moving", true);
            thrustL.enabled = true;
        }
        else
        {
            diagThrustL.SetBool("Moving", false);
            thrustL.enabled = false;
        }

        if (h < 0)
        {
            diagThrustR.SetBool("Moving", true);
            thrustR.enabled = true;
        }
        else
        {
            diagThrustR.SetBool("Moving", false);
            thrustR.enabled = false;
        }
        if (v > 0)
        {
            vertThrust.SetBool("Moving", true);
        }
        else
        {
            vertThrust.SetBool("Moving", false);
        }
        if (v < 0)
        {
            brakeL.intensity = brakeLightIntensity;
            brakeR.intensity = brakeLightIntensity;
        }
        else
        {
            brakeL.intensity = 0f;
            brakeR.intensity = 0f;
        }
    }
}
