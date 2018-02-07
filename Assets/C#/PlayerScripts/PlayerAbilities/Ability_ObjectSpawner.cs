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
        ObjectSpawner_Start();
    }
    public virtual void ObjectSpawner_Start()
    {

    }
    public override void cooldown_Update() {
        
    }
    private void SpawnSpell(PlayerComponent.Buf data) {
        Vector3 spawnAngle = data.vectorList[0];
        Vector3 spawnPosition = data.vectorList[1];

        
        // Spawn our spell in the place the server told us
        // However if we are the client, we don't wait for that luxury.
        GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);
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
			// Find the first object colliding in front of us, aim at that if necessary
            Vector3 localAngle = aimAngle.forward; // aim forward by default
			RaycastHit[] hits = Physics.RaycastAll (aimAngle.position, aimAngle.forward * 100);
			Debug.DrawRay(aimAngle.position, aimAngle.forward * 100, Color.green, 10);
			foreach (RaycastHit h in hits) {
				PlayerStats tmpSts;
				if (tmpSts = h.transform.GetComponentInParent<PlayerStats> ()) {
					if (tmpSts.gameObject == this.gameObject)
						continue;
				}
				if (h.collider.isTrigger) {
					continue;
				}
				//print ("overriding with object: " + h.transform);
				localAngle = Vector3.Normalize(h.point - spawnPoint.position);
				break;
			}
			Debug.DrawRay(spawnPoint.position, localAngle * 100, Color.red, 10);
            Vector3 localPosition = spawnPoint.position;
            Buf buf = new Buf();
            buf.methodName = OBJECT_SPAWN_METHOD_NAME;
            buf.vectorList = new Vector3[] { localAngle, localPosition };
            this.NotifyAllClientDelegates(buf);
        }
        
    }
}
