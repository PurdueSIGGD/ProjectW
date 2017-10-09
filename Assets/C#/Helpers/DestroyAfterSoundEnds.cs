using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSoundEnds : MonoBehaviour {
    public AudioSource s;
    public void Start() {
        if (s == null) {
            s = this.GetComponent<AudioSource>();
        }
        if (!s.isPlaying) {
            s.Play();
        }
    }

	// Update is called once per frame
	void Update () {
        if (s.isPlaying) {
            
        } else {
            Destroy(s.gameObject);
        }


    }
}
 