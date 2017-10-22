using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_SlowTime : Effect {
    public float timeScale = 0.5f;
    public override void Effect_Start(PlayerEffects target) {
        target.timeModifier *= timeScale;
    }

    public override void Effect_End(PlayerEffects target) {
        target.timeModifier /= timeScale;
    }

    public override void Effect_Update() {

    }

}
