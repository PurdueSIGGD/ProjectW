using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_LineSpawner : Ability_PointSpawner {

	public int numSpawns;
	public float delay;

	public override void SpawnSpell(PlayerComponent.Buf data)
	{
		float incr = spawnRange / numSpawns;

		StartCoroutine(spawnDelay(data, incr));



	}

	public IEnumerator spawnDelay(PlayerComponent.Buf data, float incr)
	{
		//GetComponent(Ability_LineSpawner).spawnRange = i * incr;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		ray = new Ray(ray.origin, new Vector3(ray.direction.x, 0, ray.direction.z));
		for (int i = 1; i <= numSpawns; i++)
		{
			SpawnSpell(data, i * incr, ray);
			yield return new WaitForSeconds(delay);
		}
	}
}
