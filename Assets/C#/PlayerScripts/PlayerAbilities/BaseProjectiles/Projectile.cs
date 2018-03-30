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
    public bool hitSameTeam = false;
    public float currentVelocity = 0;

    private bool hasHit;

    //private GameObject hitPlayer;

    void Start() {
		Invoke("DestroyMe", lifetime);
        currentVelocity = this.GetComponent<Rigidbody>().velocity.magnitude;
    }

    void DestroyMe()
    {
        Destroy(this.gameObject);
    }

	void OnTriggerEnter(Collider col) {
        /* CHECKS FOR HIT VALIDIDTY */
        if (hasHit && dieOnHit) return; // We only want to hit one object... for some reason it collides multiple times before destroying itself
        if (col.isTrigger) return; // Only want our own trigger effects
        PlayerStats ps;
        if (!sourcePlayer) return; // Shouldn't collide with anything that isn't a source player
        if ((ps = col.GetComponentInParent<PlayerStats>())) {
            if (ps.gameObject == sourcePlayer.gameObject) return;
            if (!hitSameTeam && ps.teamIndex == sourcePlayer.GetComponent<PlayerStats>().teamIndex && ps.teamIndex != -1) return; // dont hit players on same team
        }
        /* ACTIONS TO TAKE POST-HIT */

        hasHit = true;
		Rigidbody myRigid = this.GetComponent<Rigidbody> ();
        if (col.GetComponentInParent<IHittable>() != null) {
            HitManager.HitClientside(HitManager.HitVerificationMethod.projectile, new HitArguments(((Component)col.gameObject.GetComponentInParent<IHittable>()).gameObject, sourcePlayer.GetComponentInParent<PlayerStats>().gameObject)
                .withDamage(damage)
                .withDamageType(damageType)
                .withEffect(effect)
				.withEffectDuration(effectDuration)
				.withSourcePosition(new Vector3(transform.position.x, transform.position.z) - 3 * new Vector3(myRigid.velocity.x, myRigid.velocity.z))
                .withHitSameTeam(hitSameTeam));
        }

        if (dieOnHit) {
            if (explodeParticles) {
				trailParticles.Stop ();
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
