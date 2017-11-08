using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedItemController : MonoBehaviour {
	public Camera myCamera;
	private GameObject player;

	private ArrayList trackedItems;
	public RectTransform trackedPlayerParent;
	public RectTransform trackedObjectParent;

	public GameObject trackedPlayerPrefab;
	public GameObject trackedObjectPrefab;

	// Use this for initialization
	void Start () {
		trackedItems = new ArrayList();
	}

	// Update is called once per frame
	void Update () {
		UpdateTrackedItems(); // This could probably be done every second or so, slow down amount of calls	
	}
    public void UpdateCamera(Camera newCamera)
    {
        myCamera = newCamera;
        ItemToTrack target;
        if (target = myCamera.GetComponentInParent<ItemToTrack>())
        {
            player = target.gameObject;
        }
        else
        {
            player = myCamera.gameObject;
        }
        foreach (TrackedItem it in trackedPlayerParent.GetComponentsInChildren<TrackedItem>())
        {
            it.myCamera = myCamera;
            PlayerStats stats;
            if ((stats = it.locationToTrack.GetComponentInParent<PlayerStats>()) && stats.gameObject == player)
            {
                Destroy(it.gameObject);
            }
        }
    }
	void UpdateTrackedItems() {
		if (myCamera == null) return; // Can't do anything without a camera
		if (player == null) {
			ItemToTrack target;
			if (target = myCamera.GetComponentInParent<ItemToTrack>()) {
				player = target.gameObject;
			} else {
				player = myCamera.gameObject;
			}
		}
		foreach (ItemToTrack item in GameObject.FindObjectsOfType<ItemToTrack>()) {
			if (trackedItems.Contains(item) || item.gameObject == player) {
				// Ignore
			} else {
				// Add to list
				trackedItems.Add(item);
				switch (item.trackType) {
				case ItemToTrack.ItemTrackType.Generic:
					GameObject instancedObjectTracker = GameObject.Instantiate(trackedPlayerPrefab, trackedPlayerParent);
					TrackedItem trackedObject = instancedObjectTracker.GetComponent<TrackedItem>();
					trackedObject.StartTracker(item.transform, myCamera);
					break;
				case ItemToTrack.ItemTrackType.Player:
					GameObject instancedPlayerTracker = GameObject.Instantiate(trackedPlayerPrefab, trackedPlayerParent);
					TrackedPlayer trackedPlayer = instancedPlayerTracker.GetComponent<TrackedPlayer>();
					trackedPlayer.StartTracker(item.transform, myCamera);
					break;
				default:
					break;
				}
			}

		}
	}
}
