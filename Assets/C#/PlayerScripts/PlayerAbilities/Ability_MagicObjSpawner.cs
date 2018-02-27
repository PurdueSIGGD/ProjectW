using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_MagicObjSpawner : Ability_ObjectSpawner {

    public override void OnSpellSpawned(GameObject spawn)
    {
        MagicObject p;
        if ( p = spawn.GetComponent<MagicObject>())
        {
            p.sourcePlayer = this.gameObject;
            
        }
    }
}
