using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expanding_Projectile : Projectile {

    private Vector3 initialPosition;
    public float expansionRate = 1;

    void Start()
    {
        Invoke("DestroyMe", lifetime);
        currentVelocity = this.GetComponent<Rigidbody>().velocity.magnitude;
        initialPosition = this.GetComponent<Transform>().position;
    }

    void Update()
    {
        float newSize = (this.GetComponent<Transform>().position - initialPosition).magnitude * expansionRate;
        this.GetComponent<Transform>().localScale = new Vector3(newSize, newSize, newSize);
    }
}
