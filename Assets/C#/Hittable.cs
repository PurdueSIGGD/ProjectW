using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
/**
 * Class for useful enums or static methods
 * 
 */
public abstract class Hittable {
    public enum DamageType { Neutral, Fire, Ice, Electric, Denim };
   
    /**
     * Determine if we should favor the shooter, the server, or do local collision
     * 
     */
    public static void Hit(GameObject target, GameObject sourcePlayer, float damage, DamageType damageType, PlayerEffects.Effects effect) {
        //print("i am going to hit" + hitPlayer);
        PlayerStats myPlayerStats = sourcePlayer.GetComponent<PlayerStats>();
        NetworkBehaviour targetBehavior;
        if ((targetBehavior = target.GetComponentInParent<NetworkBehaviour>())) {
            // If the target is network bound, and we are tracking our own collision
            // This is an  object that exists and is tracked on all clients
            if (myPlayerStats.isServer) {
                // Favor the server
            }
            if (myPlayerStats.isLocalPlayer) {
                // Favor the client
                //hitPlayer = targetBehavior.gameObject;

                //Debug.Log("CMD Hitting for " + damage);
                myPlayerStats.CmdApplyDamage(targetBehavior.gameObject, damage, damageType, effect);
            }
        } else {
            // This is an object that may or may not exist on all clients, so we will handle collision locally
            //Debug.Log("local hitting for " + damage);
            target.GetComponentInParent<IHittable>().Hit(damage, sourcePlayer, damageType, effect);
        }
    }
}