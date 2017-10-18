using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_ExplosionSpawner : Ability_ObjectSpawner {
    public override void OnSpellSpawned(GameObject spawn)
    {
        print("Spawning explosion");
        Explosion e;
        if (e = spawn.GetComponent<Explosion>())
        {
            e.sourcePlayer = this.gameObject;
        }
    }
}
