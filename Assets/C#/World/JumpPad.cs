using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    public int jumpModifier;
    private ArrayList onCooldown;

    void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger) return;
        BasePlayer b;
        if ((b = col.GetComponentInParent<BasePlayer>()) != null && !onCooldown.Contains(b))
        {
            b.myRigid.AddForce(Vector3.up * b.myRigid.mass * jumpModifier);
            onCooldown.Add(b);
        }
        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.isTrigger) return;
        BasePlayer b;
        if ((b = col.GetComponentInParent<BasePlayer>()) != null && onCooldown.Contains(b))
        {
            onCooldown.Remove(b);
        }
    }

    public IEnumerator Cooldown(BasePlayer b)
    {
        do
        {
            // Local damage only
            if ((Component)b != null && ((Component)b).gameObject != null)
            {
                yield return new WaitForSeconds(1);
                onCooldown.Remove(b);
            }
        } while (b != null && onCooldown.Contains(b));
    }
}
