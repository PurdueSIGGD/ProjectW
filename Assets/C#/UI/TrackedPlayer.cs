using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedPlayer : TrackedItem {
	[HideInInspector]
	public PlayerStats trackedPlayerStats;
	public RectTransform health;

	/**
	 * When extending this class, you can override whatever code you want here to initialize information about this object to track
	 */
	public override void Start_Extended(Transform player) {
		trackedPlayerStats = player.GetComponent<PlayerStats>();
	}

	/**
	 * When extending this class, you can override whatever code you want here to update information about this object to track
	 */
	public override void Update_Extended() {
		health.localScale = new Vector3(trackedPlayerStats.health / trackedPlayerStats.healthMax, 1, 1);
		if (trackedPlayerStats.death) {
			GameObject.Destroy(this.gameObject);
		}
	}
}
