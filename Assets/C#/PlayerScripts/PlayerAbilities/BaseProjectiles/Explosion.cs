using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {
    //[HideInInspector]
    public GameObject sourcePlayer;
    public HitArguments.DamageType damageType;
    public PlayerEffects.Effects effect;
    public float effectDuration = 3;

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
            if (!inLOS(hit)) continue;

            PlayerStats ps;
            if ((ps = hit.transform.GetComponentInParent<PlayerStats>())) {
                //print(ps);
                if (ps.gameObject == sourcePlayer.gameObject) isSourcePlayer = true;
            }
            Rigidbody r;
            if ((r = hit.transform.GetComponent<Rigidbody>()) != null && !r.GetComponent<Projectile>()) {
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
                    HitManager.HitClientside(new HitArguments(((Component)hit.transform.gameObject.GetComponentInParent<IHittable>()).gameObject, sourcePlayer.GetComponentInParent<PlayerStats>().gameObject)
                        .withDamage( (isSourcePlayer ? sourcePlayerDamageMultiplier : 1) * ((maxDamage/Vector3.Distance(target.position, transform.position)) + minDamage))
                        .withDamageType(damageType)
                        .withEffect(effect)
                        .withEffectDuration(effectDuration));
                    hitHittables.Add(h);
                }

            }
        }
	}

    public bool inLOS(RaycastHit hit)
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, hit.transform.position - transform.position, Vector3.Distance(transform.position, hit.transform.position));
        // Hits returns an array of everything in the sphere length, however we are just looking at the start

		//print(hits.size);
        foreach (RaycastHit hitBetween in hits)
        {
			if (hitBetween.transform == hit.transform)
				continue;
            Collider c;
			if ((c = hitBetween.transform.GetComponent<Collider> ()) && c.isTrigger)
				continue;
			if (hitBetween.transform.GetComponentInParent<PlayerStats> ()) 
				continue;
				Debug.DrawLine(transform.position, hitBetween.point, Color.red, 10);
			//print (hitBetween.transform);
			return false;
        }
        return true; 
    }
}
