using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipPuddle : MonoBehaviour {

    [HideInInspector]
    public GameObject sourcePlayer;

    public float damage;
    public HitArguments.DamageType damageType;

    public PlayerEffects.Effects[] effects; // list of effects
    public float effectDuration = 3;

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Enter slip puddle");

        /* CHECKS FOR HIT VALIDIDTY */
        PlayerStats ps;
        if (!sourcePlayer) return; // Shouldn't collide with anything that isn't a source player
        if ((ps = col.GetComponentInParent<PlayerStats>()))
        {
            if (ps.gameObject == sourcePlayer.gameObject) return;
        }

        /* ACTIONS TO TAKE POST-HIT */
        if (col.GetComponentInParent<IHittable>() == null)
            return;

        // apply effect on player
        foreach (PlayerEffects.Effects effect in effects) {

            HitManager.HitClientside(HitManager.HitVerificationMethod.projectile, new HitArguments(((Component)col.gameObject.GetComponentInParent<IHittable>()).gameObject, sourcePlayer.GetComponentInParent<PlayerStats>().gameObject)
              .withDamage(damage)
              .withDamageType(damageType)
              .withEffect(effect)
              .withEffectDuration(effectDuration));
        }
       

    }
}
