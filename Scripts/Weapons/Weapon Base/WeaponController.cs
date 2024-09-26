using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script (base) para controle de armas
/// </summary>

public class WeaponController : MonoBehaviour
{

    [Header("Weapon Stats")]
    public WeaponScriptableObject weaponStats;
    float currentCooldown;

    protected PlayerMovement pm;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        currentCooldown = weaponStats.CooldownDuration;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        currentCooldown -= Time.deltaTime;

        if(currentCooldown <= 0f)
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        currentCooldown = weaponStats.CooldownDuration;
    }
}
