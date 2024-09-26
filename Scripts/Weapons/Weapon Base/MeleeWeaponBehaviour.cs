using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script base para armas de corpo a corpo (deve ser colocado em um prefab de arma melee)
/// </summary>

public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponStats;
     
    public float destroyAfter;

    // Stats atuais
    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuration;
    protected int currentPierce;

    void Awake()
    {
        currentDamage = weaponStats.Damage;
        currentSpeed = weaponStats.Speed;
        currentCooldownDuration = weaponStats.CooldownDuration;
        currentPierce = weaponStats.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().CurrentMight;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            EnemyData enemy = col.GetComponent<EnemyData>();
            enemy.GripDamage(GetCurrentDamage());
        }
        else if (col.CompareTag("Prop"))
        {
            if(col.gameObject.TryGetComponent(out DestroyableProps destroyable))
            {
                destroyable.GripDamage(GetCurrentDamage());
            }
        }
    }
}
