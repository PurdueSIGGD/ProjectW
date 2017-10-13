using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialScaling : MonoBehaviour {
    // Updates the texture scale with the scale of the object, for linear scaling
	void Start () {
        MeshRenderer msh;
        if ((msh = this.GetComponent<MeshRenderer>())) {
            foreach (Material m in msh.materials) {
                m.mainTextureScale = transform.localScale / 2;
            }
        }
	}
	
}
