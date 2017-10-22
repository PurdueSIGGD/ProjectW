using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * A class from another project that causes objects moving fast enough to play their sound
 */
public class Noisy : MonoBehaviour {
    //public bool loud;
    AudioSource mySound;
    public float cooldown = 0.4f;
    private float lastHit = -100;
    public AudioClip[] clips;
    private Vector3 lastPos;
    void Start() {
        mySound = this.GetComponent<AudioSource>();
    }

	void OnTriggerEnter(Collider col) {
        if (Time.time - lastHit > cooldown && 
            Vector3.Distance(transform.position, lastPos) > .2 &&
            !col.GetComponent<Noisy>()) {
            

            lastHit = Time.time;
            mySound.clip = clips[Random.Range(0, clips.Length - 1)];
            mySound.Play();
        }

        lastPos = transform.position;
    }
}
