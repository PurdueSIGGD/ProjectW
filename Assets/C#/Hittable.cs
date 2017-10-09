using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public abstract class Hittable {
    public enum DamageType { Neutral, Fire, Ice, Electric, Denim };
   
}