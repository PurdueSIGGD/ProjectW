using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Effect: push player in their forward direction
 */
public class Effect_Slip : Effect
{

   public float slipForce; 

    public override void Effect_End(PlayerEffects target)
    {

    }


    public override void Effect_Start(PlayerEffects target)
    {
        // find target.forward direction
        Transform tTransform = target.GetComponent<Transform>();
        Rigidbody tRigidBody = target.GetComponent<Rigidbody>();

        // push target in their own forward direction
        tRigidBody.AddForce(tTransform.forward * slipForce, ForceMode.Acceleration);
       
    }

    public override void Effect_Update()
    {

    }
}
