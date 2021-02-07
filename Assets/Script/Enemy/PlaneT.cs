using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneT : Plane
{
    [SerializeField] protected Transform spawnPlace2 = null;
    [SerializeField] protected Transform spawnPlace3 = null;
    protected override GameObject Shoot()
    {
        Projectile p = Instantiate(projectile, spawnPlace.position, Quaternion.identity);
        p.damage = attack;
        p = Instantiate(projectile, spawnPlace2.position, Quaternion.identity);
        p.damage = attack;
        p = Instantiate(projectile, spawnPlace3.position, Quaternion.identity);
        p.damage = attack;

        return null;
    }
}
