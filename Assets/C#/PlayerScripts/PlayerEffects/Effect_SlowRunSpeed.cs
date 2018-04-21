using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_SlowRunSpeed : Effect {
    public float runSpeedScale = 0.5f;
    public override void Effect_Start(PlayerEffects target)
    {
        target.runSpeedModifier *= runSpeedScale;
    }

    public override void Effect_End(PlayerEffects target)
    {
        target.runSpeedModifier /= runSpeedScale;
    }

    public override void Effect_Update()
    {

    }
}
