using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Expanding_Children : MonoBehaviour {

    private Vector3 initialPosition;
    public float expansionMultiplier = 1;

    void Start()
    {
        initialPosition = this.GetComponent<Transform>().position;
    }

    void Update () {
        float newSize = (this.GetComponent<Transform>().position - initialPosition).magnitude * expansionMultiplier;
        this.GetComponent<Transform>().localScale = new Vector3(newSize, newSize, newSize);
	}
}
