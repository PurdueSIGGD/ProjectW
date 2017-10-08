using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInput : PlayerComponent {
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
    }
    /**
     * To be implemented by the different sort of player inputs
     */
    public abstract InputData getData();
	// Update is called once per frame
	void Update () {
        InputData myData = getData();
        myBase.myMovement.processMovement(myData);

        for (int i = 0; i < myBase.myAbilities.Length; i++) {
            if (myData.useAbilities[i]) {
                myBase.myAbilities[i].use();
            }
        }
	}
}
