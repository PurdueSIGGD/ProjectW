using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Ability_ObjectSpawner : CooldownAbility {
    public GameObject itemToSpawn;
    public Transform spawnPoint;
    public Vector3 spawnOffset;
    public Transform aimAngle;
    public float spawnSpeed;

    private static string OBJECT_SPAWN_METHOD_NAME = "ObjectSpawn";



    public void Death() {
        
    }
    public override void use_CanUse() {
        // nothing, maybe an animation
    }

    public override void cooldown_Start() {
        this.ResgisterDelegate(OBJECT_SPAWN_METHOD_NAME, SpawnSpell);
    }
    public override void cooldown_Update() {
        
    }
    private void SpawnSpell(PlayerComponent.Buf data) {
        Vector3 spawnAngle = data.vectorList[0];
        Vector3 spawnPosition = data.vectorList[1];

        Debug.DrawRay(spawnPoint.position + spawnOffset, spawnAngle, Color.green, 10);
        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), Quaternion.identity);
        Rigidbody r;
        if (r = spawn.GetComponent<Rigidbody>())
        {
            r.AddForce(spawnAngle * spawnSpeed);
        }
        OnSpellSpawned(spawn);
    }
    /**
     * Called whenever we spawn a spell, to be implemented by the inheriting class
     */
    public virtual void OnSpellSpawned(GameObject spawn)
    {

    }
    /**
     * Called by the input controller. 
     */
    public override void use_UseAbility() {
        // PlayerAbility puts input for each player around here. 
        // Since we send another message to get things done, we shouldn't bother with anyone who isn't local
        // Because we take care of all the networking
        if (isLocalPlayer || myBase.myInput.isBot()) {
            Vector3 localAngle = aimAngle.forward;
            Vector3 localPosition = spawnPoint.position;
            Buf buf = new Buf();
            buf.methodName = OBJECT_SPAWN_METHOD_NAME;
            buf.vectorList = new Vector3[] { localAngle, localPosition };
            this.NotifyAllClientDelegates(buf);
        }
        
    }
}
