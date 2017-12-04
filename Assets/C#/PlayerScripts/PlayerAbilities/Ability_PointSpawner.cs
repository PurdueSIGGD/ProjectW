using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_PointSpawner : Ability_ObjectSpawner {
	public float spawnRange;

	public override void SpawnSpell(PlayerComponent.Buf data)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		SpawnSpell(data, spawnRange, ray);
	}


	public void SpawnSpell(PlayerComponent.Buf data, float rangeVaried, Ray ray) {
		Vector3 spawnAngle = data.vectorList[0];
		Vector3 spawnPosition = data.vectorList[1];
		Debug.Log("varied " + rangeVaried);

		RaycastHit hit;
		GameObject spawn;

		if (Physics.Raycast (ray, out hit, rangeVaried)) {

			//	Debug.DrawRay(spawnPoint.position + spawnOffset, spawnAngle, Color.green, 10);
			// Spawn our spell in the place the server told us
			// However if we are the client, we don't wait for that luxury.
			spawn = collided(hit);



		} 
		else {
			spawn = maxRangeReached (ray, rangeVaried);
		}

		Rigidbody r;
		if (r = spawn.GetComponent<Rigidbody>())
		{
			r.AddForce(spawnAngle * spawnSpeed);
		}
		OnSpellSpawned(spawn);
	}


	public virtual GameObject collided(RaycastHit hit)
	{
		GameObject spawn;

		spawn = GameObject.Instantiate(itemToSpawn, hit.point, Quaternion.identity);

		return spawn;
	}

	public virtual GameObject maxRangeReached(Ray ray, float rangeVaried)
	{
		GameObject spawn;
		//spawn = GameObject.Instantiate (itemToSpawn, ray.origin + ray.direction * spawnRange, Quaternion.identity);


		Ray drop = new Ray(ray.origin + ray.direction * rangeVaried, -Vector3.up);
		RaycastHit hit;

		Physics.Raycast (drop, out hit, rangeVaried * 3);

		spawn = GameObject.Instantiate (itemToSpawn, hit.point, Quaternion.identity);


		return spawn;
	}

	public override void OnSpellSpawned(GameObject spawn)
	{
		Explosion e;
		if (e = spawn.GetComponent<Explosion>())
		{
			e.sourcePlayer = this.gameObject;
		}
	}

}
