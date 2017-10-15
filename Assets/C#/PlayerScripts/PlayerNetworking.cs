using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetworking : PlayerComponent {
    public Camera[] playerCameras;
    public AudioListener playerListener;

	void Start () {
        
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
    void initializeCameras(bool shouldI) {
        foreach (Camera c in playerCameras) {
            c.enabled = shouldI;
        }
        playerListener.enabled = shouldI;
    }
    public void Update() {

    }



}
