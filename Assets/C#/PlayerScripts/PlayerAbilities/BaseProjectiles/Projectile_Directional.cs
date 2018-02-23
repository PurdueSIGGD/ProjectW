using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Networking;

public class Projectile_Directional : Projectile
{
    void Update()
    {
        Quaternion up = (Quaternion.Euler(this.GetComponent<Rigidbody>().velocity));
        this.transform.LookAt(this.GetComponent<Rigidbody>().velocity, up.eulerAngles);
    }
}
