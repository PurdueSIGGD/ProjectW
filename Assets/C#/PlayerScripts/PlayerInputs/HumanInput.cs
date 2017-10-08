﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanInput : PlayerInput { 
	public override PlayerInput.InputData getData() {
        InputData data = new InputData();
        data.horizontal = Input.GetAxis("Horizontal");
        data.vertical = Input.GetAxis("Vertical");
        data.mouseX = Input.GetAxis("Mouse X");
        data.jump = Input.GetAxis("Jump") > 0;
        data.useAbilities = new bool[ABILITY_INPUTS];
        // ability list starts at 1
        for (int i = 1; i <= PlayerInput.ABILITY_INPUTS; i++) {
            data.useAbilities[i - 1] = Input.GetAxis("Ability" + i) > 0;
        }
        data.melee = Input.GetAxis("Melee") > 0;
        return data;
    }
}
