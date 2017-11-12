using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWall : MonoBehaviour {

    public GameObject sourcePlayer;
    public float cooldown;

    private ArrayList hitByThese;
    private ArrayList velocities;
	void Start () {
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
            return;
        }
    }

    void OnDestroy()
    {
        for(int i=0; i<hitByThese.Count; i++)
        {
            Rigidbody r = (Rigidbody) hitByThese[i];
            if(r != null)
                r.AddForce(-((Vector3) velocities[i]), ForceMode.VelocityChange);
        }
    }

    void Update()
    {
        for(int i=0; i<hitByThese.Count; i++)
        {
            Rigidbody r;
            if((r = (Rigidbody)hitByThese[i]).velocity != Vector3.zero){
                r.AddForce(-r.velocity, ForceMode.VelocityChange);
            }
        }
    }
}
