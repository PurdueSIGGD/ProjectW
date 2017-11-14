using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour {

    PlayerEffects target;
	public GameObject sourcePlayer;
    float endTime = -1;
    public float duration;
    bool started;
    void Start() {

       
    }
    void Update() {
        if (!started) {
            // Check to see if we ever get a parent
            if (transform.parent != null && (target = transform.parent.GetComponent<PlayerEffects>())) {
                endTime = Time.time + duration;
                Effect_Start(target);
                started = true;
            } else {
                Debug.LogWarning("Effect was not spawned as a child of a target, destroying effects");
            }
        }
		Effect_Update ();
    
        if (endTime != -1 && Time.time >= endTime) {
            Effect_End(target);
            Destroy(this.gameObject);
        }
    }
    public abstract void Effect_Start(PlayerEffects target);
    public abstract void Effect_End(PlayerEffects target);
    public abstract void Effect_Update();
}