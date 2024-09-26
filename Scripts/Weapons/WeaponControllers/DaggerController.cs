using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

   protected override void Shoot()
    {
        base.Shoot();
        GameObject spawnedDagger = Instantiate(weaponStats.Prefab);
        spawnedDagger.transform.position = transform.position; // Posicao inicial da arma
        spawnedDagger.GetComponent<DaggerBehaviour>().DirectionChecker(pm.lastMovedVector); // Direcao da arma
    }
}
