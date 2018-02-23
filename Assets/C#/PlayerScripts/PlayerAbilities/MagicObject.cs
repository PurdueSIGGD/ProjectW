using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * many objects just need sourcePlayer
 * use this to wrap around those stuff
 */
public class MagicObject : MonoBehaviour {

    [HideInInspector]
    public GameObject sourcePlayer;
}
