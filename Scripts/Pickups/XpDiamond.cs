using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpDiamond : ParentPickup, InfCollector
{
    public int xpConceded;

    public void Collect()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        playerStats.AddXP(xpConceded);
    }
 
}
