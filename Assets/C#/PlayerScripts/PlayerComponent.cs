using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class PlayerComponent : NetworkBehaviour {
    /* Base class that stores a reference to the player movement */
    protected BasePlayer myBase;
    public PlayerComponent initialize(BasePlayer p) {
        myBase = p;
        return this;
    }

    // Script start execution is grumpy with me, so I am manually overriding this to make sure BasePlayer starts first
    public abstract void PlayerComponent_Start();
    public abstract void PlayerComponent_Update();
    bool hasStarted = false;
    public void Start() {
        if (myBase == null) {
            hasStarted = false;
        } else {
            PlayerComponent_Start();
            hasStarted = true;
        }
    }
    public void Update() {
        if (myBase != null) {
            if (!hasStarted) {
                Start(); // Try again
            } 
            PlayerComponent_Update();
        }
    }
}
