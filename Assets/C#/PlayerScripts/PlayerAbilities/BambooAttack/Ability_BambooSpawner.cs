using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_BambooSpawner : Ability_PointSpawner {

    public float bambooLifetime = 4;
    public float bambooShootsSpawned = 5;
    public float damage = 20;
    public GameObject bambooShootToSpawn;
    public float spreadMultiplier;

    public override void OnSpellSpawned(GameObject spawn)
    {
        Bamboo b;
        if(b = spawn.GetComponent<Bamboo>())
        {
            b.StartBamboo(this, this.bambooLifetime, this.GetComponentInParent<PlayerStats>().gameObject);
        }
    }

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnPosition = data.vectorList[0];
        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        for (int i = 0; i < bambooShootsSpawned; i++)
        {
            Vector3 t = new Vector3(0,0,0);
            t.x += (float)((Random.Range(0.0f, 1.0f) - .5) * spreadMultiplier);
            t.z += (float)((Random.Range(0.0f, 1.0f) - .5) * spreadMultiplier);
            GameObject spawn = GameObject.Instantiate(bambooShootToSpawn, spawnPosition + transform.TransformDirection(spawnOffset) + t, transform.rotation);
            Destroy(spawn, bambooLifetime);
        }
        GameObject newSpawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
        OnSpellSpawned(newSpawn);
    }
}
