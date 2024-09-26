using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion :ParentPickup, InfCollector
{

    public int healthConceded;

    public void Collect()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.CureHealth(healthConceded);
    }
  
}
