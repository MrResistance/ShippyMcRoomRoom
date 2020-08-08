using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public Transform firePoint; //For firing from
    public float cooldownShooting, maximumShootingDistance, projectileSpeed = 1f;
    public GameObject projectilePrefab, tHealth;
    public AudioSource audioSource;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Start()
    {
        firePoint = transform.GetChild(0);
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

        tHealth = transform.GetChild(1).GetChild(0).gameObject;
        StartCoroutine(TargetPlayer());
    }

    void Update()
    {
        //Move();
        Rotate();
        tHealth.GetComponent<TextMeshProUGUI>().text = GetComponent<EntityHealth>().health.ToString();
    }

    IEnumerator TargetPlayer()
    {
        while (true)
        {
            if (player != null)
            { 
                if (distanceToPlayerfloat < maximumShootingDistance)
                {
                    ShootPlayer();
                }
            }
            else
            {
                player = GameObject.Find("Player");
            }
            yield return new WaitForSeconds(cooldownShooting);
        }
    }
    void ShootPlayer()
    {
        //Debug.Log("Firing at player");
        audioSource.PlayOneShot(audioSource.clip);
        GameObject projectile = Instantiate(projectilePrefab, firePoint.transform.position, firePoint.transform.rotation);
        Rigidbody2D rbproj = projectile.GetComponent<Rigidbody2D>();
        rbproj.AddForce(firePoint.right * projectileSpeed, ForceMode2D.Impulse);
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
    void Move()
    {
        if (!isMoving)
        {
            isMoving = true;
            timeElapsed = 0f;
        }
        else
        {
            if (timeElapsed > curveX[curveX.length - 1].time)
            {
                isMoving = false;
            }
            else
            {
                timeElapsed += Time.deltaTime;
                //Debug.Log(timeElapsed+","+ curveX[curveX.length - 1].time); //Time elapsed and the maximum time
                transform.position = new Vector2(curveX.Evaluate(timeElapsed), curveY.Evaluate(timeElapsed));
                //transform.rotation = Quaternion.Euler(0, 0, 360 * curveX.Evaluate(timeElapsed));
                
                //Debug.Log(direction.SqrMagnitude());
            }
        }
    }
    void Rotate()
    {
        GetDistanceToPlayer();
        if (player != null)
        {
            float angle = Mathf.Atan2(distanceToPlayer.y, distanceToPlayer.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle;
        }
        else
        {
            rb.rotation = 0;
        }
    }
}
