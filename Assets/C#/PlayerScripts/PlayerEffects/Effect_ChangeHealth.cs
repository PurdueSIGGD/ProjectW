using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;	

public class Effect_ChangeHealth : Effect {
	private PlayerStats targetStats;
	public float damage = 5;
	public float coolDown = .3f;
	float lastDamage = -1f;
    public override void Effect_Start(PlayerEffects target) {
		this.targetStats = target.GetComponent<PlayerStats> ();
        targetStats.poisonStacks++;
    }

    public override void Effect_End(PlayerEffects target) {
        targetStats.poisonStacks--;
    }

    public override void Effect_Update() {
        
        if (Time.time - lastDamage > coolDown) {
            if (targetStats.poisonStacks > 1)
            {
                targetStats.poisonStacks--;
                Destroy(this.gameObject);
            }
			lastDamage = Time.time;
			HitManager.HitClientside (new HitArguments (targetStats.gameObject, this.sourcePlayer.GetComponentInParent<PlayerStats> ().gameObject)
				.withDamage (damage));
		} 
	
    }

}
