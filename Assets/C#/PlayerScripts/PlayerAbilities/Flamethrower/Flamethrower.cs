using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Expanding_Projectile {

    private ArrayList hasHitThem = new ArrayList();
    private bool hitThem;

    void OnTriggerEnter(Collider col)
    {
        /* CHECKS FOR HIT VALIDIDTY */
        if (hitThem && dieOnHit) return; // We only want to hit one object... for some reason it collides multiple times before destroying itself
        if (col.isTrigger) return; // Only want our own trigger effects
        PlayerStats ps;
        if (!sourcePlayer) return; // Shouldn't collide with anything that isn't a source player
        if ((ps = col.GetComponentInParent<PlayerStats>()))
        {
            if (ps.gameObject == sourcePlayer.gameObject) return;
            if (!hitSameTeam && ps.teamIndex == sourcePlayer.GetComponent<PlayerStats>().teamIndex && ps.teamIndex != -1) return; // dont hit players on same team
        }
        /* ACTIONS TO TAKE POST-HIT */

        hitThem = true;
        if (col.GetComponentInParent<IHittable>() != null && !hasHitThem.Contains(ps))
        {
            HitManager.HitClientside(HitManager.HitVerificationMethod.projectile, new HitArguments(((Component)col.gameObject.GetComponentInParent<IHittable>()).gameObject, sourcePlayer.GetComponentInParent<PlayerStats>().gameObject)
                .withDamage(damage)
                .withDamageType(damageType)
                .withEffect(effect)
                .withEffectDuration(effectDuration)
                .withHitSameTeam(hitSameTeam));
            hasHitThem.Insert(hasHitThem.Count, ps);
        }

        if (dieOnHit)
        {
            if (explodeParticles)
            {
                trailParticles.Stop();
                ParticleSystem.MainModule m = trailParticles.main;
                m.loop = false;
                trailParticles.transform.parent = null;
                GameObject spawn = GameObject.Instantiate(explodeParticles, transform.position, Quaternion.identity);
                Explosion e;
                if ((e = spawn.GetComponent<Explosion>()))
                {
                    e.sourcePlayer = sourcePlayer;
                }

            }
            Destroy(this.gameObject);

        }
    }
}
