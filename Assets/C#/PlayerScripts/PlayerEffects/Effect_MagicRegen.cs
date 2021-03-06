﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_MagicRegen : Effect {
    public float regenRate = 0.5f;

    public override void Effect_End(PlayerEffects target) {
        target.magicRegenModifier *= regenRate;
    }

    public override void Effect_Start(PlayerEffects target) {
        target.magicRegenModifier /= regenRate;
    }

    public override void Effect_Update() {
    }

    
}
