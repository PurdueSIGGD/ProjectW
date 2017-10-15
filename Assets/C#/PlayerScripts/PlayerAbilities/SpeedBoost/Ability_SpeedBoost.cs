using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ability_SpeedBoost : CooldownAbility {
    public float percentageGain = 0.3f;
    public float duration = 3f;
    public GameObject startEffect, endSound;

    public override void use_UseAbility() {
        GameObject.Instantiate(startEffect, transform);
    }

    public override void cooldown_Update() { }

    public override void use_CanUse() { }

    public override void cooldown_Start() {

    }
}
