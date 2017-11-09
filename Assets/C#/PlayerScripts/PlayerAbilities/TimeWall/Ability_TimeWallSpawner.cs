using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_TimeWallSpawner : Ability_ObjectSpawner {
    public override void OnSpellSpawned(GameObject spawn)
    {
        TimeWall t;
        if (t = spawn.GetComponent<TimeWall>())
        {
            t.sourcePlayer = this.gameObject;
            t.cooldown = this.cooldown;
        }
    }
}
