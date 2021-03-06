﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_BambooSpawner : Ability_PointSpawner {
    
    public float bambooShootsSpawned = 5;
    public float damage = 20;
    public float attackDuration = .1f;
    public float spreadMultiplier;
    public bool hitSameTeam = false;

    private float lastUsage = -100; // Last time we used it, in seconds;

    public override void OnSpellSpawned(GameObject spawn)
    {
        Bamboo b;
        if(b = spawn.GetComponent<Bamboo>())
        {
            b.StartBamboo(this, this.attackDuration, this.GetComponentInParent<PlayerStats>().gameObject, hitSameTeam);
        }
    }

    public override void SpawnSpell(PlayerComponent.Buf data)
    {
        Vector3 spawnPosition = data.vectorList[0];
        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        for (int i = 0; i < bambooShootsSpawned; i++)
        {
            Vector3 t = new Vector3(0,0,0);
            t.x += (float)((Random.Range(0.0f, 1.0f) - .5) * spreadMultiplier);
            t.z += (float)((Random.Range(0.0f, 1.0f) - .5) * spreadMultiplier);
            if(t.magnitude > spreadMultiplier)
            {
                float mag = t.magnitude;
                t.x /= mag;
                t.z /= mag;
            }
            GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset) + t, transform.rotation);
            OnSpellSpawned(spawn);
        }
    }

    public override void use()
    {
        // Clients should not worry about magic draw
        if (Time.time - lastUsage > cooldown && !myBase.myMovement.isInAir())
        {
            if (!isLocalPlayer || myBase.myStats.canUseMagic(magicDraw))
            {
                if (abilityIcon != null && cooldown > 0.2f)
                { // We don't let it go if it takes like no time
                    this.abilityIcon.myAnimator.SetFloat("CooldownSpeed", 1 / cooldown);
                    string extra = UnityEngine.Random.Range(0.0f, 1.0f) < 0.001 ? "1" : ""; // ;)
                    this.abilityIcon.myAnimator.SetTrigger("Cooldown" + extra);
                }
                switch (animationTriggerType) {
                    case AnimationType.Trigger:
                        this.myBase.myAnimator.SetTrigger(CAST_STRING);
                        break;
                    case AnimationType.Bool:
                        this.myBase.myAnimator.SetBool(CAST_STRING, true);
                        break;
                    default:
                        break;
                }
                myBase.myStats.changeMagic(-1 * magicDraw);
                lastUsage = Time.time;
                use_UseAbility();
            }
            else
            {
                // Not enough magic
            }

        }
    }
}
