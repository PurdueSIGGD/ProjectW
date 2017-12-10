using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackedItem : MonoBehaviour {
	public Camera myCamera;
	public Transform locationToTrack;
	public Vector3 hudOffset, worldOffset;
	public Text itemName;
    public Image sprite;
	private bool hasStarted = false;
    public bool seeThrough = true;
    public Animator myAnim;

	public void StartTracker (Transform target, Camera myCamera) {
		this.myCamera = myCamera;
		locationToTrack = target;
		hasStarted = true;
        sprite.sprite = target.GetComponent<ItemToTrack>().sprite;
        myAnim = this.GetComponent<Animator>();
        myAnim.SetBool("Showing", true);
		Start_Extended(target);
	}

	void LateUpdate () {
		if (locationToTrack == null) {
			if (hasStarted)
				GameObject.Destroy(this.gameObject);
		} else {
			if (myCamera != null) {
                RaycastHit[] hits = Physics.RaycastAll(new Ray(myCamera.transform.position, (locationToTrack.position + Vector3.up) - myCamera.transform.position), Vector3.Distance((locationToTrack.position + Vector3.up),  myCamera.transform.position));
                //Debug.DrawRay(myCamera.transform.position, (locationToTrack.position + Vector3.up) - myCamera.transform.position);
                bool hit = false;
                foreach (RaycastHit h in hits)
                {
                    if (!h.transform.GetComponentInParent<PlayerStats>())
                    {
                        myAnim.SetBool("Showing", false);
                        hit = true;
                    }
                }
                if (!hit)
                {
                    myAnim.SetBool("Showing", true);
                }

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
