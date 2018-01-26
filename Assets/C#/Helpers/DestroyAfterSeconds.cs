using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {

    public float lifeTime;  // in seconds

	// Use this for initialization
	void Start () {
        Invoke("DestroyMe", lifeTime);
	}
	
	void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
