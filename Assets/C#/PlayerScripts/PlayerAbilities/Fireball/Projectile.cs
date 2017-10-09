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
    public Hittable.DamageType damageType;
    public bool dieOnHit = true;
    public GameObject explodeParticles;
    public ParticleSystem trailParticles;

    private GameObject hitPlayer;

	void OnTriggerEnter(Collider col) {
        if (hitPlayer) return; // We only want to hit one object... for some reason it collides multiple times before destroying itself
        if (col.isTrigger) return; // Only want our own trigger effects
        PlayerStats ps;
        if (!sourcePlayer) return; // Shouldn't collide with anything that isn't a source player
        if ((ps = col.GetComponentInParent<PlayerStats>())) {
            //print(ps);
            if (ps.gameObject == sourcePlayer.gameObject) return;
        }
        IHittable h;
        if ((h = col.GetComponentInParent<IHittable>()) != null) {
            //print("i am going to hit" + hitPlayer);
            PlayerStats myPlayerStats = sourcePlayer.GetComponent<PlayerStats>();
            NetworkBehaviour targetBehavior;
            if ((targetBehavior = col.GetComponentInParent<NetworkBehaviour>())) {
                // If the target is network bound, and we are tracking our own collision
                // This is an  object that exists and is tracked on all clients
                if (myPlayerStats.isServer) {
                    // Favor the server
                }
                if (myPlayerStats.isLocalPlayer) {
                    // Favor the client
                    hitPlayer = targetBehavior.gameObject;
                    myPlayerStats.CmdApplyDamage(targetBehavior.gameObject, damage, damageType);
                } 
            } else {
                // This is an object that may or may not exist on all clients, so we will handle collision locally
                h.Hit(damage, sourcePlayer, damageType);
            }
            
            
        }

        if (dieOnHit) {
            if (explodeParticles) {
                ParticleSystem.MainModule m = trailParticles.main;
                m.loop = false;
                trailParticles.transform.parent = null;
                GameObject.Instantiate(explodeParticles, transform.position, Quaternion.identity);
               
            } 
            Destroy(this.gameObject);
            
        }
    }
	
	
}
