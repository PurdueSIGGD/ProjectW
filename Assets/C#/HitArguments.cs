﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArguments {
    public enum DamageType { Neutral, Fire, Ice, Electric, Denim };

    public GameObject target;
    public GameObject sourcePlayer;
    public int sourcePlayerTeam = -1;
    public float damage;
    public DamageType damageType;
    public PlayerEffects.Effects effect;
    public float effectDuration;
	public float effectDamage;
    public bool hitSameTeam;
	public Vector2 sourcePosition = new Vector2(0,0); // Used for where it comes from
    public int weaponType = 0;

	public HitArguments() {
		//Debug.LogError ("HitArguments require a target and a source player");
	}
    public HitArguments(GameObject target, GameObject sourcePlayer)
    {
        this.target = target;
        this.sourcePlayer = sourcePlayer;
        effect = PlayerEffects.Effects.none;
        damageType = DamageType.Neutral;
    }

    public HitArguments withTarget(GameObject target)
    {
        this.target = target;
        return this;
    }
    public HitArguments withSourcePlayer(GameObject sourcePlayer)
    {
        this.sourcePlayer = sourcePlayer;
        return this;
    }
    public HitArguments withDamage(float damage)
    {
        this.damage = damage;
        return this;
    }
    public HitArguments withDamageType(DamageType damageType)
    {
        this.damageType = damageType;
        return this;
    }
    public HitArguments withEffect(PlayerEffects.Effects effect)
    {
        this.effect = effect;
        return this;
    }
    public HitArguments withEffectDuration(float effectDuration)
    {
        this.effectDuration = effectDuration;
        return this;
    }
	public HitArguments withEffectDamage(float effectDamage) {
		this.effectDamage = effectDamage;
		return this;
	}	
    public HitArguments withSourcePlayerTeam(int team)
    {
        this.sourcePlayerTeam = team;
        return this;
    }
    public HitArguments withHitSameTeam(bool hitSameTeam)
    {
        this.hitSameTeam = hitSameTeam;
        return this;
    }
	public HitArguments withSourcePosition(Vector2 sourcePosition) {
		this.sourcePosition = sourcePosition;
		return this;
	}
    public HitArguments withWeaponType(int weaponType) 
    {
        this.weaponType = weaponType;
        return this;
    }
}
