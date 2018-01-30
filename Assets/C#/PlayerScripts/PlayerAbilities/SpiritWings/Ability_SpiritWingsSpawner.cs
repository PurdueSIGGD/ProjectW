using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SpiritWingsSpawner : Ability_ObjectSpawner {

    public float spellDuration = 2;
    public float boostMultiplier = 1f;
	public float maxVelocity = 30;
	public float endVelocity = 8;
    public float verticalForce = .5f;
    public float initialUpwardVelocity = 4;
    public float pushForce = 1000;

    public override void OnSpellSpawned(GameObject spawn)
    {
        SpiritWings sw;
        if (sw = spawn.GetComponent<SpiritWings>())
        {
            sw.sourcePlayer = this.gameObject;
            sw.StartSpiritWings(this, spellDuration);
            sw.transform.SetParent(this.gameObject.transform);
        }
    }
    public void boostUp()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, initialUpwardVelocity, 0), ForceMode.VelocityChange);
    }
    public void boost()
    {
		Rigidbody r = this.gameObject.GetComponent<Rigidbody> ();
		if (r.velocity.magnitude <= maxVelocity) {
			r.AddForce (aimAngle.rotation * new Vector3 (0, 0, boostMultiplier), ForceMode.VelocityChange);
		} else {
			r.AddForce (((maxVelocity-r.velocity.magnitude)/r.velocity.magnitude)*r.velocity, ForceMode.VelocityChange);
		}
        r.AddForce((new Vector3(0, verticalForce, 0)), ForceMode.VelocityChange);
    }
	public void slowToStop(){
		Rigidbody r = this.gameObject.GetComponent<Rigidbody> ();
		if(r.velocity.magnitude > endVelocity)
			r.AddForce (((endVelocity-r.velocity.magnitude)/r.velocity.magnitude)*r.velocity, ForceMode.VelocityChange);
	}
}
