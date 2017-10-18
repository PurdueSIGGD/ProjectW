using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Ability_ObjectSpawner : CooldownAbility {
    public GameObject itemToSpawn;
    public Transform spawnPoint;
    public Vector3 spawnOffset;
    [SyncVar]
    Vector3 spawnAngle;
    [SyncVar]
    Vector3 spawnPosition;
    [SyncVar]
    int spawnCount;
    private Ability_ObjectSpawner[] myObjectSpawners;
    private int myIndex;
    private Vector3 lastSpawnPosition;
    private Vector3 lastSpawnAngle;
    private int lastSpawnCount;
    private Vector3 localAngle;
    private Vector3 localPosition;
    private int localSpawnCount;
    public Transform aimAngle;
    public float spawnSpeed;

    public override void use_UseAbility() {
        if (isLocalPlayer) {
            // We dictate our own angle instantly, everyone else uses our own
            localAngle = aimAngle.forward;
            localPosition = spawnPoint.position;
            localSpawnCount = spawnCount + 1;
            CmdSetTrajectory(myIndex, localAngle, localPosition, localSpawnCount);
            SpawnSpell();
        }
    }
    [Command]
    public void CmdSetTrajectory(int spawnerIndex, Vector3 angle, Vector3 position, int count) {
        myObjectSpawners[spawnerIndex].SetTrajectory(angle, position, count);
    }
    /**
     * We have to tell a specific object spawner to spawn, otherwise it just assumes the first
     * So if we have multiple object spawners, we have to account for that
     */
    public void SetTrajectory(Vector3 angle, Vector3 position, int count)
    {
        spawnAngle = angle;
        spawnPosition = position;
        spawnCount = count;
    }
    public void Death() {
        spawnAngle = Vector3.zero;
        spawnPosition = Vector3.zero;
        lastSpawnAngle = Vector3.zero;
        lastSpawnPosition = Vector3.zero;
        spawnCount = 0;
    }
    public override void use_CanUse() {
        // nothing
    }

    public override void cooldown_Start() {
        myObjectSpawners = GetComponents<Ability_ObjectSpawner>();
        for (int i = 0; i < myObjectSpawners.Length; i++)
        {
            if (myObjectSpawners[i] == this)
            {
                myIndex = i;
            }
        }
    }
    public override void cooldown_Update() {
        // Spawn the object as soon as we get a new angle, but not if we are the player
        if (!isLocalPlayer && 
            (lastSpawnAngle != spawnAngle ||  spawnPosition != lastSpawnPosition || spawnCount != lastSpawnCount) && 
            !myBase.myStats.death) {
            SpawnSpell();
            lastSpawnAngle = spawnAngle;
            lastSpawnCount = spawnCount;
            lastSpawnPosition = spawnPosition;
        }
       
    }
    private void SpawnSpell() {
        Debug.DrawRay(spawnPoint.position + spawnOffset, spawnAngle, Color.green, 10);
        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, (isLocalPlayer ? localPosition : spawnPosition) + transform.TransformDirection(spawnOffset), Quaternion.identity);
        Rigidbody r;
        if (r = spawn.GetComponent<Rigidbody>())
        {
            r.AddForce((isLocalPlayer ? localAngle : spawnAngle) * spawnSpeed);
        }
        OnSpellSpawned(spawn);
    }
    /**
     * Called whenever we spawn a spell, to be implemented by the inheriting class
     */
    public virtual void OnSpellSpawned(GameObject spawn)
    {

    }

    
}
