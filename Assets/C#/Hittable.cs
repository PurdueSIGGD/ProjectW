﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif
public abstract class Hittable  {
    public enum DamageType { Neutral, Fire, Ice, Electric, Denim };
}