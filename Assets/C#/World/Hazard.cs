using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Hazard : MonoBehaviour {
    private ArrayList toDamage;
    public HitArguments.DamageType type;
    public float damage;
    public float rate;
	
	void Start() {
        toDamage = new ArrayList();
    }
	void OnTriggerEnter(Collider col) {
        if (col.isTrigger) return;
        IHittable h;
        if ((h = col.GetComponent<IHittable>()) != null) {
            toDamage.Add(h);
            StartCoroutine(DamagingBehavior(h));
        }
    }
    void OnTriggerExit(Collider col) {
        if (col.isTrigger) return;
        IHittable h;
        if ((h = col.GetComponent<IHittable>()) != null) {
            toDamage.Remove(h);
        }
    }

    public IEnumerator DamagingBehavior(IHittable h) {
        do {
            // Local damage only
			if (((Component)h).gameObject != null) {
				h.Hit(new HitArguments(this.gameObject, ((Component)h).gameObject)
					.withDamage(damage)
					.withDamageType(type));
				yield return new WaitForSeconds(rate);
			}
        } while (h != null && toDamage.Contains(h));
    }
}
