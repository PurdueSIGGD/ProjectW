using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWall : MonoBehaviour {
    
    private Ability_TimeWallSpawner ability;

    private ArrayList hitByThese;
    private ArrayList velocities;
	public void StartTimeWall (Ability_TimeWallSpawner ability, float cooldown) {
        
        this.ability = ability;
       
        Destroy(this.gameObject, cooldown);
        hitByThese = new ArrayList();
        velocities = new ArrayList();
	}

    private void OnTriggerEnter(Collider col)
    {
        if(col.GetComponentInParent<IHittable>() != null)
            return;
        if (col.isTrigger)
        {
            Rigidbody r;
            if((r = col.GetComponent<Rigidbody>()) != null)
            {
                hitByThese.Add(r);
                velocities.Add(r.velocity);
                r.AddForce(-(r.velocity), ForceMode.VelocityChange);
            }
            Projectile p;
            if (p = col.transform.GetComponent<Projectile>())
            {
                p.sourcePlayer = ability.gameObject;
            }
            return;
        }
    }

    void OnDestroy()
    {
        ability.TimeWallFinished(hitByThese, velocities);
    }

    void Update()
    {
        for(int i=0; i<hitByThese.Count; i++)
        {
            Rigidbody r;
            if((Rigidbody) hitByThese[i] != null && (r = (Rigidbody)hitByThese[i]).velocity != Vector3.zero){
                r.AddForce(-r.velocity, ForceMode.VelocityChange);
            }
        }
    }
}
