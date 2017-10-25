using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class HitManager : NetworkBehaviour {
    
    public enum HitVerificationMethod { none, melee, projectile, hitscan }
    
    public static bool VerifyHit(HitVerificationMethod verificationMethod, HitArguments args) {
        bool verified = false;
        switch (verificationMethod)
        {
            case HitVerificationMethod.none:
                verified = true;
                break;
            case HitVerificationMethod.melee:
                verified = true;
                break;
            case HitVerificationMethod.projectile:
                verified = true;

                break;
            case HitVerificationMethod.hitscan:
                verified = true;

                break;
            default:
                break;
        }
        if (verified)
        {
            // Apply damage
            return true;
        }
        else
        {
            /* consider kicking the violator */
            return false;
        }
    }

    public static void HitClientside(HitArguments args)
    {
        HitClientside(HitVerificationMethod.none, args);
    }
    public static void HitClientside(HitVerificationMethod verificationMethod, HitArguments args)
    {
        PlayerStats myPlayerStats = args.sourcePlayer.GetComponent<PlayerStats>();
        NetworkBehaviour targetBehavior;
        if ((targetBehavior = args.target.GetComponentInParent<NetworkBehaviour>()))
        {
            if (myPlayerStats.isLocalPlayer)    
            {
                // Call network event through server
                myPlayerStats.CmdApplyDamage(verificationMethod, args);
            }
        }
        else
        {
            // This is an object that may or may not exist on all clients, so we will handle collision locally
            args.target.GetComponentInParent<IHittable>().Hit(args);
        }
    }
}
