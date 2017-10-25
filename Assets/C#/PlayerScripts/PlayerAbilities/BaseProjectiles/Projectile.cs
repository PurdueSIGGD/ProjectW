using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

public class Projectile : MonoBehaviour {
    [HideInInspector]
    public GameObject sourcePlayer;

    public float damage;
    public HitArguments.DamageType damageType;
    public bool dieOnHit = true;
    public GameObject explodeParticles;
    public ParticleSystem trailParticles;
    public float lifetime = 10;
    public PlayerEffects.Effects effect;
    public float effectDuration = 3;

    private bool hasHit;

    //private GameObject hitPlayer;

    void Start() {
        Destroy(this.gameObject, 10);
    }

	void OnTriggerEnter(Collider col) {
        /* CHECKS FOR HIT VALIDIDTY */
        if (hasHit && dieOnHit) return; // We only want to hit one object... for some reason it collides multiple times before destroying itself
        if (col.isTrigger) return; // Only want our own trigger effects
        PlayerStats ps;
        if (!sourcePlayer) return; // Shouldn't collide with anything that isn't a source player
        if ((ps = col.GetComponentInParent<PlayerStats>())) {
            if (ps.gameObject == sourcePlayer.gameObject) return;
        }
        /* ACTIONS TO TAKE POST-HIT */

        hasHit = true;
        if (col.GetComponentInParent<IHittable>() != null) {
            HitManager.HitClientside(HitManager.HitVerificationMethod.projectile, new HitArguments()
                .withTarget(((Component)col.gameObject.GetComponentInParent<IHittable>()).gameObject)
                .withSourcePlayer(sourcePlayer.GetComponentInParent<PlayerStats>().gameObject)
                .withDamage(damage)
                .withDamageType(damageType)
                .withEffect(effect)
                .withEffectDuration(effectDuration));
        }

        if (dieOnHit) {
            if (explodeParticles) {
                ParticleSystem.MainModule m = trailParticles.main;
                m.loop = false;
                trailParticles.transform.parent = null;
                GameObject spawn = GameObject.Instantiate(explodeParticles, transform.position, Quaternion.identity);
                Explosion e;
                if ((e = spawn.GetComponent<Explosion>())) {
                    e.sourcePlayer = sourcePlayer;
                }
               
            } 
            Destroy(this.gameObject);
            
        }
    }
	
	
}
