using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {
    public int jumpModifier;
    private ArrayList onCooldown = new ArrayList();

    void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger) return;
        BasePlayer b;
        if (col.GetComponentInParent<BasePlayer>() != null)
        {
            if (!onCooldown.Contains(col.GetComponentInParent<BasePlayer>()))
            {
                b = col.GetComponentInParent<BasePlayer>();
                b.myRigid.AddForce(Vector3.up * b.myRigid.mass * jumpModifier);
                onCooldown.Add(b);
            }
        }
        
    }

    void OnTriggerExit(Collider col)
    {
        if (col.isTrigger) return;
        BasePlayer b;
        if (col.GetComponentInParent<BasePlayer>() != null && onCooldown.Contains(col.GetComponentInParent<BasePlayer>()))
        {
            b = col.GetComponentInParent<BasePlayer>();
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
