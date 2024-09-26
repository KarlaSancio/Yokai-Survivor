using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> enemiesHit;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        enemiesHit = new List<GameObject>();
    }

   protected override void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Enemy") && !enemiesHit.Contains(collision.gameObject))
       {
           EnemyData enemy = collision.GetComponent<EnemyData>();
           enemy.GripDamage(GetCurrentDamage());

            enemiesHit.Add(collision.gameObject);
       }
        else if (collision.CompareTag("Prop"))
        {
            if(collision.gameObject.TryGetComponent(out DestroyableProps destroyable) && !enemiesHit.Contains(collision.gameObject))
            {
                destroyable.GripDamage(GetCurrentDamage());

                enemiesHit.Add(collision.gameObject);
            }
        }
    }
}
