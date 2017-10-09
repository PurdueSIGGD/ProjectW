using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterParticlesEnd : MonoBehaviour {
    public ParticleSystem p;
    public void Start() {
        if (p == null) {
            p = this.GetComponent<ParticleSystem>();
        }
    }


	// Update is called once per frame
	void Update () {
		if (!p.IsAlive(true)) {
                Destroy(this.gameObject);
        }
	}
}
