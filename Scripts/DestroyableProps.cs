using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableProps : MonoBehaviour
{
    public float health;

    public void GripDamage(float dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }


}
