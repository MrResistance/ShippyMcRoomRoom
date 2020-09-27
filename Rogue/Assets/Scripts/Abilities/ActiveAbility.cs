using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbility : EntityAbility
{

    public bool isSelected = false;

    public bool isAbilityReady = true;
    public float abilityCooldown = 0;
    public float abilityTimeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateCooldown()
    {
        isAbilityReady = false;
        abilityTimeRemaining = abilityCooldown;
        Invoke("DeactivateCooldown", abilityCooldown);
        
    }

    void DeactivateCooldown()
    {
        isAbilityReady = true;
        abilityTimeRemaining = 0;
    }
}
