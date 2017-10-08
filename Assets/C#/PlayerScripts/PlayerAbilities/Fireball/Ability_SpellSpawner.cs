using System;
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
    public Transform aimAngle;
    public float spawnSpeed;
    
    public override void use_CooledDown() {
        //SpawnProjectile(false);
        Vector3 spawnAngle = aimAngle.forward;
        CmdSpawnProjcetile(spawnAngle);
    }
    [Command]
    public void CmdSpawnProjcetile(Vector3 angle) {
        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPoint.position, Quaternion.identity);
        NetworkServer.Spawn(spawn);
        Projectile p;
        if (p = spawn.GetComponent<Projectile>()) {
            p.sourcePlayer = this.gameObject;
        }
        spawn.GetComponent<Rigidbody>().AddForce(angle * spawnSpeed);
    }
}
