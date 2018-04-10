using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_FlamethrowerSpawner : Ability_ObjectSpawner {

    public GameObject FlameThrowerEffect;
    private GameObject flameSpawned;
    public float lifetime = 1;
    public PlayerEffects.Effects effect;

    public override void OnSpellSpawned(GameObject spawn)
    {
        Flamethrower f;
        if (f = spawn.GetComponent<Flamethrower>())
        {
            f.sourcePlayer = this.gameObject;
            f.initEffect(flameSpawned);
        }

    }

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnAngle = data.vectorList[0];
        Vector3 spawnPosition = data.vectorList[1];


        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
        Rigidbody r;

        Quaternion aimAngle = Quaternion.LookRotation(spawnAngle);
        flameSpawned = GameObject.Instantiate(FlameThrowerEffect, spawnPosition + transform.TransformDirection(spawnOffset), aimAngle);
        if (r = spawn.GetComponent<Rigidbody>())
        {
            r.AddForce(spawnAngle * spawnSpeed);
        }
        OnSpellSpawned(spawn);

        Destroy(flameSpawned, lifetime);
        PlayerStats sourcePlayerStats = GetComponentInParent<PlayerStats>();
        HitManager.HitClientside(new HitArguments(sourcePlayerStats.gameObject, sourcePlayerStats.gameObject).withEffect(effect).withEffectDuration(cooldown));
    }
}
