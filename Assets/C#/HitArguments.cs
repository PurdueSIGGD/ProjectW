using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitArguments {
    public enum DamageType { Neutral, Fire, Ice, Electric, Denim };

    public PlayerStats target;
    public PlayerStats sourcePlayer;
    public float damage;
    public DamageType damageType;
    public PlayerEffects.Effects effect;
    public float effectDuration;

    public HitArguments()
    {
        effect = PlayerEffects.Effects.none;
        damageType = DamageType.Neutral;
    }

    public HitArguments withTarget(PlayerStats target)
    {
        this.target = target;
        return this;
    }
    public HitArguments withSourcePlayer(PlayerStats sourcePlayer)
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
