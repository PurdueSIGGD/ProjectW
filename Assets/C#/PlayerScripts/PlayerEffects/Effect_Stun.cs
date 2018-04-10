using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Stun : Effect
{
    public override void Effect_End(PlayerEffects target)
    {
        target.stunned = false;
    }

    public override void Effect_Start(PlayerEffects target)
    {
        target.stunned = true;
    }

    public override void Effect_Update()
    {
        
    }
}
