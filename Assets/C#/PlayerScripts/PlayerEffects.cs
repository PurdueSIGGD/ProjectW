using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerEffects : PlayerComponent {
    /**
     * A script containing every modifier that may be affecting the player
     * If adding a new modifier, be sure to add it into any classes that may use it, and add it to the clear modifiers list
     * I should have a reference for each effect prefab that I should stun
     */
    // 0 for time slow, 1 for time fast, 2 for run speed, 3 for magic regen slow, 4 for magic regen fast, 5 for stunning 
    public PrefabHolder effectPrefabHolder;
    public GameObject[] effectTypes;
    public enum Effects { none, timeSlow, timeFast, runSpeed, magicRegenSlow, magicRegenFast, changeHealth, poisonDamage, immobilize };
    // Time and speed
    public float timeModifier = 1;
    public float runSpeedModifier = 1;
    // Health and magic
    public float magicRegenModifier = 1;
    // Blocking effects
    public bool stunned = false;
    public bool immobilized = false;
    

    public override void PlayerComponent_Start() {
        effectTypes = effectPrefabHolder.prefabs;
    }
    public void AddEffect(HitArguments hit) {
        if (hit.effect != Effects.none) {
            RpcAddEffect(hit);
        }
        
    }
    [ClientRpc]
    public void RpcAddEffect(HitArguments hit) {
        GameObject eff = GameObject.Instantiate(effectTypes[(int)hit.effect - 1], transform);
		Effect effect = eff.GetComponent<Effect> ();
		effect.duration = hit.effectDuration;
		effect.sourcePlayer = hit.sourcePlayer;
    }

    public override void PlayerComponent_Update() {

    }


    public void ClearModifiers() {
        timeModifier = 1;
        magicRegenModifier = 1;
        runSpeedModifier = 1;
        stunned = false;
        immobilized = false;
    }
    public void Death() {
        ClearModifiers();
        // Delete all effects on us
        foreach (Effect e in transform.GetComponentsInChildren<Effect>()) {
            GameObject.Destroy(e.gameObject);
        }
    }
}
