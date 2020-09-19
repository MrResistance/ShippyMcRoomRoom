using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public WeaponData wd;
    public float damageBonus;
    public Transform firepoint;
    public float destroyTimer = 10f;
    public SpriteRenderer sr;
    private BoxCollider2D bc;
    private Rigidbody2D rb;
    public GameObject target, pointLight;
    public AudioSource audioSource;
    //For if weapon has 'even' spread
    public float spreadNumber;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        pointLight = transform.GetChild(0).gameObject;
    }

    private void Start()
    {
        //Play launch sfx
        gameObject.name = wd.name;
        if (wd.name == "Missile")
        {
            StartCoroutine(DelaySound());
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
        //If this weapon is a homing missile, start the tracking code
        if (wd.name == "Missile")
        {
            StartCoroutine(HomingMissile());
        }
        else if(wd.name != "Missile") //If not, then just give the standard straight-line force
        {
            rb.AddForce(transform.up * wd.speed, ForceMode2D.Impulse);
        }
        if (wd.lifetimeProjectile > 0) //Destroy the projectile after amount of time set in WeaponData
        {
            Invoke("LifetimeDestroy", wd.lifetimeProjectile);
        }

    }
    private IEnumerator DelaySound()
    {
        yield return new WaitForSeconds(0.75f);
        audioSource.PlayOneShot(wd.launchSound);
    }
    private IEnumerator HomingMissile()
    {
        rb.gravityScale = 2f;
        yield return new WaitForSeconds(1f);
        rb.velocity.Set(0, 0);
        rb.gravityScale = 0f;
        StartCoroutine(TrackTarget());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If this projectile is a missle, spawn an explosion on any collision
        if (wd.explosionType != "none")
            {
                CauseExplosion();
            }
        {
            CauseExplosion();
            //audioSource.PlayOneShot(wd.impactSound);
        }
        if (collision.gameObject.tag.Contains("Boundary"))
        {
            Destroy(gameObject);
        }

        //Check if hitting another projectile
        if (collision.gameObject.tag.Contains("Projectile"))
        {
            //this.gameObject.GetComponent<EntityHealth>().TakeDamage(damage);
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(CalculateDamage(),wd);
            pointLight.SetActive(false);
            //Debug.Log("Giving " + CalculateDamange().ToString() + " to " + collision.gameObject.name);
            //Destroy(collision.gameObject);
            //Destroy(this.gameObject);
        }
        //Check if enemy
        if (collision.gameObject.tag == ("Player") || collision.gameObject.tag == ("Enemy") || collision.gameObject.tag == ("Ally"))
        {
            collision.gameObject.GetComponent<EntityHealth>().TakeDamage(CalculateDamage(), wd);
            pointLight.SetActive(false);
            sr.enabled = false;
            bc.enabled = false;
            Destroy(this.gameObject, 3);
        }
        //sr.enabled = false;
        //Destroy(this.gameObject, 3);
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
        while(target != null)
        {
            Debug.Log("Allowed to go!");
            Vector3 direction = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotatetoTarget = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotatetoTarget, Time.deltaTime * wd.trackingRotationSpeed);
            //rb.velocity = new Vector2(direction.x * wd.speed, direction.y * wd.speed);
            rb.velocity = transform.up * wd.speed;
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
    }

}
