using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ProjectileSpawner : Ability_ObjectSpawner
{
    public override void OnSpellSpawned(GameObject spawn)
    {
        Projectile p;
        if (p = spawn.GetComponent<Projectile>())
        {
            p.sourcePlayer = this.gameObject;
        }
    }

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnAngle = data.vectorList[0];
        Vector3 spawnPosition = data.vectorList[1];

        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation*Quaternion.Euler(itemToSpawn.transform.rotation.eulerAngles));
        Rigidbody r;
        if (r = spawn.GetComponent<Rigidbody>())
        {
            r.AddForce(spawnAngle * spawnSpeed);
        }
        Quaternion up = (Quaternion.Euler(r.GetComponent<Rigidbody>().velocity));
        r.transform.LookAt(r.transform.position + r.GetComponent<Rigidbody>().velocity, up.eulerAngles);
        OnSpellSpawned(spawn);
    }
}
