using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornsController : WeaponController
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    protected override void Shoot()
    {
        base.Shoot();
        GameObject spawnedThorns = Instantiate(weaponStats.Prefab);
        spawnedThorns.transform.position = transform.position; // Posicao inicial da arma
        spawnedThorns.transform.parent = transform; // Parenteia o objeto com o jogador
    }
}
