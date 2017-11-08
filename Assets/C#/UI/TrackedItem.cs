using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackedItem : MonoBehaviour {
	public Camera myCamera;
	public Transform locationToTrack;
	public Vector3 hudOffset, worldOffset;
	public Text itemName;
	private bool hasStarted = false;

	public void StartTracker (Transform target, Camera myCamera) {
		this.myCamera = myCamera;
		locationToTrack = target;
		hasStarted = true;
		Start_Extended(target);
	}

	void Update () {
		if (locationToTrack == null) {
			if (hasStarted)
				GameObject.Destroy(this.gameObject);
		} else {
			if (myCamera != null) {
				Vector3 targetPosition = myCamera.WorldToScreenPoint(locationToTrack.position + worldOffset) + hudOffset;
				// If it is being annoying and updating in negative space, hide it
				if (targetPosition.z > 0) {
					transform.position = targetPosition;
				} else {
					transform.position = new Vector3(-10000f, targetPosition.y, targetPosition.z);
                }
                itemName.text = locationToTrack.gameObject.name;
                Update_Extended();
			}
		}
	}

	/**
	 * When extending this class, you can override whatever code you want here to initialize information about this object to track
	 */
	public virtual void Start_Extended(Transform target) {

	}

	/**
	 * When extending this class, you can override whatever code you want here to update information about this object to track
	 */
	public virtual void Update_Extended() {
		
	}
}
