﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_RecursiveSpawner : Ability_ObjectSpawner {// spawns a line of objects in front of the caster
	public int numSpawns;
	public float spawnRange;
	public float delay;
	protected float offset;
	protected int check;
	protected int layer;

	public override void SpawnSpell(PlayerComponent.Buf data)
	{
		Vector3 spawnAngle = data.vectorList[0];
		Vector3 spawnPosition = data.vectorList[1];
		Ray ray1 = new Ray (spawnPosition, spawnAngle);
		ray1 = new Ray(ray1.origin, new Vector3(ray1.direction.x, 0, ray1.direction.z));
		Ray ray2 = new Ray(ray1.origin, new Vector3(0, -1, 0));
		check = 0;
		layer = 1 << 11;
		SpawnSpell (data, 0, ray1, ray2);
	}

	public void SpawnSpell(PlayerComponent.Buf data, int it, Ray ray1, Ray ray2)
	{
		

		offset = spawnRange/numSpawns;

		GameObject spawn;
		spawn = castOne (data, ref ray1, ref ray2, ref it);

		Rigidbody r;
		r = spawn.GetComponent<Rigidbody> ();
		/*if (r = spawn.GetComponent<Rigidbody>())
		{
			r.AddForce(spawnAngle * spawnSpeed);
		}*/
		OnSpellSpawned(spawn);
		StartCoroutine(spawnDelay(data, it, ray1, ray2));

	}

	public GameObject castOne(PlayerComponent.Buf data, ref Ray ray1, ref Ray ray2, ref int it)
	{
		GameObject spawn;
		RaycastHit hit;
		if (Physics.Raycast (ray1, out hit, offset, layer)) {

			Ray temp = ray1;
			ray1 = new Ray (ray1.origin, -ray2.direction);
			ray2 = temp;
			spawn = castOne (data, ref ray1, ref ray2, ref it);
			

		} 
		else {
			spawn = castTwo (data, ref ray1, ref ray2, ref it);
		}
		return spawn;
	}

	public GameObject castTwo(PlayerComponent.Buf data, ref Ray ray1, ref Ray ray2, ref int it)
	{
		GameObject spawn;
		RaycastHit hit;
	

		ray2 = new Ray (ray1.origin + ray1.direction * offset, ray2.direction);
		if (Physics.Raycast (ray2, out hit, offset, layer)) {
			spawn = GameObject.Instantiate (itemToSpawn, hit.point, Quaternion.LookRotation(ray1.direction));
			check = 0;
		} 
		else {
			
			if (check < 4) {
				check++;
				Ray temp = ray1;
				ray1 = new Ray (ray1.origin + ray1.direction * offset, ray2.direction);
				ray2 = new Ray (ray1.origin, -temp.direction);
				spawn = castOne (data, ref ray1, ref ray2, ref it);
				check = 0;
			} else {
				if (Physics.Raycast (ray2, out hit, 2 * spawnRange, layer)) {
					spawn = GameObject.Instantiate (itemToSpawn, hit.point, Quaternion.identity);
					ray1 = new Ray (ray1.origin + ray2.direction * (hit.distance - offset * .5f), ray1.direction);
				} else {
					spawn = GameObject.Instantiate (itemToSpawn, ray2.origin + ray2.direction * 2 * spawnRange, Quaternion.identity);
					it = numSpawns;
				}
			}

		}


		return spawn;
	}
		

	public IEnumerator spawnDelay(PlayerComponent.Buf data, int it, Ray ray1, Ray ray2)
	{
		yield return new WaitForSeconds (delay);
		float offset = spawnRange/numSpawns;

		ray1 = new Ray (ray1.origin + ray1.direction * offset, ray1.direction);

		it++;
		if (it < numSpawns) {
			SpawnSpell (data, it, ray1, ray2);
		}

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
