using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArguments {
    public enum DamageType { Neutral, Fire, Ice, Electric, Denim };

    public GameObject target;
    public GameObject sourcePlayer;
    public float damage;
    public DamageType damageType;
    public PlayerEffects.Effects effect;
    public float effectDuration;

    public HitArguments()
    {
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
}
