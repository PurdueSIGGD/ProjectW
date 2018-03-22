using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_GrassTrapSpawner : Ability_PointSpawner
{

    public float effectDuration = 3;
    public float trapLifetime = 6;
    public float damage = 20;

    public override void OnSpellSpawned(GameObject spawn)
    {
        GrassTrap g;
        if(g = spawn.GetComponent<GrassTrap>())
        {
            g.StartGrassTrap(this, this.trapLifetime, this.effectDuration, this.GetComponentInParent<PlayerStats>().gameObject);
        }
    }

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnPosition = data.vectorList[0];

        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
        OnSpellSpawned(spawn);
    }
}