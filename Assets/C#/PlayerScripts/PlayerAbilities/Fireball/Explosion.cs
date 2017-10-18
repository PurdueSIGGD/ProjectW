using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    //[HideInInspector]
    public GameObject sourcePlayer;
    public Hittable.DamageType damageType;

    public float pushForce, range, verticalPushForce = 1;
    public float minDamage, maxDamage; // Distance based damages, i.e. minDamage at range away, maxDamage when direct hit

    public float sourcePlayerDamageMultiplier;
    public float sourcePlayerForceMultiplier;

    private ArrayList hitHittables;

    // Use this for initialization
    void Start () {
        hitHittables = new ArrayList();

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, Vector3.one);
        // Hits returns an array of everything in the sphere length, however we are just looking at the start
        foreach (RaycastHit hit in hits) {
            bool isSourcePlayer = false;
            PlayerStats ps;
            if ((ps = hit.transform.GetComponentInParent<PlayerStats>())) {
                //print(ps);
                if (ps.gameObject == sourcePlayer.gameObject) isSourcePlayer = true;
            }
            Rigidbody r;
            if ((r = hit.transform.GetComponent<Rigidbody>()) != null) {
                if (ps) {
                    // Do something different, not as harsh hit forces
                    if (!ps.death) r.AddForce(Vector3.up * verticalPushForce * (isSourcePlayer ? (pushForce * sourcePlayerForceMultiplier):pushForce) * 2);
                    r.AddExplosionForce((isSourcePlayer ? (pushForce * sourcePlayerForceMultiplier) : pushForce) * 0.5f, transform.position, range);
                } else {
                    r.AddExplosionForce(pushForce, transform.position, range);

                }
            }
            IHittable h;
            if (( h = hit.transform.GetComponentInParent<IHittable>()) != null) {
                if (hitHittables.Contains(h)) {
                    //print("Not hitting " + hit.transform.name + ", has already been hit");
                } else {
                    Transform target = hit.transform;
                    // If this is a player, we should be hitting from the distance of the root of the player, not the limb
                    if (ps) {
                        target = ps.transform;
                    }
                    Hittable.Hit(hit.transform.gameObject, sourcePlayer, (isSourcePlayer ? sourcePlayerDamageMultiplier : 1) * ((maxDamage/Vector3.Distance(target.position, transform.position)) + minDamage), damageType, PlayerEffects.Effects.none);
                    hitHittables.Add(h);
                }

            }
        }
	}
}
