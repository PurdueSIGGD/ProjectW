using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Effect_SpeedBoost : Effect {
    public float speedIncreasePercentage;
    public GameObject endSound;
    

    public override void Effect_Start(PlayerEffects target) {
        target.runSpeedModifier *= speedIncreasePercentage;
    }
    public override void Effect_End(PlayerEffects target) {
        target.runSpeedModifier /= speedIncreasePercentage;
        SpawnBoost(endSound);
    }

    public override void Effect_Update() {

    }
    public void SpawnBoost(GameObject toInstantiate) {
        GameObject result = GameObject.Instantiate(toInstantiate, transform.parent);
        result.transform.localPosition = Vector3.zero; // Center on parent
    }
}
