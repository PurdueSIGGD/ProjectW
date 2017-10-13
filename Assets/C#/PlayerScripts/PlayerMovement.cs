using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMovement : PlayerComponent {
    public Transform cameraRotator;
    public float runSpeed = 7;
    public float jumpCooldown = 1;
    public float jumpHeight = 1000;
    public float airborneSpeedMultiplier = 0.5f; // How much control a player should have while jumping
    public float jumpPushoffMultiplier = 1000; // If going in a direction before jumping, push off at this force
    public Collider[] collidersToIgnore;

    private float lastJump = -1; //Time at last jump
    private bool isGrounded;
    private const float checkGroundDistance = 0.5f;
    private const float checkGroundWidth = 0.5f;
    // Needed for push-off velocity
    private Vector3 lastPosition;

    /* network dictated values for player animation */

    [SyncVar]
    float sVertical = 0;
    [SyncVar]
    float sHorizontal = 0;
    [SyncVar]
    bool sAirborne = false;
    [SyncVar]
    bool sJump = false;

    public float sRunSpeedModifier = 1;


    public void FixedUpdate() {
        if (isLocalPlayer) {
            CheckGroundStatus();
            lastPosition = transform.position;
        } else {

        }

        myBase.myAnimator.SetFloat("Vertical", sVertical);
        myBase.myAnimator.SetFloat("Horizontal", sHorizontal);
        myBase.myAnimator.SetBool("Airborne", sAirborne);
        myBase.myAnimator.SetBool("Jump", sJump);
        myBase.myAnimator.SetFloat("SpeedFactor", sRunSpeedModifier);
        
    }
    

    public void processMovement(PlayerInput.InputData data) {
      
        if (data.vertical < 0) {
            // If moving backwards, move a wee bit slower
            data.horizontal /= 2;
            data.vertical /= 2;
        }
        if (!isGrounded) {
            data.vertical *= airborneSpeedMultiplier;
            data.horizontal *= airborneSpeedMultiplier;
        }
        if (data.jump && (Time.time - lastJump < jumpCooldown || !isGrounded)) {
            data.jump = false;
        }
        
        transform.Rotate(0, data.mouseX, 0);

        float newX = cameraRotator.rotation.eulerAngles.x + data.mouseY;
        //We do some fancy math to ensure 0 < newX < 360, nothing more
        newX = (newX + 360) % 360;
        //Ensure it doesn't go past our top or low bounds
        if ((newX > 0 && newX < 90) || (newX < 360 && newX > 270)) {
            // Camera rotation
            cameraRotator.Rotate(data.mouseY, 0, 0);
        } else {
            // We don't want you to look all the way behind you, that's weird
        }


        transform.Translate(Time.deltaTime * runSpeed * sRunSpeedModifier * new Vector3(data.horizontal, 0, data.vertical));
        sVertical =  data.vertical * sRunSpeedModifier;
        sHorizontal = data.horizontal * sRunSpeedModifier;
        sAirborne = !isGrounded;
        sJump = data.jump;

        CmdSetPlayerData(sVertical, sHorizontal, sAirborne, sJump);
        
    }
    [Command]
    public void CmdSetPlayerData(float vertical, float horizontal, bool airborne, bool jump) {
        sVertical = vertical;
        sHorizontal = horizontal;
        sAirborne = airborne;
        sJump = jump;

    }

    public void addJumpForce() {
        if (isGrounded) {
            lastJump = Time.time;
            Vector3 verticalDirection = Vector3.up * myBase.myRigid.mass * jumpHeight;
            Vector3 pushOffDirection = myBase.myRigid.mass * jumpPushoffMultiplier * (transform.position - lastPosition);
            myBase.myRigid.AddForce(verticalDirection + pushOffDirection);
        }
    }


    void CheckGroundStatus() {
        RaycastHit hitInfo;
        Vector3 root = transform.position + Vector3.up * 0.3f;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(root, transform.position + (Vector3.down * checkGroundDistance));
        Debug.DrawLine(root + Vector3.forward * checkGroundWidth, transform.position + (Vector3.down * checkGroundDistance));
        Debug.DrawLine(root + Vector3.left * checkGroundWidth, transform.position + (Vector3.down * checkGroundDistance));
        Debug.DrawLine(root + Vector3.right * checkGroundWidth, transform.position + (Vector3.down * checkGroundDistance));
        Debug.DrawLine(root + Vector3.back * checkGroundWidth, transform.position + (Vector3.down * checkGroundDistance));
#endif
        // We check at all edges of the capsule, to verify we are in range, even at the edges
        if (Physics.Raycast(root, Vector3.down, out hitInfo, checkGroundDistance) ||
            Physics.Raycast(root + Vector3.forward * checkGroundWidth, Vector3.down, out hitInfo, checkGroundDistance) ||
            Physics.Raycast(root + Vector3.left * checkGroundWidth, Vector3.down, out hitInfo, checkGroundDistance) ||
            Physics.Raycast(root + Vector3.right * checkGroundWidth, Vector3.down, out hitInfo, checkGroundDistance) ||
            Physics.Raycast(root + Vector3.back * checkGroundWidth, Vector3.down, out hitInfo, checkGroundDistance) 
            ) {
            if (collidersToIgnore != null && !System.Array.Find(collidersToIgnore, c => c == hitInfo.collider)) {
                isGrounded = true;
            }
        } else {
            isGrounded = false;
        }
    }
}
