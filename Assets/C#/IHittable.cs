using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public interface IHittable {
    void Hit(float damage, GameObject owner, Hittable.DamageType type, PlayerEffects.Effects effect, float effectDuration);
}