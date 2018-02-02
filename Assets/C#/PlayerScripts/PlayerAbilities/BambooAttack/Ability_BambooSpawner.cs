using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_BambooSpawner : Ability_ObjectSpawner {

    public float bambooLifetime = 4;

    public override void OnSpellSpawned(GameObject spawn)
    {
        Bamboo b;
        if(b = spawn.GetComponent<Bamboo>())
        {
            b.StartBamboo(this, this.bambooLifetime);
        }
    }
}
