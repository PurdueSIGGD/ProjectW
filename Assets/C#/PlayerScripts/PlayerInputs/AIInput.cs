using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInput : PlayerInput {
	public float sensitivityy = 0.05f;
	public override PlayerInput.InputData getData() {
		InputData data = new InputData();
		data.horizontal = 0;
		data.vertical = 0;
		data.mouseX = 0 * sensitivityy;
		data.mouseY = 0 * sensitivityy;
		data.pause = false;
		data.jump = true; //Jump is set to true for testing purposes. If connected, ai should jump whenever possible
		data.scoreboard = false;
		data.useAbilities = new bool[ABILITY_INPUTS];
		// ability list starts at 1
		for (int i = 1; i <= PlayerInput.ABILITY_INPUTS; i++) {
			data.useAbilities[i - 1] = false;
		}
		data.melee = false;
		return data;
	}


}
