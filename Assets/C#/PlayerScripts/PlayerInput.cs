using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerInput : PlayerComponent {
    public Transform reticles;
    public Transform cameraSlider;
    public Transform deathTarget;
    public Transform rotator;

    public static float MAX_CAMERA_DISTANCE = -1.63f;
    public static int ABILITY_INPUTS = 3;
	public class InputData {
        // Class to store all input data
        public float mouseX;
        public float mouseY;
        public float horizontal;
        public float vertical;
        public bool jump;
        public bool[] useAbilities;
        public bool melee;
        public bool pause;
    }
    /**
     * To be implemented by the different sort of player inputs
     */
    public abstract InputData getData();
    void Start() {
        if (!isLocalPlayer) {
            reticles.gameObject.SetActive(false);
        }
    }
	public void Update() {
        if (isLocalPlayer) {
            InputData myData = getData();
            if (myData.pause) {
                myBase.myGUI.TogglePause();
            }
            if (myBase.myGUI.isPaused) {
                // Empty inputs
                myBase.myMovement.processMovement(new InputData());
            } else {
                if (myBase.myStats.death) {
                    // take care of camera
                    cameraSlider.LookAt(deathTarget);
                    
                } else {
                    // Have camera move closer if up against a wall
                    // This raycast is returning nothing for some reason
                    /*RaycastHit[] hits = Physics.RaycastAll(new Ray(rotator.position, rotator.forward * -1), MAX_CAMERA_DISTANCE * 100, ~0);
                    Debug.DrawRay(rotator.position, rotator.forward * -1, Color.green, 10);
                    print(hits.Length);
                    if (hits.Length > 0) {
                       cameraSlider.localPosition = new Vector3(0, 0, hits[0].distance);
                    } else {
                        cameraSlider.localPosition = new Vector3(0, 0, MAX_CAMERA_DISTANCE);
                    }*/
                   
                    myBase.myMovement.processMovement(myData);

                    for (int i = 0; i < myBase.myAbilities.Length; i++) {
                        if (myData.useAbilities[i]) {
                            //print(myBase.myAbilities[i] is Ability_SpeedBoost);
                            myBase.myAbilities[i].CmdUse(myBase.myAbilities[i].GetType().ToString());
                        }
                    }
                }
            }
           
        }
        
	}
    void Death() {
        reticles.gameObject.SetActive(false);
    }

    
}
