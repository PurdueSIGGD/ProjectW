using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerInput : PlayerComponent {
    public Transform cameraSlider;
    public Transform deathTarget; // the target we  will look at when we die
    [HideInInspector]
    public Transform deathTargetMe; // our own hips
    public Transform rotator;
    private Animator scoreboard;
    public bool disabled;
    private bool lastDeath = false;

    [SyncVar]
    public int bot = -1; // Default -1 for player
    [SyncVar]
    public int playerId;

    public static float MAX_CAMERA_DISTANCE = -1.63f;
    public static int ABILITY_INPUTS = 4;
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
        public bool scoreboard;
    }
    /**
     * To be implemented by the different sort of player inputs
     */
    public abstract InputData getData();
    public override void PlayerComponent_Start() {
        scoreboard = GameObject.FindObjectOfType<Scoreboard>().GetComponent<Animator>();
        deathTargetMe = deathTarget;
    }
    public override void PlayerComponent_Update() {
    }
    public void FixedUpdate()
    {
        if (isLocalPlayer) {
            InputData myData = getData();
            if (myData.pause && !isBot() && !disabled) {
                myBase.myGUI.TogglePause();
            }
            scoreboard.SetBool("Showing", myData.scoreboard);
            if (myBase.myStats.death)
            {
                if (!lastDeath) cameraSlider.LookAt(deathTarget);
                lastDeath = true;
                // take care of camera
                Quaternion targetRotation = Quaternion.LookRotation(deathTarget.transform.position - cameraSlider.position);

                // Smoothly rotate towards the target point.
                cameraSlider.rotation = Quaternion.Slerp(cameraSlider.rotation, targetRotation, 5 * Time.deltaTime);
            } else {
                lastDeath = false;

            }
            if (myBase.myGUI.isPaused) {
                // Empty inputs
                myBase.myMovement.processMovement(new InputData());
            } else {
                if (!myBase.myStats.death && !disabled) {
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

                    // if stunned, don't allow play to use abilities
                    if (this.myBase.myEffects.stunned)
                        return;

                    for (int i = 0; i < myBase.myAbilities.Length; i++) {
						if (myData.useAbilities [i]) {
							//print(myBase.myAbilities[i] is Ability_SpeedBoost);
							myBase.myAbilities [i].ClientsUse ();
							if (myBase.myAbilities[i].abilityIcon != null && myBase.myAbilities[i].abilityIcon.myAnimator != null) myBase.myAbilities [i].abilityIcon.myAnimator.SetBool ("Key", true);
						} else {
							if (myBase.myAbilities[i].abilityIcon != null && myBase.myAbilities[i].abilityIcon.myAnimator != null) myBase.myAbilities [i].abilityIcon.myAnimator.SetBool ("Key", false);
						}
                    }

                }
            }
           
        }
        
	}

    public void GameOver()
    {
        this.disabled = true;
        this.scoreboard.SetBool("Showing", true);
    }
    public void Reset_GameOver()
    {

        this.disabled = false;
    }

    void setBot(int botId) {
        //print(botId);
        bot = botId;
        if (isBot())
        {
            playerId = -1 * bot;
        }
        
    }
    public void setPlayerId(int newPlayerId)
    {
        playerId = newPlayerId;
    }
    /**
     * Returns true if and only if called on the server, and this player is a bot with no player attached to it
     * isServer and isClient both return the same if there is nothing attached to it. Weird, huh. 
     * If a game manager instantiates the player prefab, it says it is neither the server or the client. 
     * But if it is loaded with the scene, it says it is both the server and the client.
     */
    public bool isBot() {
        return bot > 0;
    }
    public int getBot()
    {
        return bot;
    }
    void Death() {
		
    }
    public int GetPlayerId()
    {
        return playerId;
    }
   
    
}
