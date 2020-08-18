using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptables/WeaponData")]
public class WeaponData : ScriptableObject
{
    public string name; //Weapon name
    public string description;
    public int damage; //Damage of projectile on-impact
    public float rateoffire;
    public float speed;
    public float chargetime; //How long it takes to charge the weapon for it to fire
    public float spread; //Accuracy; 
    public float range; //How far the projectile travels before it either evaporates or explodes
    public bool isUnlimitedAmmo;
    public int ammo;

    //Explosion stuff
    public bool isExplosiveProjectile; //Required to be true for any explosive stuff
    public string explosionType; //"Throughout" - same damage to the end -- "Degrading" - damage more towards center
    public bool isExplosionOnImpact;
    public float explosiveradius;
    public int explosivedamage;

    //Scattered projectiles - fireworks
    public bool isScatteredProjectilesOnImpact;
    public string scatteredprojectiletype; //"even" - Always same dispersion -- "random" - they can fly out anywhere
    public int scatteredprojectilecount;
    public float scatteredprojectilespeed;
    public float scatteredprojectilerange;
    public int scattedprojectiledamage;
}

