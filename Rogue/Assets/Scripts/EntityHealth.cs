using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
    //What is this?
    //This script is for entity health. This also includes the player.
    //Variables
    public float health; //The main variable of an entity
    public float healthMaximum; //The starting and maximum health of the entity - not including healthbonus
    public float healthBonus; //Adds onto the healthMaximum - mainly for the player but we can use later
    public GameUIManager gameUIManager;
    public WaveManager wm;
    public StatTrak statTrak;
    public bool isInvunerable = true, isDying = false;
    public float thisObjectPoints;
    public GameObject explosionPrefab;
    //Shields
    public Shield_Obj shieldObj;
    public float shield, shieldMaximum, shieldRechargeDelay = 3f, shieldRechargeRate, shieldBonus;
    public bool shieldRechargeAllowed = true;
    //UI
    public Slider healthBar, shieldBar;
    private void Awake()
    {
        statTrak = GameObject.Find("StatTrak").GetComponent<StatTrak>();
        gameUIManager = GameObject.Find("GameManager").GetComponent<GameUIManager>();
        wm = GameObject.Find("GameManager").GetComponent<WaveManager>();
        shieldObj = GetComponentInChildren<Shield_Obj>();
    }
    void Start()
    {
        Invoke("DisableInvunerability", 1.5f);
        //InvokeRepeating("RechargeShield", 1f, 2f);
        StartCoroutine(RechargeShield());
    }

    public void MoveHealthBarUI()
    {
        if (shieldBar != null)
        {
            shieldBar.gameObject.SetActive(false);
            healthBar.gameObject.transform.localPosition = new Vector3(shieldBar.gameObject.transform.localPosition.x, shieldBar.gameObject.transform.localPosition.y, shieldBar.gameObject.transform.localPosition.z);
        }
    }

    public void AllowShieldRecharge()
    {
        shieldRechargeAllowed = true;
    }

    public IEnumerator RechargeShield()
    {
        while (true)
        {
            if (shield < shieldMaximum && shieldRechargeAllowed)
            {
                shield += shieldRechargeRate;
                if (shield > shieldMaximum)
                {
                    shield = shieldMaximum;
                }
                if (gameObject.tag == ("Player"))
                {
                    gameUIManager.updateShield(shield);
                }
                else if (shieldBar != null)
                {
                    shieldBar.value = shield;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void TakeDamage(float damage, WeaponData wd)
    {
        //temps
        float dShield = damage * wd.d_shieldMultiplier;
        float dHealth = damage * wd.d_healthMultiplier;
        //
        CancelInvoke("AllowShieldRecharge");
        shieldRechargeAllowed = false;
        Invoke("AllowShieldRecharge", shieldRechargeDelay);
        float remainderDamageFromShields;
        if (!isInvunerable)
            { 
            if (shield > 0)
            {
                if (shield- dShield < 0)
                {
                    //Get remainder that would have been from shields
                    remainderDamageFromShields = shield % dShield;
                    //Do damage to shields
                    shield -= dShield;
                    shield = 0;

                    //Do remaining damage to health
                    health -= remainderDamageFromShields;
                }
                else
                {
                    shield -= dShield;
                }
            }
            else
            {
                if (shieldObj != null && shieldObj.isActiveAndEnabled)
                {
                    shieldObj.gameObject.SetActive(false);
                }
                health -= dHealth;
            }
            if (gameObject.tag == "Player")
            {
                //Update shields
                statTrak.damageTaken += damage;
                statTrak.timesPlayerWasHit++;
                gameUIManager.updateHealth(health);
                gameUIManager.updateShield(shield);
            }
            else if (gameObject.tag == "Enemy")
            {
                statTrak.playerShotsHit++;
                statTrak.damageGiven += Mathf.Round(damage);
            }
            UpdateBars();
        }
        CheckIfDead();
    }
    public void SetMaxOnBars()
    {
        if (healthBar != null)
        {
            SetMaxHealthBar();
        }
        if (shieldBar != null)
        {
            SetMaxShieldBar();
        }
    }
    public void SetMaxHealthBar()
    {
        healthBar.maxValue = healthMaximum;
    }
    public void SetMaxShieldBar()
    {
        shieldBar.maxValue = shieldMaximum;
    }
    public void UpdateBars()
    {
        if (healthBar != null)
        {
            UpdateLocalHealthbar();
        }
        if (shieldBar != null)
        {
            UpdateLocalShieldbar();
        }
    }
    public void UpdateLocalHealthbar()
    {
        healthBar.value = health;
    }
    public void UpdateLocalShieldbar()
    {
        shieldBar.value = shield;
    }
    public void CheckIfDead()
    {
        if (health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (gameObject.tag == ("Player"))
        {
            transform.parent.gameObject.GetComponent<PlayerManager>().PlayerHasDied();
        }
        //Finally
        if (gameObject.tag == ("Enemy"))
        {
            if (!isDying)
            {
                isDying = true;
                gameUIManager.PointsForKillingEnemy(gameObject.transform, thisObjectPoints);
                gameUIManager.UpdateHiScore(thisObjectPoints);
                wm.listEnemies.Remove(this.gameObject);


            }
        }
        if (gameObject.tag == "allied")
        {
            wm.listAllies.Remove(this.gameObject);
        }
        if (!gameObject.tag.Contains("Projectile"))
        {
            GameObject explosionClone = Instantiate(explosionPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            explosionClone.transform.localScale = new Vector3(this.gameObject.transform.lossyScale.x + 17.5f, this.gameObject.transform.lossyScale.y + 17.5f, 1f);
        }
        Destroy(gameObject);
    }

    void DisableInvunerability()
    {
        isInvunerable = false;
    }
    public void RestoreHealthToMaximum()
    {
        health = healthMaximum + healthBonus;
        UpdateBars();
        if (gameObject.tag == ("Player"))
        {
            gameUIManager.updateHealth(health);
        }
    }
    public void RestoreShieldToMaximum()
    {
        shield = shieldMaximum + shieldBonus;
        UpdateBars();
        if (gameObject.tag == ("Player"))
        {
            gameUIManager.updateShield(shield);
        }
    }
    //Have its own collision detection
    //If is player
    //If has been hit by non-player projectile
    //Did it damage me lol
    //Take from health
    //Did it damage me enough for me to kill me
}
