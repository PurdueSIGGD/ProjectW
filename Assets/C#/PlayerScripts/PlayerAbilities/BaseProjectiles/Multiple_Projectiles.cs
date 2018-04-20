using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiple_Projectiles : Ability_ProjectileSpawner {

    public float amountToSpawn;
    public float spreadMultiplier;

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnAngle = data.vectorList[0];
        Vector3 spawnPosition = data.vectorList[1];

        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
            Rigidbody r;
            Vector3 spawnAngleTemp = spawnAngle;
            if (r = spawn.GetComponent<Rigidbody>())
            {
                spawnAngleTemp.x += (float)((Random.Range(0.0f, 1.0f) - .5) * spreadMultiplier);
                spawnAngleTemp.y += (float)((Random.Range(0.0f, 1.0f) - .5) * spreadMultiplier);
                spawnAngleTemp.z += (float)((Random.Range(0.0f, 1.0f) - .5) * spreadMultiplier);
                
                r.AddForce(spawnAngleTemp * spawnSpeed);
            }
            OnSpellSpawned(spawn);
        }
    }
}
