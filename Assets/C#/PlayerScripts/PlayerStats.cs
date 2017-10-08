using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : PlayerComponent, IHittable {
    public GameObject healthBar;
    [SyncVar]
    public float Health = 100;
    [SyncVar]
    public float Magic = 100;

    [Command]
    public void CmdHit(float damage, GameObject owner, Hittable.DamageType type) {

    }

   void Update() {
        healthBar.transform.localScale = new Vector3(Health, 1, 1);
   }
}
