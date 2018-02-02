using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWings : MonoBehaviour {

    public GameObject sourcePlayer;
    public HitArguments.DamageType damageType;
    private float previousBoost;
    private float coolDown = .02f;
	private float radius = 10;
    private Ability_SpiritWingsSpawner ability;

    public void StartSpiritWings(Ability_SpiritWingsSpawner ability, float spellDuration) {
        this.ability = ability;
        ability.boostUp();
        Destroy(this.gameObject, spellDuration);
        previousBoost = Time.time;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.GetComponentInParent<IHittable>() == null)
            return;
		PlayerStats ps;
		if ((ps = col.GetComponentInParent<PlayerStats> ()) != null && ps.gameObject == sourcePlayer.gameObject) {
			return;
		} else {
            Rigidbody r;
			if ((r = col.transform.GetComponent<Rigidbody>()) != null)
            {
                if (ps)
                {
                    r.GetComponentInParent<BasePlayer>().myRigid.AddExplosionForce(ability.pushForce, transform.position, radius);
                    HitManager.HitClientside(new HitArguments(r.GetComponentInParent<BasePlayer>().gameObject, sourcePlayer).withDamage(ability.damage));
                }
                else
                    r.AddExplosionForce(ability.pushForce, transform.position, radius);
            }
        }
    }//TODO: add continuous upward force

    void Update()
    {
        if(Time.time - previousBoost > coolDown)
        {
            previousBoost = Time.time;
            ability.boost();
        }
    }
	void OnDestroy(){
		ability.slowToStop ();
	}
}
