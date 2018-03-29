using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    public int jumpModifier;

    void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger) return;
        PlayerMovement m;
        if ((m = col.GetComponentInParent<PlayerMovement>()) != null)
        {
            m.jumpHeight += jumpModifier;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.isTrigger) return;
        PlayerMovement m;
        if ((m = col.GetComponentInParent<PlayerMovement>()) != null)
        {
            m.jumpHeight -= jumpModifier;
        }
    }
}
