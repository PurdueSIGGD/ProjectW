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
    }

    public override void Effect_End(PlayerEffects target) {
        
    }

    public override void Effect_Update() {

		if (Time.time - lastDamage > coolDown) {
			lastDamage = Time.time;
			HitManager.HitClientside (new HitArguments (targetStats.gameObject, this.sourcePlayer.GetComponentInParent<PlayerStats> ().gameObject)
				.withDamage (damage));
		} 
	
    }

}
