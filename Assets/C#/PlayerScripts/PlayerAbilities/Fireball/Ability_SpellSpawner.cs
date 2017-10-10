﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Ability_SpellSpawner : CooldownAbility {
    /**
     * IF YOU ARE EVER USING THIS 
     * PLEASE ENSURE TO ADD THE PREFAB TO THE LIST IN ProjectWNetworkManager
     */
    public GameObject itemToSpawn;
    public Transform spawnPoint;
    [SyncVar]
    Vector3 spawnAngle;
    [SyncVar]
    Vector3 spawnPosition;
    private Vector3 lastSpawnPosition;
    private Vector3 lastSpawnAngle;
    private Vector3 localAngle;
    private Vector3 localPosition;
    public Transform aimAngle;
    public float spawnSpeed;

    public override void use_UseAbility() {
        if (isLocalPlayer) {
            // We dictate our own angle, everyone else uses our own
            localAngle = aimAngle.forward;
            localPosition = spawnPoint.position;
            CmdSetTrajectory(localAngle, localPosition);
            SpawnSpell();
        }
    }
    [Command]
    public void CmdSetTrajectory(Vector3 angle, Vector3 position) {
        spawnAngle = angle;
        spawnPosition = position;
    }
    public void Death() {
        spawnAngle = Vector3.zero;
        spawnPosition = Vector3.zero;
        lastSpawnAngle = Vector3.zero;
        lastSpawnPosition = Vector3.zero;
    }
    public override void use_CanUse() {
        // nothing
    }

    public override void cooldown_Start() {

    }
    public override void cooldown_Update() {
        // Shoot the ball as soon as we get the latest angle
        if (!isLocalPlayer && lastSpawnAngle != spawnAngle && spawnPosition != lastSpawnPosition && !myBase.myStats.death) {
            SpawnSpell();
            lastSpawnAngle = spawnAngle;
        }
       
    }
    private void SpawnSpell() {
        Debug.DrawRay(spawnPoint.position, spawnAngle, Color.green, 10);
        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, (isLocalPlayer ? localPosition : spawnPosition), Quaternion.identity);
        Projectile p;
        if (p = spawn.GetComponent<Projectile>()) {
            p.sourcePlayer = this.gameObject;
        }
        spawn.GetComponent<Rigidbody>().AddForce((isLocalPlayer ? localAngle : spawnAngle) * spawnSpeed);
    }

    
}
