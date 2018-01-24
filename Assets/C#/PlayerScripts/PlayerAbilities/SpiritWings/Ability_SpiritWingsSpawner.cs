using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_SpiritWingsSpawner : Ability_ObjectSpawner {

    public float spellDuration = 2;
    public float boostMultiplier = 1f;

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
    public void boost()
    {
        this.gameObject.GetComponent<Rigidbody>().AddForce(aimAngle.rotation * new Vector3(0,0,boostMultiplier), ForceMode.VelocityChange);
    }
}
