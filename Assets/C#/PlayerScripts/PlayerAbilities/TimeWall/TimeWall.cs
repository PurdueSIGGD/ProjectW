using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWall : MonoBehaviour {
    
    private Ability_TimeWallSpawner ability;

    private ArrayList hitByThese;
    private ArrayList magnitudes;
	public void StartTimeWall (Ability_TimeWallSpawner ability, float cooldown) {
        
        this.ability = ability;
       
        Destroy(this.gameObject, cooldown);
        hitByThese = new ArrayList();
        magnitudes = new ArrayList();
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
                r.AddForce(-(r.velocity), ForceMode.VelocityChange);
            }
            Projectile p;
            if (p = r.transform.GetComponent<Projectile>())
            {
                magnitudes.Add(p.currentVelocity);
                p.sourcePlayer = ability.gameObject;
                p.CancelInvoke("DestroyMe");
                p.Invoke("DestroyMe", p.lifetime);
            }else
            {
                magnitudes.Add(Vector3.Magnitude(r.velocity));
            }

            return;
        }
    }

    void OnDestroy()
    {
        ability.TimeWallFinished(hitByThese, magnitudes);
    }

    void Update()
    {
        for(int i=0; i<hitByThese.Count; i++)
        {
            Rigidbody r;
            if((Rigidbody) hitByThese[i] == null)
            {
                hitByThese.RemoveAt(i);
                magnitudes.RemoveAt(i);
            }
            else if((r = (Rigidbody)hitByThese[i]).velocity != Vector3.zero){
                if((float)magnitudes[i] < Vector3.Magnitude(r.velocity))
                {
                    magnitudes[i] = (float)magnitudes[i] + Vector3.Magnitude(r.velocity);
                }
                r.AddForce(-r.velocity, ForceMode.VelocityChange);
            }
        }
    }
}
