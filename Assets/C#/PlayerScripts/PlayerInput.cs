using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerInput : PlayerComponent {
    public Transform cameraSlider;
    public Transform deathTarget;
    public Transform rotator;

    private bool bot; // Default false

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
    public override void PlayerComponent_Start() {
    }
	public override void PlayerComponent_Update() {
        if (isLocalPlayer) {
            InputData myData = getData();
            if (myData.pause && !isBot()) {
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
                            myBase.myAbilities[i].ClientsUse();
                        }
                    }
                }
            }
           
        }
        
	}
    void setBot() {
        bot = true;
    }
    /**
     * Returns true if and only if called on the server, and this player is a bot with no player attached to it
     * isServer and isClient both return the same if there is nothing attached to it. Weird, huh. 
     * If a game manager instantiates the player prefab, it says it is neither the server or the client. 
     * But if it is loaded with the scene, it says it is both the server and the client.
     */
    public bool isBot() {
        return bot;
    }
    void Death() {
		
    }

    
}
