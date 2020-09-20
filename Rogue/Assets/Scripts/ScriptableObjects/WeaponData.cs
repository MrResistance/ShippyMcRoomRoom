using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptables/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string name; //Weapon name
    public string description;
    public Sprite sprite; //For projectile
    public Color colourSprite;
    public float scale = 1; //for size

    public int projectileCount = 1;
    //Projectile damage
    public float damage = 10; //Damage of projectile on-impact
    public float d_healthMultiplier = 1; //How much more/less damage done to health
    public float d_shieldMultiplier = 1; //How much more/less damange done to shields
    public float rateoffire = 1;
    public float speed = 50;
    public float chargetime = 0; //How long it takes to charge the weapon for it to fire

    public string firePosition = "single"; //"single" - firing from middle-front // "twin" - firing from middle-front offset to left and right //"side" - firing from sides of ship at 90' angle 

    public int healthProjectile = 10;

    public float lifetimeProjectile = 3; //In seconds, how long projectile will last before dying - if 0, it will only die on impact (or leaving combat area)
    [MinMaxSlider(-1, 1,true)]
    public Vector2 lifetimeMinMaxMultiplier = new Vector2(0, 0);

    [Title("Ammo and weapon lifetime")]
    public bool isUnlimitedAmmo = true;
    public int ammoMax;
    public bool isWeaponLifeless = true;
    public float lifetimeWeapon;

    [Title("Spread")]
    //Spread
    //test
    [ValueDropdown("spreadValues")]
    public string spreadType;

    private ValueDropdownList<string> spreadValues = new ValueDropdownList<string>()
    {
        {"None","none"},
        {"Random","random"},
        {"Even","even"},
    };

    //public string spreadType = "none"; //Types: "none", "random", "even"
    [ShowInInspector,PropertyRange(0,180)]
    public float spread; //Accuracy; 

    public float range; //How far the projectile travels before it either evaporates or explodes
    
    

    [Title("Explosive")]
    //Explosion stuff
    [ValueDropdown("explosionTypes")]
    public string explosionType = "none"; //"none" , "Throughout" - same damage to the end -- "Degrading" - damage more towards center
    private ValueDropdownList<string> explosionTypes = new ValueDropdownList<string>()
    {
        {"None","none"},
        {"Throughout","throughout"},
        {"Degrading","degrading"},
    };
    public bool isExplosionOnImpact;
    public float explosiveradius;
    public int explosivedamage;
    public GameObject prefabExplosive;

    [Title("Scattered projectile")]
    //Scattered projectiles - fireworks
    public bool isScatteredProjectilesOnImpact;
    public string scatteredprojectiletype; //"even" - Always same dispersion -- "random" - they can fly out anywhere
    public int scatteredprojectilecount;
    public float scatteredprojectilespeed;
    public float scatteredprojectilerange;
    public int scattedprojectiledamage;

    [Title("Piercing")]
    //Piercing
    public bool canPierce = false;
    public int amountCanPierce;

    [Title("Tracking")]
    //Tracking - for missiles and anything else we want the projectile to track
    public bool isTrackingProjectile;
    public float trackingRotationSpeed;
    public bool hasHomingDelay = false; //If true, the projectile waits for the rateoffire time before setting off - used on missiles
    [Title("Audio")]
    public AudioClip launchSound;
    public AudioClip impactSound;

    [Title("UI")]
    public Sprite uiSprite; //NOT YET USED - TO BE USED INSTEAD OF USING THE PROJECTILE SPRITE FOR DISPLAY wea-PON
}

