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
    [HideInInspector]
    [SyncVar]
    public bool death;
    private bool hasDeath;
    public GameObject[] deathSounds;
    
    public void Hit(float damage, GameObject owner, Hittable.DamageType type) {
        changeHealth(-1 * damage);
    }
    public float changeHealth(float f) {
        //print("took hit " + f);
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
        healthBar.transform.localScale = new Vector3(health, health>0?1:0, health > 0 ? 1 : 0);

        if (health == 0 && !hasDeath && isLocalPlayer) {
            // Player has to handle their own death
            CmdDeath();
            this.BroadcastMessage("Death");
            hasDeath = true;
        }
        if (death && !hasDeath) {
            // Other players have to react to the death
            this.BroadcastMessage("Death");
            hasDeath = true;
        }
    }
    /**
     * You can only tell the server to do a command inside of the root player control, so this will allow the server to do damage
     */
    [Command]
    public void CmdApplyDamage(GameObject target, float damage, Hittable.DamageType type) {
        //print("applying damage");
        target.GetComponentInParent<IHittable>().Hit(damage, this.gameObject, type);
    }
    [Command]
    public void CmdDeath() {
        //print("adding death");
        GameObject.FindObjectOfType<ProjectWGameManager>().AddDeath(this.gameObject, Network.player.ToString());
        death = true;
    }
    public void Death() {
        // Take care of death on each client's end
        foreach (Rigidbody r in GetComponentsInChildren<Rigidbody>()) {
            r.isKinematic = false;
        }
        myBase.myRigid.isKinematic = true;
        myBase.myAnimator.enabled = false;
        myBase.myCollider.enabled = false;
        // Move all parts to the ragdoll layer
        // So it interacts with the world, but not itself
        MoveToLayer(myBase.myAnimator.transform, 10);
        // Add death sound
        GameObject result = GameObject.Instantiate(deathSounds[UnityEngine.Random.Range(0, deathSounds.Length - 1)], transform);
        result.transform.localPosition = Vector3.zero; // Center on parent
    }

    public void MoveToLayer(Transform root, int layer) {
        // Recursively move all children and self to layer
        root.gameObject.layer = layer;
        foreach (Transform child in root) {
            MoveToLayer(child, layer);
        }
    }
    
}
