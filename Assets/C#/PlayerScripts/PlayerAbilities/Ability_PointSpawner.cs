using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_PointSpawner : CooldownAbility {// Spawns an object where the camera is pointing
	public float spawnRange;

	public GameObject itemToSpawn;

	public Vector3 spawnOffset;

	public float spawnSpeed;

	private static string OBJECT_SPAWN_METHOD_NAME = "ObjectSpawn";
	public bool groundSpawn;


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
	public virtual void SpawnSpell(PlayerComponent.Buf data) {
		
		Vector3 spawnPosition = data.vectorList[0];


		// Spawn our spell in the place the server told us
		// However if we are the client, we don't wait for that luxury.
		GameObject spawn = GameObject.Instantiate(itemToSpawn, spawnPosition + transform.TransformDirection(spawnOffset), transform.rotation);

		OnSpellSpawned(spawn);
	}
	/**
     * Called whenever we spawn a spell, to be implemented by the inheriting class
     */

	/**
     * Called by the input controller. 
     */
	public override void use_UseAbility() {
		// PlayerAbility puts input for each player around here. 
		// Since we send another message to get things done, we shouldn't bother with anyone who isn't local
		// Because we take care of all the networking
		if (isLocalPlayer || myBase.myInput.isBot()) {
			// Find the first object colliding in front of us, aim at that if necessary

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			Vector3? localPosition;
			Buf buf;

			RaycastHit[] hits = Physics.SphereCastAll (ray.origin, .5f, ray.direction, spawnRange);
			foreach (RaycastHit h in hits) {
				PlayerStats tmpSts;
				if (tmpSts = h.transform.GetComponentInParent<PlayerStats> ()) {
					//if (tmpSts.gameObject == this.gameObject)
						continue;
				}
				if (h.transform.GetComponent<Collider> ().isTrigger) {
					continue;
				}
				//print ("overriding with object: " + h.transform);
				localPosition = collided(h);
				if (localPosition.HasValue) {
					buf = new Buf ();
					buf.methodName = OBJECT_SPAWN_METHOD_NAME;
					buf.vectorList = new Vector3[] { (Vector3)localPosition };
					this.NotifyAllClientDelegates (buf);
					return;
				}
			}


		
			localPosition = maxRangeReached(ray, spawnRange);
			if (localPosition.HasValue) {
				buf = new Buf ();
				buf.methodName = OBJECT_SPAWN_METHOD_NAME;
				buf.vectorList = new Vector3[] { (Vector3)localPosition };
				this.NotifyAllClientDelegates (buf);
			}
		}

	}
		

	/*
	public void SpawnSpell(PlayerComponent.Buf data, float rangeVaried, Ray ray) {
		GameObject spawn;
		RaycastHit hit;
		bool validHit = false;
		Physics.Raycast (ray, out hit, 1);

		RaycastHit[] hits = Physics.RaycastAll (ray.origin, ray.direction, spawnRange);
		foreach (RaycastHit h in hits) {
			PlayerStats tmpSts;
			if (tmpSts = h.transform.GetComponentInParent<PlayerStats> ()) {
				if (tmpSts.gameObject == this.gameObject)
					continue;
			}
			if (h.transform.GetComponent<Collider> ().isTrigger) {
				continue;
			}
			//print ("overriding with object: " + h.transform);
			hit = h;
			validHit = true;
			break;
		}

		if (validHit) {
			spawn = collided (hit);
		}
		else
		{
			spawn = maxRangeReached(ray, spawnRange);
		}

		OnSpellSpawned (spawn);
		/*
		RaycastHit hit;
		GameObject spawn;

		for (int i = 0; i < 100; i++) {
			if (Physics.Raycast (ray, out hit, rangeVaried)) {


				PlayerStats tmpSts;
				if (tmpSts = hit.transform.GetComponentInParent<PlayerStats> ()) {
					if (tmpSts.gameObject == this.gameObject)
						ray = new Ray (ray.origin + ray.direction * .1f, ray.direction);
						continue;
				}
				spawn = collided (hit);



			} else {
				spawn = maxRangeReached (ray, rangeVaried);

			}
			OnSpellSpawned(spawn);
			break;
		}


	} */


	public virtual Vector3? collided(RaycastHit hit)
	{
		if (groundSpawn) {
			Vector3 spot = hit.point;
			if (!Physics.SphereCast (new Ray (spot + Vector3.up * .6f, Vector3.down), .5f, out hit, spawnRange, 1 << 11)) {
				return null;
			}
		}

		return hit.point;
	}

	public virtual Vector3? maxRangeReached(Ray ray, float rangeVaried)
	{

        RaycastHit[] hits = Physics.SphereCastAll(ray.origin + ray.direction * rangeVaried, .5f, -Vector3.up);
        foreach (RaycastHit h in hits)
        {
            PlayerStats tmpSts;
            if (tmpSts = h.transform.GetComponentInParent<PlayerStats>())
            {
                //if (tmpSts.gameObject == this.gameObject)
                continue;
            }
            else
            {
                return h.point;
            }
        }
        return null;
	}

	public virtual void OnSpellSpawned(GameObject spawn)
	{
		Explosion e;
		if (e = spawn.GetComponent<Explosion>())
		{
			e.sourcePlayer = this.gameObject;
		}
	}

}
