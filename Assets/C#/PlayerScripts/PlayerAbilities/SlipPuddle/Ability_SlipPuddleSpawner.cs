using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SlipPuddleSpawner : Ability_ObjectSpawner {

    public override void OnSpellSpawned(GameObject spawn)
    {
        SlipPuddle p;
        if (p = spawn.GetComponentInChildren<SlipPuddle>())
        {
            Debug.Log("SlipPuddle: apply sourceplayer");
            p.sourcePlayer = this.gameObject;
        }
    }
}
