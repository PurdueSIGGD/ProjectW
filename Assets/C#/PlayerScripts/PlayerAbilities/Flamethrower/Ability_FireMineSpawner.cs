using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_FireMineSpawner : Ability_PointSpawner {

    public float mineLifetime = 6;
    public bool hitSameTeam = false;

    public override void OnSpellSpawned(GameObject spawn)
    {
        FireMine f;
        if (f = spawn.GetComponent<FireMine>())
        {
            f.StartFireMine(this.mineLifetime, this.GetComponentInParent<PlayerStats>(), hitSameTeam);
        }
    }

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnPosition = data.vectorList[0];

        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
        OnSpellSpawned(spawn);
    }
}
