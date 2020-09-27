using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniTurret : PassiveAbility
{
    
    public WaveManager wm;
    public AudioSource audioSource;
    public int boundaryXmin = -40;
    public int boundaryXmax = 40;
    public int boundaryYmin = -25;
    public int boundaryYmax = 25;

    //Targetting
    public GameObject target;
    Vector2 distanceToTarget;
    public float distanceToTargetFloat;
    public GameObject hs;

    //Weapon
    public WeaponData wd;
    public GameObject projectilePrefab;
    Transform firePoint;
    GameObject ProjectilesGO;

    public string test;

    protected override void Start()
    {
        base.Start();

        wm = GameObject.Find("GameManager").GetComponent<WaveManager>();
        ProjectilesGO = GameObject.Find("Projectiles").gameObject;
        audioSource = gameObject.GetComponent<AudioSource>();
        StartCoroutine(TargetEnemyOfOwner());
        StartCoroutine(ShootEnemyOfOwner());

        //firePoint = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }
    void Rotate()
    {
        if (target != null)
        {
            GetDistanceToTarget();
            float angle = Mathf.Atan2(distanceToTarget.y, distanceToTarget.x) * Mathf.Rad2Deg - 90f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            //Vector3 angle2 = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y * -1, transform.rotation.eulerAngles.z);
            Vector3 angles2 = transform.rotation.eulerAngles;
            //transform.localRotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10f);

            //transform.localRotation = WeaponFiringAngle();
            transform.localRotation = rot;
            //transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            //transform.rotation = Quaternion.Euler(angles2);


        }
    }
    void GetDistanceToTarget()
    {
        distanceToTarget = target.transform.position - transform.position;
        distanceToTargetFloat = Vector2.Distance(target.transform.position, transform.position);
    }
    IEnumerator TargetEnemyOfOwner()
    {
        while (true)
        {
            if (hasEnemies() && wm.isWaveChanging == false && isWithinArenaBoundary()) //Has enemies and is within boundary
            {
                distanceToTargetFloat = Mathf.Infinity;
                if (side == "enemy")
                {
                    //Get closest
                    foreach (GameObject go in wm.listAllies)
                    {
                        if (go != null && isWithinArenaBoundary(go) == true)
                        {
                            Vector2 distanceToGO = go.transform.position - transform.position;
                            float distanceToGOFloat = Vector2.Distance(go.transform.position, transform.position);
                            if (distanceToGOFloat < distanceToTargetFloat)
                            {
                                target = go.gameObject;
                                distanceToTargetFloat = distanceToGOFloat;
                            }
                        }

                    }

                }
                else if (side == "allied")
                {
                    //Get closest
                    //Make that the target

                    for (int e = 0; e < wm.listEnemies.Count; e++)
                    {
                        GameObject go = wm.listEnemies[e].gameObject;
                        if (go != null && isWithinArenaBoundary(go) == true)
                        {
                            Vector2 distanceToGO = go.transform.position - transform.position;
                            float distanceToGOFloat = Vector2.Distance(go.transform.position, transform.position);
                            if (distanceToGOFloat < distanceToTargetFloat)
                            {
                                target = go.gameObject;
                                distanceToTargetFloat = distanceToGOFloat;
                            }
                        }

                    }

                }
                yield return new WaitForSeconds(wd.rateoffire * 3); //Ensures that the target is shot at least 3 times before switching
            }
            //Enemies exist but NPC isn't in the boundary
            else if (hasEnemies() && wm.isWaveChanging == false && isWithinArenaBoundary() == false)
            {
                //Move towards center 
                
                target = wm.gameObject;
                yield return new WaitForSeconds(0.5f);

            }
            else //No enemies for NPC available
            {
                //Guess no target.
                //Just wait
                yield return new WaitForSeconds(1f); //Waits one second before trying again
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    bool hasEnemies()
    {
        if (side == "enemy")
        {
            if (wm.listAllies.Count != 0)
            {
                return true;
            }
        }
        else if (side == "allied")
        {
            if (wm.listEnemies.Count != 0 && wm.EnemiesGO.transform.childCount != 0)
            {
                return true;
            }
        }
        //No target available
        distanceToTargetFloat = Mathf.Infinity; //So getting shorter distance will be easier
        return false;
    }

    bool isWithinArenaBoundary(GameObject go) //For checking other GOs
    {
        if (go.transform.position.x >= boundaryXmin && go.transform.position.x <= boundaryXmax)
        {
            if (go.transform.position.y >= boundaryYmin && go.transform.position.y <= boundaryYmax)
            {
                return true;
            }
        }
        return false;
    }

    IEnumerator ShootEnemyOfOwner()
    {
        while (true)
        {
            if (target != null && CanShoot())
            {
                ShootTarget();
                yield return new WaitForSeconds(wd.rateoffire);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
            
            
        }
    }
    void ShootTarget()
    {
        //Debug.Log("Firing at player");
        audioSource.PlayOneShot(audioSource.clip);

        for (int p = 0; p < wd.projectileCount; p++)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, WeaponFiringAngle());
            projectile.GetComponent<Projectile>().firepoint = transform;
            projectile.GetComponent<Projectile>().wd = wd;
            projectile.GetComponent<Projectile>().SetParent(this.gameObject);
            projectile.transform.parent = ProjectilesGO.transform;
            //NPC Side stuff - only for NPCs
            if (side == "enemy")
            {
                projectile.tag = "Enemy Projectile";
                projectile.layer = 11;
            }
            if (side == "allied")
            {
                projectile.tag = "Player Projectile";
                projectile.layer = 10;
            }
            //Spread
            if (wd.spreadType == "even")
            {
                projectile.GetComponent<Projectile>().spreadNumber = p;
            }
            //Targetting
            if (wd.isTrackingProjectile == true)
            {
                projectile.GetComponent<Projectile>().target = target;
            }
        }
    }
    Quaternion WeaponFiringAngle() //Adds weapon spread to rotation - moved to projectile
    {
        Quaternion quat = transform.localRotation;
        //Somehow add the spread - weaponData.spread
        //quat *= Quaternion.Euler(new Vector3(0f, 0f, 5f));
        //Debug.Log(quat.ToString());
        return quat;
    }
    //TAKEN FROM NPCMoverScript1
    bool CanShoot() //Checks to make sure the NPC passes all checks before they can shoot;
    {
        if (wm.isWaveChanging == false)
        {
            if (target != null)
            {
                if (isCloseEnoughToShoot())
                {
                    if (isCurrentlyLookingAtTarget())
                    {
                        if (isWithinArenaBoundary())
                        {
                            if (target != wm.gameObject)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    bool isCloseEnoughToShoot() //So entity knows it can hit the target
    {
        if (wd.range == 0) //The weapon has no limit therefore it can shoot as far as it wants
        {
            return true;
        }
        if (distanceToTargetFloat < wd.range)
        {
            return true;
        }
        return false;

    }
    bool isCurrentlyLookingAtTarget() //So the entity is not just shooting aimlessly
    {
        return true;
    }
    public bool isWithinArenaBoundary()
    {
        if (transform.position.x >= boundaryXmin && transform.position.x <= boundaryXmax)
        {
            if (transform.position.y >= boundaryYmin && transform.position.y <= boundaryYmax)
            {
                return true;
            }
        }
        return false;
    }
}
