using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

public class Projectile : NetworkBehaviour {
    [HideInInspector]
    [SyncVar]
    public GameObject sourcePlayer;

    public float damage;
    public Hittable.DamageType damageType;
    public bool dieOnHit = true;
    public bool faked; // If we are faked, we won't collide with anything, since the server processes the collision

	void OnTriggerEnter(Collider col) {
        if (col.isTrigger) return; // Only want our own trigger effects
        PlayerStats ps;
        //print("foo");
        if (!sourcePlayer) return; // Shouldn't collide with anything that isn't a source player
        if ((ps = col.GetComponentInParent<PlayerStats>())) {
            //print(ps);
            if (ps.gameObject == sourcePlayer.gameObject) return;
        }

        IHittable h;
        if (!faked && (h = col.gameObject.GetComponent<IHittable>()) != null) {
            // You have to call the static method here, since you can't directly tell it to damage
            //print("i am going to hit");
            sourcePlayer.GetComponent<PlayerStats>().CmdApplyDamage(col.gameObject, damage, damageType);
        }

        if (dieOnHit) {
            Destroy(this.gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
