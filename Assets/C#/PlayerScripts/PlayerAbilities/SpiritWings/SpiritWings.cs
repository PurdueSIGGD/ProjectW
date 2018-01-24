using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWings : MonoBehaviour {

    public GameObject sourcePlayer;
    public HitArguments.DamageType damageType;
    private float previousBoost;
    private float coolDown = .02f;
    private Ability_SpiritWingsSpawner ability;

    public void StartSpiritWings(Ability_SpiritWingsSpawner ability, float spellDuration) {
        this.ability = ability;
        Destroy(this.gameObject, spellDuration);
        previousBoost = Time.time;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponentInParent<IHittable>() != null)
            return;
        if (col.isTrigger)
        {
            Rigidbody r;
            if ((r = col.GetComponent<Rigidbody>()) != null)
            {
                
                //r.AddForce(-(r.velocity), ForceMode.VelocityChange);
            }
        }
    }

    void Update()
    {
        if(Time.time - previousBoost > coolDown)
        {
            previousBoost = Time.time;
            ability.boost();
        }
    }
}
