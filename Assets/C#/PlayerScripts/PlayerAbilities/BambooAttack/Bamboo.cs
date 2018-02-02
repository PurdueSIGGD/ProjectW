using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bamboo : MonoBehaviour {

    Ability_BambooSpawner ability;

    public void StartBamboo(Ability_BambooSpawner ability, float cooldown)
    {
        this.ability = ability;
        Destroy(this.gameObject, cooldown);
    }
}
