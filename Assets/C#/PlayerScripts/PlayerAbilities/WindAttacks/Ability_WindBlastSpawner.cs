using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_WindBlastSpawner : Ability_ObjectSpawner
{
    private Vector3 angle;

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnAngle = data.vectorList[0];
        Vector3 spawnPosition = data.vectorList[1];


        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
        Rigidbody r;
        if (r = spawn.GetComponent<Rigidbody>())
        {
            r.AddForce(spawnAngle * spawnSpeed);
        }
        angle = spawnAngle;
        OnSpellSpawned(spawn);
    }

    public override void OnSpellSpawned(GameObject spawn)
    {
        WindBlast w;
        if (w = spawn.GetComponent<WindBlast>())
        {
            w.sourcePlayer = this.gameObject;
            w.angle = this.angle;
        }
    }
}
