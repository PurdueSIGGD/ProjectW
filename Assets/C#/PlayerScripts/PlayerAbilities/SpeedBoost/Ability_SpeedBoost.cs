using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ability_SpeedBoost : CooldownAbility {
    public float percentageGain = 0.3f;
    public float duration = 3f;
    public GameObject startSound, endSound;
    [SyncVar]
    bool used = false;
    bool hasUsed = false;

    public override void use_CooledDown() {
        used = true;
    }
    // Oh, so unity doesn't like to have two updates in the same compnent, likea parent class. So that wouldn't work.
    void LateUpdate() {
        if (used && !hasUsed) {
            hasUsed = true;
            Effect_SpeedBoost boost = this.gameObject.AddComponent<Effect_SpeedBoost>();
            boost.Initialize(myBase.myMovement, percentageGain, duration, startSound, endSound);
        }
    }

    public override void use_CanUse() {
        used = false;
        hasUsed = false;
    }
}
