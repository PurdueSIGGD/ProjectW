using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerStats : PlayerComponent, IHittable {
    public GameObject healthBar;
    [SyncVar]
    public float health = 100;
    public float healthMax = 100;
    [SyncVar]
    public float magic = 100;
    public float magicMax = 100;
    
    public void Hit(float damage, GameObject owner, Hittable.DamageType type) {

        if (isServer) {
            changeHealth(-1 * damage);
        }
    }
    public float changeHealth(float f) {
        float returnVal = 0;
        health += f;
        if (health > healthMax) {
            returnVal = health - healthMax;
            health = healthMax;
        } else if (health < 0) {
            returnVal = health;
            health = 0;
        }
        return returnVal;
    }
    void Update() {
        healthBar.transform.localScale = new Vector3(health, 1, 1);
    }
    /**
     * You can only tell the server to do a command inside of the root player control, so this will allow the server to do damage
     */
    [Command]
    public void CmdApplyDamage(GameObject target, float damage, Hittable.DamageType type) {
        target.GetComponent<IHittable>().Hit(damage, this.gameObject, type);
    }
}
