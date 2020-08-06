﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMoverScript1 : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationCurve curveX, curveY;
    public Keyframe[] keyframesX;
    public Keyframe[] keyframesY;
    float timeElapsed;
    bool isMoving;
    public GameObject player;
    Vector2 distanceToPlayer;
    public float distanceToPlayerfloat;

    Rigidbody2D rb;
    public float cooldownShooting;
    public float minimumShootingDistance; 
    public float projectileSpeed = 1f;
    public GameObject projectilePrefab;
    // Update is called once per frame
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curveX = new AnimationCurve();
        curveY = new AnimationCurve();
        curveX.AddKey(0, transform.position.x);
        curveY.AddKey(0, transform.position.x);
        curveX.AddKey(4, 25);
        curveY.AddKey(4, -25);
        curveX.AddKey(8, -25);
        curveY.AddKey(8, -25);
        curveX.AddKey(12, -25);
        curveY.AddKey(12, 25);
        curveX.AddKey(16, 25);
        curveY.AddKey(16, 25);

        //Debug.Log("Key count:" + curveX.keys.Length);

       
        keyframesX = curveX.keys;
        keyframesY = curveY.keys;
        StartCoroutine(TargetPlayer());
    }

    void Update()
    {
        if (!isMoving)
        {
            isMoving = true;
            timeElapsed = 0f;
        }
        else
        {
            if (timeElapsed > curveX[curveX.length-1].time)
            {
                isMoving = false;
            }
            else
            {
                timeElapsed += Time.deltaTime;
                //Debug.Log(timeElapsed+","+ curveX[curveX.length - 1].time); //Time elapsed and the maximum time
                transform.position = new Vector2(curveX.Evaluate(timeElapsed), curveY.Evaluate(timeElapsed));
                //transform.rotation = Quaternion.Euler(0, 0, 360 * curveX.Evaluate(timeElapsed));
                GetDistanceToPlayer();
                if (player != null)
                { 
                    float angle = Mathf.Atan2(distanceToPlayer.y, distanceToPlayer.x) *Mathf.Rad2Deg - 90f;
                rb.rotation = angle;
                }
                else
                {
                    rb.rotation = 0;
                }
                //Debug.Log(direction.SqrMagnitude());
            }
        }

    }

    IEnumerator TargetPlayer()
    {
        while (true)
        {
            if (distanceToPlayerfloat < minimumShootingDistance)
            {
                ShootPlayer();
            }
            yield return new WaitForSeconds(cooldownShooting);
        }
    }
    void ShootPlayer()
    {
        Debug.Log("Firing at player");
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        Rigidbody2D rbproj = projectile.GetComponent<Rigidbody2D>();
        rbproj.AddForce(transform.forward * projectileSpeed, ForceMode2D.Impulse);
    }
    void GetDistanceToPlayer()
    {
        if (player != null)
        {
            distanceToPlayer = player.transform.position - transform.position;
            distanceToPlayerfloat = Vector2.Distance(player.transform.position, transform.position);
            //Debug.Log(distanceToPlayerfloat);
        }
    }
}