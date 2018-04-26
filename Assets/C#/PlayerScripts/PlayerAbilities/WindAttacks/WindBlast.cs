using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlast : MonoBehaviour {

    [HideInInspector]
    public GameObject sourcePlayer;
    [HideInInspector]
    public Vector3 angle;

    public float pushForce, multiplierForProjectiles = 1; // Because projectiles are only pushed once, while players are pushed as long as they are in the blast,
                                                          // projectiles are pushed with a greater force, as determined by this multiplier.
    public float pushDelay = .02f; // Delay between each time force is applied to something within the effect.

    public bool hitSameTeam = false;

    private float previousPush;
    private ArrayList hasHit;

    // Use this for initialization
    void Start () {
        hasHit = new ArrayList();
        previousPush = Time.time;
		
	}

    private void OnTriggerEnter(Collider col)
    {
        Rigidbody r;
        if ((r = col.GetComponent<Rigidbody>()) != null)
        {
            if(col.GetComponentInParent<IHittable>() != null) // Hit a player
            {
                r.AddForce(pushForce * angle, ForceMode.Force);
                if (col.GetComponentInParent<PlayerStats>() != null)
                    hasHit.Add(col.GetComponent<PlayerStats>());
            }
            else if(r.transform.GetComponent<Projectile>() != null) // Hit a projectile
            {
                r.AddForce(multiplierForProjectiles * pushForce * angle, ForceMode.Force);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponentInParent<IHittable>() != null)
        {
            PlayerStats ps;
            if((ps = col.GetComponentInParent<PlayerStats>()) != null)
            {
                if (hasHit.Contains(ps))
                    hasHit.Remove(ps);
            }

        }
    }

        // Update is called once per frame
        void Update()
    {
        if (hasHit.Count > 0 && Time.time - previousPush > pushDelay)
        {
            previousPush = Time.time;
            Rigidbody r;
            for(int i=0; i<hasHit.Count; i++) // Push everything in hasHit
            {
                r = ((GameObject) hasHit[i]).GetComponent<Rigidbody>();
                r.AddForce(pushForce * angle, ForceMode.Force);
            }
        }
    }
}
