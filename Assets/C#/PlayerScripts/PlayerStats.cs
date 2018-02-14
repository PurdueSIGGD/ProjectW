﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : PlayerComponent, IHittable {
    public GameObject healthBar;
    public GameObject magicBar;
    [SyncVar]
    public float health = 100;
    public float healthMax = 100;
    public float magic = 100;
    public float magicMax = 100;
    [HideInInspector]
    [SyncVar]
    public bool death;
    private bool hasDeath;
    [SyncVar]
    public int teamIndex = -1;
    [SyncVar]
    public Color teamColor = Color.red;
    [SyncVar]
    public string playerName;
    [SyncVar]
    public int classIndex = -1;
    public GameObject[] deathSounds;
	public AudioSource hitSound;
	[HideInInspector]
    public Animator hitAnimator; // Assigned in playerGUI
    [HideInInspector]
    public int lastHitPlayerId;
    private float lastHitTime;

    public override void PlayerComponent_Start() {
        
        if (!isLocalPlayer) {
            magicBar.SetActive(false);
            healthBar.SetActive(false);
        } else {
            // once we have a GUI
            healthBar.SetActive(false);
            magicBar.SetActive(false);
        }
    }
    
    public void Hit(HitArguments hit) {
        if (hit.sourcePlayerTeam != teamIndex || teamIndex == -1)
        {
            lastHitPlayerId = hit.sourcePlayer.GetComponent<PlayerInput>().GetPlayerId();
            lastHitTime = Time.time;
        }
        changeHealth(-1 * hit.damage);
        if (hit.effect != PlayerEffects.Effects.none) {
            myBase.myEffects.AddEffect(hit);
        }
    }
    public float changeHealth(float f) {
		if (!isServer && !isLocalPlayer)
			return 0;
		if (float.IsNaN (health)) {
			health = 0;
			// Something causes health to be NaN. no idea what it comes from.
			// I see no instant issues whenever health is NaN, so this should fix some of those issues.
		}
        float returnVal = 0;
        health += f;
        if (health > healthMax) {
            returnVal = health - healthMax;
            health = healthMax;
        } else if (health < 0) {
            returnVal = health;
            health = 0;
        }
        return returnVal;
    }
    public bool canUseMagic(float amt) {
        return amt <= magic;
    }
    public float changeMagic(float f) {
        float returnVal = 0;
        magic += f;
        if (magic > magicMax) {
            returnVal = magic - magicMax;
            magic = magicMax;
        } else if (magic < 0) {
            returnVal = magic;
            magic = 0;
        }
        return returnVal;
    }
    public override void PlayerComponent_Update() {
        //healthBar.transform.localScale = new Vector3(health, health>0?1:0, health > 0 ? 1 : 0);
        //magicBar.transform.localScale = new Vector3(magic, magic > 0 ? 1 : 0, magic > 0 ? 1 : 0);
        changeMagic(Time.deltaTime * 30 * myBase.myEffects.magicRegenModifier); // Update magic at our regen rate
        //print(this.gameObject.name + " " + this.playerControllerId + " " + isServer + " " + isClient);
        // Spawned enemies can 
        if (health == 0 && !hasDeath) {
			if (isLocalPlayer) {
				// Player has to handle their own death
				CmdDeath ();
				this.BroadcastMessage ("Death");
				hasDeath = true;
			} else if (isServer && myBase.myInput.isBot ()) {
				// Specific message for non-cmd things
				ServerDeath ();
				this.BroadcastMessage ("Death");
				hasDeath = true;
			} else {
				// Only the server/client should be handling their death
			}
        }

        if (Time.time - lastHitTime > 5 && lastHitPlayerId != 0)
        {
            // Reset last hit
            lastHitPlayerId = 0;
        }
        
        if (death && !hasDeath) {
            // Other players have to react to the death
            this.BroadcastMessage("Death");
            hasDeath = true;
        }


    }
    /**
     * You can only tell the server to do a command inside of the root player control, so this will allow the server to do damage
     */
    [Command]
    public void CmdApplyDamage(HitManager.HitVerificationMethod ver, HitArguments hit) {
        //print("applying damage");
        PlayerStats targetStats;
        if (hit.sourcePlayerTeam != -1 && (targetStats = hit.target.GetComponentInParent<PlayerStats>()))
        {
            if (targetStats.teamIndex == this.teamIndex && !hit.hitSameTeam)
            {
				Debug.LogWarning("Same team, not registering hit on target " + hit.target);
                return;
            }
        }
		if (HitManager.VerifyHit(ver, hit) && hit.target != null && hit.target.GetComponentInParent<IHittable>() != null)
        {
            hit.target.GetComponentInParent<IHittable>().Hit(hit);
            if (hit.target.GetComponentInParent<PlayerStats>() && hit.target != this.gameObject) {
                RpcConfirmHit(hit.damage);
            }
        }
    }
    [ClientRpc]
    public void RpcConfirmHit(float damage) {
		if (isLocalPlayer && !myBase.myInput.isBot()) {
            hitSound.Play();
            hitAnimator.SetTrigger("Hit");
        }
    }
    [Command]
    public void CmdDeath() {
        //print("adding death");
        GameObject.FindObjectOfType<ProjectWGameManager>().AddDeath(this.gameObject, Network.player.ToString());
        death = true;
    }
    // Called to kill a bot, only by a server
    public void ServerDeath() {
        GameObject.FindObjectOfType<ProjectWGameManager>().AddDeath(this.gameObject, Network.player.ToString());
        death = true;
    }
    public void Death() {
        // Take care of death on each client's end

        healthBar.SetActive(false);
        magicBar.SetActive(false);

        foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>()) {
            r.isKinematic = false;
        }
        myBase.myRigid.isKinematic = true;
        myBase.myAnimator.enabled = false;
        myBase.myCollider.enabled = false;
        myBase.myNoFrictionCollider.enabled = false;
        // Move all parts to the ragdoll layer
        // So it interacts with the world, but not itself
        MoveToLayer(myBase.myAnimator.transform, 10);
        // Add death sound
        GameObject result = GameObject.Instantiate(deathSounds[UnityEngine.Random.Range(0, deathSounds.Length - 1)], transform);
        result.transform.localPosition = Vector3.zero; // Center on parent
    }

    public void MoveToLayer(Transform root, int layer) {
        // Recursively move all children and self to layer
        root.gameObject.layer = layer;
        foreach (Transform child in root) {
            MoveToLayer(child, layer);
        }
    }
	public void despawnCorpse() {
		RpcDespawnCorpse ();
	}
	[ClientRpc]
	public void RpcDespawnCorpse() {
		this.gameObject.SetActive (false);
	}
  


}
