using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Plane
{
    protected override GameObject Shoot()
    {
        GameObject missile = base.Shoot();
        missile.GetComponent<HomingMissile>().target = GameSceneManager.main.player.transform.position;
       // missile.transform.rotation = Quaternion.FromToRotation(Vector2.up, Vector2.down);
        return missile;
    }
}
