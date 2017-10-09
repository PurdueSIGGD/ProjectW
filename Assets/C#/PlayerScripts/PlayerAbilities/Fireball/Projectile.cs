﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {
    [HideInInspector]
    //[SyncVar]
    public GameObject sourcePlayer;

    public float damage;
    public Hittable.DamageType damageType;
    public bool dieOnHit = true;
    public bool faked; // If we are faked, we won't collide with anything, since the server processes the collision
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
        if (!faked && (col.GetComponentInParent<IHittable>() != null)) {
            
            //print("i am going to hit" + hitPlayer);
            hitPlayer = col.gameObject;
            sourcePlayer.GetComponent<PlayerStats>().CmdApplyDamage(col.gameObject, damage, damageType);
        }

        if (dieOnHit) {
            if (explodeParticles) {
                ParticleSystem.MainModule m = trailParticles.main;
                m.loop = false;
                trailParticles.transform.parent = null;
                GameObject p = GameObject.Instantiate(explodeParticles, transform.position, Quaternion.identity);
                if (isServer) {
                    NetworkServer.Spawn(p);
                }
            } 
            Destroy(this.gameObject);
            
        }
    }
	
	
}
