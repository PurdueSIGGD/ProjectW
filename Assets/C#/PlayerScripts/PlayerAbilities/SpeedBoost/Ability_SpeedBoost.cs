using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ability_SpeedBoost : CooldownAbility {
    public GameObject startEffect;

    public override void use_UseAbility() {
        GameObject.Instantiate(startEffect, transform);
    }

    public override void cooldown_Update() { }

    public override void use_CanUse() { }

    public override void cooldown_Start() {

    }
}
