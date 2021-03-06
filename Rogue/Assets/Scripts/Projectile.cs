﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    //Variables
    public WeaponData wd;
    public float damageBonus;
    public Transform firepoint;
    public float destroyTimer = 10f;
    public SpriteRenderer sr;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    public GameObject target, pointLight;
    public AudioSource audioSource;
    public StatTrak statTrak;
    //For if weapon has 'even' spread
    public float spreadNumber;
    GameObject owner;
    WaveManager wm;

    //Missile-based
    bool isWarmingUp = false;
   
    /// <summary>
    /// ///////////////////// METHODS //////////////////////////////////////////////
    /// </summary>
    
    //7am, waking up in the morning
    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        pointLight = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        statTrak = GameObject.Find("StatTrak").GetComponent<StatTrak>();
        //Play launch sfx
        gameObject.name = wd.name;
        if (wd.name == "Missile") //For the missile sound
        {
            StartCoroutine(DelaySound(0.75f,wd.launchSound));
        }
        else
        {
            audioSource.PlayOneShot(wd.launchSound);
        }    
        //Puts all projectiles under the 'Projectile' empty in the inspector, helpful for debugging
        transform.parent = GameObject.Find("Projectiles").transform;
        //For spread
        transform.rotation = CalculateRotationForProjectile();
        //Sets the correct sprite and colour
        sr.sprite = wd.sprite;
        sr.color = wd.colourSprite;
        //Sets correct scale
        transform.localScale = new Vector3(wd.scale,wd.scale,wd.scale);
        //Sets health and rigidbody
        GetComponent<EntityHealth>().health = wd.healthProjectile;
        rb = GetComponent<Rigidbody2D>();
        wm = GameObject.Find("GameManager").GetComponent<WaveManager>();
        //If this weapon is a homing missile, start the tracking code
        if (wd.isTrackingProjectile)
        { 
            if (wd.hasHomingDelay)
            {
                StartCoroutine(HomingMissile());
            }
            else
            {
                StartCoroutine(TrackTarget());
            }
        }
        else if(!wd.isTrackingProjectile) //If not, then just give the standard straight-line force
        {
            rb.AddForce(transform.up * wd.speed, ForceMode2D.Impulse);
        }
        if (wd.lifetimeProjectile > 0) //Destroy the projectile after amount of time set in WeaponData
        {
            if (wd.lifetimeMinMaxMultiplier == new Vector2(0, 0))
            {
                Invoke("LifetimeDestroy", wd.lifetimeProjectile);
            }
            else //Adds the minimum and maximum time
            {
                float min = wd.lifetimeProjectile + (wd.lifetimeProjectile * wd.lifetimeMinMaxMultiplier.x);
                float max = wd.lifetimeProjectile + (wd.lifetimeProjectile * wd.lifetimeMinMaxMultiplier.y);
                Invoke("LifetimeDestroy", Random.Range(min, max));
            }
        }


    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If this projectile is a missle, spawn an explosion on any collision
        if (wd.explosionType != "none")
            {
                //CauseExplosion();
            }
        
        if (collision.gameObject.tag.Contains("Boundary"))
        {
            if (gameObject.tag.Contains("Player"))
            {
                statTrak.playerShotsMissed++;
            }
            Destroy(gameObject);
        }

        //Check if hitting another projectile
        if (collision.gameObject.tag.Contains("Projectile"))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(CalculateDamage()*wd.d_projectileMultiplier,wd);
            pointLight.SetActive(false);
        }
        //Check if enemy
        if (collision.gameObject.tag == ("Player") || collision.gameObject.tag == ("Enemy") || collision.gameObject.tag == ("Ally"))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(CalculateDamage(), wd);
            pointLight.SetActive(false);
            if (!wd.canPierce) //If cannot pierce
            {
                
                //If can explode
                if (wd.explosionType != "none")
                {
                    CauseExplosion();
                }
                sr.enabled = false;
                bc.enabled = false;
                Destroy(this.gameObject, 3);
            }
        }

    }

    public float CalculateDamage()
    {
        float number;
        number = wd.damage + damageBonus;
        number /= wd.projectileCount;
        return number;
    }

    IEnumerator TrackTarget()
    {
        //Rotate towards
        while(true)
        {
            if (target == null)
            {
                if (tag.Contains("Enemy"))
                {
                    float minDist = 100000;
                    foreach (GameObject ge in wm.listAllies)
                    {
                        if (Vector3.Distance(ge.transform.position, transform.position) < minDist && ge.GetComponent<NPCMoverScript1>().isWithinArenaBoundary())
                        {
                            target = ge;
                            minDist = Vector3.Distance(ge.transform.position, transform.position);
                        }
                    }
                }
                if (tag.Contains("Player"))
                {
                    float minDist = 100000;
                    foreach (GameObject ge in wm.listEnemies)
                    {
                        if (Vector3.Distance(ge.transform.position, transform.position) < minDist && ge.GetComponent<NPCMoverScript1>().isWithinArenaBoundary())
                        {
                            target = ge;
                            minDist = Vector3.Distance(ge.transform.position, transform.position);
                        }
                    }
                }
            }
            if (target != null)
            { 
                Vector3 direction = (target.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotatetoTarget = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotatetoTarget, Time.deltaTime * wd.trackingRotationSpeed);
                //rb.velocity = new Vector2(direction.x * wd.speed, direction.y * wd.speed);
                rb.velocity = transform.up * wd.speed;
            }
            if (target == null)
            {
                if (wd.explosionType != "none")
                {
                    //CauseExplosion();
                    //DestroySelf(0);
                }
                else
                {
                    //DestroySelf(0);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    Vector2 GetDistanceToTarget()
    {
        Vector2 dtt;
        dtt = target.transform.position - transform.position;
        return dtt;
    }

    Quaternion CalculateRotationForProjectile()
    {
        Quaternion quat = transform.rotation;
        //Somehow add the spread - weaponData.spread
        if (wd.spreadType != "none" )
        { 
            if (wd.spreadType == "random")
            { 
                float spread = Random.Range(wd.spread * -1, wd.spread); //Get the random number
                quat *= Quaternion.Euler(new Vector3(0f, 0f, spread));
            }
            if (wd.spreadType == "even")
            {
                if (wd.projectileCount == 1)
                {
                    //doesn't need to be spread because there is only one so
                    //it goes in the middle
                }
                else if (wd.projectileCount % 2 == 0) //dis bitch even and not 1
                {
                    //needs to be spread evenly
                    float spreadDistance = wd.spread * 2;

                    float angleSpread = spreadDistance / (wd.projectileCount + 1);

                    float totalAngle = wd.spread * -1;

                    totalAngle += (angleSpread * (spreadNumber+1));

                    quat *= Quaternion.Euler(new Vector3(0f, 0f, totalAngle));

                }
                else //dis bitch odd and not 1
                {

                    //means you can have a projectile on each end
                    //e.g if spread = 5, one would be on -5, on 0, and on 5

                    //take total spread distance
                    float spreadDistance = wd.spread * 2;
                    //e.g if 5, that would be 10 degrees, and divide by amount of projectiles (-1) to 
                    //determine where they would be.
                    float angleSpread = spreadDistance / (wd.projectileCount - 1);
                    //New float that has the start spread angle
                    float totalAngle = wd.spread *-1;
                    //Times angle spread by the projectile number to get where it would be
                    //e.g - 0 = -5, 1 = 0, 2 = 5
                    totalAngle += angleSpread * spreadNumber;
                    quat *= Quaternion.Euler(new Vector3(0f, 0f, totalAngle));
                    

                }
                //Do some annoying ass maths
            }
        }
        return quat;

    }

    void LifetimeDestroy()
    {
        if (wd.explosionType != "none")
        { 
            CauseExplosion();
        }
        GetComponent<EntityHealth>().Die();
    }
    void CauseExplosion()
    {
        GameObject explosionClone = Instantiate(wd.prefabExplosive, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        explosionClone.transform.localScale = new Vector3(gameObject.transform.lossyScale.x + 17.5f, gameObject.transform.lossyScale.y + 17.5f, 1f);
        CircleCollider2D cc = GetComponent<CircleCollider2D>();
        cc.enabled = true;
        cc.radius = wd.explosiveradius;
        var hits = Physics2D.OverlapCircleAll(transform.position, wd.explosiveradius);
        foreach (var objs in hits)
        {
            Debug.Log("Explosion has hit: "+objs.name);
            //Check tag of projectile to determine who to damage
            if (tag.Contains("Player")) //Damage enemies
            {
                if (objs.tag.Contains("Enemy"))
                {
                    objs.GetComponent<EntityHealth>().TakeDamage(wd.explosivedamage+ damageBonus, wd);
                }
            }
            if (tag.Contains("Enemy")) //Damage allies
            {
                if (objs.tag.Contains("Player") || objs.tag.Contains("Ally"))
                {
                    objs.GetComponent<EntityHealth>().TakeDamage(wd.explosivedamage+ damageBonus, wd);
                }
            }
        }

    }
    //To delay 
    private IEnumerator DelaySound(float delayTime, AudioClip sound)
    {
        yield return new WaitForSeconds(delayTime);
        audioSource.PlayOneShot(sound);
    }

    private IEnumerator HomingMissile()
    {
        isWarmingUp = true;
        StartCoroutine(HoldingPositionOfOwner());
        //rb.gravityScale = 2f;
        yield return new WaitForSeconds(wd.rateoffire);
        isWarmingUp = false;
        rb.velocity.Set(0, 0);
        rb.gravityScale = 0f;
        StartCoroutine(TrackTarget());
    }
    public void SetParent(GameObject go)
    {
        owner = go;
    }

    private IEnumerator HoldingPositionOfOwner()
    {
        while (isWarmingUp)
        {
            transform.position = owner.transform.position;
            transform.rotation = owner.transform.rotation;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
    void DestroySelf(float delay)
    {
        sr.enabled = false;
        bc.enabled = false;
        Destroy(this.gameObject, delay);
    }
}
