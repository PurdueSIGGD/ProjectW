using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CooldownAbility : PlayerAbility {
    /**
     * Abstract class for repeated instances of cooldowns.
     * You do not have to override the 'use' metod as a child of this, all you have to do is implement use_CooledDown
     */
    public float cooldown = 1; // Cooldown, in seconds
    private float lastUse = -100; // Last time we used it, in seconds;
    public override void use() {
        if (Time.time - lastUse > cooldown) {
            use_CooledDown();
            lastUse = Time.time;
        }
    }
    public abstract void use_CooledDown();

}
