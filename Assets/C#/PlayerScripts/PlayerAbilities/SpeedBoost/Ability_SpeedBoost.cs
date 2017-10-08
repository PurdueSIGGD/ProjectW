using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SpeedBoost : CooldownAbility {
    public float percentageGain = 0.3f;
    public float duration = 3f;

    public override void use_CooledDown() {
        Effect_SpeedBoost boost = this.gameObject.AddComponent<Effect_SpeedBoost>();
        boost.Initialize(myBase.myMovement, percentageGain, duration);
    }
}
