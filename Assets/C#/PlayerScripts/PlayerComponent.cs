using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour {
    /* Base class that stores a reference to the player movement */
    protected BasePlayer myBase;
    public PlayerComponent initialize(BasePlayer p) {
        myBase = p;
        return this;
    }
}
