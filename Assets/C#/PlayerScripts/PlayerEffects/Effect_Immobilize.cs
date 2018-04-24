using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Immobilize : Effect {

    PlayerEffects effectTarget;
    public override void Effect_Start(PlayerEffects target)
    {
        effectTarget = target;
        target.immobilized = true;
    }
    public override void Effect_End(PlayerEffects target)
    {
        target.immobilized = false;
    }
    public override void Effect_Update()
    {
        if (effectTarget.immobilized == false)
            effectTarget.immobilized = true;
    }
}
