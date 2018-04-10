using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : Expanding_Projectile {

    private ArrayList hasHitThem = new ArrayList();
    private bool hitThem;
    private GameObject particleEffect;
    private bool done = false;
    public GameObject particleEffectForWhenItHitsAWall;
    public float endingParticleEffectLifetime;
    public float maxEndParticleScale = 1.6f;
    public float endParticleScaleDivisor = 3.5f;
    public float maxDistanceToHaveEndParticleEffect = 7;

    public void initEffect(GameObject e)
    {
        particleEffect = e;
    }

    void OnTriggerEnter(Collider col)
    {
        /* CHECKS FOR HIT VALIDIDTY */
        if (done) return;
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
        }else if(this.transform.localScale.x < maxDistanceToHaveEndParticleEffect)
        {
            done = true;
            Destroy(this.particleEffect);
            Destroy(this.GetComponentInChildren<ParticleSystem>().gameObject, .2f);
            trailParticles.Stop();
            GameObject endParticleEffect = GameObject.Instantiate(particleEffectForWhenItHitsAWall, this.transform.position, this.transform.rotation);
            if (this.transform.localScale.x / endParticleScaleDivisor < maxEndParticleScale)
            {
                endParticleEffect.transform.localScale = this.transform.localScale / endParticleScaleDivisor;
            }
            else
                endParticleEffect.transform.localScale = new Vector3(maxEndParticleScale, maxEndParticleScale, maxEndParticleScale);
            Destroy(endParticleEffect, endingParticleEffectLifetime);
            Destroy(this.gameObject, endingParticleEffectLifetime);
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
