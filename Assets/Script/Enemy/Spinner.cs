using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : Plane
{
    protected override GameObject Shoot()
    {
        Projectile p = Instantiate(projectile, spawnPlace.position, Quaternion.identity);
        p.damage = attack;
        Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
        rb.AddForce(1000 * new Vector2(  Random.Range(0, 1f) < 0.5 ?(Random.Range(-3, -1)):(Random.Range(1, 3)) , 1) );

        return null;
    }
}
