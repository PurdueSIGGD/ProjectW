using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : PlayerComponent {
    public float runSpeed = 7;
    public float runSpeedModifier = 1;
    public float jumpCooldown = 1;
    public float jumpHeight = 1000;
    public float airborneSpeedMultiplier = 0.5f; // How much control a player should have while jumping
    public float jumpPushoffMultiplier = 1000; // If going in a direction before jumping, push off at this force

    public Collider[] collidersToIgnore;

    private float lastJump = -1; //Time at last jump
    private bool isGrounded;
    private const float checkGroundDistance = 0.5f;
    private const float checkGroundWidth = 0.5f;
    private Vector2 lastInputs;
    // Needed for push-off velocity
    private Vector3 lastPosition;
    

    public void FixedUpdate() {
        CheckGroundStatus();
        lastPosition = transform.position;
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

        lastInputs = new Vector2(data.horizontal, data.vertical);
        transform.Rotate(0, data.mouseX, 0);
        transform.Translate(Time.deltaTime * runSpeed * runSpeedModifier * new Vector3(data.horizontal, 0, data.vertical));
        myBase.myAnimator.SetFloat("Vertical", data.vertical * runSpeedModifier);
        myBase.myAnimator.SetFloat("Horizontal", data.horizontal * runSpeedModifier);
        myBase.myAnimator.SetBool("Airborne", !isGrounded);
        myBase.myAnimator.SetBool("Jump", data.jump);
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
