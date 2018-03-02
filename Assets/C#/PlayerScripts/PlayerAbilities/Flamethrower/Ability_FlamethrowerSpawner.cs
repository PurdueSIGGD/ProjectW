using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_FlamethrowerSpawner : Ability_ObjectSpawner {

    public override void OnSpellSpawned(GameObject spawn)
    {
        Projectile p;
        if (p = spawn.GetComponent<Projectile>())
        {
            p.sourcePlayer = this.gameObject;
        }
    }
}
