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
    public enum Effects { none, timeSlow, timeFast, runSpeed, magicRegenSlow, magicRegenFast, stun };
    // Time and speed
    public float timeModifier = 1;
    public float runSpeedModifier = 1;
    // Health and magic
    public float magicRegenModifier = 1;
    // Blocking effects
    public bool stunned = false;

    [SyncVar]
    PlayerEffects.Effects effectToSpawn;
    [SyncVar]
    int effectCount;
    int lastEffectCount;

    public override void PlayerComponent_Start() {
        effectTypes = effectPrefabHolder.prefabs;
    }
    public void AddEffect(PlayerEffects.Effects effect) {
        if (effect != Effects.none) {
            effectToSpawn = effect;
            effectCount++;
        }
        
    }
    public override void PlayerComponent_Update() {
        if (effectCount != lastEffectCount) {
            lastEffectCount = effectCount;
            //print("spawning effect " + (effectToSpawn - 1));
            GameObject.Instantiate(effectTypes[(int)effectToSpawn - 1], transform);
        }
    }

    public void ClearModifiers() {
        timeModifier = 1;
        magicRegenModifier = 1;
        runSpeedModifier = 1;
        stunned = false;
    }
    public void Death() {
        ClearModifiers();
    }
}
