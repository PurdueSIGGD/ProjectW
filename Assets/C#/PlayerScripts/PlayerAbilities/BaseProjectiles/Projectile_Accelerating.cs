using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Accelerating : Projectile {
    public float speedMultiplier;
    public float maxVelocity;
    private Rigidbody myRigid;

    void Start() {
        myRigid = this.GetComponent<Rigidbody>();
        currentVelocity = myRigid.velocity.magnitude;
    }
    // Update is called once per frame
    void Update () {
        if (myRigid.velocity.magnitude < maxVelocity)
        {
            myRigid.AddForce(myRigid.velocity * Mathf.Pow(myRigid.velocity.magnitude, 2) * speedMultiplier * Time.deltaTime);
            currentVelocity = myRigid.velocity.magnitude;
        }
	}
}
