using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_RockLeap : CooldownAbility {
	public float jHeight;
	public float jForward;
	private bool active = false;
	private bool landing = false;

	public GameObject itemToSpawn;
	public Transform spawnPoint;
	public Vector3 spawnOffset;
	public Transform aimAngle;
	public float spawnSpeed;
	private float delay = .001f;

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
	public virtual void SpawnSpell(PlayerComponent.Buf data) {

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

	public IEnumerator spawnDelay()
	{
		Rigidbody rp = myBase.myRigid;
		if (active && Mathf.Abs(rp.velocity.y) < 2) {
			Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);


			//Vector3 vec = new Vector3 (ray1.direction.x, Mathf.Min(-ray1.direction.magnitude * .5f, ray1.direction.y), ray1.direction.z);

			//vec = 30 * Vector3.Normalize(vec);

			Vector3 vec = -2 * ray1.direction;
			vec.y = -2 * jHeight;

			rp.velocity = vec;
			Debug.Log ("peaked");
			active = false;
			landing = true;
		}
		if (landing && Mathf.Abs(rp.velocity.y) < 2) {
			Debug.Log ("landed");

			rp.velocity = new Vector3 (0, 0, 0);
			sendSpawnData();

			landing = false;
		}

		if (active || landing) {
			yield return new WaitForSeconds (delay);
			StartCoroutine(spawnDelay());
		}

	}


	/**
     * Called whenever we spawn a spell, to be implemented by the inheriting class
     */
	public virtual void OnSpellSpawned(GameObject spawn)
	{
		Explosion e;
		if (e = spawn.GetComponent<Explosion>())
		{
			e.sourcePlayer = this.gameObject;
		}
	}
	/**
     * Called by the input controller. 
     */

	public void sendSpawnData()
	{
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
				if (h.transform.GetComponent<Collider> ().isTrigger) {
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

	public override void use_UseAbility() {
		Rigidbody rp = myBase.myRigid;
		Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 vec = new Vector3 (ray1.direction.x, 0, ray1.direction.z);
		vec = rp.velocity + jForward * Vector3.Normalize(vec);
		vec.y = jHeight;
		rp.velocity = vec;
		active = true;
		landing = false;
		StartCoroutine(spawnDelay());


	}

}
