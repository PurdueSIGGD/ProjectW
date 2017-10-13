using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerComponent : NetworkBehaviour {
    /* Base class that stores a reference to the player movement */
    protected BasePlayer myBase;
    public PlayerComponent initialize(BasePlayer p) {
        myBase = p;
        return this;
    }
}
