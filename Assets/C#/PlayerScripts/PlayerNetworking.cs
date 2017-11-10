using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworking : PlayerComponent {
    public Camera[] playerCameras;
    public AudioListener playerListener;

	public override void PlayerComponent_Start () {
        
        if (myBase.isLocalPlayer) {
            // If we have any spectator cameras, get rid of them
            foreach (Camera c in GameObject.FindObjectsOfType<Camera>()) {
                c.enabled = false;
            }
            foreach (AudioListener a in GameObject.FindObjectsOfType<AudioListener>()) {
                a.enabled = false;
            }
            initializeCameras(true);
        } else {
            initializeCameras(false);
        }
    }
    public void initializeCameras(bool shouldI) {
		if (!shouldI) {
			foreach (Camera c in playerCameras) {
				GameObject.Destroy (c.gameObject);
			}
		} else {
			foreach (Camera c in playerCameras) {
				c.enabled = true;
			}
			playerListener.enabled = true;
		}
        
    }
    public override void PlayerComponent_Update() {

    }



}
