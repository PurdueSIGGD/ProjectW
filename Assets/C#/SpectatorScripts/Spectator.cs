using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spectator : NetworkBehaviour {
    public ParticleSystem myParticles;
    public float moveSpeed;
    private Rigidbody myRigid;
	// Use this for initialization
	void Start () {
		if (isLocalPlayer)
        {
            myParticles.Pause();
        }
        myRigid = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        bool pause = Input.GetAxis("Pause") > 0;
        float up = Input.GetAxis("Jump");
        float down = Input.GetAxis("Crouch");

        transform.Rotate(0, mouseX, 0);

        float newX = transform.rotation.eulerAngles.x + mouseY;
        //We do some fancy math to ensure 0 < newX < 360, nothing more
        newX = (newX + 360) % 360;
        //Ensure it doesn't go past our top or low bounds
        if ((newX > 0 && newX < 90) || (newX < 360 && newX > 270))
        {
            // Camera rotation
            transform.Rotate(mouseY, 0, 0);
        }
        else
        {
            // We don't want you to look all the way behind you, that's weird
        }


        Vector3 dirVector = new Vector3(horizontal, up - down, vertical);
        float distanceMultiplier = Time.deltaTime * moveSpeed;
        myRigid.MovePosition(transform.position + transform.TransformDirection(dirVector) * distanceMultiplier);
    }
}
